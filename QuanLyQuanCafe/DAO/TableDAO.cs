using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class TableDAO
    {
        public static int TableWidth = 100;
        public static int TableHeight = 100;

        private static TableDAO instance;
        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return instance; }
            private set => instance = value;
        }
        private TableDAO() { }
        
        public List<TableDTO> GetListTable()
        {
            List<TableDTO> tables = new List<TableDTO>();

            DataTable data = DataProvider.Instance.ExecuteQuery("exec dbo.usp_GetTableList");

            foreach (DataRow item in data.Rows)
            {
                TableDTO table = new TableDTO(item);
                tables.Add(table);
            }

            return tables;
        }
        public List<TableDTO> GetListFreeTable()
        {
            List<TableDTO> tables = new List<TableDTO>();
            string query = "exec usp_GetListFreeTable";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                TableDTO table = new TableDTO(item);
                tables.Add(table);
            }
            return tables;
        }
        public void SwitchTable(int idTableO,int idTableN)
        {
            string query = "exec usp_SwitchTable @id1 , @id2 ";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { idTableO,idTableN});
        }

        public List<TableDTO> ShowTable()
        {
            List<TableDTO> tables = new List<TableDTO>();
            string query = "select * from TableFood";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                TableDTO table = new TableDTO(item);
                tables.Add(table);
            }
            return tables;
        }
        public bool InsertTable(string name)
        {
            string query = string.Format("INSERT INTO [dbo].[TableFood] ([name]) VALUES (N'{0}')", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool UpdateTable(int id,string name)
        {
            string query = string.Format("UPDATE [dbo].[TableFood] SET[name] = N'{0}' WHERE id = {1}", name,id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteTable(int id)
        {
            string query = string.Format("DELETE FROM [dbo].[TableFood] WHERE id = {0}", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

    }
}
