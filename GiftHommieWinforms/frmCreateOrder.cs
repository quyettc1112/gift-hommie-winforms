﻿using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GiftHommieWinforms
{
    public partial class frmCreateOrder : Form
    {
        private IProductRepository productRepository = new ProductRepository();
        private IOrderRepository orderRepository = new OrderRepository();
        private IUserRepository userRepository = new UserRepository();
        private BindingSource bindingSource = null;
        private List<Product> selectedProducts = new List<Product>();
        public frmCreateOrder()
        {
            InitializeComponent();
        }
        private void frmCreateOrder_Load(object sender, EventArgs e)
        {
            HomeLoadData();
            List<User> users = userRepository.GetAll().Where(u => u.Role.Equals("CUSTOMER")).ToList();
            cbCustomer.DataSource = users;
            cbCustomer.ValueMember = "Username";
            cbCustomer.DisplayMember = "";
        }
        private void HomeLoadData()
        {
            // Load products
            List<Product> products = productRepository.GetAllWithFilter(
                "",
                txtProductNameSearch.Text,
                "", "",
                "", "", 0,
                true
                );

            HomeLoadDataToGridView(products);
            

            

        }
        private void HomeLoadDataToGridView(IEnumerable<Product> products)
        {
            if (products == null)
                products = new List<Product>()
                {
                };

            try
            {


                bindingSource = new BindingSource();
                bindingSource.DataSource = products;

                HomeReBinding();

                dgvProducts.DataSource = null;
                dgvProducts.DataSource = bindingSource;
                dgvProducts.Columns["Id"].Visible = false;
                dgvProducts.Columns["Avatar"].Visible = false;
                dgvProducts.Columns["Status"].Visible = false;
                dgvProducts.Columns["Carts"].Visible = false;
                dgvProducts.Columns["Category"].Visible = false;
                dgvProducts.Columns["CategoryId"].Visible = false;
                dgvProducts.Columns["OrderDetails"].Visible = false;
                dgvProducts.Columns["isDelete"].Visible = false;
                setRowNumber(dgvProducts);
                LoadChoosenItems();



                //if (products.Count() == 0)
                //{
                //    ClearText();
                //    //btnNext.Enabled = false;
                //    //btnBack.Enabled = false;
                //}
                //else
                //{
                //    //btnNext.Enabled = true;
                //    //btnBack.Enabled = true;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void setRowNumber(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
            }
        }
        private void HomeReBinding()
        {
            lbProductName.DataBindings.Clear();
            txtPrice.DataBindings.Clear();
            txtAvailable.DataBindings.Clear();
            txtDesc.DataBindings.Clear();
            pbProductAvatar.DataBindings.Clear();


            lbProductName.DataBindings.Add("Text", bindingSource, "Name");
            txtPrice.DataBindings.Add("Text", bindingSource, "Price");
            txtAvailable.DataBindings.Add("Text", bindingSource, "Quantity");
            txtDesc.DataBindings.Add("Text", bindingSource, "Description");
            pbProductAvatar.DataBindings.Add(new System.Windows.Forms.Binding(
                                "ImageLocation", bindingSource, "Avatar", true));

        }

        private void HomeClearText()
        {
            lbProductName.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtAvailable.Text = string.Empty;
            txtDesc.Text = string.Empty;
        }

        private void LoadChoosenItems()
        {
            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                int id = (int)row.Cells["Id"].Value;
                row.Cells["Check"].Value = selectedProducts.SingleOrDefault(x => x.Id == id) != null;
            }
        }

        private bool flagLoadSelected = false;
        private void LoadSelectedProducts()
        {
            flagLoadSelected = true;
            dgvSelectedProducts.DataSource = null;
            dgvSelectedProducts.DataSource = selectedProducts;
            dgvSelectedProducts.Columns["Id"].Visible = false;
            dgvSelectedProducts.Columns["Avatar"].Visible = false;
            dgvSelectedProducts.Columns["Status"].Visible = false;
            dgvSelectedProducts.Columns["Carts"].Visible = false;
            dgvSelectedProducts.Columns["Category"].Visible = false;
            dgvSelectedProducts.Columns["CategoryId"].Visible = false;
            dgvSelectedProducts.Columns["OrderDetails"].Visible = false;
            dgvSelectedProducts.Columns["isDelete"].Visible = false;
            dgvSelectedProducts.Columns["Quantity"].ReadOnly = false;
            dgvSelectedProducts.Columns["Description"].Visible = false;
            dgvSelectedProducts.Columns["Price"].ReadOnly = true;
            dgvSelectedProducts.Columns["Name"].ReadOnly = true;
            
            // Add the column to the DataGridView
            if (dgvSelectedProducts.Columns["Total"] == null)
                dgvSelectedProducts.Columns.Add("Total", "Total");

            //Calculate and assign the total value for each row
            foreach (DataGridViewRow row in dgvSelectedProducts.Rows)
                {
                    dgvSelectedProducts.Columns["Total"].ReadOnly = true;
                    int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                    decimal price = Convert.ToDecimal(row.Cells["Price"].Value);

                    decimal total = quantity * price;

                    row.Cells["Total"].Value = total;
                }
            dgvSelectedProducts.Columns["Total"].DisplayIndex = 4;
            dgvSelectedProducts.Columns["Total"].DataPropertyName = "Total";

            flagLoadSelected = false;
        }
        private void pbProductAvatar_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Address_Click(object sender, EventArgs e)
        {

        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int c, r;

            if (e.ColumnIndex == 0 && dgvProducts.RowCount > 0)
            {
                c = e.ColumnIndex;
                r = e.RowIndex;

                dgvProducts.Rows[r].Cells[c].Value = !((bool)dgvProducts.Rows[r].Cells[c].Value);

                bool check = (bool)dgvProducts.Rows[r].Cells[c].Value;
                int id = (int)dgvProducts.Rows[r].Cells["Id"].Value;

                if (check == false)
                {
                    selectedProducts = selectedProducts.Where(p => p.Id != id).ToList();
                }
                else if (selectedProducts.SingleOrDefault(p => p.Id == id) == null)
                {
                    Product product = productRepository.Get(id);
                    product.Quantity = 1;
                    selectedProducts.Add(product);
                }
            }

            LoadSelectedProducts();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show("dfsafd");
            LoadSelectedProducts();
        }

        private void dgvSelectedProducts_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (flagLoadSelected == false)
            //{
            //    int c, r;

            //    if (dgvProducts.RowCount > 0)
            //    {
            //        DataGridViewRow row = dgvProducts.Rows[e.RowIndex];
            //        c = e.ColumnIndex;
            //        r = e.RowIndex;
            //        dgvSelectedProducts.Columns["Total"].ReadOnly = true;
            //        int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
            //        decimal price = Convert.ToDecimal(row.Cells["Price"].Value);

            //        decimal total = quantity * price;

            //        row.Cells["Total"].Value = total;
            //    }
            //}
                
        }

        private void dgvSelectedProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dgvSelectedProducts.Rows)
            {
                dgvSelectedProducts.Columns["Total"].ReadOnly = true;
                int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                decimal price = Convert.ToDecimal(row.Cells["Price"].Value);

                decimal total = quantity * price;

                row.Cells["Total"].Value = total;
            }
        }
    }
}
