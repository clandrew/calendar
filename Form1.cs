using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace Calendar
{
    public partial class Form1 : Form
    {
        DateTime displayedMonth;
        int dayOfWeekStartIndexForThisMonth;
        int daysThisMonth;

        LaidOut laidOut;

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

            string[] lines = System.IO.File.ReadAllLines(@"..\..\Data.txt");
            for (int i=0; i<lines.Length; ++i)
            {
                Entry e = new Entry();

                DateTime convertedDate = DateTime.Parse(lines[i]);
                e.Date = convertedDate;
                e.Notes = new List<string>();
                ++i;
                while (lines[i][0] != '/')
                {
                    e.Notes.Add(lines[i]);
                    ++i;
                }

                entries.Add(e);
            }

            laidOut = new LaidOut();
            laidOut.Initialize();
            laidOut.GeneralLayout(panel1.CreateGraphics(), panel1.Width, panel1.Height);
            displayedMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            InitializeMonth();
        }

        void InitializeMonth()
        {
            dayOfWeekStartIndexForThisMonth = (int)displayedMonth.DayOfWeek;
            daysThisMonth = DateTime.DaysInMonth(displayedMonth.Year, displayedMonth.Month);
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

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Save Calendar File";
            saveFileDialog1.Filter = "Text file|*.txt";
            saveFileDialog1.ShowDialog();
            if(saveFileDialog1.FileName == "")
            {
                return;
            }

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.FileName))
            {
                for (int i=0; i<entries.Count; ++i)
                {
                    sw.WriteLine(entries[i].Date.ToShortDateString());
                    for (int j=0; j<entries[i].Notes.Count; ++j)
                    {
                        sw.WriteLine(entries[i].Notes[j]);
                    }
                    sw.WriteLine("/");
                }
            }
        }

        LaidOut.PickResult lastClicked;

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            int x = e.X;
            int y = e.Y;

            lastClicked = laidOut.Pick(x, y);

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
    }
}
