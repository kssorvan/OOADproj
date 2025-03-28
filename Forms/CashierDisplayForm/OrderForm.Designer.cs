namespace OOADPRO.Forms.CashierDisplayForm
{
    partial class OrderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderForm));
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            btnAddStaff = new Button();
            dgvOrder = new DataGridView();
            OrderID = new DataGridViewTextBoxColumn();
            DataOrder = new DataGridViewTextBoxColumn();
            TotalPrice = new DataGridViewTextBoxColumn();
            CustomerID = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvOrder).BeginInit();
            SuspendLayout();
            // 
            // btnAddStaff
            // 
            btnAddStaff.FlatStyle = FlatStyle.Flat;
            btnAddStaff.ForeColor = Color.FromArgb(243, 244, 243);
            btnAddStaff.Image = (Image)resources.GetObject("btnAddStaff.Image");
            btnAddStaff.Location = new Point(34, 44);
            btnAddStaff.Name = "btnAddStaff";
            btnAddStaff.Size = new Size(49, 46);
            btnAddStaff.TabIndex = 2;
            btnAddStaff.UseVisualStyleBackColor = true;
            // 
            // dgvOrder
            // 
            dgvOrder.AllowUserToAddRows = false;
            dgvOrder.AllowUserToDeleteRows = false;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = SystemColors.Control;
            dataGridViewCellStyle7.Font = new Font("Sitka Small", 12F, FontStyle.Bold);
            dataGridViewCellStyle7.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            dgvOrder.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            dgvOrder.ColumnHeadersHeight = 30;
            dgvOrder.Columns.AddRange(new DataGridViewColumn[] { OrderID, DataOrder, TotalPrice, CustomerID });
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Window;
            dataGridViewCellStyle8.Font = new Font("Sitka Small", 11.249999F, FontStyle.Bold);
            dataGridViewCellStyle8.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            dgvOrder.DefaultCellStyle = dataGridViewCellStyle8;
            dgvOrder.Location = new Point(34, 112);
            dgvOrder.Name = "dgvOrder";
            dgvOrder.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = SystemColors.Control;
            dataGridViewCellStyle9.Font = new Font("Sitka Small", 11.249999F, FontStyle.Bold);
            dataGridViewCellStyle9.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            dgvOrder.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            dgvOrder.Size = new Size(1029, 607);
            dgvOrder.TabIndex = 4;
            // 
            // OrderID
            // 
            OrderID.HeaderText = "Order ID";
            OrderID.Name = "OrderID";
            OrderID.ReadOnly = true;
            OrderID.Width = 200;
            // 
            // DataOrder
            // 
            DataOrder.HeaderText = "Date Time";
            DataOrder.Name = "DataOrder";
            DataOrder.ReadOnly = true;
            DataOrder.Width = 300;
            // 
            // TotalPrice
            // 
            TotalPrice.HeaderText = "Total Price";
            TotalPrice.Name = "TotalPrice";
            TotalPrice.ReadOnly = true;
            TotalPrice.Width = 300;
            // 
            // CustomerID
            // 
            CustomerID.HeaderText = "CustomerID";
            CustomerID.Name = "CustomerID";
            CustomerID.ReadOnly = true;
            CustomerID.Width = 200;
            // 
            // OrderForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1076, 763);
            Controls.Add(dgvOrder);
            Controls.Add(btnAddStaff);
            FormBorderStyle = FormBorderStyle.None;
            Name = "OrderForm";
            Text = "OrderForm";
            Load += OrderForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvOrder).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnAddStaff;
        private DataGridView dgvOrder;
        private DataGridViewTextBoxColumn OrderID;
        private DataGridViewTextBoxColumn DataOrder;
        private DataGridViewTextBoxColumn TotalPrice;
        private DataGridViewTextBoxColumn CustomerID;
    }
}