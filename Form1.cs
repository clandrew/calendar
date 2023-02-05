using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Calendar
{
    public partial class Form1 : Form
    {
        DateTime displayedMonth;
        LaidOut laidOut;
        LaidOut.PickResult lastClicked;
        string currentlyOpenedFilePath;

        public class Entry
        {
            public DateTime Date;
            public List<string> Notes;
        }
        List<Entry> entries;

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            monthLabel.Location = new Point(
                this.Width / 2 - (monthLabel.Width / 2), 
                monthLabel.Location.Y);

            entries = new List<Entry>();

            laidOut = new LaidOut();
            laidOut.Initialize();
            laidOut.GeneralLayout(panel1.CreateGraphics(), panel1.Width, panel1.Height);
            displayedMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            InitializeMonth();

            string lastPath = Properties.Settings.Default.LastOpenedDataFilePath;
            if (lastPath != null && lastPath.Length > 0)
            {
                OpenImpl(lastPath);
            }
        }

        // Returns null on success.
        // Returns string error on failure.
        string TryParseDataFile(string filePath)
        {
            string[] lines = null;
            try
            {
                lines = System.IO.File.ReadAllLines(filePath);
            }
            catch (System.IO.FileNotFoundException e)
            {
                return "Unable to open file " + filePath + ".";
            }
            for (int i = 0; i < lines.Length; ++i)
            {
                Entry e = new Entry();

                DateTime convertedDate;
                if (!DateTime.TryParse(lines[i], out convertedDate))
                {
                    return "Error at line " + (i + 1) + ": expected a date in format mm/dd/yyyy, instead found: " + lines[i] + ".";
                }

                e.Date = convertedDate;
                e.Notes = new List<string>();

                ++i;
                while (lines[i][0] != '/')
                {
                    if (i + 1 == lines.Length)
                    {
                        return "Error at line " + ( i + 1) + ": expected a '/' delimiter, instead found <end of file>.";
                    }
                    if (e.Notes.Count >= 5)
                    {
                        return "Error at line " + (i + 1) + ": attempted to add more than 4 lines of notes.";
                    }
                    if (lines[i].Length > 64)
                    {
                        return "Error at line " + (i + 1) + ": the note is length " + lines[i].Length + ", which is longer than the maximum of 64.";
                    }
                    e.Notes.Add(lines[i]);
                    ++i;
                }

                entries.Add(e);
            }
            return null;
        }

        private void OpenImpl(string filePath)
        {
            string errorMsg = TryParseDataFile(filePath);
            if (errorMsg != null)
            {
                MessageBox.Show(errorMsg, "Error loading file");
                return;
            }

            currentlyOpenedFilePath = filePath;

            laidOut.AttachNotes(entries);
            panel1.Invalidate();

            Properties.Settings.Default.LastOpenedDataFilePath = filePath;
            Properties.Settings.Default.Save();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            if (currentlyOpenedFilePath != null)
            {
                openFileDialog1.InitialDirectory = Path.GetDirectoryName(currentlyOpenedFilePath);
                openFileDialog1.FileName = Path.GetFileName(currentlyOpenedFilePath);
            }
            else
            {
                openFileDialog1.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                openFileDialog1.FileName = "Data.txt";
            }
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.DefaultExt = "txt";

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            OpenImpl(openFileDialog1.FileName);
        }

        void InitializeMonth()
        {
            laidOut.LayOut(panel1.Width, panel1.Height, displayedMonth, entries);
            ModifyMonthLabel();
        }

        public void ModifyMonthLabel()
        {
            string text = displayedMonth.ToString("MMMM") + " " + displayedMonth.Year;
            monthLabel.Text = text;
            monthLabel.Location = new Point((panel1.Width - monthLabel.Width) / 2, monthLabel.Location.Y);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            laidOut.Draw(e.Graphics, 0, 0);
        }
        private void previousMonthButton_Click(object sender, EventArgs e)
        {
            displayedMonth = displayedMonth.AddMonths(-1);
            InitializeMonth();
            panel1.Invalidate();
        }

        private void nextMonthButton_Click(object sender, EventArgs e)
        {
            displayedMonth = displayedMonth.AddMonths(1);
            InitializeMonth();
            panel1.Invalidate();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDocument1.DefaultPageSettings.Landscape = true;

            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.PrintPreviewControl.Zoom = 1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap temp = new Bitmap(monthLabel.Width, monthLabel.Height);
            Color prevColor = monthLabel.BackColor;
            monthLabel.BackColor = Color.White;
            monthLabel.DrawToBitmap(temp, new Rectangle(0, 0, temp.Width, temp.Height));
            e.Graphics.DrawImageUnscaled(
                temp, 
                e.MarginBounds.X + (e.MarginBounds.Width/2) - (temp.Width / 2), 
                0);
            monthLabel.BackColor = prevColor;

            int w = panel1.Width - 1;
            int xOrigin = e.MarginBounds.X + (e.MarginBounds.Width / 2) - (w / 2);
            laidOut.Draw(e.Graphics, xOrigin, temp.Height+5);
        }

        private bool SaveImpl(string fileName)
        {
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName))
                {
                    for (int i = 0; i < entries.Count; ++i)
                    {
                        sw.WriteLine(entries[i].Date.ToShortDateString());
                        for (int j = 0; j < entries[i].Notes.Count; ++j)
                        {
                            sw.WriteLine(entries[i].Notes[j]);
                        }
                        sw.WriteLine("/");
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Save Calendar File";
            saveFileDialog1.Filter = "Text file|*.txt";
            saveFileDialog1.ShowDialog();
            if(saveFileDialog1.FileName == "")
            {
                return;
            }

            if (!SaveImpl(saveFileDialog1.FileName))
            {
                MessageBox.Show("An error occurred when attempting to save " + saveFileDialog1.FileName + ".", "Save");
            }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (SaveImpl(currentlyOpenedFilePath))
            {
                MessageBox.Show("Save completed.", "Save");
            }
            else
            {
                MessageBox.Show("An error occurred when attempting to save " + currentlyOpenedFilePath + ".", "Save");
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            int x = e.X;
            int y = e.Y;

            lastClicked = laidOut.Pick(x, y);

            if (lastClicked == null)
                return;

            editNotesToolStripMenuItem.Text = "Edit (" + lastClicked.Date.ToShortDateString() + ") ...";

            contextMenuStrip1.Show(
                Cursor.Position.X,
                Cursor.Position.Y);
        }

        private void editNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditDialog d = new EditDialog();

            string title = "Add notes (" + lastClicked.Date.ToShortDateString() + ")";
            d.SetTitle(title);

            d.StartPosition = FormStartPosition.CenterParent;

            if (lastClicked.Entry != null)
            {
                d.SetSourceText(lastClicked.Entry.Notes);
            }

            if (d.ShowDialog() == DialogResult.OK)
            {
                if (lastClicked.Entry == null)
                {
                    Entry entry = new Entry();
                    entry.Date = lastClicked.Date;
                    entry.Notes = d.GetModifiedText();
                    entries.Add(entry);
                    laidOut.SetCellSourceEntry(lastClicked.CellIndex, entry);
                }
                else
                {
                    lastClicked.Entry.Notes = d.GetModifiedText();
                }
                panel1.Invalidate();
            }

            lastClicked = null;
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            displayedMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            InitializeMonth();
        }
    }
}
