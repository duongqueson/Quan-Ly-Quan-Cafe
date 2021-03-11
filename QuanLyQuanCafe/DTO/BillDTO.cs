using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class BillDTO
    {
        private int iD;
        private DateTime? dateCheckIn;
        private DateTime? dateCheckOut;
        private int tableID;
        private int status;
        private int discount;

        public int ID { get => iD; set => iD = value; }
        public DateTime? DateCheckIn { get => dateCheckIn; set => dateCheckIn = value; }
        public DateTime? DateCheckOut { get => dateCheckOut; set => dateCheckOut = value; }
        public int Status { get => status; set => status = value; }
        public int TableId { get => tableID; set => tableID = value; }
        public int Discount { get => discount; set => discount = value; }

        public BillDTO(int id, DateTime checkIn,DateTime checkOut, int tableId,int status,int discount = 0)
        {
            this.ID = id;
            this.DateCheckIn = checkIn;
            this.DateCheckOut = checkOut;
            this.TableId = tableId;
            this.Status = status;
            this.Discount = discount;
        }
        public BillDTO(DataRow dataRow)
        {
            this.ID = (int)dataRow["id"];
            this.DateCheckIn = (DateTime)dataRow["DateCheckin"];
            var dateCheckOutTemp = dataRow["DateCheckout"];
            if(dateCheckOutTemp.ToString() != "")
                this.DateCheckOut = (DateTime)dataRow["DateCheckout"];
            this.TableId = tableID;
            this.Status = (int)dataRow["status"];
            this.Discount = (int)dataRow["discount"];
        }
    }
}
