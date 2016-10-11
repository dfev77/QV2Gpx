using System;
using System.Windows.Forms;

namespace QV2Gpx
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public RichTextBox LoggingControl
        {
            get { return txtLog; }
        }

        private void btnInputPicker_Click(object sender, EventArgs e)
        {
            filePicker.Filter = "Quo Vadis|" + Processor.FolderProcessor.QuoVadisExtension;
            filePicker.CheckFileExists = true;
            filePicker.CheckPathExists = true;

            if (DialogResult.OK == filePicker.ShowDialog(this))
            {
                txtInput.Text = filePicker.FileName;
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            CliArguments args = new CliArguments()
            {
                InputPath = txtInput.Text,
                Command = Commands.Export
            };

            Program.Process(args);
        }
    }
}
