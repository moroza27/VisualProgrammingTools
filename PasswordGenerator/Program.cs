using System.Security.Cryptography;

ApplicationConfiguration.Initialize();
Application.Run(new PasswordForm());

internal sealed class PasswordForm : Form
{
	private readonly NumericUpDown lengthInput = new();
	private readonly CheckBox digitsCheckBox = new();
	private readonly CheckBox uppercaseCheckBox = new();
	private readonly CheckBox specialCheckBox = new();
	private readonly TextBox passwordOutput = new();

	private const string LowercaseCharacters = "abcdefghijklmnopqrstuvwxyz";
	private const string DigitCharacters = "0123456789";
	private const string UppercaseCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	private const string SpecialCharacters = "!@#$%^&*()-_=+[]{};:,.?/";

	public PasswordForm()
	{
		Text = "Генератор паролів";
		StartPosition = FormStartPosition.CenterScreen;
		MinimumSize = new Size(420, 330);
		Size = new Size(500, 390);

		var title = new Label
		{
			AutoSize = true,
			Text = "Генератор паролів",
			Font = new Font("Segoe UI", 16, FontStyle.Bold),
			Location = new Point(24, 20)
		};

		var lengthLabel = new Label
		{
			AutoSize = true,
			Text = "Довжина пароля:",
			Location = new Point(24, 72)
		};

		lengthInput.Location = new Point(150, 68);
		lengthInput.Size = new Size(80, 23);
		lengthInput.Minimum = 4;
		lengthInput.Maximum = 128;
		lengthInput.Value = 12;

		var optionsLabel = new Label
		{
			AutoSize = true,
			Text = "Додаткові символи:",
			Location = new Point(24, 112)
		};

		ConfigureCheckBox(digitsCheckBox, "Цифри", new Point(150, 108));
		ConfigureCheckBox(uppercaseCheckBox, "Великі літери", new Point(235, 108));
		ConfigureCheckBox(specialCheckBox, "Спецсимволи", new Point(150, 142));

		var hint = new Label
		{
			AutoSize = true,
			Text = "Малі літери додаються завжди",
			ForeColor = Color.DimGray,
			Location = new Point(24, 180)
		};

		var generateButton = new Button
		{
			Text = "Згенерувати",
			Location = new Point(24, 218),
			Size = new Size(130, 34)
		};
		generateButton.Click += (_, _) => GeneratePassword();

		passwordOutput.Location = new Point(24, 278);
		passwordOutput.Size = new Size(430, 30);
		passwordOutput.ReadOnly = true;
		passwordOutput.Font = new Font("Consolas", 12);
		passwordOutput.PlaceholderText = "Тут з'явиться пароль";

		Controls.AddRange([
			title, lengthLabel, lengthInput, optionsLabel,
			digitsCheckBox, uppercaseCheckBox, specialCheckBox,
			hint, generateButton, passwordOutput
		]);

		AcceptButton = generateButton;
	}

	private static void ConfigureCheckBox(CheckBox checkBox, string text, Point location)
	{
		checkBox.AutoSize = true;
		checkBox.Text = text;
		checkBox.Location = location;
	}

	private void GeneratePassword()
	{
		var selectedGroups = new List<string> { LowercaseCharacters };

		if (digitsCheckBox.Checked)
		{
			selectedGroups.Add(DigitCharacters);
		}

		if (uppercaseCheckBox.Checked)
		{
			selectedGroups.Add(UppercaseCharacters);
		}

		if (specialCheckBox.Checked)
		{
			selectedGroups.Add(SpecialCharacters);
		}

		var passwordLength = (int)lengthInput.Value;
		var passwordCharacters = selectedGroups
			.SelectMany(group => group)
			.ToArray();
		var password = new List<char>(passwordLength);

		foreach (var group in selectedGroups)
		{
			password.Add(group[RandomNumberGenerator.GetInt32(group.Length)]);
		}

		while (password.Count < passwordLength)
		{
			password.Add(passwordCharacters[RandomNumberGenerator.GetInt32(passwordCharacters.Length)]);
		}

		for (var index = password.Count - 1; index > 0; index--)
		{
			var swapIndex = RandomNumberGenerator.GetInt32(index + 1);
			(password[index], password[swapIndex]) = (password[swapIndex], password[index]);
		}

		passwordOutput.Text = new string(password.ToArray());
	}
}
