using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchGUI
{
    class cls_historyitem
    {
        public cls_historyitem(string name, string url)
        {
            _name = name;
            _url = url;
        }

        private string _name, _url;

        public string Name
        {
            get { return _name; }
        }

        public string Url
        {
            get { return _url; }
        }
    }
}
