using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fTableManager : Form
    {
        private AccountDTO accountlogin;

        public AccountDTO Accountlogin
        {
            get { return accountlogin; }
            set { accountlogin = value; ChangeAccount(accountlogin.Type); }
        }
        public fTableManager(AccountDTO acc)
        {           
            InitializeComponent();
            this.Accountlogin = acc;
            LoadTable();
            LoadCatagory();
            LoadFreeTable();
            
        }
        void ChangeAccount(int type)
        {
            if (type == 1)
            {
                adminToolStripMenuItem.Enabled = true;
                thôngTinTàiKhoảnToolStripMenuItem.Text += "("+Accountlogin.DisplayName+")";
            }
            else
            {
                adminToolStripMenuItem.Enabled = false;
                thôngTinTàiKhoảnToolStripMenuItem.Text += "(" + Accountlogin.DisplayName + ")";
            }
        }
        void LoadFreeTable()
        {
            List<TableDTO> tables = TableDAO.Instance.GetListFreeTable();
            cbbSwitchTable.DataSource = tables;
            cbbSwitchTable.DisplayMember = "name";
            cbbSwitchTable.SelectedIndex = -1;
        }
        public void LoadCatagory()
        {
            List<CategoryDTO> categories = CategoryDAO.Instance.GetListCatagory();
            cbbCategory.DataSource = categories;
            cbbCategory.DisplayMember = "name";
            cbbCategory.SelectedIndex = -1;
            cbbFood.SelectedIndex = -1;
        }
        void LoadFoodListByIdCatagory(int id)
        {           
            List<FoodDTO> foods = FoodDAO.Instance.GetFoodByIdCategory(id);
            cbbFood.DataSource = foods;
            cbbFood.DisplayMember = "name";           
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<TableDTO> tables = TableDAO.Instance.GetListTable();
            foreach (TableDTO item in tables)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                flpTable.Controls.Add(btn);
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += Btn_Click;
                btn.Tag = item;
                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Green;
                        break;
                    default:
                        btn.BackColor = Color.Yellow;
                        break;
                }
            }
        }
        void ShowBill(int id)
        {
            float tottalPrice = 0;
            lsvBill.Items.Clear();
            List<MenuDTO> menus = MenuDAO.Instance.GetListMenuByTable(id);
            foreach (MenuDTO item in menus)
            {
                ListViewItem listView = new ListViewItem(item.NameFood);
                listView.SubItems.Add(item.Count.ToString());
                listView.SubItems.Add(item.Price.ToString());
                tottalPrice += item.TottalPrice;
                listView.SubItems.Add(item.TottalPrice.ToString());
                lsvBill.Items.Add(listView);
            }
            CultureInfo culture = new CultureInfo("vi-VN");

            //Thread.CurrentThread.CurrentCulture = culture;

            txtTottalPrice.Text = tottalPrice.ToString("c",culture);
        }
        private void Btn_Click(object sender, EventArgs e)
        {
            cbbCategory.SelectedIndex = -1;
            cbbFood.SelectedIndex = -1;
            nmDisCount.Value = 0;
            int tableID = ((sender as Button).Tag as TableDTO).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }
        
        private void CbbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id;

            ComboBox combo = sender as ComboBox;

            if (combo.SelectedItem == null)
                return;

            CategoryDTO selected = combo.SelectedItem as CategoryDTO;
            id = selected.ID;

            LoadFoodListByIdCatagory(id);
        }

        private void BtnAddFood_Click(object sender, EventArgs e)
        {
            TableDTO table = lsvBill.Tag as TableDTO;
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            
            if (cbbFood.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn món!", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                int idFood = (cbbFood.SelectedItem as FoodDTO).ID;
                if (idBill == -1)
                {
                    BillDAO.Instance.InsertBill(table.ID);
                    BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIdBill(), idFood, (int)nmFoodCount.Value);
                    LoadTable();
                }
                else
                {
                    BillInfoDAO.Instance.InsertBillInfo(idBill, idFood, (int)nmFoodCount.Value);
                }
            }           
            ShowBill(table.ID);
        }

        private void BtnCheckOut_Click(object sender, EventArgs e)
        {
            TableDTO table = lsvBill.Tag as TableDTO;
            if (table == null)
                return;
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int discount = (int)nmDisCount.Value;
            double totalPrice = double.Parse(txtTottalPrice.Text, NumberStyles.Currency, new CultureInfo("vi-VN"));
            double finalTotalPrice=totalPrice-(totalPrice/100)*discount;
            if (idBill != -1)
            {
                if(MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho bàn {0}\nTổng tiền - (Tổng tiền / 100) x Giảm giá\n=> {1} - ({1} / 100) x {2} = {3}", table.Name, totalPrice, discount, finalTotalPrice),"Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill,discount,(float)finalTotalPrice);
                    Form Bill = new fBill(idBill,accountlogin.DisplayName,totalPrice,discount,finalTotalPrice);
                    Bill.Show();
                    LoadTable();
                    ShowBill(table.ID);
                }               
            }
        }

        private void ĐăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ThôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile accountProfile = new fAccountProfile(Accountlogin);
            accountProfile.ShowDialog();
        }

        private void AdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin
            {
                loginAccount = Accountlogin
            };
            this.Hide();
            f.ShowDialog();

        }

        private void BtnSwitchTable_Click(object sender, EventArgs e)
        {
            int idTableO = (lsvBill.Tag as TableDTO).ID;
            int idTableN= (cbbSwitchTable.SelectedItem as TableDTO).ID;
            lsvBill.Tag = cbbSwitchTable.SelectedItem as TableDTO;
            TableDAO.Instance.SwitchTable(idTableO,idTableN);
            LoadTable();
            LoadFreeTable();
        }
    }
}
