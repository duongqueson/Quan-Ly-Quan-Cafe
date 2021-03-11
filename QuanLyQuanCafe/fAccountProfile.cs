using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAccountProfile : Form
    {
        private AccountDTO loginAccount;
        public AccountDTO LoginAccount
        {
            get => loginAccount;
            set { loginAccount = value; AccountChange(LoginAccount); }
        }
        public fAccountProfile(AccountDTO acc)
        {
            InitializeComponent();

            LoginAccount = acc;
        }
        void AccountChange(AccountDTO acc)
        {
            txtDisplayName.Text = acc.DisplayName;
            txtUserName.Text = acc.UserName;
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            string hasPass = HashPass.Instance.HashPassWord(txtPassWord.Text);
            if (hasPass != LoginAccount.PassWord)
            {
                MessageBox.Show("Mật khẩu không đúng!", "Thông báo", MessageBoxButtons.OK);
            }
            else if (txtNewPassWord.Text != txtReEnterPassWord.Text) 
            {
                MessageBox.Show("Mật khẩu mới không trùng khớp!", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                string hasNewPass = HashPass.Instance.HashPassWord(txtNewPassWord.Text);
                if(AccountDAO.Instance.UpdateAccount(txtUserName.Text, txtDisplayName.Text, hasPass, hasNewPass) > 0)
                    MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK);
                else
                    MessageBox.Show("Cập nhật không thành công", "Thông báo", MessageBoxButtons.OK);
            }          
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
