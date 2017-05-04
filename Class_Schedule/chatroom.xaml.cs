using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Networking.Connectivity;
using Windows.Storage.Streams;
using Windows.ApplicationModel.Core;
using Windows.UI.Text;
using Windows.UI;
using System.Text;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Class_Schedule
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class chatroom : Page
    {
        public StreamSocket clientSocket=new StreamSocket();
        public TextBlock show_connect = new TextBlock();
        public Border border = new Border();
        private bool flag_test=false;
        public chatroom()
        {
            this.InitializeComponent();
            //start_listen();
            sever_address.Text = "172.28.43.160";
        }
        public void set_control_block()
        {
            border.Child = show_connect;
            show_message.Children.Add(border);
        }
        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
             try
            {
                string ipaddress = sever_address.Text;
                HostName serverHost = new HostName(ipaddress);
                await clientSocket.ConnectAsync(serverHost,"11211");
                show_connect.HorizontalAlignment = HorizontalAlignment.Center;
                show_connect.Text = "online";
                Brush brush = new SolidColorBrush(Colors.Green);
                border.Background = brush;
                if(flag_test==false)
                {
                    flag_test = true;
                    set_control_block();
                }
            }
            catch
            {
                Debug.WriteLine("erro1");
            }
        }
        private void ReleaseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clientSocket.Dispose();
                show_connect.Text = "offline";
                Brush brush = new SolidColorBrush(Colors.Red);
                border.Background = brush;
            }
            catch
            {
                Debug.WriteLine("erro2");
            }
        }
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataWriter writer = new DataWriter(clientSocket.OutputStream);
                string content = send_message.Text;
                TextBlock tmp = new TextBlock();
                tmp.Text = content;
                Border border = new Border();
                Brush brush = new SolidColorBrush(Colors.LightGray);
                border.Background = brush;
                border.Child = tmp;
                border.CornerRadius= new CornerRadius(5);
                border.HorizontalAlignment = HorizontalAlignment.Right;
                border.Margin = new Thickness(0, 5, 0, 5) ;
                border.Padding= new Thickness(2, 2, 2, 2);
                show_message.Children.Add(border);
                send_message.Text = "";
                byte[] data = Encoding.UTF8.GetBytes(content);  //将字符串转换为字节类型，完全可以不用转换  
                writer.WriteBytes(data);  //写入字节流，当然可以使用WriteString直接写入字符串  
                await writer.StoreAsync();  //异步发送数据  
                writer.DetachStream();  //分离  
                writer.Dispose();  //结束writer
                get_message();
            }
            catch
            {
                Debug.WriteLine("erro2");
            }
        }
        private async void get_message()
        {
            try
            {
                    DataReader reader = new DataReader(clientSocket.InputStream);
                    reader.InputStreamOptions = InputStreamOptions.Partial;  //采用异步方式  
                    await reader.LoadAsync(1024);  //获取一定大小的数据流  
                    string response = reader.ReadString(reader.UnconsumedBufferLength);
                    TextBlock tmp = new TextBlock();
                    tmp.Text = response;
                    Border border = new Border();
                    Brush brush = new SolidColorBrush(Colors.LightGray);
                    border.Background = brush;
                    border.Child = tmp;
                    border.CornerRadius = new CornerRadius(5);
                    border.HorizontalAlignment = HorizontalAlignment.Left;
                    border.Margin = new Thickness(0, 5, 0, 5);
                    border.Padding = new Thickness(2, 2, 2, 2);
                    show_message.Children.Add(border);
            }
            catch
            {
                Debug.WriteLine("erro3");
            }
        }
        async void start_listen()
        {
            StreamSocketListener listener = new StreamSocketListener();
            try
            {
                await listener.BindServiceNameAsync("11211");
                listener.ConnectionReceived += OnConnection;
            }
            catch
            {

            }
        }
        private async void OnConnection(
           StreamSocketListener sender,
           StreamSocketListenerConnectionReceivedEventArgs args)
        {
            DataReader reader = new DataReader(args.Socket.InputStream);
            while (true)
            {
                reader.InputStreamOptions = InputStreamOptions.Partial;  //采用异步方式  
                await reader.LoadAsync(1024);  //获取一定大小的数据流  
                string response = reader.ReadString(reader.UnconsumedBufferLength);
                if (response == "") continue;

                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    TextBlock tmp = new TextBlock();
                    tmp.Text = response;
                    Border border = new Border();
                    Brush brush = new SolidColorBrush(Colors.LightGray);
                    border.Background = brush;
                    border.Child = tmp;
                    border.CornerRadius = new CornerRadius(5);
                    border.HorizontalAlignment = HorizontalAlignment.Left;
                    border.Margin = new Thickness(0, 5, 0, 5);
                    border.Padding = new Thickness(2, 2, 2, 2);
                    show_message.Children.Add(border);
                }
                );
                

                DataWriter writer = new DataWriter(args.Socket.OutputStream);
                string content = "receive";
                byte[] data = Encoding.UTF8.GetBytes(content);  //将字符串转换为字节类型，完全可以不用转换  
                writer.WriteBytes(data);  //写入字节流，当然可以使用WriteString直接写入字符串  
                await writer.StoreAsync();  //异步发送数据  
                writer.DetachStream();  //分离  
                writer.Dispose();  //结束writer
            }
        }
    }
}
