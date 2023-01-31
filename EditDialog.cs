using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calendar
{
    public partial class EditDialog : Form
    {
        public string[] TextLines;

        public EditDialog()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        public void SetTitle(string title)
        {
            this.Text = title;
        }

        public void SetSourceText(List<string> sourceText)
        {
            StringBuilder s = new StringBuilder();
            for (int i=0; i<sourceText.Count; ++i)
            {
                s.AppendLine(sourceText[i]);
            }
            textBox1.Text = s.ToString();
        }

        public List<string> GetModifiedText()
        {
            char[] separator = new char[] { '\r', '\n' };
            string[] lines = textBox1.Text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            List<string> result = new List<string>();
            for (int i=0; i<lines.Length; ++i)
            {
                result.Add(lines[i]);
            }
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.OK;
        }
    }
}
