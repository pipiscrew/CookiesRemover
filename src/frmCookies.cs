using DBManager.DBASES;
using Finisar.SQLite;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CookiesRemover
{
    public partial class frmCookies : Form
    {
        private static SQLiteClass db = null;
        private BrowserItem record = null;
        private string jsonFilepath = null;

        public frmCookies(General.BrowserType browser, string filepath)
        {
            InitializeComponent();

            this.Text = filepath;

            //open suggested dbase
            SQLiteException exErrorCode;
            db = new SQLiteClass("Data Source=" + filepath + ";Version=3;", out exErrorCode);

            if (db.GetConnection() == null)
            {
                General.Mes(exErrorCode.Message, MessageBoxIcon.Error);
                this.Enabled = false;
                return;
            }

            //exec query depend on browser type
            if (browser == General.BrowserType.chromium)
            {
                dgDB.DataSource = db.GetDATATABLE("select host_key as host, last_access_utc as last_access from cookies group by host_key order by last_access_utc desc");
            }
            else if (browser == General.BrowserType.firerox)
            {
                dgDB.DataSource = db.GetDATATABLE("select host, lastAccessed as last_access from moz_cookies group by host order by last_access desc");
            }

            //create the default save location near application
            Directory.CreateDirectory(Path.Combine(Application.StartupPath, "data"));

            //prepare JSON record -- MD5 is the filename itself!!
            this.record = new BrowserItem{
                browserType = browser,
                dbFilePath = filepath,
                md5 = General.CalculateMD5Hash(browser.ToString() + filepath)
            };

            this.Text += " [" + this.record.md5 + "]";

            //add the host dummy column to 2nd grid
            dg2.Columns.Add("host", "host");

            //browser JSON dbase filepath
            jsonFilepath = Path.Combine(Application.StartupPath, "data" , this.record.md5 + ".json");

            //check (vs MD5) if a dbase found for the specific browser!!
            if (File.Exists(jsonFilepath))
            {
                BrowserItem x = JsonSerializer.Deserialize<BrowserItem>(File.ReadAllText(jsonFilepath, Encoding.UTF8));

                //add domains to 2nd grid
                foreach (string item in x.excluded)
                    dg2.Rows.Add(item);
            }

        }

        private void btnExclude_Click(object sender, EventArgs e)
        {
            if (dgDB.SelectedRows.Count == 0)
            { General.Mes("Please select rows on left grid"); return; }

            dgDB.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Cells[0].Value.ToString()).ToList().ForEach(row =>
            {
                if (dg2.Rows.Cast<DataGridViewRow>().Where(s => s.Cells[0].Value != null && s.Cells[0].Value.ToString().Equals(row, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    dg2.Rows.Add(row.ToString());
            });
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            record.excluded = dg2.Rows.Cast<DataGridViewRow>().Select(y => y.Cells[0].Value.ToString()).ToList();

            File.WriteAllText(jsonFilepath, record.Serialize(), Encoding.UTF8);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void dg2_KeyDown(object sender, KeyEventArgs e)
        {
            if (dg2.SelectedRows.Count == 1)
            {
                if (General.Mes("Do you like to delete the selected domain ?", MessageBoxIcon.Information, MessageBoxButtons.YesNoCancel) == System.Windows.Forms.DialogResult.Yes)
                {
                    dg2.Rows.Remove(dg2.SelectedRows[0]);
                }
            }
        }

        private void frmCookies_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (db!=null)
                db.ConnectionClose();
        }

    }
}
