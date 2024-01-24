using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookiesRemover
{
    public class BrowserItem
    {
       public General.BrowserType browserType { get; set; }
       public string dbFilePath { get; set; }
       public string md5 { get; set; }
       public List<string> excluded { get; set; }
    }
}
