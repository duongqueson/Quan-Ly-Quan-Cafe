using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class MenuDTO
    {
        private string nameFood;
        private int count;
        private float price;
        private float tottalPrice;

        public string NameFood { get => nameFood; set => nameFood = value; }
        public int Count { get => count; set => count = value; }
        public float Price { get => price; set => price = value; }
        public float TottalPrice { get => tottalPrice; set => tottalPrice = value; }
        public MenuDTO(string nameFood,int count,float price,float tottalPrice)
        {
            this.NameFood = nameFood;
            this.Count = count;
            this.Price = price;
            this.TottalPrice = tottalPrice;
        }
        public MenuDTO(DataRow dataRow)
        {
            this.NameFood = (string)dataRow["Name"];
            this.Count = (int)dataRow["Count"];
            this.Price = (float)Convert.ToDouble(dataRow["Price"].ToString());
            this.TottalPrice = (float)Convert.ToDouble(dataRow["tottalPrice"].ToString());
        }
    }
}
