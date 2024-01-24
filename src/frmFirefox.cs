using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CookiesRemover
{
    public partial class frmFirefox : Form
    {
        private string user_custom_browsers_location = Path.Combine(Application.StartupPath, "data", "firefox_custom_locations.txt");

        public frmFirefox()
        {
            InitializeComponent();
        }

        private void frmFirefox_Load(object sender, EventArgs e)
        {
            string mozilla = Path.Combine(General.GetUserAppDataRoamingPath(), "Mozilla");
            if (Directory.Exists(mozilla))
                SearchCookiesFile(mozilla);
            string mercury = Path.Combine(General.GetUserAppDataRoamingPath(), "mercury");
            if (Directory.Exists(mercury))
                SearchCookiesFile(mercury);
            string librewolf = Path.Combine(General.GetUserAppDataRoamingPath(), "librewolf");
            if (Directory.Exists(librewolf))
                SearchCookiesFile(librewolf);
            

            //create the default save location near application
            Directory.CreateDirectory(Path.Combine(Application.StartupPath, "data"));

            //when user has define custom browsers via dragdrop show them!
            if (File.Exists(user_custom_browsers_location )){
                foreach (string item in File.ReadAllLines(user_custom_browsers_location))
                {
                    if (File.Exists(item))
                        lst.Items.Add(item);
                } 
            }
        }

        private int SearchCookiesFile(string folder)
        {
            var directory = new DirectoryInfo(folder);

            int found = 0;
            foreach (FileInfo item in directory.GetFiles("cookies.sqlite", SearchOption.AllDirectories).ToList())
            {
                lst.Items.Add(Path.Combine( item.DirectoryName, item.Name));
                found++;
            }

            return found;
        }

        #region " LST DRAG and DROP "

        private void lst_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll;
            }
        }

        private void lst_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (SearchCookiesFile(data[0]) == 0)
                    MessageBox.Show("Could not find the 'cookies.sqlite' file to " + data[0]);
                else
                {
                    General.AppendTextToFile(user_custom_browsers_location, lst.Items[lst.Items.Count-1].ToString());
                }
            }
        }

        #endregion


        private void btnNext_Click(object sender, EventArgs e)
        {
            if (lst.SelectedIndex == -1)
            {
                General.Mes("Please select a profile from listbox to continue");
                return;
            }

            frmCookies x = new frmCookies(General.BrowserType.firerox, lst.SelectedItem.ToString());
            x.ShowDialog();
        }
    }
}
