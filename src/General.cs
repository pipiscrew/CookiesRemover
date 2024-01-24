using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CookiesRemover
{
    public class General
    {
        public enum BrowserType {
            chromium,
            firerox
        }

        internal static string GetUserLocalAppDataPath()
        {
            string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return localAppDataPath;
        }

        internal static string GetUserAppDataRoamingPath()
        {
            string appDataRoamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return appDataRoamingPath;
        }

        internal static string CalculateMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();

                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

       internal static void AppendTextToFile(string filePath, string textToAppend)
        {
            // Set the second parameter to 'true' to append to an existing file
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(textToAppend);
            }
        }

        internal static DialogResult Mes(string descr, MessageBoxIcon icon = MessageBoxIcon.Information, MessageBoxButtons butt = MessageBoxButtons.OK)
        {
            if (descr.Length > 0)
                return MessageBox.Show(descr, Application.ProductName, butt, icon);
            else
                return DialogResult.OK;

        }


        internal static bool ReplaceBytesInFile(string inputFileName, string outputFileName, byte[] searchTextBytes, byte[] replaceTextBytes)
        {
            bool success = false;
            try
            {
                // Read the content of the input file
                byte[] fileBytes = File.ReadAllBytes(inputFileName);

                // Convert the ASCII text to bytes
                //byte[] searchTextBytes = Encoding.ASCII.GetBytes(searchText);
                //byte[] replaceTextBytes = Encoding.ASCII.GetBytes(replaceText);

                // Search for the specified ASCII text and replace it
                for (int i = 0; i <= fileBytes.Length - searchTextBytes.Length; i++)
                {
                    bool match = true;

                    for (int j = 0; j < searchTextBytes.Length; j++)
                    {
                        if (fileBytes[i + j] != searchTextBytes[j])
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match)
                    {
                        success = true;

                        Array.Copy(replaceTextBytes, 0, fileBytes, i, replaceTextBytes.Length);
                        // Assuming only one replacement is needed; modify as needed for multiple replacements.

                        // Save the modified content to the output file
                        File.WriteAllBytes(outputFileName, fileBytes);

                        break;
                    }
                }

                return success;
            }
            catch (Exception ex)
            {
                General.Mes("An error occurred: " + ex.Message, MessageBoxIcon.Error);
                return false;
            }
        }

        internal static string GenerateRandomWord(int length)
        {
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random random = new Random();

            return new string(Enumerable.Repeat(allowedChars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

    public static class JsonSerializer
    {
        public static string Serialize<T>(this T data)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                var stream = new MemoryStream();
                serializer.WriteObject(stream, data);
                string jsonData = Encoding.UTF8.GetString(stream.ToArray(), 0, (int)stream.Length);
                stream.Close();
                return jsonData;
            }
            catch (Exception x)
            {
                General.Mes(x.InnerException.ToString(), System.Windows.Forms.MessageBoxIcon.Stop);
                return "";
            }
        }
        public static T Deserialize<T>(this string jsonData)
        {
            try
            {
                DataContractJsonSerializer slzr = new DataContractJsonSerializer(typeof(T));
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonData));
                T data = (T)slzr.ReadObject(stream);
                stream.Close();
                return data;
            }
            catch (Exception x)
            {
                General.Mes(x.InnerException.ToString(), System.Windows.Forms.MessageBoxIcon.Stop);
                return default(T);
            }
        }
    }

}
