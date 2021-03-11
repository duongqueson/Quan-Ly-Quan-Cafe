using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;
        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return instance; }

            private set => instance = value;
        }
        private BillDAO() { }
        
        public DataTable GetListBillByDate(DateTime checkIn,DateTime checkOut)
        {
            string query = "exec usp_GetListBillByDate @checkIn , @checkOut ";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { checkIn, checkOut }); ;
        }

        //Thành công: billID
        //Thất bại: id=-1
        public int GetUncheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from dbo.Bill where idTable="+ id +" and status = 0");
            if (data.Rows.Count > 0)
            {
                BillDTO bill = new BillDTO(data.Rows[0]);
                return bill.ID;
            }
            return -1;
        }
        public void CheckOut(int idBill,int discount, float totalPrice)
        {
            string query = "update dbo.Bill set DateCheckout = GETDATE() , status=1, " + "discount="+ discount + ", totalPrice = "+ totalPrice +" where id=" + idBill + "";
            DataProvider.Instance.ExecuteNonQuery(query);
        }
        public void InsertBill(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("exec usp_InsertBill @idTable ", new object[] { id});
        }
        public int GetMaxIdBill()
        {
            return (int)DataProvider.Instance.ExecuteScalar("select max(id) from dbo.Bill"); 
        }

        public DataTable ShowBill(string checkIn, string checkOut, string page)
        {
            string query = "EXEC dbo.GetListBillByDateAndPage @checkIn = '" + checkIn + " ', @checkOut = '" + checkOut + " ', @page = " + page;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            return data;
        }
        public int CountBill(string checkIn, string checkOut)
        {
            string query = "exec usp_CountBillByDate @checkIn ='" + checkIn +"', @checkOut='" + checkOut + "'";
            int count = (int)DataProvider.Instance.ExecuteScalar(query);
            return count;
        }
        public double Sum(string checkIn, string checkOut)
        {
            string query = "EXEC usp_SumPrice @checkIn = '" + checkIn + " ',@checkOut = '" + checkOut + "'";
            double sum = (double)DataProvider.Instance.ExecuteScalar(query);
            return sum;
        }

        
    }
}
