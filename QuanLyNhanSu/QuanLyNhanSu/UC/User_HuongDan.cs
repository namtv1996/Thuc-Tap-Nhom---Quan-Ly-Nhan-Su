﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace QuanLyNhanSu
{
    public partial class User_HuongDan : UserControl
    {
        public User_HuongDan()
        {
            InitializeComponent();
            webBrowser1.Navigate(@"C:\Users\namtv1996\Desktop\Thuc Tap Nhom\Thuc Tap Nhom - Quan Ly Nhan Su\QuanLyNhanSu\QuanLyNhanSu\Resources\HuongDan.htm");
        }
        
    }
}
