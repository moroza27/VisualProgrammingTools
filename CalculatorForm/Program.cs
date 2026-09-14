using System;
using System.Drawing;
using System.Windows.Forms;

namespace CalculatorForm;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new CarbonFootprintForm());
    }
}

public class CarbonFootprintForm : Form
{
    private readonly TextBox carTripsTextBox;
    private readonly TextBox applianceHoursTextBox;
    private readonly Button calculateButton;
    private readonly Label resultLabel;
    private readonly Label formulaLabel;

    public CarbonFootprintForm()
    {
        Text = "Калькулятор CO₂";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(620, 300);
        MinimumSize = new Size(520, 260);
        BackColor = Color.FromArgb(245, 248, 252);

        var titleLabel = new Label
        {
            Text = "Оцінка викидів CO₂",
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 20)
        };

        var carTripsLabel = new Label
        {
            Text = "Кількість поїздок на авто:",
            AutoSize = true,
            Location = new Point(20, 70),
            Size = new Size(210, 25),
            Font = new Font("Segoe UI", 10, FontStyle.Regular)
        };

        carTripsTextBox = new TextBox
        {
            Location = new Point(270, 67),
            Width = 230,
            Text = "0"
        };

        var applianceHoursLabel = new Label
        {
            Text = "Годин використання електроприладів:",
            AutoSize = true,
            Location = new Point(20, 110),
            Size = new Size(210, 25),
            Font = new Font("Segoe UI", 10, FontStyle.Regular)
        };

        applianceHoursTextBox = new TextBox
        {
            Location = new Point(270, 107),
            Width = 230,
            Text = "0"
        };

        calculateButton = new Button
        {
            Text = "Розрахувати",
            Location = new Point(20, 155),
            Width = 140,
            Height = 35,
            BackColor = Color.FromArgb(34, 139, 230),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        calculateButton.FlatAppearance.BorderSize = 0;
        calculateButton.Click += CalculateButton_Click;

        resultLabel = new Label
        {
            Text = "Приблизно: 0.00 кг CO₂",
            AutoSize = true,
            Location = new Point(20, 205),
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = Color.FromArgb(22, 101, 52)
        };

        formulaLabel = new Label
        {
            Text = "Розрахунок: 0 поїздок × 4.50 + 0 год × 0.35",
            AutoSize = true,
            Location = new Point(20, 235),
            ForeColor = Color.FromArgb(60, 60, 60)
        };

        Controls.Add(titleLabel);
        Controls.Add(carTripsLabel);
        Controls.Add(carTripsTextBox);
        Controls.Add(applianceHoursLabel);
        Controls.Add(applianceHoursTextBox);
        Controls.Add(calculateButton);
        Controls.Add(resultLabel);
        Controls.Add(formulaLabel);
    }

    private void CalculateButton_Click(object? sender, EventArgs e)
    {
        double carTrips;
        double applianceHours;

        if (!double.TryParse(carTripsTextBox.Text, out carTrips) || !double.TryParse(applianceHoursTextBox.Text, out applianceHours))
        {
            resultLabel.Text = "Помилка: введіть число";
            resultLabel.ForeColor = Color.DarkRed;
            formulaLabel.Text = "Перевірте правильність введених значень.";
            return;
        }

        if (carTrips < 0 || applianceHours < 0)
        {
            resultLabel.Text = "Помилка: значення не можуть бути від'ємними";
            resultLabel.ForeColor = Color.DarkRed;
            formulaLabel.Text = "Введіть додатні числа.";
            return;
        }

        const double carTripFactor = 4.5;
        const double applianceFactor = 0.35;

        double totalCo2 = carTrips * carTripFactor + applianceHours * applianceFactor;

        resultLabel.Text = $"Приблизно: {totalCo2:F2} кг CO₂";
        resultLabel.ForeColor = Color.FromArgb(22, 101, 52);
        formulaLabel.Text = $"Розрахунок: {carTrips:F0} поїздок × {carTripFactor:F2} + {applianceHours:F1} год × {applianceFactor:F2} = {totalCo2:F2} кг";
    }
}
