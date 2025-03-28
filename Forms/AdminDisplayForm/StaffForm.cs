using Microsoft.Data.SqlClient;
using OOADPRO.Models;
using OOADPRO.Utilities;

namespace OOADPRO.Forms.AdminDisplayForm;

public partial class StaffForm : Form
{
    int staffCount = 0;
    private System.Windows.Forms.Timer clickTimer;
    private const int DoubleClick = 300;
    public StaffForm()
    {
        InitializeComponent();
        btnAddStaff.Click += DoClickAddStaff;
        clickTimer = new System.Windows.Forms.Timer();
        clickTimer.Interval = DoubleClick;
        clickTimer.Tick += ClickTimer_Tick;
    }
    private Staff _selectedStaff;
    private void DoClickAddStaff(object? sender, EventArgs e)
    {
        StaffAddForm staffAddForm = new StaffAddForm(this);
        staffAddForm.StaffLoadingChanged += (sender, result) =>
        {
            if (result)
            {
                flowLayoutPanelStaff.Controls.Clear();
                LoadingDataStaff();
            }
        };
        staffAddForm.Show();

    }
    private void StaffForm_Load(object sender, EventArgs e)
    {
        LoadingDataStaff();
    }
    private void LoadingDataStaff()
    {
        try
        {
            flowLayoutPanelStaff.Padding = new Padding(20, 20, 20, 20);
            var result = StaffFunc.GetAllStaff(Program.Connection);
            foreach (var staff in result)
            {
                Panel productPanel = new Panel
                {
                    Width = 185,
                    Height = 250,
                    BorderStyle = BorderStyle.FixedSingle,
                    Padding = new Padding(20)
                };
                productPanel.Margin = new Padding(20, 20, 20, 20);
                PictureBox pictureBox = new PictureBox
                {
                    Width = 180,
                    Height = 180,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Image = staff.Photo != null ? ConvertImageClass.ConvertByteArrayToImage(staff.Photo) : null
                };

                Label staffNameLabel = new Label
                {
                    Text = staff.StaffName,
                    Font = new Font("Arial", 12, FontStyle.Bold), 
                    Size = new Size(180, 30),
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Location = new Point(5, 190)
                };

                Label staffPosition = new Label
                {
                    Text = staff.StaffPosition,             
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    Size = new Size(180, 30),
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Location = new Point(5, 210)
                };
                staffPosition.TextAlign = ContentAlignment.MiddleCenter;

                pictureBox.MouseDown += (s, e) =>
                {
                    if (e.Clicks == 1)
                    {
                        _selectedStaff = staff; 
                        clickTimer.Start();   
                    }
                    else if (e.Clicks == 2)
                    {
                        clickTimer.Stop();  
                        DeleteStaffById(staff.StaffID); 
                    }
                };

                flowLayoutPanelStaff.Padding = new Padding(20);
                productPanel.Controls.Add(pictureBox);
                productPanel.Controls.Add(staffNameLabel);
                productPanel.Controls.Add(staffPosition);
                flowLayoutPanelStaff.Controls.Add(productPanel);
            }

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Retriving staff", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void ClickTimer_Tick(object? sender, EventArgs e)
    {
        clickTimer.Stop();
        if (_selectedStaff != null)
        {
            LoadStaffForUpdate(_selectedStaff);
        }
    }

    private void LoadStaffForUpdate(Staff staff)
    {
        StaffAddForm updateForm = new StaffAddForm(this);
        updateForm.LoadStaffDetails(staff);
        updateForm.ShowDialog();
        flowLayoutPanelStaff.Controls.Clear();
        LoadingDataStaff();
    }
    private void DeleteStaffById(int staffid)
    {
        try
        {
            DialogResult dialogResult = MessageBox.Show(
                "Are you sure you want to delete this member?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (dialogResult == DialogResult.Yes)
            {
                var result = StaffFunc.GetAllStaff(Program.Connection);
                bool isDeleted = StaffFunc.DeleteStaff(Program.Connection, staffid);
                if (isDeleted)
                {
                    MessageBox.Show("Staff deleted successfully!");
                    flowLayoutPanelStaff.Controls.Clear();
                    LoadingDataStaff();
                }
                else
                {
                    MessageBox.Show("Failed to delete the staff member.");
                }
            }
            else
            {
              
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting staff: {ex.Message}");
        }
    }


}
