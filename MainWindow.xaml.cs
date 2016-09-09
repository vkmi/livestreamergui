using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Media;
using System.Net;
using System.Text.RegularExpressions;
<<<<<<< HEAD
using System.IO;
=======
>>>>>>> b5759978389aeacf62db4f92227865746d7d20f5

namespace TwitchGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //lista di stringhe da concatenare per passare parametri
        #region Global variables
        string path_to_ls = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\Livestreamer\\livestreamer.exe";
        string video_quality = "best";
        cls_qualityitem[] qualitylist = new cls_qualityitem[] {
            new cls_qualityitem(0,"Audio", "audio_mp4","audio"),
            new cls_qualityitem(1,"Best (default)", "720p","source"),
            new cls_qualityitem(2,"High", "480p","high"),
            new cls_qualityitem(3,"Medium", "360p","medium"),
            new cls_qualityitem(4,"Low", "240p","low"),
            new cls_qualityitem(5,"Worst", "144p","mobile")};
<<<<<<< HEAD
        List<cls_historyitem> typedhistory = new List<cls_historyitem>(); 
        List<cls_historyitem> tmptypedhistory = new List<cls_historyitem>(); 
        #endregion
=======
        cls_historyitem[] typedhistory = new cls_historyitem[20];
>>>>>>> b5759978389aeacf62db4f92227865746d7d20f5

        public MainWindow()
        {
            InitializeComponent();

            load_history();

            #region fill combobox/listbox
            cmb_quality.ItemsSource = qualitylist;
            cmb_quality.DisplayMemberPath = "Name";
            lst_typedhistory.ItemsSource = typedhistory;
<<<<<<< HEAD
            lst_typedhistory.DisplayMemberPath = "Name"; 
            #endregion
=======
            lst_typedhistory.DisplayMemberPath = "Name";
>>>>>>> b5759978389aeacf62db4f92227865746d7d20f5
        }

        // play function
        private void btn_play_Click(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo video = new ProcessStartInfo();
            video.CreateNoWindow = true;
            video.UseShellExecute = false;
            video.FileName = path_to_ls;
            video.WindowStyle = ProcessWindowStyle.Normal;
            asssign_quality();
            string args = "";
            if ((chk_timeline.IsChecked == true) && txtin_url.Text.Contains("twitch"))
            {
                args += " --player-passthrough hls";
            }
            args += " " +txtin_url.Text + " " + video_quality;
            video.Arguments = args;
            //MessageBox.Show("url= " + path_to_ls );
            //MessageBox.Show("args= " + args);
            
            Process.Start(video);
            if ((chk_chat.IsChecked == true) && (txtin_url.Text.Contains("twitch")))
            {
                System.Diagnostics.Process.Start(txtin_url.Text + "/chat");
            }
            add_to_history();
        }

        // just passes all typed text as argument to livestreamer to use it like from cli
        private void btn_advanced_Click(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo video = new ProcessStartInfo();
            video.CreateNoWindow = true;
            video.UseShellExecute = false;
            video.FileName = path_to_ls;
            video.WindowStyle = ProcessWindowStyle.Normal;
            string args = txtin_advanced.Text;
            video.Arguments = args;
            Process.Start(video);
        }

        
        // simple code to assign the right keywords based on the kind of stream (youtube/twitch)
        private void asssign_quality()
        {
            cls_qualityitem temp = cmb_quality.SelectedItem as cls_qualityitem;
            if (temp != null)
            {
                if (txtin_url.Text.Contains("youtu"))
                {
                    video_quality = temp.YT_arg;
                }
                else if (txtin_url.Text.Contains("twitch"))
                {
                    video_quality = temp.TW_arg;
                }
            }
            else if (txtin_url.Text.Contains("youtu"))
            {
                video_quality = "720p";
            }
        }

        #region Stuff related to history functions
        // handles typedhistory
        private void add_to_history()
        {
            string title = txtin_url.Text;
<<<<<<< HEAD
            string sauce = "";
            if (txtin_url.Text.Contains("youtu"))
            {
                title = yt_parser();
                sauce = "YouTube";
            }
            else if (txtin_url.Text.Contains("twitch"))
=======
            if (txtin_url.Text.Contains("youtu")) title = yt_parser();
            else if (txtin_url.Text.Contains("twitch")) title = tw_parser();
            for (int i = typedhistory.Length-2; i > 0; i--)
>>>>>>> b5759978389aeacf62db4f92227865746d7d20f5
            {
                title = tw_parser();
                if (txtin_url.Text.Contains("/v/")) sauce = "Twitch VOD";
                else sauce = "Twitch stream";
            }
<<<<<<< HEAD
            typedhistory.Insert(0,new cls_historyitem(title, txtin_url.Text, sauce));
            StreamWriter tempsw = new StreamWriter("History.txt", true);
            tempsw.WriteLine(title + "§" + txtin_url.Text + "§" + sauce);
            tempsw.Close();
            tempsw.Dispose();
=======
            typedhistory[0] = new cls_historyitem(title,txtin_url.Text);
>>>>>>> b5759978389aeacf62db4f92227865746d7d20f5
            string[] fake = new string[30];
            lst_typedhistory.ItemsSource = fake;
            lst_typedhistory.ItemsSource = typedhistory;
        }

        // change content of url field and goes back to main tab after selecting an item from history
        private void lst_typedhistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
