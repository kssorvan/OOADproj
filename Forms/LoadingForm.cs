using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOADPRO.Forms;

public partial class LoadingForm : Form
{
    public LoadingForm()
    {
        InitializeComponent();
        timerLoading.Tick += DoTimerTicker;
        timerLoading.Start();
    }
    int startpos = 0;
    private void DoTimerTicker(object? sender, EventArgs e)
    {
        startpos += 4;
        progressBarLoading.Value = startpos;
        if (progressBarLoading.Value == 100)
        {
            progressBarLoading.Value = 0;
            timerLoading.Stop();
            this.Hide();
            DBConnectionForm databaseConnectionForm = new DBConnectionForm(this);
            if (File.Exists($"{Environment.CurrentDirectory}/appSettings.json"))
            {
                databaseConnectionForm.Hide();
                LoginForm loginForm = new LoginForm(this, databaseConnectionForm);
                loginForm.Show();
            }
            else
            {

                databaseConnectionForm.Show();
            }
        }
    }

}
