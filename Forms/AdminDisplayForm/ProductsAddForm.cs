using OOADPRO.Models;
using OOADPRO.Utilities;
using System.Drawing.Imaging;

namespace OOADPRO.Forms.AdminDisplayForm;

public partial class ProductsAddForm : Form
{
    string? imgLocation = "";
    int productCount = 0;
    int indexOfUpdateProduct;
    Products? effectedProduct = null;
    public ProductsAddForm(ProductsForm productsForm)
    {
        InitializeComponent();
        btnClear.Click += DoClickClearFormInput;
        btnInsert.Click += DoClickInsertProduct;
        btnUpdate.Click += DoClickUpdateProduct;
        btnUploadPhoto.Click += DoClickUploadProductPhoto;
    }

    private void DoClickUpdateProduct(object? sender, EventArgs e)
    {
        byte[] ProductImages = null;
        if (txtProductName.Text == "" || txtProductName.Text.Trim().Length > 100)
        {
            MessageBox.Show("Product is required or name too long", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        if (cBCategoryID.SelectedItem == null)
        {
            MessageBox.Show("Category is required", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        if (rtxtProductDescription.Text == "" || rtxtProductDescription.Text.Trim().Length > 1000)
        {
            MessageBox.Show("Staff Address is required or address too long", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        if (decimal.TryParse(txtPrice.Text.ToString(), out decimal productPrice) == false)
        {
            MessageBox.Show("Product price is required or something wrong", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        if (int.TryParse(txtProductStock.Text.ToString(), out int productStock) == false)
        {
            MessageBox.Show("Product stock is required or something wrong", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        effectedProduct.ProductName = txtProductName.Text.Trim();
        effectedProduct.ProductsPrice = productPrice;
        effectedProduct.ProductDescription = rtxtProductDescription.Text.Trim();
        effectedProduct.ProductsStock = productStock;
        effectedProduct.Category.CategoryID = int.Parse(cBCategoryID.SelectedItem.ToString());


        if (picProduct.Image != null)
        {
            Image image = picProduct.Image;
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            ProductImages = ms.ToArray();
        }
        effectedProduct.ProductImage = ProductImages;
        try
        {

            var result = ProductFunc.UpdateProducts(Program.Connection, effectedProduct);
            if (result == true)
            {

                MessageBox.Show($"Successfully updated an existing product with the id {txtProductID.Text}");
                ProductLoadingChanged?.Invoke(this, result);
            }

        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to update an existing product > {ex.Message}");
        }
    }

    private void DoClickInsertProduct(object? sender, EventArgs e)
    {
        byte[] ProductImages = null;
        if (txtProductName.Text == "" || txtProductName.Text.Trim().Length > 100)
        {
            MessageBox.Show("Product Name is required or name too long", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        if (cBCategoryID.SelectedItem == null)
        {
            MessageBox.Show("Category  is required", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        if (rtxtProductDescription.Text == "" || rtxtProductDescription.Text.Trim().Length > 1000)
        {
            MessageBox.Show("Product description is required ", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        if (decimal.TryParse(txtPrice.Text.ToString(), out decimal productPrice) == false)
        {
            MessageBox.Show("Product price is required or something wrong", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }
        if (int.TryParse(txtProductStock.Text.ToString(), out int productStock) == false)
        {
            MessageBox.Show("Product stock is required or something wrong", "Creating",
            MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }

        if (imgLocation != "")
        {
            FileStream stream = new FileStream(imgLocation, FileMode.Open, FileAccess.Read);
            BinaryReader breader = new BinaryReader(stream);
            ProductImages = breader.ReadBytes((int)stream.Length);
        }

        Products newProduct = new Products()
        {
            ProductName = txtProductName.Text.Trim(),
            ProductsPrice = productPrice,
            ProductDescription = rtxtProductDescription.Text.Trim(),
            ProductsStock = productStock,
            ProductImage = ProductImages,
            Category = new Category() { CategoryID = int.Parse(cBCategoryID.SelectedItem.ToString()) }
        };
        try
        {
            var result = ProductFunc.AddProducts(Program.Connection, newProduct);
            if (result == true)
            {
                MessageBox.Show($"Successfully inserted product with the id {txtProductID.Text}");
                ProductLoadingChanged?.Invoke(this, result);
            }

        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Submitting", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        clearFormInput();
    }

    private void DoClickClearFormInput(object? sender, EventArgs e)
    {
        clearFormInput();
    }
    private void clearFormInput()
    {
        txtProductID.Text = (productCount + 1).ToString();
        cBCategoryID.SelectedItem = null;
        txtProductName.Text = "";
        txtPrice.Text = "";
        txtProductStock.Text = "";
        rtxtProductDescription.Text = "";
        picProduct.Image = null;
    }
    private void DoClickUploadProductPhoto(object? sender, EventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filter = "SELECT Photo(*.Jpg; *.png; *.Gif)|*.Jpg; *.png; *.Gif";
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            imgLocation = dialog.FileName.ToString();
            picProduct.ImageLocation = imgLocation;
        }
    }
    private void LoadingDataCategoryID()
    {
        try
        {
            var result = CategoryFunc.GetAllCategoryID(Program.Connection);
            List<string> ls = new List<string>();
            foreach (var category in result)
            {
                ls.Add(category.CategoryID.ToString());
            }
            cBCategoryID.DataSource = ls;
            
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Retriving Category ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ProductsAddForm_Load(object sender, EventArgs e)
    {
        LoadingDataCategoryID();
        LoadProduct();

        if (txtProductName.Text == "")
        {
            txtProductID.Text = (productCount + 1).ToString();
            cBCategoryID.SelectedIndex = -1;
        }
        else
        {
            cBCategoryID.SelectedItem = effectedProduct.Category.CategoryID.ToString();
        }

    }
    private void LoadProduct()
    {
        try
        {
            var result = ProductFunc.GetAllProducts(Program.Connection);
            if (result.LastOrDefault() != null) { productCount = result.LastOrDefault().ProductsID; }
            else { productCount = 0; }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Retriving Payment", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    public void LoadProductDetails(Products products)
    {
        if (products == null)
            return;

        txtProductID.Clear();
        txtProductID.Text = products.ProductsID.ToString();
        
       // cBCategoryID.SelectedItem = products.Category.CategoryID.ToString();
        txtProductName.Text = products.ProductName;
        txtPrice.Text = products.ProductsPrice.ToString();
        txtProductStock.Text = products.ProductsStock.ToString();
        rtxtProductDescription.Text = products.ProductDescription;
        if (products.ProductImage != null)
        {
            picProduct.Image = ConvertImageClass.ConvertByteArrayToImage(products.ProductImage);
        }
        else
        {
            picProduct.Image = null;
        }

        effectedProduct = products;
    }
    public event LoadingEventHandler? ProductLoadingChanged;
}
