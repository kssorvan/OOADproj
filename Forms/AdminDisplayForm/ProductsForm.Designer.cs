﻿namespace OOADPRO.Forms.AdminDisplayForm
{
    partial class ProductsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductsForm));
            btnAddProduct = new Button();
            flowLayoutPanelProducts = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // btnAddProduct
            // 
            btnAddProduct.FlatStyle = FlatStyle.Flat;
            btnAddProduct.ForeColor = Color.FromArgb(243, 244, 243);
            btnAddProduct.Image = (Image)resources.GetObject("btnAddProduct.Image");
            btnAddProduct.Location = new Point(36, 36);
            btnAddProduct.Name = "btnAddProduct";
            btnAddProduct.Size = new Size(49, 46);
            btnAddProduct.TabIndex = 2;
            btnAddProduct.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanelProducts
            // 
            flowLayoutPanelProducts.AutoScroll = true;
            flowLayoutPanelProducts.Location = new Point(36, 103);
            flowLayoutPanelProducts.Name = "flowLayoutPanelProducts";
            flowLayoutPanelProducts.Size = new Size(1044, 587);
            flowLayoutPanelProducts.TabIndex = 3;
            // 
            // ProductsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1092, 802);
            Controls.Add(flowLayoutPanelProducts);
            Controls.Add(btnAddProduct);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ProductsForm";
            Text = "ProductsForm";
            Load += ProductsForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button btnAddProduct;
        private FlowLayoutPanel flowLayoutPanelProducts;
    }
}