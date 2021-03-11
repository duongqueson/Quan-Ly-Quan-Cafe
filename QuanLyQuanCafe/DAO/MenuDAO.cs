using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;

        public static MenuDAO Instance
        {
            get { if (instance == null) instance = new MenuDAO();return instance; }
            private set => instance = value;
        }
        public MenuDAO() { }

        public List<MenuDTO> GetListMenuByTable(int id)
        {
            List<MenuDTO> menus = new List<MenuDTO>();

            string query = "select f.name,bi.count,f.price,f.price*bi.count as tottalPrice from dbo.Bill as b,dbo.BillInfo as bi,dbo.Food as f where bi.idBill = b.id and bi.idFood = f.id and b.idTable=" + id + " and b.status=0";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                MenuDTO menu = new MenuDTO(item);
                menus.Add(menu);
            }

            return menus;
        }
    }
}
