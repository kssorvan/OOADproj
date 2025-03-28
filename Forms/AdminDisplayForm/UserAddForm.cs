
using OOADPRO.Models;
using OOADPRO.Utilities;

namespace OOADPRO.Forms.AdminDisplayForm;

public partial class UserAddForm : Form
{
    User? effectedUser = null;
    Staff? effectedStaff = null;

    int userCount = 0;
    int indexOfUpdateUser;
    public UserAddForm(UserForm userform)
    {
        InitializeComponent();
        btnClear.Click += DoClickClearFormInput;
        btnInsert.Click += DoClickInsertUser;
        btnUpdate.Click += DoClickUpdateUser;
        cBStaffID.SelectedValueChanged += Select_Handling_StaffID;
        chBShowPass.CheckedChanged += CheckedShowPassword;

    }
    private void CheckedShowPassword(object? sender, EventArgs e)
    {
        if (chBShowPass.Checked)
        {
            txtPassword.UseSystemPasswordChar = false;
        }
        else
        {
            txtPassword.UseSystemPasswordChar = true;
        }
    }

    private void DoClickUpdateUser(object? sender, EventArgs e)
    {
        if (txtUsername.Text == "" || txtUsername.Text.Trim().Length > 100)
        {
            MessageBox.Show("Username is required or username too long", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        if (txtPassword.Text == "" || txtPassword.Text.Trim().Length > 100)
        {
            MessageBox.Show("Password is required or password too long", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        if (cBStaffID.SelectedItem == null)
        {
            MessageBox.Show("Staff Gender is required", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        string? staffID = cBStaffID.SelectedItem.ToString();
        if (staffID == null) return;

        effectedUser.Username = txtUsername.Text.Trim();
        effectedUser.Password = txtPassword.Text.Trim();
        effectedUser.Staff.StaffID = int.Parse(cBStaffID.SelectedItem.ToString());
        effectedUser.Staff.StaffName = txtStaffName.Text.Trim();
        effectedUser.Staff.StaffPosition = txtStaffPosition.Text.Trim();
        try
        {

            var result = UserFunc.UpdateUser(Program.Connection, effectedUser);
            if (result == true)
            {
                MessageBox.Show($"Successfully updated an existing staff with the id {txtUserID.Text}");
                UserLoadingChanged?.Invoke(this, result);
            }

        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to update an existing staff > {ex.Message}");
        }

    }

    private void DoClickInsertUser(object? sender, EventArgs e)
    {
        if (txtUsername.Text == "" || txtUsername.Text.Trim().Length > 100)
        {
            MessageBox.Show("Username is required or username too long", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        if (txtPassword.Text == "" || txtPassword.Text.Trim().Length > 100)
        {
            MessageBox.Show("Password is required or password too long", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        if (cBStaffID.SelectedItem == null)
        {
            MessageBox.Show("Staff Gender is required", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        string? staffID = cBStaffID.SelectedItem.ToString();
        if (staffID == null) return;


        User newUser = new User()
        {
            Username = txtUsername.Text.Trim(),
            Password = txtPassword.Text.Trim(),
            Staff = new Staff()
            {
                StaffID = int.Parse(staffID.Trim()),
                StaffName = txtStaffName.Text.Trim(),
                StaffPosition = txtStaffPosition.Text.Trim(),

            }
        };
        try
        {
            var result = UserFunc.AddUser(Program.Connection, newUser);
            if (result == true)
                MessageBox.Show($"Successfully inserted user with the id {txtUserID.Text}");
            UserLoadingChanged?.Invoke(this, result);
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Submitting", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        LoadingUser();
        clearFormInput();
    }

    private void DoClickClearFormInput(object? sender, EventArgs e)
    {
        clearFormInput();
    }
    private void clearFormInput()
    {
        txtUserID.Text = (userCount + 1).ToString();
        txtStaffName.Text = "";
        cBStaffID.SelectedItem = null;
        txtUsername.Text = "";
        txtPassword.Text = "";
        txtStaffPosition.Text = "";
        picStaff.Image = null;
    }
    private void UserAddForm_Load(object sender, EventArgs e)
    {

        LoadingUser();
        LoadingDataStaffID();
        if (txtUsername.Text == "")
        {
            txtUserID.Text = (userCount + 1).ToString();
            cBStaffID.SelectedIndex = -1;
            txtStaffName.Clear();
            txtStaffPosition.Clear();
            picStaff.Image = null;
        }
        else
        {
            cBStaffID.SelectedItem =effectedUser.Staff.StaffID.ToString();
            txtStaffName.Text = effectedUser.Staff.StaffName;
            txtStaffPosition.Text = effectedUser.Staff.StaffPosition;
            if (effectedUser.Staff.Photo != null)
            {
                picStaff.Image = ConvertImageClass.ConvertByteArrayToImage(effectedUser.Staff.Photo);
            }
            else
            {
                picStaff.Image = null;
            }
        }
            
    }
    private void LoadingUser()
    {
        try
        {
            var result = UserFunc.GetAllUser(Program.Connection);
            if (result.LastOrDefault() != null) { userCount = result.LastOrDefault().UserID; }
            else { userCount = 0; }
            
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Retrieving user", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void Select_Handling_StaffID(object? sender, EventArgs e)
    {
        if (cBStaffID.SelectedItem != null)
        {
            string? staffID = cBStaffID.SelectedItem.ToString();
            if (staffID == null) return;

            try
            {
                effectedStaff = StaffFunc.GetOneStaffNameandPositionPhoto(Program.Connection, int.Parse(staffID.Trim()));
                txtStaffName.Text = effectedStaff.StaffName;
                txtStaffPosition.Text = effectedStaff.StaffPosition;
                picStaff.Image = effectedStaff.Photo != null ? ConvertImageClass.ConvertByteArrayToImage(effectedStaff.Photo) : null;


            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }

        }
    }
    private void LoadingDataStaffID()
    {
        try
        {
            var result = StaffFunc.GetAllStaffID(Program.Connection);
            List<string> ls = new List<string>();
            foreach (var staff in result)
            {
                ls.Add(staff.StaffID.ToString());
            }
            cBStaffID.DataSource = ls;
            
            
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Retrieving staff ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    public void LoadUserDetails(User user)
    {
        Console.WriteLine(user);
        if (user == null)
            return;
        txtUserID.Clear();
        txtUserID.Text = user.UserID.ToString();
        txtUsername.Text = user.Username;
        txtPassword.Text = user.Password;
       

        

        effectedUser = user;
    }
    public event LoadingEventHandler? UserLoadingChanged;
}
