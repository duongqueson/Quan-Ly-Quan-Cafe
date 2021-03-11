using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    class HashPass
    {
        private static HashPass instance;

        public static HashPass Instance {
            get { if (instance == null) instance = new HashPass();return instance; }
            set => instance = value; 
        }

        public string HashPassWord(string passWord)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(passWord);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);

            /*var list = hasData.ToString();
            list.Reverse();*/

            string hasPass = "";

            foreach (byte item in hasData)
            {
                hasPass += item;
            }
            return hasPass;
        }
    }
}
