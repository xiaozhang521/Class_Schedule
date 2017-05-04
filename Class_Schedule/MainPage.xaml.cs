using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Class_Schedule
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public string week_now;
        public string Lay_in;
        public MainPage()
        {
            this.InitializeComponent();
            week_now = "1";
            Lay_in = "";
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;
            todayDate.Text = currentTime.Month.ToString()+"月" +currentTime.Day.ToString()+"日";
            MyFrame.Navigate(typeof(show_class_schedule));
            ClassSheduleListBoxItem.IsSelected =true;

        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }
        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClassSheduleListBoxItem.IsSelected) { MyFrame.Navigate(typeof(show_class_schedule),week_now); }
            else if (WeatherListBoxItem.IsSelected) { MyFrame.Navigate(typeof(weather)); }
            else if (ChatRoom_ListBoxItem.IsSelected) { MyFrame.Navigate(typeof(chatroom)); }
            else if (Commom_Info_ListBoxItem.IsSelected) { MyFrame.Navigate(typeof(common_info)); }
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if(ClassSheduleListBoxItem.IsSelected||(!ClassSheduleListBoxItem.IsSelected&&!WeatherListBoxItem.IsSelected&&!ChatRoom_ListBoxItem.IsSelected&&!Commom_Info_ListBoxItem.IsSelected))
            AddSplitView.IsPaneOpen = !AddSplitView.IsPaneOpen;
        }
        private void change_week_button_click(object sender, RoutedEventArgs e)
        {
            displaygetweekDialog();
        }
        private async void displaygetweekDialog()
        {
            ContentDialog getweekDialog = new ContentDialog()
            {
                Title = "Change Week",
                Content = new TextBox(),
                PrimaryButtonText = "OK",
                SecondaryButtonText = "Cancel"
            };
            
            ContentDialogResult result = await getweekDialog.ShowAsync();
            if(result== ContentDialogResult.Primary)
            {
                TextBox tmp = getweekDialog.Content as TextBox;
                week_now = tmp.Text;
                MyFrame.Navigate(typeof(show_class_schedule), week_now);
            }
        }
        private void lay_in_button_click(object sender, RoutedEventArgs e)
        {
            Lay_in = "Layin";
            MyFrame.Navigate(typeof(show_class_schedule),Lay_in);
        }
    }
}