<<<<<<< HEAD
            if (lst_typedhistory.SelectedItem != null)
            {
                cls_historyitem temp = lst_typedhistory.SelectedItem as cls_historyitem;
                txtin_url.Text = temp.Url;
                tabControl.SelectedItem = main; 
            }
        }

        // parser for the title of a youtube video
        private string yt_parser()
        {
            string html = new WebClient().DownloadString(txtin_url.Text);
            string[] splitted_html_t1 = Regex.Split(html, "<meta name=\"title\" content=\"");
            string[] splitted_html_t2 = Regex.Split(splitted_html_t1[1], "\">");
            return splitted_html_t2[0];
        }

        // parser for the title of a twitch stream/VOD
        private string tw_parser()
        {
            string html = new WebClient().DownloadString(txtin_url.Text);
            string[] splitted_html_t1 = Regex.Split(html, "' property='og:description'>");
            string[] splitted_html_t2 = Regex.Split(splitted_html_t1[0], "<meta content='");
            return splitted_html_t2[splitted_html_t2.Length - 1];
        }

        private void btn_clrhistory_Click(object sender, RoutedEventArgs e)
        {
            string[] fake = new string[30];
            lst_typedhistory.ItemsSource = fake;
            typedhistory = new List<cls_historyitem>();
            File.Delete("History.txt");
            load_history();
            lst_typedhistory.ItemsSource = typedhistory;
        }

        private void load_history()
        {
            if (!File.Exists("History.txt")) File.Create("History.txt");
            else
            {
                string[] lines = File.ReadAllLines("History.txt");
                List<string[]> triplets = new List<string[]>();
                foreach (string l in lines)
                {
                    triplets.Add(l.Split('§'));
                }
                foreach (string[] ss in triplets)
                {
                    tmptypedhistory.Add(new cls_historyitem(ss[0], ss[1], ss[2]));
                }
                for (int i = 1; i <= tmptypedhistory.Count; i++)
                {
                    if (i <= 20) typedhistory.Add(tmptypedhistory[tmptypedhistory.Count - i]);
                }
            }
        }
        #endregion

        #region GUI input behaviour
        // paste from clipboard in url field on doubleclick
        private void txtin_url_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtin_url.Text = Clipboard.GetText();
        }

        // simple code to clear the text field upon focus
        private void txtin_url_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= txtin_url_GotFocus;
        } 
        #endregion

=======
            cls_historyitem temp = lst_typedhistory.SelectedItem as cls_historyitem;
            txtin_url.Text = temp.Url;
            tabControl.SelectedItem = main;
        }

        // parser for the title of a youtube video
        private string yt_parser()
        {
            string html = new WebClient().DownloadString(txtin_url.Text);
            string[] splitted_html_t1 = Regex.Split(html, "<meta name=\"title\" content=\"");
            string[] splitted_html_t2 = Regex.Split(splitted_html_t1[1], "\">");
            return splitted_html_t2[0];
        }

        // parser for the title of a twitch stream/VOD
        private string tw_parser()
        {
            string html = new WebClient().DownloadString(txtin_url.Text);
            string[] splitted_html_t1 = Regex.Split(html, "' property='og:description'>");
            string[] splitted_html_t2 = Regex.Split(splitted_html_t1[0], "<meta content='");
            return splitted_html_t2[splitted_html_t2.Length - 1];
        }
>>>>>>> b5759978389aeacf62db4f92227865746d7d20f5
    }
}
