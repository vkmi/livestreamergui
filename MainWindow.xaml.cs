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

namespace TwitchGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //lista di stringhe da concatenare per passare parametri
        string path_to_ls = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)+"\\Livestreamer\\livestreamer.exe";
        //string temp_quality = "source";
        string video_quality = "source";
        cls_qualityitem[] qualitylist = new cls_qualityitem[] {
            new cls_qualityitem(0,"Audio", "audio_mp4","audio"),
            new cls_qualityitem(1,"Best (default)", "720p","source"),
            new cls_qualityitem(2,"High", "480p","high"),
            new cls_qualityitem(3,"Medium", "360p","medium"),
            new cls_qualityitem(4,"Low", "240p","low"),
            new cls_qualityitem(5,"Worst", "144p","mobile")};
        string[] typedhistory = new string[20];

        public MainWindow()
        {
            InitializeComponent();
            cmb_quality.ItemsSource = qualitylist;
            cmb_quality.DisplayMemberPath = "Name";
            lst_typedhistory.ItemsSource = typedhistory;
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

        // simple code to clear the text field upon focus
        private void txtin_url_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= txtin_url_GotFocus;
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

        // handles typedhistory
        private void add_to_history()
        {
            for (int i = typedhistory.Length-2; i > 0; i--)
            {
                typedhistory[i] = typedhistory[i - 1];
            }
            typedhistory[0] = txtin_url.Text;
            string[] fake = new string[30];
            lst_typedhistory.ItemsSource = fake;
            lst_typedhistory.ItemsSource = typedhistory;
        }

        // change content of url field and goes back to main tab after selecting an item from history
        private void lst_typedhistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string temp = lst_typedhistory.SelectedItem as string;
            txtin_url.Text = temp;
            tabControl.SelectedItem = main;
        }

    }
}
