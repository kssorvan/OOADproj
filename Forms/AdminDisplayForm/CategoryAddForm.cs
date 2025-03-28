using OOADPRO.Models;
using OOADPRO.Utilities;

namespace OOADPRO.Forms.AdminDisplayForm;

public partial class CategoryAddForm : Form
{
    int categoryCount = 0;
    int indexOfUpdateCategory;
    Category? effectedCategory = null;
    public CategoryAddForm()
    {
        InitializeComponent();
        btnClear.Click += DoClickClearFormInput;
        btnInsert.Click += DoClickInsertCategory;
        btnUpdate.Click += DoClickUpdateCategory;
    }

    private void DoClickUpdateCategory(object? sender, EventArgs e)
    {
        if (txtCategoryName.Text == "" || txtCategoryName.Text.Trim().Length > 100)
        {
            MessageBox.Show("Category is required or name too long", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }

        if (rtxtCategoryDescription.Text.Trim().Length > 1000)
        {
            MessageBox.Show("Category Descriptionis required or Descriptionis too long", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        effectedCategory.CategoryName = txtCategoryName.Text.Trim();
        effectedCategory.CategoryDescription = rtxtCategoryDescription.Text.Trim();
        try
        {
            var result = CategoryFunc.UpdateCategory(Program.Connection, effectedCategory);
            if (result == true)
            {
                MessageBox.Show($"Successfully updated category with the id {txtCategoryID.Text}");
                CategoryHanlder?.Invoke(this, result);
            }
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Submitting", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private void DoClickInsertCategory(object? sender, EventArgs e)
    {
        if (txtCategoryName.Text == "" || txtCategoryName.Text.Trim().Length > 100)
        {
            MessageBox.Show("Category is required or name too long", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }

        if ( rtxtCategoryDescription.Text.Trim().Length > 1000)
        {
            MessageBox.Show("Category Descriptionis required or Descriptionis too long", "Creating",
               MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }


        Category newCategory = new Category()
        {
            CategoryName = txtCategoryName.Text.Trim(),
            CategoryDescription = rtxtCategoryDescription.Text.Trim(),
        };
        try
        {
            var result = CategoryFunc.AddCategory(Program.Connection, newCategory);
            if (result == true)
            {
                MessageBox.Show($"Successfully inserted category with the id {txtCategoryID.Text}");
                CategoryHanlder?.Invoke(this,result);
            }
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Submitting", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        clearFormInput();
    }

    private void DoClickClearFormInput(object? sender, EventArgs e)
    {
        clearFormInput();
    }
    private void CategoryAddForm_Load(object sender, EventArgs e)
    {
        LoadCategory();
        if (txtCategoryName.Text == "")
            txtCategoryID.Text = (categoryCount + 1).ToString();
    }
    private void LoadCategory()
    {
        try
        {
            var result = CategoryFunc.GetAllCategory(Program.Connection);
            if (result.LastOrDefault() != null) { categoryCount = result.LastOrDefault().CategoryID; }
            else { categoryCount = 0; }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Retriving category", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void clearFormInput()
    {
        txtCategoryID.Text = (categoryCount + 1).ToString();
        txtCategoryName.Text = "";
        rtxtCategoryDescription.Text = "";
    }
    public void LoadCategoryToUpdate(Category category)
    {
        if (category == null)
           MessageBox.Show("Category is null");
        txtCategoryID.Clear();
        txtCategoryID.Text = category.CategoryID.ToString();
        txtCategoryName.Text = category.CategoryName;
        rtxtCategoryDescription.Text = category.CategoryDescription;
        effectedCategory = category;
    }
    public event LoadingEventHandler? CategoryHanlder;

}
