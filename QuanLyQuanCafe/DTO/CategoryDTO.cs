using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class CategoryDTO
    {
        private int iD;
        private string name;

        public int ID { get => iD; set => iD = value; }
        public string Name { get => name; set => name = value; }
        public CategoryDTO(int iD,string name)
        {
            this.ID = iD;
            this.Name = name;
        }
        public CategoryDTO(DataRow dataRow)
        {
            this.ID = (int)dataRow["id"];
            this.Name = (string)dataRow["name"];
        }

    }
}
