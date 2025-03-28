using OOADPRO.Forms.CashierDisplayForm;
using OOADPRO.Models;

namespace OOADPRO.Forms
{
    public partial class CashierForm : Form
    {
        private LoadingForm loadingFormReference;
        private LoginForm loginFormReference;
        private DBConnectionForm databaseConnectionFormReference;
        public CashierForm(LoadingForm loadingForm, DBConnectionForm databaseConnectionForm, LoginForm login,User user)
        {
            InitializeComponent();
            loadingFormReference = loadingForm;
            loginFormReference = login;
            databaseConnectionFormReference = databaseConnectionForm;
            this.FormClosed += new FormClosedEventHandler(CashierForm_FormClosed);
            btnCashierProducts.Click += DoClickProducts;
            btnOrder.Click += DoClickOrder;
            btnLogout.Click += (_, _) => { this.Hide(); loginFormReference.Show(); };
            if (user != null)
                LabelUser.Text += " " + user.Username.ToUpper();
        }
        private void CashierForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            loadingFormReference.Close();
            loginFormReference.Close();
            databaseConnectionFormReference.Close();
        }
        private void DoClickProducts(object? sender, EventArgs e)
        {
            AddControl(new CashierProductForm());
        }

        private void DoClickOrder(object? sender, EventArgs e)
        {
            AddControl(new OrderForm());
        }
        public void AddControl(Form f)
        {
            panelcontrolMain.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            panelcontrolMain.Controls.Add(f);
            f.Show();
        }
        private void CashierForm_Load(object sender, EventArgs e)
        {
            AddControl(new CashierProductForm());

        }
    }
}
