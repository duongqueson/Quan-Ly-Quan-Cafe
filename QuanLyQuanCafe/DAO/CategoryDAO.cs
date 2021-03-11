using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;
        public static CategoryDAO Instance
        {
            get { if (instance == null) instance = new CategoryDAO(); return instance; }
            private set => instance = value;
        }
        public CategoryDAO() { }
        public List<CategoryDTO> GetListCatagory()
        {
            List<CategoryDTO> catagories = new List<CategoryDTO>();

            DataTable data = DataProvider.Instance.ExecuteQuery("select * from dbo.FoodCategory");

            foreach (DataRow item in data.Rows)
            {
                CategoryDTO category = new CategoryDTO(item);
                catagories.Add(category);
            }

            return catagories;
        }
        public CategoryDTO GetCatagoryById(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from dbo.FoodCategory where id="+ id +"");

            CategoryDTO category = new CategoryDTO(data.Rows[0]);

            return category;
        }
        public List<CategoryDTO> ShowCategory()
        {
            List<CategoryDTO> categories = new List<CategoryDTO>();
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from FoodCategory");
            foreach (DataRow item in data.Rows)
            {
                CategoryDTO category = new CategoryDTO(item);
                categories.Add(category);
            }
            return categories;
        }
        public bool InsertCategory(string name)
        {
            int result;
            string query = string.Format("INSERT INTO [dbo].[FoodCategory]([name]) VALUES('{0}')",name);
            result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool UpdateCategory(int id,string name)
        {
            int result;
            string query = string.Format("UPDATE [dbo].[FoodCategory] SET[name] = N'{0}' WHERE id = {1}", name,id);
            result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteCategory(int id)
        {
            int result;
            string query = string.Format("DELETE FROM [dbo].[FoodCategory] WHERE id = {0}", id);
            result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

    }
}
