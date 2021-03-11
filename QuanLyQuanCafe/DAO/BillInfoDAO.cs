using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;
        public static BillInfoDAO Instance
        {
            get { if (instance == null) instance = new BillInfoDAO(); return instance; }
            private set => instance = value;
        }
        private BillInfoDAO() { }

        public List<BillInfoDTO> GetListBillInfo(int id)
        {
            List<BillInfoDTO> billInfos = new List<BillInfoDTO>();

            DataTable data = DataProvider.Instance.ExecuteQuery("select * from dbo.BillInfo where idBill="+ id +"");

            foreach (DataRow item in data.Rows)
            {
                BillInfoDTO billInfo = new BillInfoDTO(item);
                billInfos.Add(billInfo);
            }

            return billInfos;
        }
        public void InsertBillInfo(int idBill,int idFood,int count)
        {
            DataProvider.Instance.ExecuteNonQuery("exec usp_InsertBillInfo @idBill , @idFood , @count ", new object[] { idBill,idFood,count});
        }

        public DataTable LoadBillLast(int idb)
        {
            DataTable dt = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.ShowBillTableLast(" + idb + ")");
            return dt;
        }
    }
}
