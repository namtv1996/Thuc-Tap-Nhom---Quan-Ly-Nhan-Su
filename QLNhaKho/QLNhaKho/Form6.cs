using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLNhaKho
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
            Load += Form6_Load;
            btnRefresh.Click += BtnRefresh_Click;
            btnAdd.Click += BtnAdd_Click;
            dgvList.DataError += DgvList_DataError;
            btnEdit.Click += BtnEdit_Click;
            txtDayOfBirth.Validating += TxtDayOfBirth_Validating;
        }

        private void TxtDayOfBirth_Validating(object sender, CancelEventArgs e)
        {
            string s = "Ngay khong hop le";
            if (s.IsNumber())
            {
                switch (cbxMonthOfBirth.Text)
                {
                    case "1":
                    case "3":
                    case "5":
                    case "7":
                    case "8":
                    case "10":
                    case "12":
                        if (int.Parse(txtDayOfBirth.Text) > 0 && int.Parse(txtDayOfBirth.Text) <= 31)
                            s = string.Empty;
                        break;

                    case "4":
                    case "6":
                    case "9":
                    case "11":
                        if (int.Parse(txtDayOfBirth.Text) > 0 && int.Parse(txtDayOfBirth.Text) <= 30)
                            s = string.Empty;
                        break;

                    case "2":
                        if (int.Parse(cbxYearOfBirth.Text) % 4 == 0 && int.Parse(cbxYearOfBirth.Text) % 100 != 0)
                        {
                            if (int.Parse(txtDayOfBirth.Text) > 0 && int.Parse(txtDayOfBirth.Text) <= 29)
                                s = string.Empty;
                        }
                        else
                        {
                            if (int.Parse(txtDayOfBirth.Text) > 0 && int.Parse(txtDayOfBirth.Text) <= 28)
                                s = string.Empty;
                        }
                        break;
                }
            }
            if (s != string.Empty)
            {
                errorProvider.SetError(txtDayOfBirth, s);
                
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (txtCustomerName.Text == string.Empty)
            {
                MessageBox.Show("Nhap thong tin");
                return;
            }

        }

        private void DgvList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // DO NOTHING HERE
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (txtCustomerName.Text == string.Empty || txtDayOfBirth.Text == string.Empty
                || !txtDayOfBirth.Text.IsPhoneNumber())
            {
                MessageBox.Show("Nhap thong tin");
                return;
            }
            int res = 0;
            try
            {
                string dob = $"{cbxMonthOfBirth.Text}/{txtDayOfBirth.Text}/{cbxYearOfBirth.Text}";
                using (QLKhoDbContext db = new QLKhoDbContext())
                {
                    object[] obj =
                    {
                    new SqlParameter("@tenkh", txtCustomerName.Text),
                    new SqlParameter("@gioitinh", rbnFemale.Checked ? "Nu" : "Nam"),
                    new SqlParameter("@ngaysinh", DateTime.Parse(dob)),
                    new SqlParameter("@diachi", rtbCustomerAddress.Text),
                    new SqlParameter("@sdt", txtCustomerPN.Text)
                };
                    res = db.Database.ExecuteSqlCommand("sp_customer_insert @tenkh, @gioitinh, @ngaysinh, " +
                        "@diachi, @sdt", obj);
                }

                MessageBox.Show($"{res} ban ghi da cap nhat");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex}");
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtCustomerID.Text = string.Empty;
            txtCustomerName.Text = string.Empty;
            txtDayOfBirth.Text = string.Empty;
            rtbCustomerAddress.Text = string.Empty;
            txtCustomerPN.Text = string.Empty;
            txtSearch.Text = string.Empty;
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            using (QLKhoDbContext db = new QLKhoDbContext())
            {
                dgvList.DataSource = db.KhachHangs.Select(x => x).ToList();
            }
            for (int k = 1980; k <= 2017; k++)
            {
                cbxYearOfBirth.Items.Add($"{k}\n");
            }

        }
    }
}
