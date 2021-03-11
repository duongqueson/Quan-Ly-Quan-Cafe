using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return instance; }
            private set => instance = value;
        }
        public FoodDAO() { }
        public List<FoodDTO> GetFoodByIdCategory(int id)
        {
            List<FoodDTO> foods = new List<FoodDTO>();

            string query = " select * from dbo.Food where idCategory="+ id +"";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                FoodDTO food = new FoodDTO(item);
                foods.Add(food);
            }
            return foods;
        }
        public List<FoodDTO> LoadListFood()
        {
            List<FoodDTO> foods = new List<FoodDTO>();
            string query = "select * from food";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                FoodDTO food = new FoodDTO(item);
                foods.Add(food);
            }
            return foods;
        }

        public int InsertFood(string name,int idCategory, int price)
        {
            string query = string.Format("INSERT INTO[dbo].[Food]([name],[idCategory],[price]) VALUES (N'{0}', {1},{2})",name,idCategory,price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result;
        }

        public int UpdateFood(int id, string name, int idCategory, int price)
        {
            string query = string.Format("UPDATE [dbo].[Food] SET [name] = N'{0}' , [idCategory] = {1} , [price] = {2} WHERE id = {3}", name, idCategory, price,id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result;
        }
        public int DeleteFood(int id)
        {
            string querry = string.Format("DELETE FROM [dbo].[Food]WHERE id = {0}", id);
            int result = DataProvider.Instance.ExecuteNonQuery(querry);
            return result;
        }
        public List<FoodDTO> SearchFood(string name)
        {
            List<FoodDTO> foods = new List<FoodDTO>();
            string query = "SELECT [id],[name],[idCategory],[price] FROM[dbo].[Food] Where name like N'%"+ name +"%'";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                FoodDTO food = new FoodDTO(item);
                foods.Add(food);
            }
            return foods;
        }
    }
}
