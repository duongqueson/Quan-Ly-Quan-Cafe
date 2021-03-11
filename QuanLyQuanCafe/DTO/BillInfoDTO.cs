using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class BillInfoDTO
    {
        private int iD;
        private int idBill;
        private int idFood;
        private int count;

        public int ID { get => iD; set => iD = value; }
        public int IdBill { get => idBill; set => idBill = value; }
        public int IdFood { get => idFood; set => idFood = value; }
        public int Count { get => count; set => count = value; }

        public BillInfoDTO(int iD,int idBill,int idFood,int count)
        {
            this.ID = iD;
            this.IdBill = idBill;
            this.IdFood = idFood;
            this.Count = count;
        }

        public BillInfoDTO(DataRow dataRow)
        {
            this.ID = (int)dataRow["iD"];
            this.IdBill = (int)dataRow["idBill"];
            this.IdFood = (int)dataRow["idFood"];
            this.Count = (int)dataRow["count"];
        }
    }

}
