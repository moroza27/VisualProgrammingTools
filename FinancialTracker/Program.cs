using System.Globalization;
using System.Windows.Forms;

namespace FinancialTracker;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new FinanceTrackerForm());
    }
}

public class FinanceTrackerForm : Form
{
    private readonly ComboBox typeComboBox = new();
    private readonly ComboBox categoryComboBox = new();
    private readonly TextBox amountTextBox = new();
    private readonly TextBox descriptionTextBox = new();
    private readonly Button addButton = new();
    private readonly ComboBox filterComboBox = new();
    private readonly DataGridView transactionsGrid = new();
    private readonly Label balanceLabel = new();
    private readonly Label incomeLabel = new();
    private readonly Label expensesLabel = new();
    private readonly Button saveButton = new();
    private readonly Button loadButton = new();

    private readonly List<Transaction> transactions = [];
    private readonly string dataFilePath = Path.Combine(AppContext.BaseDirectory, "transactions.txt");

    public FinanceTrackerForm()
    {
        InitializeForm();
        LoadCategories();
        LoadTransactions();
        RefreshSummary();
        ApplyFilter();
    }

    private void InitializeForm()
    {
        Text = "FinansTracker";
        Width = 980;
        Height = 620;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = Color.White;

        var titleLabel = new Label
        {
            Text = "Фінансовий трекер",
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 20)
        };

        var typeLabel = new Label { Text = "Тип:", AutoSize = true, Location = new Point(20, 70), Font = new Font("Segoe UI", 10) };
        typeComboBox.Location = new Point(90, 66);
        typeComboBox.Size = new Size(140, 28);
        typeComboBox.Font = new Font("Segoe UI", 10);
        typeComboBox.Items.AddRange(["Дохід", "Витрата"]);
        typeComboBox.SelectedIndex = 0;

        var categoryLabel = new Label { Text = "Категорія:", AutoSize = true, Location = new Point(250, 70), Font = new Font("Segoe UI", 10) };
        categoryComboBox.Location = new Point(350, 66);
        categoryComboBox.Size = new Size(170, 28);
        categoryComboBox.Font = new Font("Segoe UI", 10);
        categoryComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

        var amountLabel = new Label { Text = "Сума:", AutoSize = true, Location = new Point(540, 70), Font = new Font("Segoe UI", 10) };
        amountTextBox.Location = new Point(600, 66);
        amountTextBox.Size = new Size(130, 28);
        amountTextBox.Font = new Font("Segoe UI", 10);
        amountTextBox.TextAlign = HorizontalAlignment.Right;
        amountTextBox.KeyPress += OnlyAllowNumbers;

        var descriptionLabel = new Label { Text = "Опис:", AutoSize = true, Location = new Point(20, 115), Font = new Font("Segoe UI", 10) };
        descriptionTextBox.Location = new Point(90, 111);
        descriptionTextBox.Size = new Size(430, 28);
        descriptionTextBox.Font = new Font("Segoe UI", 10);

        addButton.Text = "Додати операцію";
        addButton.Location = new Point(540, 110);
        addButton.Size = new Size(190, 35);
        addButton.BackColor = Color.FromArgb(46, 125, 50);
        addButton.ForeColor = Color.White;
        addButton.FlatStyle = FlatStyle.Flat;
        addButton.Click += AddTransaction;

        var filterLabel = new Label { Text = "Фільтр по категорії:", AutoSize = true, Location = new Point(20, 170), Font = new Font("Segoe UI", 10) };
        filterComboBox.Location = new Point(170, 166);
        filterComboBox.Size = new Size(170, 28);
        filterComboBox.Font = new Font("Segoe UI", 10);
        filterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        filterComboBox.SelectedIndexChanged += (_, _) => ApplyFilter();

        transactionsGrid.Location = new Point(20, 210);
        transactionsGrid.Size = new Size(760, 260);
        transactionsGrid.ReadOnly = true;
        transactionsGrid.AllowUserToAddRows = false;
        transactionsGrid.AllowUserToDeleteRows = false;
        transactionsGrid.AutoGenerateColumns = false;
        transactionsGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        transactionsGrid.DefaultCellStyle.Font = new Font("Segoe UI", 9);
        transactionsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        transactionsGrid.MultiSelect = false;

        transactionsGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Тип", DataPropertyName = nameof(Transaction.Type), Width = 90 });
        transactionsGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Категорія", DataPropertyName = nameof(Transaction.Category), Width = 160 });
        transactionsGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Опис", DataPropertyName = nameof(Transaction.Description), Width = 260 });
        transactionsGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Сума", DataPropertyName = nameof(Transaction.Amount), Width = 110 });
        transactionsGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Дата", DataPropertyName = nameof(Transaction.Date), Width = 120 });
        transactionsGrid.CellFormatting += TransactionsGrid_CellFormatting;

