using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using QuanLyQuanCafe.DTO;
using QuanLyQuanCafe.DAO;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        private int page = 0;
        public AccountDTO loginAccount;
        public fAdmin()
        {
            InitializeComponent();
        }
        private void FAdmin_Load(object sender, EventArgs e)
        {
            LoadCategory();          
        }

        #region Methods
        private void LoadCategory()
        {
            List<CategoryDTO> categories = CategoryDAO.Instance.GetListCatagory();
            cbbFoodCategory.DataSource = categories;
            cbbFoodCategory.DisplayMember = "name";
            cbbFoodCategory.SelectedIndex = -1;
        }

        public void GetListBillByDate()
        {
            DataTable data = BillDAO.Instance.GetListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            
            grvBill.DataSource = data;
        }
        void LoadListFood()
        {
            List<FoodDTO> l = FoodDAO.Instance.LoadListFood();
            grvFood.DataSource = l;
        }
        void LoadListCategory()
        {
            grvCategory.DataSource = CategoryDAO.Instance.ShowCategory();
        }
        void ShowTable()
        {
            grvTable.DataSource = TableDAO.Instance.ShowTable();
        }
        void ShowAccount()
        {
            grvAccount.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void InsertAccount(string userName,string displayName,int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName,displayName,type))
            {
                MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK);
                ShowAccount();
            }
            else
            {
                MessageBox.Show("Lỗi! Thêm không thành công", "Thông báo", MessageBoxButtons.OK);
            }
        }
        void UpdateAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK);
                ShowAccount();
            }
            else
            {
                MessageBox.Show("Lỗi! Cập nhật không thành công", "Thông báo", MessageBoxButtons.OK);
            }
        }
        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Không thể xóa chính bạn!");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK);
                ShowAccount();
            }
            else
            {
                MessageBox.Show("Lỗi! Xóa không thành công", "Thông báo", MessageBoxButtons.OK);
            }
        }
        void ResetPassWord(string userName)
        {
            if (AccountDAO.Instance.ResetPassWord(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công", "Thông báo", MessageBoxButtons.OK);
                ShowAccount();
            }
            else
            {
                MessageBox.Show("Lỗi! Đặt lại mật khẩu không thành công", "Thông báo", MessageBoxButtons.OK);
            }
        }

        public void ShowFirstBillPage(string checkIn, string checkOut)
        {
            if(page == 1)
            {
                MessageBox.Show("Đây là trang đầu", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                page = 1;
                DataTable dt = BillDAO.Instance.ShowBill(checkIn, checkOut, page.ToString());
                grvBill.DataSource = dt;
            }          
        }
        public void ShowLastBillPage(string checkIn, string checkOut)
        {
            int countBill = BillDAO.Instance.CountBill(checkIn,checkOut);
            if ((countBill%10 == 0 && page == countBill/10) || page == countBill/10 + 1){
                MessageBox.Show("Đây là trang cuối", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                if (countBill % 10 == 0)
                    page = countBill / 10;
                else
                    page = countBill / 10 + 1;
                DataTable dt = BillDAO.Instance.ShowBill(checkIn, checkOut, page.ToString());
                grvBill.DataSource = dt;
            }                 
        }
        public bool ShowPreBill(string checkIn, string checkOut)
        {
            if (page == 1)
                return false;
            else
            {
                if(page == 0)
                {
                    ShowFirstBillPage(checkIn,checkOut);
                }
                else
                {
                    page--;
                    DataTable dt = BillDAO.Instance.ShowBill(checkIn, checkOut, page.ToString());
                    grvBill.DataSource = dt;
                }              
            }
            return true;
        }

        public bool ShowNextBill(string checkIn, string checkOut)
        {
            int countBill = BillDAO.Instance.CountBill(checkIn,checkOut);
            if (countBill == 0)
                return false;
            int pg;
            if (countBill % 10 == 0)
                pg = countBill / 10;
            else
                pg = countBill / 10 + 1;

            if (page == pg)
                return false;
            else
            {
                page++;
                DataTable dt = BillDAO.Instance.ShowBill(checkIn, checkOut, page.ToString());
                grvBill.DataSource = dt;
            }
            return true;
        }

        public void SumDayAndDate()
        {
            DateTime dt1 = dtpkFromDate.Value;
            DateTime dt2 = dtpkToDate.Value;
            TimeSpan ts = dt2.Subtract(dt1);
            txtDay.Text = (ts.Days + 1).ToString() + " ngày";         
            if(BillDAO.Instance.CountBill(dt1.ToString(),dt2.ToString()) != 0 )
                    txtSumPrice.Text = (BillDAO.Instance.Sum(dt1.ToString(), dt2.ToString())).ToString();
        }
        #endregion

        #region events
        private void BtnShowBill_Click(object sender, EventArgs e)
        {
            if (dtpkFromDate.Value >= dtpkToDate.Value)
                MessageBox.Show("Ngày bắt đầu phải trước ngày kết thúc");
            else
            {
                page = 0;
                GetListBillByDate();
                txtPage.Text = "";
                SumDayAndDate();
            }           
        }
        private void BtnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }
        private void GrvFood_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = grvFood.SelectedCells[0].RowIndex;
            DataGridViewRow gridViewRow = grvFood.Rows[index];
            txtFoodId.Text = gridViewRow.Cells["id"].Value.ToString();
            txtFoodName.Text = gridViewRow.Cells["name"].Value.ToString();
            CategoryDTO category = CategoryDAO.Instance.GetCatagoryById((int)gridViewRow.Cells["idcategory"].Value);
            cbbFoodCategory.Text = category.Name;
            nmFoodPrice.Text = gridViewRow.Cells["price"].Value.ToString();
        }
        private void BtnAddFood_Click(object sender, EventArgs e)
        {
            int idCategory = (cbbFoodCategory.SelectedItem as CategoryDTO).ID;
            int result = FoodDAO.Instance.InsertFood(txtFoodName.Text, idCategory, (int)nmFoodPrice.Value);
            if (result > 0)
            {
                MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK);
                LoadListFood();
            }
            else
            {
                MessageBox.Show("Lỗi! Thêm không thành công", "Thông báo", MessageBoxButtons.OK);
            }

        }
        private void BtnEditFood_Click(object sender, EventArgs e)
        {
            int result = FoodDAO.Instance.UpdateFood(int.Parse(txtFoodId.Text), txtFoodName.Text, (cbbFoodCategory.SelectedItem as CategoryDTO).ID, (int)nmFoodPrice.Value);
            if (result > 0)
            {
                MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK);
                LoadListFood();
            }
            else
            {
                MessageBox.Show("Lỗi! Sửa không thành công", "Thông báo", MessageBoxButtons.OK);
            }
        }
        private void BtnDeleteFood_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn xóa không?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                int result = FoodDAO.Instance.DeleteFood(int.Parse(txtFoodId.Text));
                if (result > 0)
                {
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK);
                    LoadListFood();
                }
                else
                {
                    MessageBox.Show("Lỗi! Xóa không thành công", "Thông báo", MessageBoxButtons.OK);
                }
            }
        }
        private void BtnSearchFood_Click(object sender, EventArgs e)
        {
            List<FoodDTO> foods = FoodDAO.Instance.SearchFood(txtSearchFood.Text);
            grvFood.DataSource = foods;
        }
        private void BtnShowCatagory_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }
        private void GrvCatagory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = grvCategory.SelectedCells[0].RowIndex;
            DataGridViewRow gridViewRow = grvCategory.Rows[index];
            txtIdCategory.Text = gridViewRow.Cells["CategoryId"].Value.ToString();
            txtNameCatagory.Text = gridViewRow.Cells["CategoryName"].Value.ToString();
        }
        private void BtnAddCatagory_Click(object sender, EventArgs e)
        {
            string name = txtNameCatagory.Text;
            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK);
                LoadListCategory();
            }
            else
            {
                MessageBox.Show("Thêm không thành công!", "Thông báo", MessageBoxButtons.OK);
            }
        }
        private void BtnEditCatagory_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtIdCategory.Text);
            string name = txtNameCatagory.Text;
            if (CategoryDAO.Instance.UpdateCategory(id, name))
            {
                MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK);
                LoadListCategory();
            }
            else
            {
                MessageBox.Show("Cập nhật không thành công", "Thông báo", MessageBoxButtons.OK);

            }
        }
        private void BtnDeleteCatagory_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắn chắn xóa không?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                int id = int.Parse(txtIdCategory.Text);
                if (CategoryDAO.Instance.DeleteCategory(id))
                {
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK);
                    LoadListCategory();
                }
                else
                {
                    MessageBox.Show("Xóa không thành công", "Thông báo", MessageBoxButtons.OK);
                }
            }
        }
        private void BtnShowTable_Click(object sender, EventArgs e)
        {
            ShowTable();
        }
        private void GrvTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = grvTable.SelectedCells[0].RowIndex;
            txtTableId.Text = grvTable.Rows[index].Cells["TableId"].Value.ToString();
            txtTableName.Text = grvTable.Rows[index].Cells["TableName"].Value.ToString();
        }
        private void BtnAddTable_Click(object sender, EventArgs e)
        {
            string name = txtTableName.Text;
            if (TableDAO.Instance.InsertTable(name))
            {
                MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK);
                ShowTable();
            }
            else
            {
                MessageBox.Show("Thêm không thành công", "Thông báo", MessageBoxButtons.OK);
            }
        }
        private void BtnEditTable_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtTableId.Text);
            string name = txtTableName.Text;
            if (TableDAO.Instance.UpdateTable(id, name))
            {
                MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK);
                ShowTable();
            }
            else
            {
                MessageBox.Show("Sửa không thành công", "Thông báo", MessageBoxButtons.OK);
            }
        }
        private void BtnDeleteTable_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắn chắn xóa không?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                int id = Convert.ToInt32(txtTableId.Text);
                if (TableDAO.Instance.DeleteTable(id))
                {
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK);
                    ShowTable();
                }
                else
                {
                    MessageBox.Show("Xóa không thành công", "Thông báo", MessageBoxButtons.OK);
                }
            }
        }
        private void BtnShowAccount_Click(object sender, EventArgs e)
        {
            ShowAccount();
        }
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string displayName = txtDisplayName.Text;
            int type = Convert.ToInt32(cbbAccountType.Text);
            InsertAccount(userName,displayName,type);
        }
        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string displayName = txtDisplayName.Text;
            int type = Convert.ToInt32(cbbAccountType.Text);
            UpdateAccount(userName, displayName, type);
        }
        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc xóa tài khoản không?" + txtUserName.Text + "", "Thông Báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string userName = txtUserName.Text;
                DeleteAccount(userName);
            }             
        }
        private void grvAccount_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = grvAccount.SelectedCells[0].RowIndex;
            txtUserName.Text = grvAccount.Rows[index].Cells["UserName"].Value.ToString();
            txtDisplayName.Text = grvAccount.Rows[index].Cells["DisplayName"].Value.ToString();
            cbbAccountType.Text = grvAccount.Rows[index].Cells["Type"].Value.ToString();
        }
        private void btnResetPassWord_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Bạn có chắc đặt lại mật khẩu cho tài khoản " + txtUserName.Text + "","Thông Báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string userName = txtUserName.Text;
                ResetPassWord(userName);
            }            
        }
        private void btnFirstBillPage_Click(object sender, EventArgs e)
        {
            if (dtpkFromDate.Value > dtpkToDate.Value)
                MessageBox.Show("Ngày bắt đầu phải trước ngày kết thúc");          
            else
                ShowFirstBillPage(dtpkFromDate.Value.ToString(), dtpkToDate.Value.ToString());
            txtPage.Text = page.ToString();
        }

        #endregion

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            if (dtpkFromDate.Value > dtpkToDate.Value)
                MessageBox.Show("Ngày bắt đầu phải trước ngày kết thúc");
            else
                ShowLastBillPage(dtpkFromDate.Value.ToString(), dtpkToDate.Value.ToString());
            txtPage.Text = page.ToString();

        }

        private void btnPrevioursBillPage_Click(object sender, EventArgs e)
        {
            if (dtpkFromDate.Value > dtpkToDate.Value)
                MessageBox.Show("Ngày bắt đầu phải trước ngày kết thúc");
            else
                if (ShowPreBill(dtpkFromDate.Value.ToString(), dtpkToDate.Value.ToString()) == false)
                    MessageBox.Show("Đây là trang đầu", "Thông báo", MessageBoxButtons.OK);
            txtPage.Text = page.ToString();
        }

        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            if (dtpkFromDate.Value > dtpkToDate.Value)
                MessageBox.Show("Ngày bắt đầu phải trước ngày kết thúc");
            else
                if (ShowNextBill(dtpkFromDate.Value.ToString(), dtpkToDate.Value.ToString()) == false)
                    MessageBox.Show("Đây là trang cuối", "Thông báo", MessageBoxButtons.OK);
            txtPage.Text = page.ToString();
        }
    }
}
