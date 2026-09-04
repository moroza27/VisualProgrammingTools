using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace hw1;

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
    private readonly TextBox waterIndexTextBox;
    private readonly TextBox lightIndexTextBox;
    private readonly TextBox gasIndexTextBox;
    private readonly TextBox waterTariffTextBox;
    private readonly TextBox lightTariffTextBox;
    private readonly TextBox gasTariffTextBox;
    private readonly Button sumButton;
    private readonly DataGridView resultGrid;

    public MainForm()
    {
        Text = "Тарифи";
        Size = new Size(620, 360);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        var titleIndex = new Label
        {
            Text = "Показники",
            Location = new Point(80, 20),
            AutoSize = true,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold)
        };

        var titleTariff = new Label
        {
            Text = "Тарифи",
            Location = new Point(300, 20),
            AutoSize = true,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold)
        };

        var waterLabel = new Label { Text = "Вода", Location = new Point(30, 60), AutoSize = true, Font = new Font("Segoe UI", 10F) };
        var lightLabel = new Label { Text = "Світло", Location = new Point(30, 100), AutoSize = true, Font = new Font("Segoe UI", 10F) };
        var gasLabel = new Label { Text = "Газ", Location = new Point(30, 140), AutoSize = true, Font = new Font("Segoe UI", 10F) };

        waterIndexTextBox = new TextBox { Location = new Point(110, 55), Width = 140, Font = new Font("Segoe UI", 10F) };
        lightIndexTextBox = new TextBox { Location = new Point(110, 95), Width = 140, Font = new Font("Segoe UI", 10F) };
        gasIndexTextBox = new TextBox { Location = new Point(110, 135), Width = 140, Font = new Font("Segoe UI", 10F) };

        waterTariffTextBox = new TextBox { Location = new Point(280, 55), Width = 140, Font = new Font("Segoe UI", 10F) };
        lightTariffTextBox = new TextBox { Location = new Point(280, 95), Width = 140, Font = new Font("Segoe UI", 10F) };
        gasTariffTextBox = new TextBox { Location = new Point(280, 135), Width = 140, Font = new Font("Segoe UI", 10F) };

        sumButton = new Button
        {
            Text = "Підрахунок",
            Location = new Point(200, 180),
            Width = 120,
            Height = 35,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold)
        };

        resultGrid = new DataGridView
        {
            Location = new Point(20, 230),
            Width = 560,
            Height = 80,
            ReadOnly = true,
            AllowUserToAddRows = false,
            RowHeadersVisible = false,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = SystemColors.Control
        };

        resultGrid.Columns.Add("Service", "Послуга");
        resultGrid.Columns.Add("Indicator", "Показник");
        resultGrid.Columns.Add("Tariff", "Тариф");
        resultGrid.Columns.Add("Result", "Результат");

        sumButton.Click += SumButton_Click;

        Controls.Add(titleIndex);
        Controls.Add(titleTariff);
        Controls.Add(waterLabel);
        Controls.Add(lightLabel);
        Controls.Add(gasLabel);
        Controls.Add(waterIndexTextBox);
        Controls.Add(lightIndexTextBox);
        Controls.Add(gasIndexTextBox);
        Controls.Add(waterTariffTextBox);
        Controls.Add(lightTariffTextBox);
        Controls.Add(gasTariffTextBox);
        Controls.Add(sumButton);
        Controls.Add(resultGrid);
    }

    private void SumButton_Click(object? sender, EventArgs e)
    {
        try
        {
            double waterIndex = ParseNumber(waterIndexTextBox.Text);
            double waterTariff = ParseNumber(waterTariffTextBox.Text);

            double lightIndex = ParseNumber(lightIndexTextBox.Text);
            double lightTariff = ParseNumber(lightTariffTextBox.Text);

            double gasIndex = ParseNumber(gasIndexTextBox.Text);
            double gasTariff = ParseNumber(gasTariffTextBox.Text);

            double waterTotal = waterIndex * waterTariff;
            double lightTotal = lightIndex * lightTariff;
            double gasTotal = gasIndex * gasTariff;
            double sum = waterTotal + lightTotal + gasTotal;

            resultGrid.Rows.Clear();
            resultGrid.Rows.Add("Вода", waterIndex, waterTariff, waterTotal);
            resultGrid.Rows.Add("Світло", lightIndex, lightTariff, lightTotal);
            resultGrid.Rows.Add("Газ", gasIndex, gasTariff, gasTotal);
            resultGrid.Rows.Add("Разом", "", "", sum);
        }
        catch
        {
            resultGrid.Rows.Clear();
            resultGrid.Rows.Add("Помилка", "", "", "введіть коректні числа");
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
