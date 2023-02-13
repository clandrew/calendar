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
    public partial class EditCustomRemoteCommandsDialog : Form
    {

        public EditCustomRemoteCommandsDialog()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        public string SaveCommandText;
        public string LoadCommandText;

        private void EditCustomRemoteCommandsDialog_Load(object sender, EventArgs e)
        {
            this.customRemoteSaveCommandEditBox.Text = SaveCommandText;
            this.customRemoteLoadCommandEditBox.Text = LoadCommandText;
        }

        private void okbutton_Click(object sender, EventArgs e)
        {
            SaveCommandText = customRemoteSaveCommandEditBox.Text;
            LoadCommandText = customRemoteLoadCommandEditBox.Text;
            this.Close();
            this.DialogResult = DialogResult.OK;
        }
    }
}
