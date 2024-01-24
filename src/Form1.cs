using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CookiesRemover
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmChromium x = new frmChromium();
            x.ShowDialog();
        }

        private void btnFirefox_Click(object sender, EventArgs e)
        {
            frmFirefox x = new frmFirefox();
            x.ShowDialog();
        }

        private void btnChromeDisableSW_Click(object sender, EventArgs e)
        {
            if (!btnChromeDisableSW.Text.Equals("Go!"))
            {
                General.Mes("drag&drop the `chrome.dll` or 'Electron executable' here!");
            }
            else
            {
                Cursor = System.Windows.Forms.Cursors.WaitCursor;

                //fileSystem#appcache#serviceWorker #==00
				//66 69 6C 65 53 79 73 74 65 6D 00 61 70 70 63 61 63 68 65 00 73 65 72 76 69 63 65 57 6F 72 6B 65 72
                byte[] srcChromeDLL = new byte[]{   
                0x66, 0x69, 0x6C, 0x65, 0x53, 0x79, 0x73, 0x74,
                0x65, 0x6D, 0x00, 0x61, 0x70, 0x70, 0x63, 0x61,
                0x63, 0x68, 0x65, 0x00, 0x73, 0x65, 0x72, 0x76,
                0x69, 0x63, 0x65, 0x57, 0x6F, 0x72, 0x6B, 0x65,
                0x72 
            };

                //fileSystem#appcache#
                byte[] replaceChromeDLL = new byte[]{ 
                0x66, 0x69, 0x6C, 0x65, 0x53, 0x79, 0x73, 0x74,
                0x65, 0x6D, 0x00, 0x61, 0x70, 0x70, 0x63, 0x61,
                0x63, 0x68, 0x65, 0x00 
            };

                //generate random word for /serviceworker/ and merge it to repl array
                replaceChromeDLL = replaceChromeDLL.Concat(Encoding.ASCII.GetBytes(General.GenerateRandomWord(13))).ToArray();

                //get filename
                string f = btnChromeDisableSW.Tag.ToString();

                bool patched = false;
                if (File.Exists(f) && General.ReplaceBytesInFile(f, f, srcChromeDLL, replaceChromeDLL))
                {
                    patched = true;
                    General.Mes("Patch success! \r\n\r\nDelete the 'Service Worker' folder is into 'Default' folder \r\n\r\n" + Path.Combine(General.GetUserLocalAppDataPath(), "BraveSoftware") + "\r\nor\r\n" + Path.Combine(General.GetUserLocalAppDataPath(), "Chromium"));
                }
                else if (File.Exists(f))
                {
                    System.Threading.Thread.Sleep(100);

                    //fileSystem#serviceWorker
                    byte[] srcElectronEXE = new byte[]{
                            0x66, 0x69, 0x6C, 0x65, 0x53, 0x79, 0x73, 0x74,
                            0x65, 0x6D, 0x00, 0x73, 0x65, 0x72, 0x76, 0x69,
                            0x63, 0x65, 0x57, 0x6F, 0x72, 0x6B, 0x65, 0x72 
                        };

                    //fileSystem# #==00
                    byte[] replaceElectronEXE = new byte[]{
                            0x66, 0x69, 0x6C, 0x65, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x00     
                        };

                    //generate random word for /serviceworker/ and merge it to repl array
                    replaceElectronEXE = replaceElectronEXE.Concat(Encoding.ASCII.GetBytes(General.GenerateRandomWord(13))).ToArray();

                    if (General.ReplaceBytesInFile(f, f, srcElectronEXE, replaceElectronEXE))
                    {
                        patched = true;
                        General.Mes("Patch success! \r\n\r\nDelete the 'Service Worker' folder is into 'Default' folder \r\n\r\n" + Path.Combine(General.GetUserAppDataRoamingPath(), "Electron application name") + "\r\nor\r\n" + Path.Combine(General.GetUserAppDataRoamingPath(), "Electron application name\\Partitions\\x\\"));
                    }
                }
                
                if (!patched)
                    General.Mes("Failed", MessageBoxIcon.Exclamation);
     

                btnChromeDisableSW.Text = "disable service worker";

                Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        #region " btnChromeDisableSW DRAG and DROP "

        private void btnChromeDisableSW_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll;
            }
        }

        private void btnChromeDisableSW_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);

                btnChromeDisableSW.Tag = data[0];
                btnChromeDisableSW.Text = "Go!";
            }
        }

        #endregion

        #region " btnDefaultWebProtocol DRAG and DROP "

        private void btnDefaultWebProtocol_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll;
            }
        }

        private void btnDefaultWebProtocol_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (data[0].ToString().ToLower().EndsWith(".exe"))
                {
                    btnDefaultWebProtocol.Tag = data[0];
                    btnDefaultWebProtocol.Text = "Go!";
                }
            }
        }

        #endregion

        private void btnDefaultWebProtocol_Click(object sender, EventArgs e)
        {
            if (!btnDefaultWebProtocol.Text.Equals("Go!"))
            {
                General.Mes("drag&drop the `brave.exe` or 'firefox.exe' or any 'browser executable' here!");
            }
            else
            {
                string executable = btnDefaultWebProtocol.Tag.ToString().Replace("\\", "\\\\");
                string regEntryID = "Browser-" + General.GenerateRandomWord(5);
                string applicationName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Path.GetFileNameWithoutExtension(executable));

                string finalRegistry = Properties.Resources.windows_browser_protocol.Replace("%regEntryID%", regEntryID).Replace("%applicationName%", applicationName).Replace("%executable%", executable);

                string filename = Path.Combine(Application.StartupPath, DateTime.Now.ToString("yyyyMMddHHmmss") + ".reg");
                File.WriteAllText(filename, finalRegistry);

                General.Mes("Registry file generated to " + filename);

            }
        }
    }
}
