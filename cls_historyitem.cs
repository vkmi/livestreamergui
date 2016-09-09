using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchGUI
{
    class cls_historyitem
    {
<<<<<<< HEAD
        public cls_historyitem(string title, string url, string source)
        {
            _name = title + " - " + source;
            _url = url;
            _source = source;
        }

        #region Properties of the class
        private string _name, _url, _source;
=======
        public cls_historyitem(string name, string url)
        {
            _name = name;
            _url = url;
        }

        private string _name, _url;
>>>>>>> b5759978389aeacf62db4f92227865746d7d20f5

        public string Name
        {
            get { return _name; }
        }

        public string Url
        {
            get { return _url; }
        }
<<<<<<< HEAD

        public string Source
        {
            get { return _source; }
        } 
        #endregion
=======
>>>>>>> b5759978389aeacf62db4f92227865746d7d20f5
    }
}
