using System;
using System.Drawing;
using System.Windows.Forms;

namespace SelectForm;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new TariffSelectorForm());
    }
}

public class TariffSelectorForm : Form
{
    private readonly NumericUpDown internetUpDown;
    private readonly NumericUpDown callsUpDown;
    private readonly NumericUpDown smsUpDown;
    private readonly Button selectButton;
    private readonly Label resultLabel;

    public TariffSelectorForm()
    {
        Text = "Вибір тарифного плану";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(560, 360);
        MinimumSize = new Size(500, 300);
        BackColor = Color.FromArgb(245, 248, 252);

        var titleLabel = new Label
        {
            Text = "Підберіть найвигідніший тариф",
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 20)
        };

        var internetLabel = new Label
        {
            Text = "Обсяг інтернету (ГБ):",
            AutoSize = true,
            Location = new Point(20, 80),
            Font = new Font("Segoe UI", 10)
        };

        internetUpDown = new NumericUpDown
        {
            Minimum = 0,
            Maximum = 500,
            Value = 10,
            DecimalPlaces = 0,
            Location = new Point(220, 75),
            Width = 180
        };

        var callsLabel = new Label
        {
            Text = "Хвилини дзвінків:",
            AutoSize = true,
            Location = new Point(20, 120),
            Font = new Font("Segoe UI", 10)
        };

        callsUpDown = new NumericUpDown
        {
            Minimum = 0,
            Maximum = 5000,
            Value = 200,
            DecimalPlaces = 0,
            Location = new Point(220, 115),
            Width = 180
        };

        var smsLabel = new Label
        {
            Text = "Кількість SMS:",
            AutoSize = true,
            Location = new Point(20, 160),
            Font = new Font("Segoe UI", 10)
        };

        smsUpDown = new NumericUpDown
        {
            Minimum = 0,
            Maximum = 5000,
            Value = 50,
            DecimalPlaces = 0,
            Location = new Point(220, 155),
            Width = 180
        };

        selectButton = new Button
        {
            Text = "Порахувати",
            Location = new Point(20, 210),
            Width = 140,
            Height = 35,
            BackColor = Color.FromArgb(34, 139, 230),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        selectButton.FlatAppearance.BorderSize = 0;
        selectButton.Click += SelectButton_Click;

        resultLabel = new Label
        {
            Text = "Рекомендований тариф: ...",
            AutoSize = true,
            Location = new Point(20, 260),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            ForeColor = Color.FromArgb(22, 101, 52)
        };

        Controls.Add(titleLabel);
        Controls.Add(internetLabel);
        Controls.Add(internetUpDown);
        Controls.Add(callsLabel);
        Controls.Add(callsUpDown);
        Controls.Add(smsLabel);
        Controls.Add(smsUpDown);
        Controls.Add(selectButton);
        Controls.Add(resultLabel);
    }

    private void SelectButton_Click(object? sender, EventArgs e)
    {
        int internet = (int)internetUpDown.Value;
        int calls = (int)callsUpDown.Value;
        int sms = (int)smsUpDown.Value;

        string bestTariff = "Тариф 1: Basic";

        if (internet <= 10 && calls <= 300 && sms <= 100)
        {
            bestTariff = "Тариф 1: Basic";
        }
        else if (internet <= 20 && calls <= 800 && sms <= 500)
        {
            bestTariff = "Тариф 2: Standard";
        }
        else if (internet <= 40 && calls <= 1500 && sms <= 1000)
        {
            bestTariff = "Тариф 3: Plus";
        }
        else
        {
            bestTariff = "Тариф 4: Unlimited";
        }

        resultLabel.Text = $"Рекомендований тариф: {bestTariff}";
    }
}
