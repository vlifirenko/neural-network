using System.Reflection;

namespace NeuralNetwork
{
    public partial class EnterData : Form
    {
        private List<TextBox> _inputs = new List<TextBox>();

        public List<TextBox> Inputs => _inputs;

        public EnterData()
        {
            InitializeComponent();

            var propertyInfo = typeof(Patient).GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                var property = propertyInfo[i];
                var textBox = CreateTextBox(i, property);

                Controls.Add(textBox);
                _inputs.Add(textBox);
            }
        }

        public bool? ShowForm()
        {
            var form = new EnterData();
            if (form.ShowDialog() == DialogResult.OK)
            {
                var patient = new Patient();

                foreach (var textBox in form.Inputs)
                {
                    patient.GetType().InvokeMember(textBox.Tag.ToString(),
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                        Type.DefaultBinder, patient, new object[] { textBox.Text });
                }

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
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
