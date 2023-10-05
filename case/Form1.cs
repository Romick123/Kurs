using System.Windows.Forms;

namespace work
{
    public partial class Транслятор : Form
    {
        public Транслятор()
        {
            InitializeComponent();
        }
        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Файлы txt (*.txt)|*.txt";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader rdr = new StreamReader(fileDialog.FileName);
                string line = rdr.ReadToEnd();
                rdr.Close();
                richTextBox1.Text = line;
                
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Token[] tokens = generation.Tokens(generation.Split(richTextBox1.Text));
            listBox1.Items.Clear();
            dataGridView1.Rows.Clear();
            foreach(Token token in tokens)
            {
                listBox1.Items.Add(token.ToString());
            }
            try {
                LR a = new LR(tokens);
                a.Check();
                for(int i = 0; i < a.Troyka.Count; i++)
                {
                    dataGridView1.Rows.Add(a.Troyka[i].operand1.Value, a.Troyka[i].operat, a.Troyka[i].operand2.Value, "M" + (i + 1));
                }
                MessageBox.Show("Проверено успешно", "Результат проверки");
            } catch(Exception E) { MessageBox.Show(E.Message, "Результат проверки"); }

        }
    }
}