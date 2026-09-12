using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace hw2;

public class StudentRegistrationForm : Form
{
    private readonly TextBox fullNameTextBox = new();
    private readonly DateTimePicker birthDatePicker = new();
    private readonly ComboBox groupComboBox = new();
    private readonly RadioButton maleRadioButton = new();
    private readonly RadioButton femaleRadioButton = new();
    private readonly CheckBox consentCheckBox = new();
    private readonly Button registerButton = new();
    private readonly LinkLabel socialLink = new();

    public StudentRegistrationForm()
    {
        Text = "Реєстрація студента";
        StartPosition = FormStartPosition.CenterScreen;
        Width = 860;
        Height = 620;
        BackColor = Color.FromArgb(245, 247, 250);
        Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;

        var container = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(28),
            BackColor = Color.FromArgb(245, 247, 250)
        };
        Controls.Add(container);

        var title = new Label
        {
            Text = "Реєстрація студента",
            Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(20, 35, 60),
            AutoSize = true,
            Anchor = AnchorStyles.Top | AnchorStyles.Left,
            Margin = new Padding(0, 0, 0, 16)
        };
        container.Controls.Add(title);

        var formLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 2,
            RowCount = 6,
            Margin = new Padding(0, 0, 0, 0),
            Padding = new Padding(0, 0, 0, 0)
        };
        formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
        formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F));

        formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var fields = new[]
        {
            CreateLabel("ПІБ:"),
            CreateLabel("Дата народження:"),
            CreateLabel("Група / спеціальність:"),
            CreateLabel("Стать:"),
            CreateLabel("Згода на обробку даних:"),
            CreateLabel("")
        };

        for (int i = 0; i < fields.Length; i++)
        {
            formLayout.Controls.Add(fields[i], 0, i);
        }

        fullNameTextBox.PlaceholderText = "Введіть повне ім'я";
        fullNameTextBox.Width = 360;
        fullNameTextBox.Height = 34;
        fullNameTextBox.BorderStyle = BorderStyle.FixedSingle;
        fullNameTextBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        fullNameTextBox.Margin = new Padding(0, 4, 0, 12);
        formLayout.Controls.Add(fullNameTextBox, 1, 0);

        birthDatePicker.Format = DateTimePickerFormat.Short;
        birthDatePicker.MaxDate = DateTime.Today;
        birthDatePicker.Width = 220;
        birthDatePicker.Height = 34;
        birthDatePicker.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
        birthDatePicker.Margin = new Padding(0, 4, 0, 12);
        formLayout.Controls.Add(birthDatePicker, 1, 1);

        groupComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        groupComboBox.Width = 340;
        groupComboBox.Height = 34;
        groupComboBox.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
        groupComboBox.Margin = new Padding(0, 4, 0, 12);
        groupComboBox.Items.AddRange(new object[]
        {
            "КН-21",
            "КН-22",
            "ПІ-11",
            "ПІ-12",
            "ІТ-31",
            "ММ-14"
        });
        formLayout.Controls.Add(groupComboBox, 1, 2);

        var genderPanel = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.LeftToRight,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Margin = new Padding(0, 4, 0, 12)
        };

        maleRadioButton.Text = "Чоловік";
        maleRadioButton.AutoSize = true;
        maleRadioButton.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);

        femaleRadioButton.Text = "Жінка";
        femaleRadioButton.AutoSize = true;
        femaleRadioButton.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);

        genderPanel.Controls.Add(maleRadioButton);
        genderPanel.Controls.Add(femaleRadioButton);
        formLayout.Controls.Add(genderPanel, 1, 3);

        consentCheckBox.Text = "Да, я даю згоду";
        consentCheckBox.AutoSize = true;
        consentCheckBox.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
        consentCheckBox.Margin = new Padding(0, 8, 0, 10);
        formLayout.Controls.Add(consentCheckBox, 1, 4);

        registerButton.Text = "Зареєструвати";
        registerButton.BackColor = Color.FromArgb(46, 125, 246);
        registerButton.ForeColor = Color.White;
        registerButton.FlatStyle = FlatStyle.Flat;
        registerButton.FlatAppearance.BorderSize = 0;
        registerButton.Cursor = Cursors.Hand;
        registerButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        registerButton.Width = 220;
        registerButton.Height = 44;
        registerButton.Margin = new Padding(0, 12, 0, 0);
        registerButton.Click += RegisterButton_Click;
        formLayout.Controls.Add(registerButton, 1, 5);

        container.Controls.Add(formLayout);

        var footer = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 32,
            BackColor = Color.FromArgb(245, 247, 250)
        };

        socialLink.Text = "Instagram";
        socialLink.LinkColor = Color.FromArgb(33, 88, 179);
        socialLink.ActiveLinkColor = Color.FromArgb(14, 52, 108);
        socialLink.VisitedLinkColor = Color.FromArgb(33, 88, 179);
        socialLink.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
        socialLink.AutoSize = true;
        socialLink.LinkClicked += (_, __) => OpenSocialLink("https://www.instagram.com/_alia.1327/");
        socialLink.Anchor = AnchorStyles.Right;
        socialLink.TextAlign = ContentAlignment.MiddleRight;
        socialLink.Dock = DockStyle.Right;
        socialLink.Margin = new Padding(0, 5, 0, 0);

        footer.Controls.Add(socialLink);
        container.Controls.Add(footer);
    }

    private static Label CreateLabel(string text)
    {
        return new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(42, 55, 72),
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(0, 10, 10, 10)
        };
    }

    private void RegisterButton_Click(object? sender, EventArgs e)
    {
        var name = fullNameTextBox.Text.Trim();
        var group = groupComboBox.SelectedItem?.ToString();
        var birthDate = birthDatePicker.Value;
        var gender = maleRadioButton.Checked ? "Чоловік" : femaleRadioButton.Checked ? "Жінка" : "";

        if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
        {
            MessageBox.Show("Введіть коректне ПІБ студента.", "Помилка валідації", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            fullNameTextBox.Focus();
            return;
        }

        if (birthDate > DateTime.Today)
        {
            MessageBox.Show("Дата народження не може бути більшою за поточну дату.", "Помилка валідації", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            birthDatePicker.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(group))
        {
            MessageBox.Show("Оберіть групу або спеціальність.", "Помилка валідації", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            groupComboBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(gender))
        {
            MessageBox.Show("Виберіть стать студента.", "Помилка валідації", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            maleRadioButton.Focus();
            return;
        }

        if (!consentCheckBox.Checked)
        {
            MessageBox.Show("Для реєстрації потрібно надати згоду на обробку даних.", "Помилка валідації", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            consentCheckBox.Focus();
            return;
        }

        var summary = $"ПІБ: {name}{Environment.NewLine}Дата народження: {birthDate:dd.MM.yyyy}{Environment.NewLine}Група: {group}{Environment.NewLine}Стать: {gender}{Environment.NewLine}Згода на обробку даних: так";
        MessageBox.Show(summary, "Реєстрація успішна", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private static void OpenSocialLink(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch (Exception)
        {
            MessageBox.Show("Не вдалося відкрити посилання.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new StudentRegistrationForm());
    }
}
