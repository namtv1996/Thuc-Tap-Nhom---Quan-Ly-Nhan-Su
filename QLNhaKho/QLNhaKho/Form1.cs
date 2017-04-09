using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace QLNhaKho
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LockControls();
            Load += Form1_Load;
            dgvList.DataError += DgvList_DataError;
            btnImport.Click += BtnAdd_Click;
            btnSave.Click += BtnSave_Click;
            btnEdit.Click += BtnEdit_Click;
            btnExport.Click += BtnDelete_Click;
            dgvList.CellClick += DgvList_CellClick;
            Activated += Form1_Activated;
            btnReport.Click += BtnReport_Click;
        }

        private void BtnReport_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            LoadData();
        }

        private void DgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvList.Rows[e.RowIndex].Cells[0].Value != null)
                txtID.Text = dgvList.Rows[e.RowIndex].Cells[0].Value.ToString();
            if (dgvList.Rows[e.RowIndex].Cells[1].Value != null)
                txtName.Text = dgvList.Rows[e.RowIndex].Cells[1].Value.ToString();
            if (dgvList.Rows[e.RowIndex].Cells[2].Value != null)
                rtbState.Text = dgvList.Rows[e.RowIndex].Cells[2].Value.ToString();
            if (dgvList.Rows[e.RowIndex].Cells[3].Value != null)
                dtpProductDate.Value = (DateTime)dgvList.Rows[e.RowIndex].Cells[3].Value;
            if (dgvList.Rows[e.RowIndex].Cells[4].Value != null)
                dtpExpireDate.Value = (DateTime)dgvList.Rows[e.RowIndex].Cells[4].Value;
            if (dgvList.Rows[e.RowIndex].Cells[5].Value != null)
                txtProducer.Text = dgvList.Rows[e.RowIndex].Cells[5].Value.ToString();
            if (dgvList.Rows[e.RowIndex].Cells[6].Value != null)
                nudAmount.Text = dgvList.Rows[e.RowIndex].Cells[6].Value.ToString();
            if (dgvList.Rows[e.RowIndex].Cells[7].Value != null)
                dtpEntryDate.Value = (DateTime)dgvList.Rows[e.RowIndex].Cells[7].Value;
            if (dgvList.Rows[e.RowIndex].Cells[8].Value != null)
                cmbSupplier.SelectedValue = dgvList.Rows[e.RowIndex].Cells[8].Value;

            LockControls();
            btnEdit.Enabled = true;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            UnlockControls();
            btnExport.Enabled = false;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            using (var db = new QLKhoDbContext())
            {
                object[] obj =
                {
                    new SqlParameter("@makho",(int)cmbStorage.SelectedValue),
                    new SqlParameter("@mahh",int.Parse(txtID.Text)),
                    new SqlParameter("@tenhh",txtName.Text),
                    new SqlParameter("@tinhtrang",rtbState.Text),
                    new SqlParameter("@mancc",(int)cmbSupplier.SelectedValue),
                    new SqlParameter("@ngaysx",dtpProductDate.Value),
                    new SqlParameter("@hansd",dtpExpireDate.Value),
                    new SqlParameter("@soluongton",int.Parse(nudAmount.Text)),
                    new SqlParameter("@ngaynhap",dtpEntryDate.Value),
                    new SqlParameter("@nhasx",txtProducer.Text)
                };
                int res = db.Database.ExecuteSqlCommand("dbo.commodity_modification @makho,@mahh,@tenhh,@tinhtrang," +
                    "@mancc,@ngaysx,@hansd,@soluongton,@ngaynhap,@nhasx", obj);
                MessageBox.Show($"result = {res}");
                if (res > 0)
                {
                    LoadData();
                    LockControls();
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void CmbStorage_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void DgvList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // do nothing here
            //throw new Exception();
        }

        private void LoadData()
        {
            using (var db = new QLKhoDbContext())
            {
                object[] obj =
                {
                    new SqlParameter("@makho", (int)cmbStorage.SelectedValue)
                };

                using (var conn = db.Database.Connection)
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "commodity_view";
                    cmd.Parameters.AddRange(obj);
                    SqlDataAdapter da = new SqlDataAdapter((SqlCommand)cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    conn.Close();
                    dgvList.DataSource = dt;
                }
                //dgvList.DataSource = db.HangHoas.SqlQuery("commodity_view @makho", obj).ToList();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (QLKhoDbContext db = new QLKhoDbContext())
            {
                cmbStorage.DataSource = (db.Khoes.Select(x => x)).ToList();
                cmbStorage.ValueMember = "makho";
                cmbStorage.DisplayMember = "tenkho";

                cmbSupplier.DataSource = (db.NhaCungCaps.Select(x => x)).ToList();
                cmbSupplier.ValueMember = "mancc";
                cmbSupplier.DisplayMember = "tenncc";

                var madd = db.Khoes.SingleOrDefault(x => x.makho == (int)cmbStorage.SelectedValue).madd;
                txtPlace.Text = db.DiaDiems.SingleOrDefault(x => x.madd == madd).tendd;
            }
            LoadData();
            cmbStorage.SelectedValueChanged += CmbStorage_SelectedValueChanged;
        }

        private void LockControls()
        {
            txtID.Enabled = false;
            txtName.Enabled = false;
            rtbState.Enabled = false;
            cmbSupplier.Enabled = false;
            dtpProductDate.Enabled = false;
            dtpExpireDate.Enabled = false;
            nudAmount.Enabled = false;
            dtpEntryDate.Enabled = false;
            txtProducer.Enabled = false;

            btnEdit.Enabled = false;
            btnSave.Enabled = false;
        }

        private void UnlockControls()
        {
            txtName.Enabled = true;
            rtbState.Enabled = true;
            cmbSupplier.Enabled = true;
            dtpProductDate.Enabled = true;
            dtpExpireDate.Enabled = true;
            nudAmount.Enabled = true;
            dtpEntryDate.Enabled = true;
            txtProducer.Enabled = true;

            btnSave.Enabled = true;
        }
    }
}
