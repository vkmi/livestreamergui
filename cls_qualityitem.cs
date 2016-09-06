using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchGUI
{
    class cls_qualityitem
    {
        private int id;
        private string name;
        private string yt_arg;
        private string tw_arg;

        public  cls_qualityitem( int num, string nome, string yt, string tw)
        {
            id = num;
            name = nome;
            yt_arg = yt;
            tw_arg = tw;
        }

        public int ID
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }

        public string YT_arg
        {
            get { return yt_arg; }
        }

        public string TW_arg
        {
            get { return tw_arg; }
        }

    }
}
