using OOADPRO.Models;
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

namespace OOADPRO.Forms.CashierDisplayForm
{
    public partial class CashierProductForm : Form
    {
        int productCount = 0;

        public CashierProductForm()
        {
            InitializeComponent(); 
            LoadingDataProducts();
            this.Controls.Add(txtSearch);
            txtSearch.TextChanged += TxtProductName_TextChanged;


        }
        private void SearchProducts(string searchText)
        {
            var allProducts = ProductFunc.GetAllProducts(Program.Connection);
            var filteredProducts = allProducts
                .Where(p => p.ProductName.ToLower().Contains(searchText.ToLower()))
                .ToList();
            LoadingDataProducts(filteredProducts);
        }
        private void TxtProductName_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                SearchProducts(searchText);
            }
            else
            {
                LoadingDataProducts();
            }
        }
        private void LoadingDataProducts(List<Products> products = null)
        {
            try
            {
                flowLayoutPanelProducts.Controls.Clear();
                flowLayoutPanelProducts.Padding = new Padding(20, 20, 20, 20);
                var result = products ?? ProductFunc.GetAllProducts(Program.Connection);

                foreach (var product in result)
                {
                    Panel productPanel = CreateProductPanel(product);
                    flowLayoutPanelProducts.Controls.Add(productPanel);
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
                Width = 185,
                Height = 270,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(20)
            };
            PictureBox pictureBox = new PictureBox
            {
                Width = 180,
                Height = 180,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = product.ProductImage != null ? ConvertImageClass.ConvertByteArrayToImage(product.ProductImage) : null
            };

            Label productNameLabel = new Label
            {
                Text = $"Name: {product.ProductName}",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Size = new Size(180, 30),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                Location = new Point(5, 220)
            };

            Label productStockLabel = new Label
            {
                Text = $"Stock: {product.ProductsStock}",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Size = new Size(180, 30),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                Location = new Point(5, 250)
            };

            Label categoryNameLabel = new Label
            {
                Text = $"Category: {product.Category.CategoryName}",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Size = new Size(180, 30),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                Location = new Point(5, 190)
            };
            productPanel.Controls.Add(pictureBox);
            productPanel.Controls.Add(categoryNameLabel);
            productPanel.Controls.Add(productNameLabel);
            productPanel.Controls.Add(productStockLabel);

            return productPanel;
        }
    }
}