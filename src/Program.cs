using DBManager.DBASES;
using Finisar.SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CookiesRemover
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Count() == 1 && args[0].Equals("-clean"))
            {
                EnumerateJSONandDelete();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //set events
                Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                Application.Run(new Form1());
            }
        }

        private static void EnumerateJSONandDelete()
        {
            string root = Path.Combine(Application.StartupPath, "data");
            string[] json = Directory.GetFiles(root, "*.json");

            foreach (string jsonFilepath in json)
            {
                BrowserItem job = JsonSerializer.Deserialize<BrowserItem>(File.ReadAllText(jsonFilepath, Encoding.UTF8));

                //open suggested dbase
                SQLiteException exErrorCode;
                SQLiteClass db = new SQLiteClass("Data Source=" + job.dbFilePath + ";Version=3;", out exErrorCode);

                if (db.GetConnection() == null)
                {
                    General.Mes(exErrorCode.Message, MessageBoxIcon.Error);
                    return;
                }

                //construct CSV for WHERE query
                string deleteDomains = string.Format("{0}{1}{0}", "'", string.Join("','", job.excluded));

                //correspond browser type, construct the final delete SQL
                string sql = string.Empty;
                if (job.browserType == General.BrowserType.chromium)
                {
                    sql = string.Format("delete from cookies where host_key not in ({0})", deleteDomains);

                    //profiles/default
                    string profileMainPath = Path.GetFullPath(Path.Combine(job.dbFilePath, "..\\..\\"));
                    //profiles/
                    string profileRootPath = Path.GetFullPath(Path.Combine(job.dbFilePath, "..\\..\\..\\"));

                    //delete profileMainPath != exlude folders
                    List<string> excludeProfileMainPath = new List<string>(){ 
                                                                "Extension Rules", 
                                                                "Extension Scripts", 
                                                                "Extension State", 
                                                                "Extensions", 
                                                                "Local Extension Settings", 
                                                                "Local Storage", 
                                                                "Network", 
                                                                "Sessions" };

                    Directory.GetDirectories(profileMainPath).Select(Path.GetFileName).Except(excludeProfileMainPath, StringComparer.OrdinalIgnoreCase).ToList().ForEach(f =>
                    {
                        Directory.Delete(profileMainPath + f, true);
                    });

                    //delete profileRootPath == include folders
                    List<string> includeProfileRootPath = new List<string>(){ 
                                                                "GraphiteDawnCache", 
                                                                "Crowd Deny",
                                                                "Crashpad",
                                                                "GrShaderCache", 
                                                                "ShaderCache", 
                                                                "BraveWallet", 
                                                                "BrowserMetrics" };

                    Directory.GetDirectories(profileRootPath).Select(Path.GetFileName).Intersect(includeProfileRootPath, StringComparer.OrdinalIgnoreCase).ToList().ForEach(f =>
                    {
                        Directory.Delete(profileRootPath + f, true);
                    });

                    //delete profileRootPath == include files
                    List<string> includeFilesProfileRootPath = new List<string>(){ 
                                                                "BrowserMetrics-spare.pma", 
                                                                "CrashpadMetrics-active.pma" };

                    Directory.GetFiles(profileRootPath).Select(Path.GetFileName).Intersect(includeFilesProfileRootPath, StringComparer.OrdinalIgnoreCase).ToList().ForEach(f =>
                    {
                        File.Delete(profileRootPath + f);
                    });


                }
                else if (job.browserType == General.BrowserType.firerox)
                    sql = string.Format("delete from moz_cookies where host not in ({0})", deleteDomains);

                //execute the deletion
                db.ExecuteNonQuery(sql);


                //close dbase
                db.ConnectionClose();
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Unhandled Thread Exception");
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show((e.ExceptionObject as Exception).Message, "Unhandled UI Exception");
        }
    }
}
