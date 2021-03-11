using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace QuanLyQuanCafe.DTO
{
    public class DataProvider
    {
        private static DataProvider instance;
        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return instance; }

            private set => instance = value; 
        }
        private DataProvider() { }

        private readonly string connectionStr = @"Data Source=DQS;Initial Catalog=quanlyquancafe;User ID=sa;Password=31101998";
        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {              
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains("@"))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }                       
                    }
                }

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

                dataAdapter.Fill(data);

                connection.Close();
            }
               
            return data;
        }


        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                        
                    }
                }

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

                data = command.ExecuteNonQuery();

                connection.Close();

            }
            return data;
        }
        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object data = 0;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        command.Parameters.AddWithValue(item, parameter[i]);
                        i++;
                    }
                }

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

                data = command.ExecuteScalar();

                connection.Close();

            }
            return data;
        }
    }
}
