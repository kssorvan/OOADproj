using Microsoft.Data.SqlClient;
using OOADPRO.Models;
using OOADPRO.Utilities;
using System.Data;

namespace OOADPRO.Forms.CashierDisplayForm;

public partial class OrderDetailAddForm : Form
{
    private int? OrderID;
    private float TotalAmount = 0;

    public OrderDetailAddForm(OrderForm orderForm,int? orderId = null)
    {

        InitializeComponent();
        this.OrderID = orderId;
        this.Controls.Add(txtProductName);
        txtProductName.TextChanged += TxtProductName_TextChanged;
        if (OrderID != null)
        {
            LoadOrderDetails(Program.Connection, (int)OrderID);
        }
        else
        {
            LoadProducts();
        }
        buttonpay.Click += btnSave_Click;
        btnClear.Click += cleardata;
    }
    private void SearchProducts(string searchText)
    {
        var allProducts = ProductFunc.GetAllProducts(Program.Connection);
        var filteredProducts = allProducts
            .Where(p => p.ProductName.ToLower().Contains(searchText.ToLower()))
            .ToList();
        LoadProducts(filteredProducts);
    }
    private void TxtProductName_TextChanged(object sender, EventArgs e)
    {
        string searchText = txtProductName.Text.Trim();

        if (!string.IsNullOrEmpty(searchText))
        {
            SearchProducts(searchText);
        }
        else
        {
            LoadProducts();
        }
    }
    private void LoadProducts(List<Products> products = null)
    {
        try
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Padding = new Padding(20, 20, 20, 20);
            var result =products?? ProductFunc.GetAllProducts(Program.Connection);

            foreach (var product in result)
            {
                Panel productPanel = CreateProductPanel(product);
                flowLayoutPanel1.Controls.Add(productPanel);
                productPanel.Margin = new Padding(20, 20, 20, 20);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error Loading Products", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private Panel CreateProductPanel(Products product)
    {
        Panel productPanel = new Panel
        {
            Width = 200,
            Height = 250,
            BorderStyle = BorderStyle.None,
            Margin = new Padding(0)
        };
        productPanel.Margin = new Padding(20, 20, 20, 20);
        PictureBox pictureBox = new PictureBox
        {
            Width = 180,
            Height = 180,
            SizeMode = PictureBoxSizeMode.StretchImage,
            Image = product.ProductImage != null ? ConvertImageClass.ConvertByteArrayToImage(product.ProductImage) : null,
            Tag = product
        };
        pictureBox.Click += (s, e) =>
        {
            AddProductToGrid((Products)pictureBox.Tag);
        };

        Label productNameLabel = new Label
        {
            Text = product.ProductName,
            AutoSize = true,
            Location = new Point(5, 190)
        };
        Label productPriceLabel = new Label
        {
            Text = $"${product.ProductsPrice}",
            AutoSize = true,
            Location = new Point(5, 210)
        };

        productPanel.Controls.Add(pictureBox);
        productPanel.Controls.Add(productNameLabel);
        productPanel.Controls.Add(productPriceLabel);
        return productPanel;
    }

    private void AddProductToGrid(Products product)
    {
        if (product.ProductsStock <= 0)
        {
            MessageBox.Show($"The product '{product.ProductName}' is out of stock.", "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return; 
        }
        foreach (DataGridViewRow row in dataGridView1.Rows)
        {
            if ((int)row.Cells["ProductsID"].Value == product.ProductsID)
            {
                int currentQty = Convert.ToInt32(row.Cells["Qty"].Value);
                if (currentQty < product.ProductsStock) 
                {
                    row.Cells["Qty"].Value = currentQty + 1;
                    CalculateRowAmount(row);
                    UpdateTotalAmount();
                }
                else
                {
                    MessageBox.Show($"Not enough stock for '{product.ProductName}'.", "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }
        }
        int rowIndex = dataGridView1.Rows.Add();
        var newRow = dataGridView1.Rows[rowIndex];
        newRow.Cells["OrderDetailID"].Value = -1;
        newRow.Cells["ProductsID"].Value = product.ProductsID;
        newRow.Cells["ProductsName"].Value = product.ProductName;
        newRow.Cells["Qty"].Value = 1;
        newRow.Cells["UnitPrice"].Value = product.ProductsPrice;

        CalculateRowAmount(newRow);
        UpdateTotalAmount();
    }  
    private void CalculateRowAmount(DataGridViewRow row)
    {
        int qty = Convert.ToInt32(row.Cells["Qty"].Value);
        float unitPrice = Convert.ToSingle(row.Cells["UnitPrice"].Value);
        row.Cells["Amount"].Value = qty * unitPrice;
    }
    private void UpdateTotalAmount()
    {
        TotalAmount = 0;

        foreach (DataGridViewRow row in dataGridView1.Rows)
        {
            TotalAmount += Convert.ToSingle(row.Cells["Amount"].Value);
        }

        txtTotal.Text = $"Total: ${TotalAmount:F2}";

    }
    private void LoadOrderDetails(SqlConnection con, int orderId)
    {
        SqlCommand cmd = new SqlCommand("GetOrderDetailsByOrderID", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@odid", orderId);

        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                int orderDetailId = reader.GetInt32(reader.GetOrdinal("OrderDetailID"));
                int orderQty = reader.GetInt32(reader.GetOrdinal("OrderQty"));
                float unitPrice = reader.GetFloat(reader.GetOrdinal("UnitPrice"));
                string productName = reader.GetString(reader.GetOrdinal("ProductName"));
                int productsId = reader.GetInt32(reader.GetOrdinal("ProductsID"));
                int rowIndex = dataGridView1.Rows.Add();
                DataGridViewRow row = dataGridView1.Rows[rowIndex];

                row.Cells["OrderDetailID"].Value = orderDetailId;
                row.Cells["ProductsID"].Value = productsId;
                row.Cells["ProductName"].Value = productName;
                row.Cells["Qty"].Value = orderQty;
                row.Cells["UnitPrice"].Value = unitPrice;
                CalculateRowAmount(row);
                UpdateTotalAmount();
            }
        }
    }
    public void SaveOrderDetails(SqlConnection con)
    {
        int customerId = OrderFunc.CreateNewCustomer(con);
        float totalPrice = CalculateTotalPrice();
        SqlCommand cmd = new SqlCommand("spCreateNewOrder", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@DateOrder", DateTime.Now);
        cmd.Parameters.AddWithValue("@TotalPrice", totalPrice);
        cmd.Parameters.AddWithValue("@CustomerID", customerId);
        try
        {
            int newOrderId = Convert.ToInt32(cmd.ExecuteScalar());
            InsertOrderDetails(con, newOrderId);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save order details: {ex.Message}");
        }
        finally
        {
            cmd.Dispose();
        }
    }
    public void InsertOrderDetails(SqlConnection con, int orderId)
    {
        foreach (DataGridViewRow row in dataGridView1.Rows)
        {
            if (row.IsNewRow) continue;

            int orderDetailId = Convert.ToInt32(row.Cells["OrderDetailID"].Value);
            int productId = Convert.ToInt32(row.Cells["ProductsID"].Value);
            int orderQty = Convert.ToInt32(row.Cells["Qty"].Value);
            float unitPrice = Convert.ToSingle(row.Cells["UnitPrice"].Value);
            if (orderDetailId == -1)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    Order = new Order { OrderID = orderId },
                    Products = new Products { ProductsID = productId },
                    OrderQty = orderQty,
                    UnitPrice = unitPrice
                };

                OrderFunc.InsertOrderDetail(con, orderDetail);

            }
        }
    }
    private float CalculateTotalPrice()
    {
        float totalPrice = 0;

        foreach (DataGridViewRow row in dataGridView1.Rows)
        {
            if (row.IsNewRow) continue;

            int qty = Convert.ToInt32(row.Cells["Qty"].Value);
            float unitPrice = Convert.ToSingle(row.Cells["UnitPrice"].Value);
            totalPrice += qty * unitPrice;
        }

        return totalPrice;
    }
    private void ResetOrderForm()
    {
        dataGridView1.Rows.Clear();
        TotalAmount = 0;
        txtTotal.Text = "Total: $0.00";
    }
    private void cleardata(object sender, EventArgs e) {
        OrderDetailAdded?.Invoke(this, true);
        ResetOrderForm();
    }
    private void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveOrderDetails(Program.Connection);
            MessageBox.Show("Order details saved successfully!");
            OrderDetailAdded?.Invoke(this, true);
            ResetOrderForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving order details: {ex.Message}");
        }

    }
    public event LoadingEventHandler? OrderDetailAdded;
}
