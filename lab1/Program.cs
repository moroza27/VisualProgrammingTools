using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace lab1;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}

public class MainForm : Form
{
    private readonly TextBox number1TextBox;
    private readonly TextBox number2TextBox;
    private readonly TextBox number3TextBox;
    private readonly Button resultButton;
    private readonly Label resultLabel;

    public MainForm()
    {
        Text = "Результати";
        Size = new Size(420, 260);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        var label1 = new Label
        {
            Text = "Число 1:",
            Location = new Point(20, 25),
            AutoSize = true,
            Font = new Font("Segoe UI", 10F)
        };

        var label2 = new Label
        {
            Text = "Число 2:",
            Location = new Point(20, 70),
            AutoSize = true,
            Font = new Font("Segoe UI", 10F)
        };

        var label3 = new Label
        {
            Text = "Число 3:",
            Location = new Point(20, 115),
            AutoSize = true,
            Font = new Font("Segoe UI", 10F)
        };

        number1TextBox = new TextBox
        {
            Location = new Point(120, 20),
            Width = 240,
            Font = new Font("Segoe UI", 10F)
        };

        number2TextBox = new TextBox
        {
            Location = new Point(120, 65),
            Width = 240,
            Font = new Font("Segoe UI", 10F)
        };

        number3TextBox = new TextBox
        {
            Location = new Point(120, 110),
            Width = 240,
            Font = new Font("Segoe UI", 10F)
        };

        resultButton = new Button
        {
            Text = "Результати",
            Location = new Point(120, 155),
            Width = 120,
            Height = 35,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold)
        };

        resultLabel = new Label
        {
            Text = "Результат: ",
            Location = new Point(20, 200),
            AutoSize = true,
            Font = new Font("Segoe UI", 10F)
        };

        resultButton.Click += ResultButton_Click;

        Controls.Add(label1);
        Controls.Add(label2);
        Controls.Add(label3);
        Controls.Add(number1TextBox);
        Controls.Add(number2TextBox);
        Controls.Add(number3TextBox);
        Controls.Add(resultButton);
        Controls.Add(resultLabel);
    }

    private void ResultButton_Click(object? sender, EventArgs e)
    {
        try
        {
            double a = ParseNumber(number1TextBox.Text);
            double b = ParseNumber(number2TextBox.Text);
            double c = ParseNumber(number3TextBox.Text);

            double sum = a + b + c;
            double difference = a - b - c;
            double product = a * b * c;

            resultLabel.Text = $"Результат: a+b+c={sum}; a-b-c={difference}; a*b*c={product}";
        }
        catch
        {
            resultLabel.Text = "Результат: введіть коректні числа";
        }
    }

    private static double ParseNumber(string value)
    {
        if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double number))
            return number;

        if (double.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out number))
            return number;

        throw new FormatException("Invalid number");
    }
}
