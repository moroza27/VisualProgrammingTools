using System.Globalization;
using System.Windows.Forms;

namespace ExpenseTracker;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new SubscriptionTrackerForm());
    }
}

public class SubscriptionTrackerForm : Form
{
    private readonly TextBox salaryTextBox = new();
    private readonly ComboBox nameComboBox = new();
    private readonly TextBox priceTextBox = new();
    private readonly Button addButton = new();
    private readonly ListBox subscriptionsListBox = new();
    private readonly Label totalLabel = new();
    private readonly Label remainingLabel = new();

    private decimal totalMonthlySubscriptions;

    public SubscriptionTrackerForm()
    {
        InitializeForm();
        RefreshSummary();
    }

    private void InitializeForm()
    {
        Text = "Трекер витрат на підписки";
        Width = 620;
        Height = 500;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = Color.White;

        var titleLabel = new Label
        {
            Text = "Трекер витрат на підписки",
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 20)
        };

        var salaryLabel = new Label
        {
            Text = "Місячна зарплата:",
            AutoSize = true,
            Font = new Font("Segoe UI", 10, FontStyle.Regular),
            Location = new Point(20, 70)
        };

        salaryTextBox.Location = new Point(180, 66);
        salaryTextBox.Size = new Size(180, 27);
        salaryTextBox.Font = new Font("Segoe UI", 10, FontStyle.Regular);
        salaryTextBox.Text = "0";
        salaryTextBox.TextAlign = HorizontalAlignment.Right;
        salaryTextBox.KeyPress += OnlyAllowNumbers;
        salaryTextBox.TextChanged += (_, _) => RefreshSummary();

        var nameLabel = new Label
        {
            Text = "Назва підписки:",
            AutoSize = true,
            Font = new Font("Segoe UI", 10, FontStyle.Regular),
            Location = new Point(20, 110)
        };

        nameComboBox.Location = new Point(180, 106);
        nameComboBox.Size = new Size(180, 27);
        nameComboBox.Font = new Font("Segoe UI", 10, FontStyle.Regular);
        nameComboBox.DropDownStyle = ComboBoxStyle.DropDown;
        nameComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        nameComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
        nameComboBox.Items.AddRange(new object[]
        {
            "Netflix",
            "Spotify",
            "YouTube Premium",
            "Apple Music",
            "Microsoft 365",
            "Adobe Creative Cloud",
            "Disney+",
            "Prime Video",
            "Dropbox",
            "Steam",
            "Xbox Game Pass",
            "PlayStation Plus"
        });

        var priceLabel = new Label
        {
            Text = "Ціна:",
            AutoSize = true,
            Font = new Font("Segoe UI", 10, FontStyle.Regular),
            Location = new Point(20, 150)
        };

        priceTextBox.Location = new Point(180, 146);
        priceTextBox.Size = new Size(180, 27);
        priceTextBox.Font = new Font("Segoe UI", 10, FontStyle.Regular);
        priceTextBox.TextAlign = HorizontalAlignment.Right;
        priceTextBox.KeyPress += OnlyAllowNumbers;

        addButton.Text = "Додати підписку";
        addButton.Size = new Size(160, 35);
        addButton.Location = new Point(390, 120);
        addButton.BackColor = Color.FromArgb(60, 120, 216);
        addButton.ForeColor = Color.White;
        addButton.FlatStyle = FlatStyle.Flat;
        addButton.Click += AddSubscription;

        var listTitle = new Label
        {
            Text = "Підписки:",
            AutoSize = true,
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            Location = new Point(20, 195)
        };

        subscriptionsListBox.Location = new Point(20, 220);
        subscriptionsListBox.Size = new Size(560, 180);
        subscriptionsListBox.Font = new Font("Segoe UI", 10, FontStyle.Regular);
        subscriptionsListBox.BorderStyle = BorderStyle.FixedSingle;

        totalLabel.Location = new Point(20, 415);
        totalLabel.AutoSize = true;
        totalLabel.Font = new Font("Segoe UI", 11, FontStyle.Bold);

        remainingLabel.Location = new Point(320, 415);
        remainingLabel.AutoSize = true;
        remainingLabel.Font = new Font("Segoe UI", 11, FontStyle.Bold);

        Controls.Add(titleLabel);
        Controls.Add(salaryLabel);
        Controls.Add(salaryTextBox);
        Controls.Add(nameLabel);
        Controls.Add(nameComboBox);
        Controls.Add(priceLabel);
        Controls.Add(priceTextBox);
        Controls.Add(addButton);
        Controls.Add(listTitle);
        Controls.Add(subscriptionsListBox);
        Controls.Add(totalLabel);
        Controls.Add(remainingLabel);
    }

    private void AddSubscription(object? sender, EventArgs e)
    {
        if (!TryReadSalary(out var salary))
        {
            MessageBox.Show("Введіть коректну місячну зарплату.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            salaryTextBox.Focus();
            return;
        }

        var subscriptionName = nameComboBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(subscriptionName))
        {
            MessageBox.Show("Введіть назву підписки.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            nameComboBox.Focus();
            return;
        }

        if (!decimal.TryParse(priceTextBox.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var price) || price < 0)
        {
            MessageBox.Show("Введіть коректну ціну підписки.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            priceTextBox.Focus();
            return;
        }

        totalMonthlySubscriptions += price;
        subscriptionsListBox.Items.Add($"{subscriptionName} — {price:C}");
        nameComboBox.Text = string.Empty;
        priceTextBox.Clear();
        nameComboBox.Focus();

        RefreshSummary();
    }

    private void RefreshSummary()
    {
        var salary = TryReadSalary(out var parsedSalary) ? parsedSalary : 0m;

        var remaining = salary - totalMonthlySubscriptions;

        totalLabel.Text = $"Витрати: {totalMonthlySubscriptions:C}";
        remainingLabel.Text = remaining >= 0
            ? $"Залишилось: {remaining:C}"
            : $"Перевищення: {Math.Abs(remaining):C}";

        remainingLabel.ForeColor = remaining >= 0 ? Color.DarkGreen : Color.DarkRed;
    }

    private bool TryReadSalary(out decimal salary)
    {
        salary = 0m;

        if (string.IsNullOrWhiteSpace(salaryTextBox.Text))
        {
            return false;
        }

        return decimal.TryParse(salaryTextBox.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out salary) && salary >= 0;
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
