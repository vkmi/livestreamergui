﻿using System;
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
using System.IO;

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
        string path_to_fav = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LivestreamerGUI\\Favourites.txt";
        string path_to_history = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LivestreamerGUI\\History.txt";

        cls_qualityitem[] qualitylist = new cls_qualityitem[] {
            new cls_qualityitem(0,"Audio", "audio_mp4","audio"),
            new cls_qualityitem(1,"Best (default)", "720p","source"),
            new cls_qualityitem(2,"High", "480p","high"),
            new cls_qualityitem(3,"Medium", "360p","medium"),
            new cls_qualityitem(4,"Low", "240p","low"),
            new cls_qualityitem(5,"Worst", "144p","mobile")
        };

        List<cls_historyitem> typedhistory = new List<cls_historyitem>(); 
        List<cls_historyitem> tmptypedhistory = new List<cls_historyitem>();
        List<cls_historyitem> favslist = new List<cls_historyitem>();
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists(path_to_ls))
            {
                MessageBox.Show("Livestreamer.exe not found in the default folder. Please install livestreamer in your \"Program Files (x86)\" folder. \nIf you don't have livestreamer installed on your computer you can download it from http://docs.livestreamer.io/install.html");
                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LivestreamerGUI"))
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LivestreamerGUI");
            }
            load_history();
            load_favs();

            #region fill combobox/listbox
            cmb_quality.ItemsSource = qualitylist;
            cmb_quality.DisplayMemberPath = "Name";
            lst_typedhistory.ItemsSource = typedhistory;
            lst_typedhistory.DisplayMemberPath = "Name";
            cmb_favs.ItemsSource = favslist;
            cmb_favs.DisplayMemberPath = "Name";
            #endregion

            // removed because it was more of an annoiance than useful
            /*if (Clipboard.GetText().Contains("youtu") || Clipboard.GetText().Contains("twitch"))
            {
                txtin_url.Text = Clipboard.GetText();
                btn_play_Click(null, null);
            }*/
        }

        // play function
        private void btn_play_Click(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo video = new ProcessStartInfo();
            video.CreateNoWindow = false;
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
            
            /*Process p =*/ Process.Start(video);
            if ((chk_chat.IsChecked == true) && (txtin_url.Text.Contains("twitch")))
            {
                System.Diagnostics.Process.Start(txtin_url.Text + "/chat");
            }
            add_to_history();
            // this should stop me from doing anything until the process is over
            // I'm not sure this is a good idea. It's a good idea for playlists though
            // I can use this only whit playlists
            // p.WaitForExit();
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
            // picks the right keywords depending on source
            string title = txtin_url.Text;
            string sauce = "";
            if (txtin_url.Text.Contains("youtu"))
            {
                title = yt_parser();
                sauce = "YouTube";
            }
            else if (txtin_url.Text.Contains("twitch")) {
                title = tw_parser();
                if (txtin_url.Text.Contains("/v/")) sauce = "Twitch VOD";
                else sauce = "Twitch stream";
            }

            // adds element to the volatile history
            typedhistory.Insert(0,new cls_historyitem(title, txtin_url.Text, sauce));

            // adds element to the txt for non volatile history
            StreamWriter tempsw = new StreamWriter(path_to_history, true);
            tempsw.WriteLine(title + "§" + txtin_url.Text + "§" + sauce);
            tempsw.Close();
            tempsw.Dispose();

            // forces the listbox to update by changing the items source to a fake list and back to the right one
            string[] fake = new string[30];
            lst_typedhistory.ItemsSource = fake;
            lst_typedhistory.ItemsSource = typedhistory;
        }

        // change content of url field and goes back to main tab after selecting an item from history
        private void lst_typedhistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lst_typedhistory.SelectedItem != null)
            {
                cls_historyitem temp = lst_typedhistory.SelectedItem as cls_historyitem;
                txtin_url.Text = temp.Url;
                tabControl.SelectedItem = main; 
            }
        }

        private void btn_clrhistory_Click(object sender, RoutedEventArgs e)
        {
            string[] fake = new string[30];
            lst_typedhistory.ItemsSource = fake;
            typedhistory = new List<cls_historyitem>();
            File.Delete(path_to_history);
            load_history();
            lst_typedhistory.ItemsSource = typedhistory;
        }
        
        // parse last 20 history items from the txt file to a local list
        private void load_history()
        {
            if (!File.Exists(path_to_history)) File.Create(path_to_history);
            else
            {
                string[] lines = File.ReadAllLines(path_to_history);
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

        #region Stuff related to favourites
        private void add_to_favs()
        {
            // picks the right keywords depending on source
            string title = txtin_url.Text;
            string sauce = "";
            if (txtin_url.Text.Contains("youtu"))
            {
                title = yt_parser();
                sauce = "YouTube";
            }
            else if (txtin_url.Text.Contains("twitch"))
            {
                title = tw_parser();
                if (txtin_url.Text.Contains("/v/")) sauce = "Twitch VOD";
                else sauce = "Twitch stream";
            }

            // adds element to the volatile history
            favslist.Add(new cls_historyitem(title, txtin_url.Text, sauce));

            // adds element to the txt for non volatile history
            StreamWriter tempsw = new StreamWriter(path_to_fav, true);
            tempsw.WriteLine(title + "§" + txtin_url.Text + "§" + sauce);
            tempsw.Close();
            tempsw.Dispose();

            // forces the listbox to update by changing the items source to a fake list and back to the right one
            string[] fake = new string[30];
            cmb_favs.ItemsSource = fake;
            cmb_favs.ItemsSource = favslist;
        }

        private void btn_rm_fav_Click(object sender, RoutedEventArgs e)
        {
            if (cmb_favs.SelectedItem != null)
            {
                cls_historyitem si = cmb_favs.SelectedItem as cls_historyitem;
                favslist = si.remove_from_list(favslist);
                remove_from_favtxt();
                string[] fake = new string[30];
                cmb_favs.ItemsSource = fake;
                cmb_favs.ItemsSource = favslist;
            }
        }

        // function called when the button is pressed
        private void btn_add_fav_Click(object sender, RoutedEventArgs e)
        {
            add_to_favs();
        }

        // change content of url field after selecting an item from favs
        private void cmb_favs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmb_favs.SelectedItem != null)
            {
                cls_historyitem temp = cmb_favs.SelectedItem as cls_historyitem;
                txtin_url.Text = temp.Url;
            }
        }

        // parse all items from the txt favourites file to the combobox
        private void load_favs()
        {
            if (!File.Exists(path_to_fav)) File.Create(path_to_fav);
            else
            {
                string[] lines = File.ReadAllLines(path_to_fav);
                List<string[]> triplets = new List<string[]>();
                foreach (string l in lines)
                {
                    triplets.Add(l.Split('§'));
                }
                foreach (string[] ss in triplets)
                {
                    favslist.Add(new cls_historyitem(ss[0], ss[1], ss[2]));
                }
                favslist.Sort((x, y) => x.Name.CompareTo(y.Name));
            }
        }
        #endregion

        #region Support functions
        // parser for the title of a youtube video
        private string yt_parser()
        {
            WebClient url_source = new WebClient();
            url_source.Encoding = Encoding.UTF8;
            string html = url_source.DownloadString(txtin_url.Text);
            string[] splitted_html_t1 = Regex.Split(html, "<meta name=\"title\" content=\"");
            string[] splitted_html_t2 = Regex.Split(splitted_html_t1[1], "\">");
            return splitted_html_t2[0];
        }

        // parser for the title of a twitch stream/VOD
        private string tw_parser()
        {
            WebClient url_source = new WebClient();
            url_source.Encoding = Encoding.UTF8;
            string html = url_source.DownloadString(txtin_url.Text);
            string[] splitted_html_t1 = Regex.Split(html, "' property='og:description'>");
            string[] splitted_html_t2 = Regex.Split(splitted_html_t1[0], "<meta content='");
            return splitted_html_t2[splitted_html_t2.Length - 1];
        }

        private void remove_from_favtxt()
        {
            StreamWriter swtemp = new StreamWriter("temp.txt", true);
            foreach (cls_historyitem item in favslist)
            {
                swtemp.WriteLine(item.Name + "§" + item.Url + "§" + item.Source);
            }
            swtemp.Close();
            swtemp.Dispose();
            File.Delete(path_to_fav);
            File.Copy("temp.txt", path_to_fav);
            File.Delete("temp.txt");
        }
        #endregion

        #region GUI input behaviour
        // paste from clipboard in url field on doubleclick
        private void txtin_url_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Clipboard.GetText().Contains("http")) 
            {
                txtin_url.Text = Clipboard.GetText();
                if (Clipboard.GetText().Contains("youtu") || Clipboard.GetText().Contains("twitch"))
                    btn_play_Click(null, null);
            }
        }

        // simple code to clear the text field upon focus
        private void txtin_url_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= txtin_url_GotFocus;
        }

        private void txtin_url_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                btn_play_Click(null, null);
            }
        }

        #endregion
    }
}
