using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string passWord = txtPassWord.Text;
            if (Login(userName,passWord))
            {
                AccountDTO accountLogin = AccountDAO.Instance.GetAccountByUserName(userName);
                fTableManager tableManager = new fTableManager(accountLogin);
                this.Hide();
                tableManager.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Lỗi đăng nhập", MessageBoxButtons.OK);
            }
            
        }
        bool Login(string userName,string passWord)
        {
            return AccountDAO.Instance.Login(userName,passWord);
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();  
        }

        private void FLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("Bạn có chắc chắn thoát không?","Thông báo",MessageBoxButtons.OKCancel,MessageBoxIcon.Question) != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }
    }
}
