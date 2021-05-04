using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security;
using System.Security.Cryptography;

namespace PassMan
{
    class Program
    {

        public static void Main()
        {
            List<string> passwordList = new List<string>();
            passwordList.Add("helloo");
            passwordList.Add("?q?$?b?x]??j??=s1????♀?????%↓g<?##?????↔|z?n¶???♀FcG\\.\\:??os???C");
            passwordList.Add("123");
            GetCredentials(passwordList);
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

        private static void GetCredentials(List<string> list)
        {
            bool isValid = false;
            
            char[] charsToTrim = {' ', '\''};

            do{
                string user_name;
                NetworkCredential credentials;
               
                Console.WriteLine("Enter username: ");
                user_name = Console.ReadLine().Trim(charsToTrim);

                Console.WriteLine("Enter password: ");
                credentials = new NetworkCredential(user_name, Console.ReadLine());
 
                if(TestPassword(credentials, list))
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

        private static bool TestPassword(NetworkCredential cred, List<string> list){

            SHA512 sHA = new SHA512Managed();
            byte[] compArr = sHA.ComputeHash(ASCIIEncoding.ASCII.GetBytes(cred.Password));

            for(int i = 0; i < list.Count; i++)
            {  
                byte[] item = ASCIIEncoding.ASCII.GetBytes(list[i]);
                bool isValid = false;

                for(int j = 0; j < item.Length; j++){

                    if(item[j] == compArr[j]){
                        isValid = true;
                    }
                    else{
                        isValid = false;
                    }                  
                }

                if(isValid)
                    return true;      
            }
            return false;
        }
    }
}
