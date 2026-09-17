using System;
using System.Drawing;
using System.Windows.Forms;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm());
    }
}
public class MainForm : Form
{
	private TextBox txtDistance;
  private ComboBox cmbTariff;
  private CheckBox chkNight;
  private CheckBox chkChildSeat;
  private Button btnCalculate;
  private Button btnClear;
  private Label lblResult;

  public MainForm()
  {
    InitializeComponent();
  }
	private void InitializeComponent()
	{
		this.Text = "Калькулятор вартості поїздки на таксі";
		this.Size = new Size(400, 300);
		this.StartPosition = FormStartPosition.CenterScreen;

		Label lblDistance = new Label { Text = "Відстань (км):", Location = new Point(20, 20), AutoSize = true };
		txtDistance = new TextBox { Location = new Point(150, 20), Width = 200 };

		Label lblTariff = new Label { Text = "Тариф:", Location = new Point(20, 60), AutoSize = true };
		cmbTariff = new ComboBox { Location = new Point(150, 60), Width = 200 };
		cmbTariff.Items.AddRange(new string[] { "Економ", "Стандарт", "Комфорт" });
		cmbTariff.SelectedIndex = 0;

		chkNight = new CheckBox { Text = "Нічний тариф", Location = new Point(20, 100), AutoSize = true };
		chkChildSeat = new CheckBox { Text = "Дитяче крісло", Location = new Point(20, 130), AutoSize = true };

		btnCalculate = new Button { Text = "Розрахувати вартість", Location = new Point(20, 170), Width = 150 };
		btnCalculate.Click += BtnCalculate_Click;

		btnClear = new Button { Text = "Очистити", Location = new Point(200, 170), Width = 150 };
		btnClear.Click += BtnClear_Click;

		lblResult = new Label { Text = "", Location = new Point(20, 210), AutoSize = true };

		this.Controls.Add(lblDistance);
		this.Controls.Add(txtDistance);
		this.Controls.Add(lblTariff);
		this.Controls.Add(cmbTariff);
		this.Controls.Add(chkNight);
		this.Controls.Add(chkChildSeat);
		this.Controls.Add(btnCalculate);
		this.Controls.Add(btnClear);
		this.Controls.Add(lblResult);
	}
	private void MainForm_Load(object sender, EventArgs e)
	{
    cmbTariff.Items.Add("Економ");
    cmbTariff.Items.Add("Стандарт");
    cmbTariff.Items.Add("Комфорт");
  }
	private void BtnCalculate_Click(object sender, EventArgs e)
	{
		if (cmbTariff.SelectedItem == null)
		{
			MessageBox.Show("Будь ласка, оберіть тариф.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}
		string inputDistance = txtDistance.Text.Trim().Replace(',', '.');
		if (string.IsNullOrEmpty(inputDistance) || !double.TryParse(inputDistance, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double distance) || distance <= 0)
		{
			MessageBox.Show("Будь ласка, введіть коректну відстань у кілометрах.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}
		double pricePerKm = 0;
		switch (cmbTariff.SelectedItem.ToString())
		{
			case "Економ":
				pricePerKm = 12;
				break;
			case "Стандарт":
				pricePerKm = 18;
				break;
			case "Комфорт":
				pricePerKm = 25;
				break;
		}

		double totalCost = distance * pricePerKm;
		if (chkNight.Checked)
		{
			totalCost *= 1.2;
		}
		if (chkChildSeat.Checked)
		{
			totalCost += 40;
		}
		lblResult.Text = $"Вартість поїздки: {totalCost:F2} грн";
	}
	private void BtnClear_Click(object sender, EventArgs e)
	{
		txtDistance.Clear();
		cmbTariff.SelectedIndex = 0;
		chkNight.Checked = false;
		chkChildSeat.Checked = false;
		lblResult.Text = "Вартість по'їздки: 0.00 грн";
	}
}
