using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace QuanLyQuanCafe.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set => instance = value;
        }

        private AccountDAO() { }

        public bool Login(string userName,string passWord)
        {
            string hasPass = HashPass.Instance.HashPassWord(passWord);

            string query = "exec dbo.usp_Login @userName , @passWord ";

            DataTable result= DataProvider.Instance.ExecuteQuery(query,new object[] {userName,hasPass /*list*/});

            return result.Rows.Count > 0;
        }

        public AccountDTO GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from dbo.Account where UserName='" + userName +"' ");
            foreach (DataRow item in data.Rows)
            {
                return new AccountDTO(item);
            }
            return null;
        }
        public int UpdateAccount(string userName, string dispalyName, string passWord, string newPassWord)
        {
            string query = "exec USP_UpdateAccount @userName , @displayName , @password , @newPassword";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { userName, dispalyName, passWord, newPassWord });
            return result;
        }
        public List<AccountDTO> GetListAccount()
        {
            List<AccountDTO> accounts = new List<AccountDTO>();
            string query = "select * from Account";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                AccountDTO account = new AccountDTO(item);
                accounts.Add(account);
            }
            return accounts;
        }
        public bool InsertAccount(string name, string displayName, int type)
        {
            string query = string.Format("INSERT INTO[dbo].[Account]([UserName],[DisplayName],[Type],[PassWord]) VALUES ('{0}',N'{1}',{2},N'{3}')", name, displayName, type, "2251022057731868917119086224872421513662");
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool UpdateAccount(string name, string displayName, int type)
        {
            string query = string.Format("UPDATE [dbo].[Account] SET [DisplayName] = N'{0}' , [type] = {1} WHERE UserName = '{2}'", displayName, type, name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteAccount(string name)
        {
            string querry = string.Format("DELETE FROM [dbo].[Account]WHERE UserName = '{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(querry);
            return result > 0;
        }
        public bool ResetPassWord(string name)
        {
            string query = string.Format("UPDATE [dbo].[Account] SET [PassWord] = N'2251022057731868917119086224872421513662' WHERE UserName = '{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
