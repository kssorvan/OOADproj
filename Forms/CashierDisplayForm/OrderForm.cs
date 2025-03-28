using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OOADPRO.Forms.AdminDisplayForm;
using OOADPRO.Models;
using OOADPRO.Utilities;

namespace OOADPRO.Forms.CashierDisplayForm;

public partial class OrderForm : Form
{
    Order? effectedOrder = null;
   
    public OrderForm()
    {
        InitializeComponent();
        btnAddStaff.Click += DoClickAddOrderDetail;
    }

    private void DoClickAddOrderDetail(object? sender, EventArgs e)
    {
        OrderDetailAddForm orderDetailAddForm =  new OrderDetailAddForm(this);
        orderDetailAddForm.OrderDetailAdded += (sender, e) =>
        {
            LoadingDataCategory();
        };
        orderDetailAddForm.Show();
    }

    private void OrderForm_Load(object sender, EventArgs e)
    {
        LoadingDataCategory();
    }

    private void LoadingDataCategory()
    {
        try
        {
            var result = OrderFunc.GetAllOrder(Program.Connection);

            dgvOrder.Rows.Clear();
            foreach (var order in result)
            {
                dgvOrder.Rows.Add(order.OrderID, order.DateOrder, order.TotalPrice,order.Customer.CustomerID);
            }

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Retriving Room", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

}
