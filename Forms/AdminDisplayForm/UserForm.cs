﻿using OOADPRO.Models;
using OOADPRO.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace OOADPRO.Forms.AdminDisplayForm;

public partial class UserForm : Form
{
    int userCount = 0;
    private System.Windows.Forms.Timer clickTimer;
    private const int DoubleClick = 300;
    public UserForm()
    {
        InitializeComponent();
        btnAddUser.Click += DoClickAddUser;
        clickTimer = new System.Windows.Forms.Timer();
        clickTimer.Interval = DoubleClick;
        clickTimer.Tick += ClickTimer_Tick;
    }
    private User _selectedUser;
    private void DoClickAddUser(object? sender, EventArgs e)
    {
        UserAddForm userAddForm = new UserAddForm(this);
        userAddForm.UserLoadingChanged += (sender, result) =>
        {
            if (result)
            {
                flowLayoutPanelUser.Controls.Clear();
                LoadingDataUser();
            }
        };
        userAddForm.Show();
    }

    private void UserForm_Load(object sender, EventArgs e)
    {
        LoadingDataUser();
    }
    private void LoadingDataUser()
    {
        try
        {
            flowLayoutPanelUser.Padding = new Padding(20, 20, 20, 20);
            var result = UserFunc.GetAllUser(Program.Connection);
            foreach (var user in result)
            {
                Panel userPanel = new Panel
                {
                    Width = 185,
                    Height = 250,
                    BorderStyle = BorderStyle.FixedSingle,
                    Padding = new Padding(20)
                };
                userPanel.Margin = new Padding(20, 20, 20, 20);
                PictureBox pictureBox = new PictureBox
                {
                    Width = 180,
                    Height = 180,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Image = user.Staff.Photo != null ? ConvertImageClass.ConvertByteArrayToImage(user.Staff.Photo) : null
                };

                Label userNameLabel = new Label
                {
                    Text = user.Username,
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    Size = new Size(180, 30),
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Location = new Point(5, 190)
                };

                Label userPosition = new Label
                {
                    Text = user.Staff.StaffPosition,
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    Size = new Size(180, 30),
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Location = new Point(5, 210)
                };
                

                pictureBox.MouseDown += (s, e) =>
                {
                    if (e.Clicks == 1)
                    {
                        _selectedUser = user;
                        clickTimer.Start();
                    }
                    else if (e.Clicks == 2)
                    {
                        clickTimer.Stop();
                        DeleteUserById(user.UserID);
                    }
                };

                flowLayoutPanelUser.Padding = new Padding(20);
                userPanel.Controls.Add(pictureBox);
                userPanel.Controls.Add(userNameLabel);
                userPanel.Controls.Add(userPosition);
                flowLayoutPanelUser.Controls.Add(userPanel);
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
        if (_selectedUser != null)
        {
            LoadUserForUpdate(_selectedUser);
        }
    }

    private void LoadUserForUpdate(User user)
    {
        UserAddForm updateForm = new UserAddForm(this);
        updateForm.LoadUserDetails(user);
        updateForm.ShowDialog();
        flowLayoutPanelUser.Controls.Clear();
        LoadingDataUser();
    }
    private void DeleteUserById(int userID)
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
                var result = UserFunc.GetAllUser(Program.Connection);
                bool isDeleted = UserFunc.DeleteUser(Program.Connection, userID);
                if (isDeleted)
                {
                    MessageBox.Show("User deleted successfully!");
                    flowLayoutPanelUser.Controls.Clear();
                    LoadingDataUser();
                }
                else
                {
                    MessageBox.Show("Failed to delete the user member.");
                }
            }
            else
            {
              
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting user: {ex.Message}");
        }
    }
}
