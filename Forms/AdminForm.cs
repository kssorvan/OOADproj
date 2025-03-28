using OOADPRO.Forms;
using OOADPRO.Forms.AdminDisplayForm;
namespace OOADPRO;

public partial class AdminForm : Form
{
    private LoadingForm loadingFormReference;
    private LoginForm loginFormReference;
    private DBConnectionForm databaseConnectionFormReference;
    public AdminForm(LoadingForm loadingForm, DBConnectionForm databaseConnectionForm, LoginForm login)
    {
        InitializeComponent();
        loadingFormReference = loadingForm;
        loginFormReference = login;
        databaseConnectionFormReference = databaseConnectionForm;
        this.FormClosed += new FormClosedEventHandler(AdminForm_FormClosed);
        btnDashboard.Click += DoClickDashboard;
        btnStaff.Click += DoClickStaff;
        btnProducts.Click += DoClickProducts;
        btnCategory.Click += DoClickCategory;
        btnUser.Click += DoClickUser;
        btnSaleReport.Click += DoClickSaleReport;
        btnLogout.Click += (_, _) => { this.Hide(); loginFormReference.Show(); };
    }

    

    private void AdminForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        loadingFormReference.Close();
        loginFormReference.Close();
        databaseConnectionFormReference.Close();
    }

    private void DoClickOverview(object? sender, EventArgs e)
    {
        AddControl(new OverviewForm());
    }

    private void DoClickSaleReport(object? sender, EventArgs e)
    {
        AddControl(new SaleReportForm());
    }

    private void DoClickUser(object? sender, EventArgs e)
    {
        AddControl(new UserForm());
    }

    private void DoClickCategory(object? sender, EventArgs e)
    {
        AddControl(new CategoryForm());
    }

    private void DoClickProducts(object? sender, EventArgs e)
    {
        AddControl(new ProductsForm());
    }

    private void DoClickStaff(object? sender, EventArgs e)
    {
        AddControl(new StaffForm());
    }

    private void DoClickDashboard(object? sender, EventArgs e)
    {

        AddControl(new DashboardForm());
    }

    public void AddControl(Form f)
    {
        panelcontrolMain.Controls.Clear();
        f.Dock = DockStyle.Fill;
        f.TopLevel = false;
        panelcontrolMain.Controls.Add(f);
        f.Show();
    }

    private void AdminForm_Load(object sender, EventArgs e)
    {
        AddControl(new DashboardForm());
    }
}
