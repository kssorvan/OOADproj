using OOADPRO.Models;
using OOADPRO.Utilities;
using System.Drawing.Imaging;

namespace OOADPRO.Forms.AdminDisplayForm;

public partial class StaffAddForm : Form
{
    string? imgLocation = "";
    Staff? effectedStaff = null;
    List<string> listBoxStaff = new List<string>();
    int staffCount = 0;
    int indexOfUpdateStaff;
    string[] staffPosition { get; set; } = new string[] { "Administrator", "Cashier" ,"Cleaner","Waiter"};
    public StaffAddForm(StaffForm staffform)
    {
        InitializeComponent();

        cBStaffGender.DataSource = Program.Genders;
        btnClear.Click += DoClickClearFormInput;
        btnInsert.Click += DoClickInsertStaff;
        btnUpdate.Click += DoClickUpdateStaff;
        btnUploadPhoto.Click += DoClickUploadStaffPhoto;
        cBStaffPosition.DataSource = staffPosition;
        dtpDOB.Format = DateTimePickerFormat.Custom;
        dtpHiredDate.Format = DateTimePickerFormat.Custom;
        listBoxStaff.Clear();

    }

    private void DoClickUploadStaffPhoto(object? sender, EventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filter = "SELECT Photo(*.Jpg; *.png; *.Gif)|*.Jpg; *.png; *.Gif";
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            imgLocation = dialog.FileName.ToString();
            picStaff.ImageLocation = imgLocation;
        }
    }

    private void DoClickUpdateStaff(object? sender, EventArgs e)
    {
        byte[] StaffImages = null;
        if (effectedStaff != null)
        {
            if (txtStaffName.Text == "" || txtStaffName.Text.Trim().Length > 100)
            {
                MessageBox.Show("Staff Name is required or name too long", "Creating",
                MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (rtxtStaffAddress.Text == "" || rtxtStaffAddress.Text.Trim().Length > 1000)
            {
                MessageBox.Show("Staff Address is required or address too long", "Creating",
                MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (txtContactNumber.Text == "" || txtContactNumber.Text.Trim().Length > 10)
            {
                MessageBox.Show("Staff Contact Number is required or contact number too long", "Creating",
                MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (cBStaffPosition.SelectedItem == null)
            {
                MessageBox.Show("Staff Position is required", "Creating",
                MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            effectedStaff.StaffName = txtStaffName.Text.Trim();
            effectedStaff.Gender = cBStaffGender.SelectedItem.ToString();
            effectedStaff.BirthDate = dtpDOB.Value;
            effectedStaff.StaffAddress = rtxtStaffAddress.Text.Trim();
            effectedStaff.ContactNumber = txtContactNumber.Text.Trim();
            effectedStaff.StaffPosition = cBStaffPosition.SelectedItem.ToString();
            effectedStaff.HiredDate = dtpHiredDate.Value;

            if (picStaff.Image != null)
            {
                Image image = picStaff.Image;
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Png);
                StaffImages = ms.ToArray();
            }
            effectedStaff.Photo = StaffImages;
           
            try
            {

                var result = StaffFunc.UpdateStaff(Program.Connection, effectedStaff);
                if (result == true)
                {
                    MessageBox.Show($"Successfully updated an existing staff with the id {txtStaffID.Text}");
                    StaffLoadingChanged?.Invoke(this, result);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update an existing staff > {ex.Message}");
            }
        }

    }

    private void DoClickInsertStaff(object? sender, EventArgs e)
    {
        byte[] StaffImages = null;
        if (txtStaffName.Text == "" || txtStaffName.Text.Trim().Length > 100)
        {
            MessageBox.Show("Staff Name is required or name too long", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        if (cBStaffGender.SelectedItem == null)
        {
            MessageBox.Show("Staff Gender is required", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        if (rtxtStaffAddress.Text == "" || rtxtStaffAddress.Text.Trim().Length > 1000)
        {
            MessageBox.Show("Staff Address is required or address too long", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        if (txtContactNumber.Text == "" || txtContactNumber.Text.Trim().Length > 10)
        {
            MessageBox.Show("Staff Contact Number is required or contact number too long", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }

        if (cBStaffPosition.SelectedItem == null)
        {
            MessageBox.Show("Staff Position is required", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        if (dtpDOB.Text == "")
        {
            MessageBox.Show("Staff BirthDate is required", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        if (dtpHiredDate.Text == "")
        {
            MessageBox.Show("Staff Hired Date is required", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        if (imgLocation != "")
        {
            FileStream stream = new FileStream(imgLocation, FileMode.Open, FileAccess.Read);
            BinaryReader breader = new BinaryReader(stream);
            StaffImages = breader.ReadBytes((int)stream.Length);
        }

        Staff newStaff = new Staff()
        {
            StaffName = txtStaffName.Text.Trim(),
            Gender = cBStaffGender.SelectedItem.ToString(),
            BirthDate = dtpDOB.Value,
            StaffAddress = rtxtStaffAddress.Text.Trim(),
            ContactNumber = txtContactNumber.Text.Trim(),

            StaffPosition = cBStaffPosition.SelectedItem.ToString(),
            HiredDate = dtpHiredDate.Value,
            Photo = StaffImages,
        };
        try
        {
            var result = StaffFunc.AddStaff(Program.Connection, newStaff);
            if (result == true)
            {
                MessageBox.Show($"Successfully inserted staff with the id {txtStaffID.Text}");
                StaffLoadingChanged?.Invoke(this, result);
            }
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Submitting", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        clearFormInput();
        LoadStaff();
    }

    private void DoClickClearFormInput(object? sender, EventArgs e)
    {
        clearFormInput();
    }
    private void clearFormInput()
    {
        txtStaffID.Text = (staffCount + 1).ToString();
        txtStaffName.Text = "";
        cBStaffGender.SelectedItem = null;
        dtpDOB.Value = DateTime.Now;
        rtxtStaffAddress.Text = "";
        txtContactNumber.Text = "";
        cBStaffPosition.SelectedItem = null;
        dtpHiredDate.Value = DateTime.Now;
        picStaff.Image = null;
    }
    private void StaffAddForm_Load(object sender, EventArgs e)
    {
        LoadStaff();
        if (txtStaffName.Text == "")

            txtStaffID.Text = (staffCount + 1).ToString();

    }
    private void LoadStaff()
    {
        try
        {
            var result = StaffFunc.GetAllStaff(Program.Connection);
            if (result.LastOrDefault() != null) { staffCount = result.LastOrDefault().StaffID; }
            else { staffCount = 0; }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Retriving staff", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    public void LoadStaffDetails(Staff staff)
    {
        if (staff == null)
            throw new ArgumentNullException(nameof(staff));
        txtStaffID.Clear();
        txtStaffID.Text = staff.StaffID.ToString();
        txtStaffName.Text = staff.StaffName;
        cBStaffGender.SelectedItem = staff.Gender;
        dtpDOB.Value = staff.BirthDate ?? DateTime.Now;
        cBStaffPosition.Text = staff.StaffPosition;
        rtxtStaffAddress.Text = staff.StaffAddress;
        txtContactNumber.Text = staff.ContactNumber;
        dtpHiredDate.Value = staff.HiredDate ?? DateTime.Now;


        if (staff.Photo != null)
        {
            picStaff.Image = ConvertImageClass.ConvertByteArrayToImage(staff.Photo);
        }
        else
        {
            picStaff.Image = null;
        }

        effectedStaff = staff;
    }


    public event LoadingEventHandler? StaffLoadingChanged;
}
