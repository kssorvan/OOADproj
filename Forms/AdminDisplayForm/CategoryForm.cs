using OOADPRO.Models;
using OOADPRO.Utilities;

namespace OOADPRO.Forms.AdminDisplayForm;

public partial class CategoryForm : Form
{
    Category? effectedCategory = null;
    public CategoryForm()
    {
        InitializeComponent();
        btnAddCategory.Click += DoClickAddCategory;
        btnClickDelete.Click += DoClickDeleteCategory;
        btnClickUpdate.Click += DoClickUpdateCategory;
        dgvCategory.CellClick += Select_Handling_Category;
    }

    private void Select_Handling_Category(object? sender, EventArgs e)
    {
        if (dgvCategory.CurrentRow == null) return;
        int no = (int)dgvCategory.CurrentRow.Cells["CategoryID"].Value;
        try
        {
            effectedCategory = CategoryFunc.GetOneCategory(Program.Connection, no);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Here: {ex.Message}");
        }
    }

    private void DoClickUpdateCategory(object? sender, EventArgs e)
    {
        CategoryAddForm categoryAddForm = new CategoryAddForm();
        categoryAddForm.LoadCategoryToUpdate(effectedCategory);
        categoryAddForm.CategoryHanlder += (sender, result) =>
        {
            if (result)
                LoadingDataCategory();
        };
        categoryAddForm.Show();
    }

    private void DoClickDeleteCategory(object? sender, EventArgs e)
    {
        int.TryParse(effectedCategory.CategoryID.ToString(), out int id);
        bool isDeleted = CategoryFunc.DeleteCategory(Program.Connection, id);
        if (isDeleted)
        {
            MessageBox.Show("Staff deleted successfully!");
            LoadingDataCategory();
        }
        else
        {
            MessageBox.Show("Failed to delete the staff member.");
        }
    }

    private void DoClickAddCategory(object? sender, EventArgs e)
    {
        CategoryAddForm categoryAddForm = new CategoryAddForm();
        categoryAddForm.CategoryHanlder += (sender, result) =>
        {
            if (result)
                LoadingDataCategory();
        };
        categoryAddForm.Show();


    }

    private void CategoryForm_Load(object sender, EventArgs e)
    {
        LoadingDataCategory();
    }
    private void LoadingDataCategory()
    {
        try
        {
            var result = CategoryFunc.GetAllCategory(Program.Connection);

            dgvCategory.Rows.Clear();
            foreach (var category in result)
            {
                dgvCategory.Rows.Add(category.CategoryID, category.CategoryName, category.CategoryDescription);
            }

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Retriving Room", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