        balanceLabel.Location = new Point(20, 490);
        balanceLabel.AutoSize = true;
        balanceLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);

        incomeLabel.Location = new Point(250, 490);
        incomeLabel.AutoSize = true;
        incomeLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        incomeLabel.ForeColor = Color.DarkGreen;

        expensesLabel.Location = new Point(430, 490);
        expensesLabel.AutoSize = true;
        expensesLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        expensesLabel.ForeColor = Color.DarkRed;

        saveButton.Text = "Зберегти";
        saveButton.Location = new Point(790, 210);
        saveButton.Size = new Size(150, 35);
        saveButton.Click += (_, _) => SaveTransactions();

        loadButton.Text = "Завантажити";
        loadButton.Location = new Point(790, 256);
        loadButton.Size = new Size(150, 35);
        loadButton.Click += (_, _) => LoadTransactions();

        Controls.Add(titleLabel);
        Controls.Add(typeLabel);
        Controls.Add(typeComboBox);
        Controls.Add(categoryLabel);
        Controls.Add(categoryComboBox);
        Controls.Add(amountLabel);
        Controls.Add(amountTextBox);
        Controls.Add(descriptionLabel);
        Controls.Add(descriptionTextBox);
        Controls.Add(addButton);
        Controls.Add(filterLabel);
        Controls.Add(filterComboBox);
        Controls.Add(transactionsGrid);
        Controls.Add(balanceLabel);
        Controls.Add(incomeLabel);
        Controls.Add(expensesLabel);
        Controls.Add(saveButton);
        Controls.Add(loadButton);
    }

    private void LoadCategories()
    {
        var categories = new[]
        {
            "Зарплата",
            "Фінансова допомога",
            "Оренда",
            "Продукти",
            "Транспорт",
            "Кави/Їжа",
            "Розваги",
            "Комунальні",
            "Медицина",
            "Освіта",
            "Подарунки",
            "Інше"
        };

        categoryComboBox.Items.Clear();
        categoryComboBox.Items.Add("Усі");
        foreach (var category in categories)
        {
            categoryComboBox.Items.Add(category);
        }

        filterComboBox.Items.Clear();
        filterComboBox.Items.Add("Усі");
        foreach (var category in categories)
        {
            filterComboBox.Items.Add(category);
        }

        categoryComboBox.SelectedIndex = 1;
        filterComboBox.SelectedIndex = 0;
    }

    private void AddTransaction(object? sender, EventArgs e)
    {
        if (!decimal.TryParse(amountTextBox.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var amount) || amount <= 0)
        {
            MessageBox.Show("Введіть коректну суму більше 0.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            amountTextBox.Focus();
            return;
        }

        var category = categoryComboBox.SelectedItem?.ToString();
        if (string.IsNullOrWhiteSpace(category) || category == "Усі")
        {
            MessageBox.Show("Виберіть категорію.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            categoryComboBox.Focus();
            return;
        }

        var type = typeComboBox.SelectedItem?.ToString() ?? "Дохід";
        var description = descriptionTextBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(description))
        {
            description = type == "Дохід" ? "Дохід" : "Витрата";
        }

        transactions.Add(new Transaction
        {
            Type = type,
            Category = category,
            Description = description,
            Amount = amount,
            Date = DateTime.Now
        });

        amountTextBox.Clear();
        descriptionTextBox.Clear();
        typeComboBox.SelectedIndex = 0;

        ApplyFilter();
        RefreshSummary();
    }

    private void ApplyFilter()
    {
        var selectedCategory = filterComboBox.SelectedItem?.ToString();

        var filteredTransactions = string.IsNullOrWhiteSpace(selectedCategory) || selectedCategory == "Усі"
            ? transactions.ToList()
            : transactions.Where(t => t.Category == selectedCategory).ToList();

        transactionsGrid.DataSource = null;
        transactionsGrid.DataSource = filteredTransactions;
        transactionsGrid.Refresh();
    }

    private void TransactionsGrid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0)
        {
            return;
        }

        if (transactionsGrid.Columns[e.ColumnIndex].DataPropertyName == nameof(Transaction.Amount))
        {
            if (e.Value is decimal amount)
            {
                e.Value = amount.ToString("C", CultureInfo.CurrentCulture);
                e.FormattingApplied = true;
            }
        }
        else if (transactionsGrid.Columns[e.ColumnIndex].DataPropertyName == nameof(Transaction.Date))
        {
            if (e.Value is DateTime date)
            {
                e.Value = date.ToString("dd.MM.yyyy");
                e.FormattingApplied = true;
            }
        }
    }

    private void RefreshSummary()
    {
        var income = transactions.Where(t => t.Type == "Дохід").Sum(t => t.Amount);
        var expenses = transactions.Where(t => t.Type == "Витрата").Sum(t => t.Amount);
        var balance = income - expenses;

        balanceLabel.Text = $"Баланс: {balance:C}";
        incomeLabel.Text = $"Дохід: {income:C}";
        expensesLabel.Text = $"Витрати: {expenses:C}";

        balanceLabel.ForeColor = balance >= 0 ? Color.DarkGreen : Color.DarkRed;
    }

    private void SaveTransactions()
    {
        var lines = transactions.Select(t =>
            string.Join("|", t.Type, t.Category, t.Description, t.Amount.ToString(CultureInfo.InvariantCulture), t.Date.ToString("O"))).ToList();

        File.WriteAllLines(dataFilePath, lines);
        MessageBox.Show($"Дані збережено до: {dataFilePath}", "Збережено", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void LoadTransactions()
    {
        if (!File.Exists(dataFilePath))
        {
            MessageBox.Show("Файл з даними не знайдено.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var lines = File.ReadAllLines(dataFilePath);
        transactions.Clear();

        foreach (var line in lines)
        {
            var parts = line.Split('|');
            if (parts.Length < 5)
            {
                continue;
            }

            if (decimal.TryParse(parts[3], NumberStyles.Number, CultureInfo.InvariantCulture, out var amount) && DateTime.TryParse(parts[4], out var date))
            {
                transactions.Add(new Transaction
                {
                    Type = parts[0],
                    Category = parts[1],
                    Description = parts[2],
                    Amount = amount,
                    Date = date
                });
            }
        }

        ApplyFilter();
        RefreshSummary();
        MessageBox.Show("Дані завантажено.", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

public class Transaction
{
    public string Type { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;

    public string AmountDisplay => Amount.ToString("C", CultureInfo.CurrentCulture);
}
