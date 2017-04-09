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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            Load += Form4_Load;
            treeView1.NodeMouseClick += TreeView1_NodeMouseClick;
        }

        private void TreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Name == "Xem")
            {
                Form1 form1 = new Form1();
                
                    form1.Show();
                
            }
            else if (e.Node.Name == "Nhap")
            {
                using (Form2 form2 = new Form2())
                {
                    form2.Show();
                }
            }
            else if (e.Node.Name == "Xuat")
            {
                using (Form3 form3 = new Form3())
                {
                    form3.Show();
                }
            }
            else if (e.Node.Name == "Help")
            {
                // helping here
                
            }
            else if (e.Node.Name == "ThongTinNguoiDung")
            {
                // user info here
            }
            else if (e.Node.Name == "ThemNguoiDung")
            {
                // adding new user here
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            using (var db = new QLKhoDbContext())
            {
                using (var conn = db.Database.Connection)
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    // load importing report
                    cmd.CommandText = "select top 7 cast(ngaynhap as date) ngaynhap, count(*) soluong " +
                        "from phieunhap group by cast(ngaynhap as date)";
                    SqlDataAdapter da = new SqlDataAdapter((SqlCommand)cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        chart1.Series["Import"].Points.AddXY(row.Field<DateTime>("ngaynhap"),
                            new object[] { row.Field<int>("soluong") });
                    }

                    // load exporting report
                    cmd.CommandText = "select top 10 cast(ngayxuat as date) ngayxuat, count(*) soluong "
                        + "from phieuxuat group by cast(ngayxuat as date)";
                    da.SelectCommand = (SqlCommand)cmd;
                    dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        chart1.Series["Export"].Points.AddXY(row.Field<DateTime>("ngayxuat"),
                            new object[] { row.Field<int>("soluong") });
                    }

                    conn.Close();
                }
            }
        }
    }
}
