﻿using OOADPRO.Models;
using OOADPRO.Utilities;

namespace OOADPRO.Forms;

public partial class LoginForm : Form
{
    public static User userVerify = null;
    private LoadingForm loadingFormReference;
    private DBConnectionForm databaseConnectionFormReference;
    public LoginForm(LoadingForm loadingForm, DBConnectionForm databaseConnectionForm)
    {
        InitializeComponent();

        Program.ConnectToDB();

        btnLogin.Click += DoClickLogin;
        this.loadingFormReference = loadingForm;
        this.databaseConnectionFormReference = databaseConnectionForm;
        this.FormClosed += new FormClosedEventHandler(LoginForm_FormClosed);
        chBShowPassword.CheckedChanged += CheckedShowPassword;
        btnBack.Click += DoClickBack;

    }
    private void DoClickBack(object? sender, EventArgs e)
    {
        File.Delete($"{Environment.CurrentDirectory}/appSettings.json");
        databaseConnectionFormReference.Show();
        // delete database file 
       
        this.Hide();
    }
    private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        databaseConnectionFormReference.Close();
        loadingFormReference.Close();
    }
    private void CheckedShowPassword(object? sender, EventArgs e)
    {
        if (chBShowPassword.Checked)
        {
            txtPassword.UseSystemPasswordChar = false;
        }
        else
        {
            txtPassword.UseSystemPasswordChar = true;
        }
    }

    private void DoClickLogin(object? sender, EventArgs e)
    {
        if (txtUsername.Text == "" && txtPassword.Text == "")
        {
            MessageBox.Show("Invalid Username and Password");
        }
        Login login = new Login();
        login._username = txtUsername.Text;
        login._password = txtPassword.Text;
        if (login._username != "" && login._password != "")
            userVerify = LoginFunc.VerifiedCredentials(Program.Connection, login);
        if (userVerify != null)
        {
            if (userVerify.Staff.StaffPosition == "Administrator")
            {
                AdminForm adminForm = new AdminForm(this.loadingFormReference, this.databaseConnectionFormReference, this);
                adminForm.Show();

            }
            else
            {
                CashierForm cashierForm = new CashierForm(this.loadingFormReference, this.databaseConnectionFormReference, this,userVerify);
                cashierForm.Show();
            }
            txtUsername.Clear();
            txtPassword.Clear();
            this.Hide();
        }
        else
        {
            txtPassword.Clear();
            labelShowMessage.ForeColor = Color.Red;
            labelShowMessage.Text = "The username or password you entered is incorrect. Please try again";
        }
    }
}
