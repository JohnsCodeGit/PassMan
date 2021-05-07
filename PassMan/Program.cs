using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security;
using System.Security.Cryptography;
using BCrypt.Net;

namespace PassMan
{
    class Program
    {



        public static void Main()
        {
            List<string> passwordList = new List<string>();

            LoginMenu(passwordList);
        }

        private static void LoginMenu(List<string> list)
        {
            Console.Clear();
            string dash = new string('-', 14);
            Console.WriteLine(dash);
            Console.WriteLine("PassMan Login");
            Console.WriteLine(dash);

            Console.WriteLine("1. Add Account");
            Console.WriteLine("2. Sign-in\n");

            Console.Write("Choice: ");

            string choice = Console.ReadLine();

            if(choice.Equals("1")){
                CreateAccount(list);
            }
            else if(choice.Equals("2")){
                SignIn(list);
            }

        }

         //by MSDN http://msdn.microsoft.com/en-us/library/844skk0h(v=vs.71).aspx
        public static SecureString UseRegex(SecureString strIn)
        {
            // Replace invalid characters with empty strings.
            string theString = new NetworkCredential("", strIn).Password;
            Regex.Replace(theString, @"\\\\", "");
            SecureString theSecureString = new NetworkCredential("", theString).SecurePassword;
            return theSecureString;
            
        }

        private static void CreateAccount(List<string> list)
        {
            Console.Clear();
            NetworkCredential cred = new NetworkCredential();

            Console.Write("Create Username: ");
            cred.UserName = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Create Password: ");
            cred.Password = Console.ReadLine();
            
            var encryptedPassword = EncryptPasswordToString(cred);

            list.Add(encryptedPassword);

            LoginMenu(list);
        }

        private static void SignIn(List<string> list)
        {
            Console.Clear();
            bool isValid = false;
            
            char[] charsToTrim = {' ', '\''};

            do{
               
                NetworkCredential credentials = new NetworkCredential();
               
                Console.WriteLine("Enter username: ");
                credentials.UserName = Console.ReadLine().Trim(charsToTrim);

                Console.WriteLine("Enter password: ");
                credentials.Password = Console.ReadLine();
                // credentials.Password = EncryptPasswordToString(credentials);
 
                if(Login(credentials, list))
                {
                    Console.WriteLine("\nPassword accepted\n");
                    
                    isValid = true;
                }
                else
                {
                    Console.WriteLine("\nCredentials invalid\n");
                }
                credentials.SecurePassword.Dispose();
                
            }
            while(!isValid);
          
        }

        private static string EncryptPasswordToString(NetworkCredential cred)
        { 

            return BCrypt.Net.BCrypt.HashPassword(cred.Password, BCrypt.Net.BCrypt.GenerateSalt());
        }

        private static bool Login(NetworkCredential cred, List<string> list)
        {

            for(int i = 0; i < list.Count; i++)
            {  
                if(BCrypt.Net.BCrypt.Verify(cred.Password, list[i]))
                {
                    return true;
                }           
            }
            return false;
        }
    }
}
