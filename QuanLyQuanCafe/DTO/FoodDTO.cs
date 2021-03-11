using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class FoodDTO
    {
        private int iD;
        private string name;
        private int categoryID;
        private float price;

        public int ID { get => iD; set => iD = value; }
        public string Name { get => name; set => name = value; }
        public int CategoryID { get => categoryID; set => categoryID = value; }
        public float Price { get => price; set => price = value; }

        public FoodDTO(int iD,string name,int categoryID, float price)
        {
            this.ID = iD;
            this.Name = name;
            this.CategoryID = categoryID;
            this.Price = price;
        }
        public FoodDTO(DataRow dataRow)
        {
            this.ID = (int)dataRow["id"];
            this.Name = (string)dataRow["name"];
            this.CategoryID = (int)dataRow["idCategory"];
            this.Price = (float)Convert.ToDouble(dataRow["price"].ToString());
        }
    }
}
