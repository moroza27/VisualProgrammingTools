using System;
using System.Windows.Forms;

namespace AverageGradeCalculator
{
    public class Form1 : Form
    {
        DataGridView dataGridView;
        Button addButton;
        Button calculateButton;
        Label resultLabel;

        public Form1()
        {
            Text = "Калькулятор середнього балу";
            Width = 600;
            Height = 450;

            dataGridView = new DataGridView();
            dataGridView.Left = 20;
            dataGridView.Top = 20;
            dataGridView.Width = 540;
            dataGridView.Height = 280;

            dataGridView.ColumnCount = 2;
            dataGridView.Columns[0].Name = "Предмет";
            dataGridView.Columns[1].Name = "Оцінка";

            addButton = new Button();
            addButton.Text = "Додати предмет";
            addButton.Left = 20;
            addButton.Top = 320;
            addButton.Width = 150;
            addButton.Click += AddButton_Click;

            calculateButton = new Button();
            calculateButton.Text = "Обчислити середній бал";
            calculateButton.Left = 190;
            calculateButton.Top = 320;
            calculateButton.Width = 180;
            calculateButton.Click += CalculateButton_Click;

            resultLabel = new Label();
            resultLabel.Text = "Середній бал: -";
            resultLabel.Left = 20;
            resultLabel.Top = 370;
            resultLabel.Width = 300;

            Controls.Add(dataGridView);
            Controls.Add(addButton);
            Controls.Add(calculateButton);
            Controls.Add(resultLabel);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Add();
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            double sum = 0;
            int count = 0;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.IsNewRow)
                    continue;

                if (double.TryParse(row.Cells[1].Value?.ToString(), out double grade))
                {
                    sum += grade;
                    count++;
                }
            }

            if (count > 0)
            {
                double average = sum / count;
                resultLabel.Text = "Середній бал: " + average.ToString("0.00");
            }
            else
            {
                resultLabel.Text = "Введіть хоча б одну оцінку";
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}