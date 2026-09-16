ApplicationConfiguration.Initialize();
Application.Run(new SleepTrackerForm());

internal sealed class SleepTrackerForm : Form
{
	private readonly DateTimePicker sleepTimePicker = new();
	private readonly DateTimePicker wakeTimePicker = new();
	private readonly ComboBox ageCategoryComboBox = new();
	private readonly Label resultLabel = new();

	private readonly (string Name, double Minimum, double Maximum)[] ageCategories =
	[
		("Дитина (6-12 років)", 9, 12),
		("Підліток (13-17 років)", 8, 10),
		("Дорослий (18-64 роки)", 7, 9),
		("Старший дорослий (65+ років)", 7, 8)
	];

	public SleepTrackerForm()
	{
		Text = "Трекер сну";
		StartPosition = FormStartPosition.CenterScreen;
		MinimumSize = new Size(460, 360);
		Size = new Size(520, 410);

		var title = new Label
		{
			AutoSize = true,
			Text = "Трекер сну",
			Font = new Font("Segoe UI", 16, FontStyle.Bold),
			Location = new Point(24, 20)
		};

		var sleepLabel = CreateLabel("Час засинання:", new Point(24, 78));
		ConfigureTimePicker(sleepTimePicker, new Point(170, 74), new DateTime(2000, 1, 1, 23, 0, 0));

		var wakeLabel = CreateLabel("Час пробудження:", new Point(24, 120));
		ConfigureTimePicker(wakeTimePicker, new Point(170, 116), new DateTime(2000, 1, 1, 7, 0, 0));

		var ageLabel = CreateLabel("Вікова категорія:", new Point(24, 162));
		ageCategoryComboBox.Location = new Point(170, 158);
		ageCategoryComboBox.Size = new Size(260, 23);
		ageCategoryComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
		ageCategoryComboBox.Items.AddRange(ageCategories.Select(category => category.Name).ToArray());
		ageCategoryComboBox.SelectedIndex = 2;

		var calculateButton = new Button
		{
			Text = "Розрахувати",
			Location = new Point(24, 210),
			Size = new Size(130, 34)
		};
		calculateButton.Click += (_, _) => CalculateSleep();

		resultLabel.AutoSize = false;
		resultLabel.BorderStyle = BorderStyle.FixedSingle;
		resultLabel.Font = new Font("Segoe UI", 11);
		resultLabel.Location = new Point(24, 270);
		resultLabel.Size = new Size(450, 65);
		resultLabel.Padding = new Padding(10);
		resultLabel.Text = "Вкажіть час і натисніть «Розрахувати».";

		Controls.AddRange([title, sleepLabel, sleepTimePicker, wakeLabel, wakeTimePicker,
			ageLabel, ageCategoryComboBox, calculateButton, resultLabel]);
		AcceptButton = calculateButton;
	}

	private static Label CreateLabel(string text, Point location)
	{
		return new Label
		{
			AutoSize = true,
			Text = text,
			Location = location
		};
	}

	private static void ConfigureTimePicker(DateTimePicker picker, Point location, DateTime value)
	{
		picker.Format = DateTimePickerFormat.Custom;
		picker.CustomFormat = "HH:mm";
		picker.ShowUpDown = true;
		picker.Location = location;
		picker.Size = new Size(100, 23);
		picker.Value = value;
	}

	private void CalculateSleep()
	{
		var sleepTime = sleepTimePicker.Value;
		var wakeTime = wakeTimePicker.Value;

		if (wakeTime <= sleepTime)
		{
			wakeTime = wakeTime.AddDays(1);
		}

		var duration = wakeTime - sleepTime;
		var category = ageCategories[ageCategoryComboBox.SelectedIndex];
		var status = duration.TotalHours < category.Minimum
			? "недосип"
			: duration.TotalHours > category.Maximum
				? "пересип"
				: "норма";

		resultLabel.Text = $"Тривалість сну: {duration.Hours} год {duration.Minutes} хв\n" +
						   $"Рекомендація для категорії «{category.Name}»: {status}.";
	}
}
