using System.Globalization;
using System.Windows.Forms;

namespace HabbitsForm;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new DailyExpenseForm());
    }
}

public class DailyExpenseForm : Form
{
    private readonly TextBox dailyExpenseTextBox = new();
    private readonly TextBox habitNameTextBox = new();
    private readonly Button calculateButton = new();
    private readonly Label monthlyLabel = new();
    private readonly Label yearlyLabel = new();
    private readonly Label fiveYearsLabel = new();
    private readonly Label comparisonLabel = new();

    public DailyExpenseForm()
    {
        InitializeForm();
        Calculate();
    }

    private void InitializeForm()
    {
        Text = "Калькулятор щоденних витрат";
        Width = 700;
        Height = 500;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = Color.White;

        var titleLabel = new Label
        {
            Text = "Щоденні витрати",
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 20)
        };

        var habitLabel = new Label
        {
            Text = "Назва витрати:",
            AutoSize = true,
            Font = new Font("Segoe UI", 10),
            Location = new Point(20, 80)
        };

        habitNameTextBox.Location = new Point(180, 76);
        habitNameTextBox.Size = new Size(220, 28);
        habitNameTextBox.Font = new Font("Segoe UI", 10);
        habitNameTextBox.Text = "Кава";

        var expenseLabel = new Label
        {
            Text = "Вартість за день:",
            AutoSize = true,
            Font = new Font("Segoe UI", 10),
            Location = new Point(20, 120)
        };

        dailyExpenseTextBox.Location = new Point(180, 116);
        dailyExpenseTextBox.Size = new Size(220, 28);
        dailyExpenseTextBox.Font = new Font("Segoe UI", 10);
        dailyExpenseTextBox.Text = "45";
        dailyExpenseTextBox.TextAlign = HorizontalAlignment.Right;
        dailyExpenseTextBox.KeyPress += OnlyAllowNumbers;

        calculateButton.Text = "Розрахувати";
        calculateButton.Location = new Point(430, 90);
        calculateButton.Size = new Size(190, 40);
        calculateButton.BackColor = Color.FromArgb(70, 130, 180);
        calculateButton.ForeColor = Color.White;
        calculateButton.FlatStyle = FlatStyle.Flat;
        calculateButton.Click += (_, _) => Calculate();

        monthlyLabel.Location = new Point(20, 180);
        monthlyLabel.AutoSize = true;
        monthlyLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);

        yearlyLabel.Location = new Point(20, 220);
        yearlyLabel.AutoSize = true;
        yearlyLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);

        fiveYearsLabel.Location = new Point(20, 260);
        fiveYearsLabel.AutoSize = true;
        fiveYearsLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);

        comparisonLabel.Location = new Point(20, 310);
        comparisonLabel.AutoSize = true;
        comparisonLabel.Font = new Font("Segoe UI", 11, FontStyle.Regular);
        comparisonLabel.MaximumSize = new Size(620, 120);

        Controls.Add(titleLabel);
        Controls.Add(habitLabel);
        Controls.Add(habitNameTextBox);
        Controls.Add(expenseLabel);
        Controls.Add(dailyExpenseTextBox);
        Controls.Add(calculateButton);
        Controls.Add(monthlyLabel);
        Controls.Add(yearlyLabel);
        Controls.Add(fiveYearsLabel);
        Controls.Add(comparisonLabel);
    }

    private void Calculate()
    {
        if (!decimal.TryParse(dailyExpenseTextBox.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var dailyExpense) || dailyExpense < 0)
        {
            MessageBox.Show("Введіть коректну суму витрат за день.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            dailyExpenseTextBox.Focus();
            return;
        }

        var monthDays = 30m;
        var monthTotal = dailyExpense * monthDays;
        var yearTotal = dailyExpense * 365m;
        var fiveYearsTotal = yearTotal * 5m;

        var name = string.IsNullOrWhiteSpace(habitNameTextBox.Text) ? "Ця витрата" : habitNameTextBox.Text.Trim();

        monthlyLabel.Text = $"За місяць: {monthTotal:C}";
        yearlyLabel.Text = $"За рік: {yearTotal:C}";
        fiveYearsLabel.Text = $"За 5 років: {fiveYearsTotal:C}";

        var purchases = new[]
        {
            new { Name = "телефон", Price = 12000m },
            new { Name = "відпустка", Price = 35000m },
            new { Name = "ноутбук", Price = 25000m },
            new { Name = "планшет", Price = 8000m },
            new { Name = "похід до туризму", Price = 20000m }
        };

        var buyList = new List<string>();
        foreach (var item in purchases)
        {
            var count = Math.Floor(fiveYearsTotal / item.Price);
            if (count > 0)
            {
                buyList.Add($"{count} {item.Name}{(count > 1 ? "ів" : "")}");
            }
        }

        if (buyList.Count == 0)
        {
            comparisonLabel.Text = $"За ці гроші можна купити лише дуже маленькі речі, а не {purchases[0].Name} або {purchases[1].Name}.";
        }
        else
        {
            comparisonLabel.Text = $"За ці гроші можна купити: {string.Join(", ", buyList)}.";
        }

        comparisonLabel.Text += $"\n{name} за 5 років обійдеться в {fiveYearsTotal:C}.";
    }

    private static void OnlyAllowNumbers(object? sender, KeyPressEventArgs e)
    {
        var allowedChars = "0123456789,.";

        if (!char.IsControl(e.KeyChar) && !allowedChars.Contains(e.KeyChar))
        {
            e.Handled = true;
        }
    }
}
