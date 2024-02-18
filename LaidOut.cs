using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Calendar
{
    class LaidOut
    {
        Pen blackPen;
        Brush blackBrush;

        Pen grayPen;
        Brush grayBrush;

        Brush blueBrush;

        Font dayNumberFont;
        Font noteFont;
        Font monthFont;

        string currentMonthName;

        const int monthLabelHeight = 56;


        class DayOfWeekLabelCell
        {
            public string Label;
            public Rectangle Region;
            public float TextLeft;
            public float TextTop;
        }
        DayOfWeekLabelCell[] dayOfWeekLabelCells;

        enum Appearance
        {
            Opaque,
            Semitransparent,
            Hidden
        }

        class DateCell
        {
            public bool Shade;
            public Appearance Appearance;
            public Rectangle Region;
            public string DayNumberString;
            public DateTime Date;
            public Form1.Entry SourceEntry;
        }
        DateCell[] dateCells;

        public void Initialize()
        {
            blackPen = new Pen(Color.Black);
            blackBrush = new SolidBrush(Color.Black);

            grayPen = new Pen(Color.LightGray);
            grayBrush = new SolidBrush(Color.LightGray);

            blueBrush = new SolidBrush(Color.LightBlue);

            FontFamily fontFamily;
            try
            {
                fontFamily = new FontFamily("Segoe UI Emoji");
            }
            catch (System.ArgumentException e)
            {
                // Use fallback font-- Win7 path
                fontFamily = new FontFamily("Arial");
            }

            dayNumberFont = new Font(fontFamily, 16, FontStyle.Regular, GraphicsUnit.Point);
            noteFont = new Font(fontFamily, 10, FontStyle.Regular, GraphicsUnit.Point);
            monthFont = new Font(fontFamily, 36, FontStyle.Bold, GraphicsUnit.Point);

        }

        public void GeneralLayout(
            Graphics graphics,
            int panelWidth,
            int panelHeight)
        {
            int w = panelWidth - 1;
            int h = panelHeight - 1;
            int dayW = w / 7;
            int dayH = (int)((float)h / 7.5f);
            int dayOfWeekLabelW = dayW;
            int dayOfWeekLabelH = dayH / 3;
            dayOfWeekLabelCells = new DayOfWeekLabelCell[7];

            for (int i = 0; i < 7; i++)
            {
                dayOfWeekLabelCells[i] = new DayOfWeekLabelCell();
            }

            dayOfWeekLabelCells[0].Label = "Sunday";
            dayOfWeekLabelCells[1].Label = "Monday";
            dayOfWeekLabelCells[2].Label = "Tuesday";
            dayOfWeekLabelCells[3].Label = "Wednesday";
            dayOfWeekLabelCells[4].Label = "Thursday";
            dayOfWeekLabelCells[5].Label = "Friday";
            dayOfWeekLabelCells[6].Label = "Saturday";

            for (int i = 0; i < 7; i++)
            {
                dayOfWeekLabelCells[i].Region = new Rectangle(
                    i * dayOfWeekLabelW,
                    monthLabelHeight,
                    dayOfWeekLabelW,
                    dayOfWeekLabelH);

                SizeF measured = graphics.MeasureString(dayOfWeekLabelCells[i].Label, dayNumberFont);

                dayOfWeekLabelCells[i].TextLeft = (dayOfWeekLabelW - measured.Width) / 2 + dayOfWeekLabelCells[i].Region.X;
                dayOfWeekLabelCells[i].TextTop = (dayOfWeekLabelH - measured.Height) / 2 + dayOfWeekLabelCells[i].Region.Y;
            }
        }

        enum Iter
        {
            BeforeCurrentMonth,
            InCurrentMonth,
            AfterCurrentMonth,
            Hidden
        }

        public void LayOut(
            int panelWidth,
            int panelHeight,
            DateTime displayedMonth,
            List<Form1.Entry> entries)
        {
            currentMonthName = displayedMonth.ToString("MMMM") + " " + displayedMonth.Year;

            int w = panelWidth - 1;
            int h = panelHeight - 1;
            int dayW = w / 7;
            int dayH = (int)((float)h / 7.5f);
            int dayOfWeekLabelH = dayH / 3;

            dateCells = new DateCell[42];

            DateTime currentDay = displayedMonth;
            int dayOfWeekStartIndexForThisMonth = (int)displayedMonth.DayOfWeek;
            currentDay = currentDay.AddDays(-dayOfWeekStartIndexForThisMonth);

            Iter iter = currentDay.Month == displayedMonth.Month? Iter.InCurrentMonth : Iter.BeforeCurrentMonth;

            int cellIndex = 0;
            for (int yDay = 0; yDay < 6; ++yDay)
            {
                for (int xDay = 0; xDay < 7; ++xDay)
                {
                    dateCells[cellIndex] = new DateCell();
                    dateCells[cellIndex].Region = new Rectangle(
                        xDay * dayW,
                        yDay * dayH + dayOfWeekLabelH + monthLabelHeight,
                        dayW,
                        dayH);

                    dateCells[cellIndex].Shade = currentDay == DateTime.Today;

                    if (iter == Iter.InCurrentMonth)
                    {
                        dateCells[cellIndex].Appearance = Appearance.Opaque;
                    }
                    else if (iter == Iter.BeforeCurrentMonth || iter == Iter.AfterCurrentMonth)
                    {
                        dateCells[cellIndex].Appearance = Appearance.Semitransparent;
                    }
                    else if (iter == Iter.Hidden)
                    {
                        dateCells[cellIndex].Appearance = Appearance.Hidden;
                    }

                    dateCells[cellIndex].DayNumberString = currentDay.Day.ToString();

                    dateCells[cellIndex].SourceEntry = entries.Find(x => (
                        x.Date.Day == currentDay.Day &&
                        x.Date.Month == currentDay.Month &&
                        x.Date.Year == currentDay.Year));

                    dateCells[cellIndex].Date = currentDay;

                    currentDay = currentDay.AddDays(1);
                    cellIndex++;

                    if (iter == Iter.BeforeCurrentMonth && currentDay.Month == displayedMonth.Month)
                    {
                        iter = Iter.InCurrentMonth;
                    }
                    else if (iter == Iter.InCurrentMonth && currentDay.Month != displayedMonth.Month)
                    {
                        iter = Iter.AfterCurrentMonth;
                    }
                    else if (iter == Iter.AfterCurrentMonth && currentDay.DayOfWeek == DayOfWeek.Sunday)
                    {
                        iter = Iter.Hidden;
                    }
                }
            }
        }

        public void AttachNotes(List<Form1.Entry> entries)
        {
            int cellIndex = 0;
            for (int yDay = 0; yDay < 6; ++yDay)
            {
                for (int xDay = 0; xDay < 7; ++xDay)
                {
                    DateTime currentDay = dateCells[cellIndex].Date;

                    dateCells[cellIndex].SourceEntry = entries.Find(x => (
                        x.Date.Day == currentDay.Day &&
                        x.Date.Month == currentDay.Month &&
                        x.Date.Year == currentDay.Year));

                    cellIndex++;
                }
            }
        }

        public void Draw(Graphics graphics, int panelWidth, int panelHeight)
        {
            SizeF monthDimensions = graphics.MeasureString(currentMonthName, monthFont);
            PointF monthTextLocation = new PointF((panelWidth - monthDimensions.Width) / 2, -10);
            graphics.DrawString(currentMonthName, monthFont, blackBrush, monthTextLocation);

            // Draw day-of-week labels
            for (int i = 0; i < 7; i++)
            {
                graphics.DrawRectangle(blackPen, dayOfWeekLabelCells[i].Region);
                graphics.DrawString(
                    dayOfWeekLabelCells[i].Label,
                    dayNumberFont,
                    blackBrush,
                    dayOfWeekLabelCells[i].TextLeft,
                    dayOfWeekLabelCells[i].TextTop);
            }

            // Draw calendar days
            for (int i = 0; i < 42; i++)
            {
                Brush brush = null;
                if (dateCells[i].Appearance == Appearance.Opaque)
                {
                    if (dateCells[i].Shade)
                    {
                        graphics.FillRectangle(blueBrush, dateCells[i].Region);
                    }

                    brush = blackBrush;
                    graphics.DrawRectangle(blackPen, dateCells[i].Region);
                }
                else if (dateCells[i].Appearance == Appearance.Semitransparent)
                {
                    brush = grayBrush;
                }
                else if (dateCells[i].Appearance == Appearance.Hidden)
                {
                    continue;
                }

                graphics.DrawString(
                    dateCells[i].DayNumberString,
                    dayNumberFont,
                    brush,
                    dateCells[i].Region.X,
                    dateCells[i].Region.Y);

                if (dateCells[i].SourceEntry != null)
                {
                    for (int j = 0; j < dateCells[i].SourceEntry.Notes.Count; ++j)
                    {
                        graphics.DrawString(
                            dateCells[i].SourceEntry.Notes[j],
                            noteFont,
                            brush,
                            dateCells[i].Region.X + 5,
                            dateCells[i].Region.Y + 30 + (20 * j));
                    }
                }
            }

            graphics.ResetTransform();
        }

        public class PickResult
        {
            public Form1.Entry Entry;
            public DateTime Date;
            public int CellIndex;
        }

        public PickResult Pick(int x, int y)
        {
            for (int i=0; i<42; ++i)
            {
                if (dateCells[i].Region.Contains(x, y) && dateCells[i].Appearance != Appearance.Hidden)
                {
                    PickResult result = new PickResult();
                    result.Entry = dateCells[i].SourceEntry;
                    result.Date = dateCells[i].Date;
                    result.CellIndex = i;
                    return result;
                }
            }
            return null;
        }

        public void SetCellSourceEntry(int cellIndex, Form1.Entry entry)
        {
            dateCells[cellIndex].SourceEntry = entry;
        }
    }
}
