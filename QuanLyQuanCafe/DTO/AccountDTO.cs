using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class AccountDTO
    {
        private string displayName;
        private string userName;
        private string passWord;
        private int type;

        public string DisplayName { get => displayName; set => displayName = value; }
        public string UserName { get => userName; set => userName = value; }
        public string PassWord { get => passWord; set => passWord = value; }
        public int Type { get => type; set => type = value; }

        public AccountDTO(string displayName,string userName, int type, string passWord = null)
        {
            this.DisplayName = displayName;
            this.UserName = userName;
            this.PassWord = passWord;
            this.Type = type;
        }

        public AccountDTO(DataRow dataRow)
        {
            this.DisplayName = (string)dataRow["DisplayName"];
            this.UserName = (string)dataRow["UserName"];
            this.PassWord = (string)dataRow["PassWord"];
            this.Type = (int)dataRow["Type"];
        }
        

    }
}
