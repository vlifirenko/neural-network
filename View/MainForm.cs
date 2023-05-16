using NeuralNetwork.View;

namespace NeuralNetwork
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutForm = new AboutBox1();
            aboutForm.ShowDialog();
        }

        private void imageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var pictureConverter = new PictureConverter();
                var inputs = pictureConverter.Convert(openFileDialog1.FileName);
                var result = Program.Controller.ImageNetwork.Predict(inputs).Output;
            }
        }

        private void inputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var enterDataForm = new EnterData();
            var result = enterDataForm.ShowForm();
        }
    }
}