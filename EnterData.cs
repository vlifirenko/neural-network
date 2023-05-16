using System.Reflection;

namespace NeuralNetwork
{
    public partial class EnterData : Form
    {
        public EnterData()
        {
            InitializeComponent();

            var propertyInfo = typeof(Patient).GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                var property = propertyInfo[i];
                var textBox = CreateTextBox(i, property);

                Controls.Add(textBox);
            }
        }

        public bool? ShowForm()
        {
            var form = new EnterData();
            if (form.ShowDialog() == DialogResult.OK)
            {
                var patient = new Patient();
                var result = Program.Controller.DataNetwork.Predict().Output;

                return result == 1.0;
            }

            return null;
        }

        private void EnterData_Load(object sender, EventArgs e)
        {

        }

        private TextBox CreateTextBox(int number, PropertyInfo property)
        {
            var y = 45 + number * 27;
            var textBox = new TextBox
            {
                Location = new Point(16, y),
                Name = "textBox" + number,
                Size = new Size(322, 23),
                TabIndex = number,
                Text = property.Name,
                Tag = property.Name
            };

            textBox.GotFocus += TextBox_GotFocus;
            textBox.LostFocus += TextBox_LostFocus;

            return textBox;
        }

        private void TextBox_GotFocus(object? sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == textBox.Tag.ToString())
                textBox.Text = "";
        }

        private void TextBox_LostFocus(object? sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "")
                textBox.Text = textBox.Tag.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
