using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchGUI
{
    class cls_historyitem
    {
        public cls_historyitem(string title, string url, string source)
        {
            _name = title + " - " + source;
            _url = url;
            _source = source;
        }

        #region Properties of the class
        private string _name, _url, _source;

        public string Name
        {
            get { return _name; }
        }

        public string Url
        {
            get { return _url; }
        }

        public string Source
        {
            get { return _source; }
        } 
        #endregion
    }
}
