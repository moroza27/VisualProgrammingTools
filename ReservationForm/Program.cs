ApplicationConfiguration.Initialize();
Application.Run(new ReservationFormWindow());

internal sealed class ReservationFormWindow : Form
{
	private const int RowCount = 5;
	private const int SeatsPerRow = 8;
	private const decimal SeatPrice = 150;

	private readonly Button[,] seatButtons = new Button[RowCount, SeatsPerRow];
	private readonly Label summaryLabel = new();

	public ReservationFormWindow()
	{
		Text = "Бронювання місць";
		StartPosition = FormStartPosition.CenterScreen;
		MinimumSize = new Size(800, 480);
		Size = new Size(850, 560);

		var title = new Label
		{
			AutoSize = true,
			Text = "Схема залу",
			Font = new Font("Segoe UI", 16, FontStyle.Bold),
			Location = new Point(24, 20)
		};

		var screenLabel = new Label
		{
			Text = "ЕКРАН",
			TextAlign = ContentAlignment.MiddleCenter,
			BackColor = Color.LightGray,
			Location = new Point(50, 65),
			Size = new Size(500, 32)
		};

		var seatsGrid = new TableLayoutPanel
		{
			ColumnCount = SeatsPerRow,
			RowCount = RowCount,
			Location = new Point(50, 120),
			Size = new Size(500, 250),
			CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
			Padding = new Padding(8)
		};

		for (var column = 0; column < SeatsPerRow; column++)
		{
			seatsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / SeatsPerRow));
		}

		for (var row = 0; row < RowCount; row++)
		{
			seatsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / RowCount));

			for (var column = 0; column < SeatsPerRow; column++)
			{
				var seatButton = CreateSeatButton(row, column);
				seatButtons[row, column] = seatButton;
				seatsGrid.Controls.Add(seatButton, column, row);
			}
		}

		summaryLabel.AutoSize = true;
		summaryLabel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
		summaryLabel.Location = new Point(50, 395);

		var resetButton = new Button
		{
			Text = "Скинути бронювання",
			Location = new Point(50, 435),
			Size = new Size(170, 34)
		};
		resetButton.Click += (_, _) => ResetSeats();

		var pricePanel = new Panel
		{
			Location = new Point(590, 120),
			Size = new Size(210, 250),
			BorderStyle = BorderStyle.FixedSingle,
			BackColor = Color.WhiteSmoke
		};

		var priceTitle = new Label
		{
			AutoSize = false,
			Text = "Ціна за місце",
			TextAlign = ContentAlignment.MiddleCenter,
			Font = new Font("Segoe UI", 11, FontStyle.Bold),
			Location = new Point(10, 20),
			Size = new Size(188, 30)
		};

		var priceLabel = new Label
		{
			AutoSize = false,
			Text = $"{SeatPrice:0} грн",
			TextAlign = ContentAlignment.MiddleCenter,
			Font = new Font("Segoe UI", 18, FontStyle.Bold),
			ForeColor = Color.DarkGreen,
			Location = new Point(10, 55),
			Size = new Size(188, 45)
		};

		var freeLegend = CreateLegendLabel(Color.LightGreen, "Вільне місце", 125);
		var reservedLegend = CreateLegendLabel(Color.LightCoral, "Заброньовано", 165);
		pricePanel.Controls.AddRange([priceTitle, priceLabel, freeLegend, reservedLegend]);

		Controls.AddRange([title, screenLabel, seatsGrid, summaryLabel, resetButton, pricePanel]);
		UpdateSummary();
	}

	private static Label CreateLegendLabel(Color color, string text, int top)
	{
		return new Label
		{
			AutoSize = false,
			Text = $"  {text}",
			TextAlign = ContentAlignment.MiddleLeft,
			BackColor = color,
			Location = new Point(20, top),
			Size = new Size(168, 28)
		};
	}

	private Button CreateSeatButton(int row, int column)
	{
		var seatButton = new Button
		{
			Dock = DockStyle.Fill,
			Margin = new Padding(4),
			Text = $"{(char)('A' + row)}{column + 1}",
			BackColor = Color.LightGreen,
			UseVisualStyleBackColor = false,
			Tag = false
		};
		seatButton.Click += SeatButton_Click;
		return seatButton;
	}

	private void SeatButton_Click(object? sender, EventArgs e)
	{
		if (sender is not Button seatButton)
		{
			return;
		}

		var isReserved = !(bool)seatButton.Tag!;
		seatButton.Tag = isReserved;
		seatButton.BackColor = isReserved ? Color.LightCoral : Color.LightGreen;
		UpdateSummary();
	}

	private void ResetSeats()
	{
		foreach (var seatButton in seatButtons)
		{
			seatButton.Tag = false;
			seatButton.BackColor = Color.LightGreen;
		}

		UpdateSummary();
	}

	private void UpdateSummary()
	{
		var reservedCount = seatButtons.Cast<Button>().Count(button => (bool)button.Tag!);
		var totalPrice = reservedCount * SeatPrice;
		summaryLabel.Text = $"Заброньовано місць: {reservedCount}    Вартість: {totalPrice:0} грн";
	}
}
