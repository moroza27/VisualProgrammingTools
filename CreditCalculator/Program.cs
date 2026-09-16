using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace CreditCalculator;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		ApplicationConfiguration.Initialize();
		Application.Run(new CreditCalculatorForm());
	}
}

public sealed class CreditCalculatorForm : Form
{
	private readonly NumericUpDown amountInput;
	private readonly NumericUpDown annualRateInput;
	private readonly NumericUpDown termInput;
	private readonly DataGridView paymentGrid;
	private readonly Label summaryLabel;

	public CreditCalculatorForm()
	{
		Text = "Кредитний калькулятор";
		StartPosition = FormStartPosition.CenterScreen;
		MinimumSize = new Size(850, 520);
		Size = new Size(1000, 650);
		BackColor = Color.FromArgb(245, 248, 252);

		var titleLabel = new Label
		{
			Text = "Графік ануїтетних платежів",
			AutoSize = true,
			Font = new Font("Segoe UI", 16, FontStyle.Bold),
			Location = new Point(24, 20)
		};

		var amountLabel = CreateInputLabel("Сума кредиту:", 24, 78);
		amountInput = CreateNumberInput(210, 73, 1000000, 100000, 2);

		var rateLabel = CreateInputLabel("Річна ставка (%):", 24, 120);
		annualRateInput = CreateNumberInput(210, 115, 100, 12, 2);

		var termLabel = CreateInputLabel("Термін (місяців):", 24, 162);
		termInput = CreateNumberInput(210, 157, 360, 12, 0);

		var calculateButton = new Button
		{
			Text = "Розрахувати",
			Location = new Point(24, 205),
			Size = new Size(150, 36),
			BackColor = Color.FromArgb(34, 139, 230),
			ForeColor = Color.White,
			FlatStyle = FlatStyle.Flat
		};
		calculateButton.FlatAppearance.BorderSize = 0;
		calculateButton.Click += CalculateButton_Click;

		summaryLabel = new Label
		{
			Text = "Введіть параметри та натисніть «Розрахувати».",
			AutoSize = true,
			Location = new Point(210, 214),
			Font = new Font("Segoe UI", 10, FontStyle.Bold),
			ForeColor = Color.FromArgb(22, 101, 52)
		};

		paymentGrid = new DataGridView
		{
			Location = new Point(24, 270),
			Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
			Size = new Size(936, 320),
			AllowUserToAddRows = false,
			AllowUserToDeleteRows = false,
			ReadOnly = true,
			RowHeadersVisible = false,
			AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
			BackgroundColor = Color.White,
			BorderStyle = BorderStyle.FixedSingle
		};
		paymentGrid.Columns.Add("Month", "Місяць");
		paymentGrid.Columns.Add("Payment", "Платіж, грн");
		paymentGrid.Columns.Add("Principal", "Тіло кредиту, грн");
		paymentGrid.Columns.Add("Interest", "Відсотки, грн");
		paymentGrid.Columns.Add("Balance", "Залишок, грн");

		Controls.Add(titleLabel);
		Controls.Add(amountLabel);
		Controls.Add(amountInput);
		Controls.Add(rateLabel);
		Controls.Add(annualRateInput);
		Controls.Add(termLabel);
		Controls.Add(termInput);
		Controls.Add(calculateButton);
		Controls.Add(summaryLabel);
		Controls.Add(paymentGrid);
	}

	private static Label CreateInputLabel(string text, int x, int y)
	{
		return new Label
		{
			Text = text,
			AutoSize = true,
			Location = new Point(x, y),
			Font = new Font("Segoe UI", 10)
		};
	}

	private static NumericUpDown CreateNumberInput(int x, int y, decimal maximum, decimal value, int decimalPlaces)
	{
		return new NumericUpDown
		{
			Location = new Point(x, y),
			Width = 160,
			Minimum = 0,
			Maximum = maximum,
			Value = value,
			DecimalPlaces = decimalPlaces,
			Increment = decimalPlaces == 0 ? 1 : 0.1m,
			ThousandsSeparator = true
		};
	}

	private void CalculateButton_Click(object? sender, EventArgs e)
	{
		decimal loanAmount = amountInput.Value;
		decimal annualRate = annualRateInput.Value;
		int termMonths = (int)termInput.Value;

		if (loanAmount <= 0 || termMonths <= 0)
		{
			MessageBox.Show("Сума кредиту та термін мають бути більшими за нуль.", "Перевірка даних", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return;
		}

		decimal monthlyRate = annualRate / 100m / 12m;
		decimal regularPayment = monthlyRate == 0
			? loanAmount / termMonths
			: loanAmount * monthlyRate / (1m - (decimal)Math.Pow((double)(1m + monthlyRate), -termMonths));

		paymentGrid.Rows.Clear();
		decimal remainingBalance = loanAmount;
		decimal totalPaid = 0;
		decimal totalInterest = 0;

		for (int month = 1; month <= termMonths; month++)
		{
			decimal interest = Math.Round(remainingBalance * monthlyRate, 2);
			decimal principal = Math.Round(regularPayment - interest, 2);
			decimal payment = Math.Round(regularPayment, 2);

			if (month == termMonths)
			{
				principal = remainingBalance;
				payment = Math.Round(principal + interest, 2);
			}

			remainingBalance = Math.Max(0, remainingBalance - principal);
			totalPaid += payment;
			totalInterest += interest;

			paymentGrid.Rows.Add(
				month.ToString(CultureInfo.InvariantCulture),
				payment.ToString("N2"),
				principal.ToString("N2"),
				interest.ToString("N2"),
				remainingBalance.ToString("N2"));
		}

		summaryLabel.Text = $"Щомісячний платіж: {regularPayment:N2} грн | Переплата: {totalInterest:N2} грн | Усього сплачено: {totalPaid:N2} грн";
	}
}
