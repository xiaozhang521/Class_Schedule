using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using System.Text;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using System.Diagnostics;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Class_Schedule
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class show_class_schedule : Page
    {
        public people student =new people();
        public string now_week = "1";
        public Button tmp_week_box = new Button();
        public show_class_schedule()
        {
            this.InitializeComponent();
        }
        private people html_serializer()
        {
            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlAgilityPack.HtmlDocument();
            string filename = "assets/class_schedule.html";
            /*StreamReader sr = File.OpenText(filename);
            ////System.Diagnostics.Debug.WriteLine(sr.CurrentEncoding);
            //System.Diagnostics.Debug.WriteLine(sr.ReadToEnd());*/
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(fs,Encoding.GetEncoding("GB2312"));
            htmldoc.Load(read);
            string tmps;
            if (htmldoc.DocumentNode != null)
            {
                ////System.Diagnostics.Debug.WriteLine(htmldoc.DocumentNode.InnerText);
                ////System.Diagnostics.Debug.WriteLine(htmldoc.DocumentNode.LastChild.InnerText);
                tmps = htmldoc.DocumentNode.LastChild.InnerText;
                string message;
                int flag = 0;
                int flag2 = 0;
                int cot_day = 0;
                for (int i = 0; i < tmps.Length; ++i)
                {
                    if (tmps[i] == '<')
                    {
                        int cotval = 1;
                        while(cotval!=0&&i<tmps.Length)
                        {
                            i++;
                            if (tmps[i] == '<') cotval++;
                            else if (tmps[i] == '>') cotval--;
                        }
                    }
                    else if(tmps[i]!=' '&&tmps[i]!='\t'&&tmps[i]!='\n')
                    {
                        int cot = i;
                        if (flag2 == 0)
                        {
                            while (i < tmps.Length && tmps[i] != ';' && tmps[i] != ' ' && tmps[i] != '\n')
                            {
                                i++;
                            }
                        }
                        else
                        {
                            while (i < tmps.Length && tmps[i] != '\n')
                                i++;
                        }
                        message = tmps.Substring(cot,i-cot);
                        if(message.IndexOf("东北大学",0)!=-1)
                        {
                            flag = 1;
                            student.school = "NEU";
                        }
                        else if(message.IndexOf("星期日",0)!=-1)
                        {
                            flag2 = 1;
                            flag = 0;
                            continue;
                        }
                        if(flag==1)
                        {
                            if(message.IndexOf("院系",0)!=-1)
                            {
                                int tmpval = message.IndexOf(":", 0);
                                tmpval++;
                                int j;
                                for (j=tmpval; j < message.Length;++j)
                                { if (message[j] == '&') break; }
                                student.institute = message.Substring(tmpval,j-tmpval);
                            }
                            if (message.IndexOf("专业", 0) != -1)
                            {
                                int tmpval = message.IndexOf(":", 0);
                                tmpval++;
                                int j;
                                for (j = tmpval; j < message.Length; ++j)
                                { if (message[j] == '&') break; }
                                student.marjor = message.Substring(tmpval, j - tmpval);
                            }
                            if (message.IndexOf("班级", 0) != -1)
                            {
                                int tmpval = message.IndexOf(":", 0);
                                tmpval++;
                                int j;
                                for (j = tmpval; j < message.Length; ++j)
                                { if (message[j] == '&') break; }
                                student.class_number = message.Substring(tmpval, j - tmpval);
                            }
                            if (message.IndexOf("学号", 0) != -1)
                            {
                                int tmpval = message.IndexOf(":", 0);
                                tmpval++;
                                int j;
                                for (j = tmpval; j < message.Length; ++j)
                                { if (message[j] == '&') break; }
                                student.school_number = message.Substring(tmpval, j - tmpval);
                            }
                            if (message.IndexOf("姓名", 0) != -1)
                            {
                                int tmpval = message.IndexOf(":", 0);
                                tmpval++;
                                int j;
                                for (j = tmpval; j < message.Length; ++j)
                                { if (message[j] == '&') break; }
                                student.name = message.Substring(tmpval, j - tmpval);
                            }
                        }
                        else if(flag2==1)
                        {
                            if(cot_day<7)
                            {
                                if (message[0] != '1')
                                {
                                    if (message[0] == '&')
                                    {
                                        class_message newclass = new class_message();
                                        newclass.flag = false;
                                    }
                                    else
                                    {
                                        //every single class
                                        string tmp_val = "";
                                        for (int j = 0; j < message.Length; ++j)
                                        {
                                            int start = j;
                                            while (j < message.Length && message[j] != ' ')
                                            { ++j; }
                                            if (j != start)
                                                tmp_val = message.Substring(start, j - start);
                                            else continue;
                                            if ('0' <= tmp_val[0] && tmp_val[0] <= '9')
                                            {
                                                student.first.Last().class_length = tmp_val[0] - '0';
                                                tmp_val = tmp_val.Substring(2);
                                            }
                                            if (tmp_val.Length != 0)
                                            {
                                                class_message newclass = new class_message();
                                                newclass.flag = true;
                                                int tmp_cot = tmp_val.Length - 2;
                                                while (tmp_cot >= 0 && tmp_val[tmp_cot] != 'A' && tmp_val[tmp_cot] != 'B')
                                                {
                                                    tmp_cot--;
                                                }
                                                tmp_cot -= 2;
                                                newclass.class_address = tmp_val.Substring(tmp_cot, 6);
                                                tmp_cot += 6;
                                                //start_time and end_time
                                                if (tmp_val.Length - tmp_cot == 2)
                                                {
                                                    newclass.class_start_time = tmp_val[tmp_cot] - '0';
                                                }
                                                else if (tmp_val.Length - tmp_cot == 3)
                                                {
                                                    newclass.class_start_time = (tmp_val[tmp_cot] - '0') * 10 + (tmp_val[tmp_cot + 1] - '0');
                                                }
                                                else
                                                {
                                                    if (tmp_val[tmp_cot + 1] != '-')
                                                    {
                                                        int start_time = tmp_val[tmp_cot] - '0';
                                                        start_time = start_time * 10 + (tmp_val[tmp_cot + 1] - '0');
                                                        newclass.class_start_time = start_time;
                                                        if (tmp_val[tmp_cot + 4] != '周')
                                                        {
                                                            int end_time = tmp_val[tmp_cot + 3] - '0';
                                                            end_time = end_time * 10 + (tmp_val[tmp_cot + 4] - '0');
                                                            newclass.class_end_time = end_time;
                                                        }
                                                        else
                                                        {
                                                            newclass.class_end_time = tmp_val[tmp_cot + 3] - '0';
                                                        }
                                                    }
                                                    else
                                                    {
                                                        newclass.class_start_time = tmp_val[tmp_cot] - '0';
                                                        if (tmp_val[tmp_cot + 3] != '周')
                                                        {
                                                            int end_time = tmp_val[tmp_cot + 2] - '0';
                                                            end_time = end_time * 10 + (tmp_val[tmp_cot + 3] - '0');
                                                            newclass.class_end_time = end_time;
                                                        }
                                                        else
                                                        {
                                                            newclass.class_end_time = tmp_val[tmp_cot + 2] - '0';
                                                        }
                                                    }
                                                }
                                                tmp_cot -= 6;
                                                //System.Diagnostics.Debug.WriteLine(tmp_val +cot_day+ "||");
                                                newclass.class_name = tmp_val.Substring(0, tmp_cot);
                                                newclass.class_day = cot_day % 7;
                                                student.first.Insert(student.first.Count, newclass);
                                            }
                                            // //System.Diagnostics.Debug.WriteLine(tmp_val);
                                        }
                                    }
                                    cot_day++;
                                } 
                            }
                            else if (cot_day < 14)
                            {
                                if (message[0] != '3')
                                {
                                    if (message[0] == '&')
                                    {
                                        class_message newclass = new class_message();
                                        newclass.flag = false;
                                    }
                                    else
                                    {
                                        //every single class
                                        string tmp_val = "";
                                        for (int j = 0; j < message.Length; ++j)
                                        {
                                            int start = j;
                                            while (j < message.Length && message[j] != ' ')
                                            { ++j; }
                                            if (j != start)
                                                tmp_val = message.Substring(start, j - start);
                                            else continue;
                                            if ('0' <= tmp_val[0] && tmp_val[0] <= '9')
                                            {
                                                student.second.Last().class_length = tmp_val[0] - '0';
                                                tmp_val = tmp_val.Substring(2);
                                            }
                                            if (tmp_val.Length != 0)
                                            {
                                                ////System.Diagnostics.Debug.WriteLine(tmp_val + "||");
                                                class_message newclass = new class_message();
                                                newclass.flag = true;
                                                int tmp_cot = tmp_val.Length - 2;
                                                while (tmp_cot >= 0 && tmp_val[tmp_cot] != 'A' && tmp_val[tmp_cot] != 'B')
                                                {
                                                    tmp_cot--;
                                                }
                                                tmp_cot -= 2;
                                                newclass.class_address = tmp_val.Substring(tmp_cot, 6);
                                                tmp_cot += 6;
                                                //start_time and end_time
                                                if (tmp_val.Length - tmp_cot == 2)
                                                {
                                                    newclass.class_start_time = tmp_val[tmp_cot] - '0';
                                                }
                                                else if (tmp_val.Length - tmp_cot == 3)
                                                {
                                                    newclass.class_start_time = (tmp_val[tmp_cot] - '0') * 10 + (tmp_val[tmp_cot + 1] - '0');
                                                }
                                                else
                                                {
                                                    if (tmp_val[tmp_cot + 1] != '-')
                                                    {
                                                        int start_time = tmp_val[tmp_cot] - '0';
                                                        start_time = start_time * 10 + (tmp_val[tmp_cot + 1] - '0');
                                                        newclass.class_start_time = start_time;
                                                        if (tmp_val[tmp_cot + 4] != '周')
                                                        {
                                                            int end_time = tmp_val[tmp_cot + 3] - '0';
                                                            end_time = end_time * 10 + (tmp_val[tmp_cot + 4] - '0');
                                                            newclass.class_end_time = end_time;
                                                        }
                                                        else
                                                        {
                                                            newclass.class_end_time = tmp_val[tmp_cot + 3] - '0';
                                                        }
                                                    }
                                                    else
                                                    {
                                                        newclass.class_start_time = tmp_val[tmp_cot] - '0';
                                                        if (tmp_val[tmp_cot + 3] != '周')
                                                        {
                                                            int end_time = tmp_val[tmp_cot + 2] - '0';
                                                            end_time = end_time * 10 + (tmp_val[tmp_cot + 3] - '0');
                                                            newclass.class_end_time = end_time;
                                                        }
                                                        else
                                                        {
                                                            newclass.class_end_time = tmp_val[tmp_cot + 2] - '0';
                                                        }
                                                    }
                                                }
                                                tmp_cot -= 6;
                                                ////System.Diagnostics.Debug.WriteLine(tmp_val + "||");
                                                newclass.class_name = tmp_val.Substring(0, tmp_cot);
                                                newclass.class_day = cot_day % 7;
                                                student.second.Insert(student.second.Count, newclass);
                                            }
                                            // //System.Diagnostics.Debug.WriteLine(tmp_val);
                                        }
                                    }
                                    cot_day++;
                                }
                            }
                            else if (cot_day < 21)
                            {
                                if (message[0] != '5')
                                {
                                    if (message[0] == '&')
                                    {
                                        class_message newclass = new class_message();
                                        newclass.flag = false;
                                    }
                                    else
                                    {
                                        //every single class
                                        string tmp_val = "";
                                        for (int j = 0; j < message.Length; ++j)
                                        {
                                            int start = j;
                                            while (j < message.Length && message[j] != ' ')
                                            { ++j; }
                                            if (j != start)
                                                tmp_val = message.Substring(start, j - start);
                                            else continue;
                                            if ('0' <= tmp_val[0] && tmp_val[0] <= '9')
                                            {
                                                student.third.Last().class_length = tmp_val[0] - '0';
                                                tmp_val = tmp_val.Substring(2);
                                            }
                                            if (tmp_val.Length != 0)
                                            {
                                                class_message newclass = new class_message();
                                                newclass.flag = true;
                                                int tmp_cot = tmp_val.Length - 2;
                                                while (tmp_cot >= 0 && tmp_val[tmp_cot] != 'A' && tmp_val[tmp_cot] != 'B')
                                                {
                                                    tmp_cot--;
                                                }
                                                tmp_cot -= 2;
                                                newclass.class_address = tmp_val.Substring(tmp_cot, 6);
                                                tmp_cot += 6;
                                                //start_time and end_time
                                                if (tmp_val.Length - tmp_cot == 2)
                                                {
                                                    newclass.class_start_time = tmp_val[tmp_cot] - '0';
                                                }
                                                else if (tmp_val.Length - tmp_cot == 3)
                                                {
                                                    newclass.class_start_time = (tmp_val[tmp_cot] - '0') * 10 + (tmp_val[tmp_cot + 1] - '0');
                                                }
                                                else
                                                {
                                                    if (tmp_val[tmp_cot + 1] != '-')
                                                    {
                                                        int start_time = tmp_val[tmp_cot] - '0';
                                                        start_time = start_time * 10 + (tmp_val[tmp_cot + 1] - '0');
                                                        newclass.class_start_time = start_time;
                                                        if (tmp_val[tmp_cot + 4] != '周')
                                                        {
                                                            int end_time = tmp_val[tmp_cot + 3] - '0';
                                                            end_time = end_time * 10 + (tmp_val[tmp_cot + 4] - '0');
                                                            newclass.class_end_time = end_time;
                                                        }
                                                        else
                                                        {
                                                            newclass.class_end_time = tmp_val[tmp_cot + 3] - '0';
                                                        }
                                                    }
                                                    else
                                                    {
                                                        newclass.class_start_time = tmp_val[tmp_cot] - '0';
                                                        if (tmp_val[tmp_cot + 3] != '周')
                                                        {
                                                            int end_time = tmp_val[tmp_cot + 2] - '0';
                                                            end_time = end_time * 10 + (tmp_val[tmp_cot + 3] - '0');
                                                            newclass.class_end_time = end_time;
                                                        }
                                                        else
                                                        {
                                                            newclass.class_end_time = tmp_val[tmp_cot + 2] - '0';
                                                        }
                                                    }
                                                }
                                                tmp_cot -= 6;
                                                //System.Diagnostics.Debug.WriteLine(tmp_val + cot_day + "||");
                                                newclass.class_name = tmp_val.Substring(0, tmp_cot);
                                                newclass.class_day = cot_day % 7;
                                                student.third.Insert(student.third.Count, newclass);
                                            }
                                            // //System.Diagnostics.Debug.WriteLine(tmp_val);
                                        }
                                    }
                                    cot_day++;
                                }
                            }
                            else if (cot_day < 28)
                            {
                                if (message[0] != '7')
                                {
                                    if (message[0] == '&')
                                    {
                                        class_message newclass = new class_message();
                                        newclass.flag = false;
                                    }
                                    else
                                    {
                                        //every single class
                                        string tmp_val = "";
                                        for (int j = 0; j < message.Length; ++j)
                                        {
                                            int start = j;
                                            while (j < message.Length && message[j] != ' ')
                                            { ++j; }
                                            if (j != start)
                                                tmp_val = message.Substring(start, j - start);
                                            else continue;
                                            if ('0' <= tmp_val[0] && tmp_val[0] <= '9')
                                            {
                                                student.fourth.Last().class_length = tmp_val[0] - '0';
                                                tmp_val = tmp_val.Substring(2);
                                            }
                                            if (tmp_val.Length != 0)
                                            {
                                                class_message newclass = new class_message();
                                                newclass.flag = true;
                                                int tmp_cot = tmp_val.Length - 2;
                                                while (tmp_cot >= 0 && tmp_val[tmp_cot] != 'A' && tmp_val[tmp_cot] != 'B')
                                                {
                                                    tmp_cot--;
                                                }
                                                tmp_cot -= 2;
                                                newclass.class_address = tmp_val.Substring(tmp_cot, 6);
                                                tmp_cot += 6;
                                                //start_time and end_time
                                                if (tmp_val.Length - tmp_cot == 2)
                                                {
                                                    newclass.class_start_time = tmp_val[tmp_cot] - '0';
                                                }
                                                else if (tmp_val.Length - tmp_cot == 3)
                                                {
                                                    newclass.class_start_time = (tmp_val[tmp_cot] - '0') * 10 + (tmp_val[tmp_cot + 1] - '0');
                                                }
                                                else
                                                {
                                                    if (tmp_val[tmp_cot + 1] != '-')
                                                    {
                                                        int start_time = tmp_val[tmp_cot] - '0';
                                                        start_time = start_time * 10 + (tmp_val[tmp_cot + 1] - '0');
                                                        newclass.class_start_time = start_time;
                                                        if (tmp_val[tmp_cot + 4] != '周')
                                                        {
                                                            int end_time = tmp_val[tmp_cot + 3] - '0';
                                                            end_time = end_time * 10 + (tmp_val[tmp_cot + 4] - '0');
                                                            newclass.class_end_time = end_time;
                                                        }
                                                        else
                                                        {
                                                            newclass.class_end_time = tmp_val[tmp_cot + 3] - '0';
                                                        }
                                                    }
                                                    else
                                                    {
                                                        newclass.class_start_time = tmp_val[tmp_cot] - '0';
                                                        if (tmp_val[tmp_cot + 3] != '周')
                                                        {
                                                            int end_time = tmp_val[tmp_cot + 2] - '0';
                                                            end_time = end_time * 10 + (tmp_val[tmp_cot + 3] - '0');
                                                            newclass.class_end_time = end_time;
                                                        }
                                                        else
                                                        {
                                                            newclass.class_end_time = tmp_val[tmp_cot + 2] - '0';
                                                        }
                                                    }
                                                }
                                                tmp_cot -= 6;
                                                //System.Diagnostics.Debug.WriteLine(tmp_val + cot_day + "||");
                                                newclass.class_name = tmp_val.Substring(0, tmp_cot);
                                                newclass.class_day = cot_day % 7;
                                                student.fourth.Insert(student.fourth.Count, newclass);
                                            }
                                            // //System.Diagnostics.Debug.WriteLine(tmp_val);
                                        }
                                    }
                                    cot_day++;
                                }
                            }
                            else if (cot_day < 35)
                            {
                                if (message[0] != '9')
                                {
                                    if (message[0] == '&')
                                    {
                                        class_message newclass = new class_message();
                                        newclass.flag = false;
                                    }
                                    else
                                    {
                                        //every single class
                                        string tmp_val = "";
                                        for (int j = 0; j < message.Length; ++j)
                                        {
                                            int start = j;
                                            while (j < message.Length && message[j] != ' ')
                                            { ++j; }
                                            if (j != start)
                                                tmp_val = message.Substring(start, j - start);
                                            else continue;
                                            if ('0' <= tmp_val[0] && tmp_val[0] <= '9')
                                            {
                                                student.fifth.Last().class_length = tmp_val[0] - '0';
                                                tmp_val = tmp_val.Substring(2);
                                            }
                                            if (tmp_val.Length != 0)
                                            {
                                                class_message newclass = new class_message();
                                                newclass.flag = true;
                                                int tmp_cot = tmp_val.Length - 2;
                                                while (tmp_cot >= 0 && tmp_val[tmp_cot] != 'A' && tmp_val[tmp_cot] != 'B')
                                                {
                                                    tmp_cot--;
                                                }
                                                tmp_cot -= 2;
                                                newclass.class_address = tmp_val.Substring(tmp_cot, 6);
                                                tmp_cot += 6;
                                                //start_time and end_time
                                                if (tmp_val.Length - tmp_cot == 2)
                                                {
                                                    newclass.class_start_time = tmp_val[tmp_cot] - '0';
                                                }
                                                else if (tmp_val.Length - tmp_cot == 3)
                                                {
                                                    newclass.class_start_time = (tmp_val[tmp_cot] - '0') * 10 + (tmp_val[tmp_cot + 1] - '0');
                                                }
                                                else
                                                {
                                                    if (tmp_val[tmp_cot + 1] != '-')
                                                    {
                                                        int start_time = tmp_val[tmp_cot] - '0';
                                                        start_time = start_time * 10 + (tmp_val[tmp_cot + 1] - '0');
                                                        newclass.class_start_time = start_time;
                                                        if (tmp_val[tmp_cot + 4] != '周')
                                                        {
                                                            int end_time = tmp_val[tmp_cot + 3] - '0';
                                                            end_time = end_time * 10 + (tmp_val[tmp_cot + 4] - '0');
                                                            newclass.class_end_time = end_time;
                                                        }
                                                        else
                                                        {
                                                            newclass.class_end_time = tmp_val[tmp_cot + 3] - '0';
                                                        }
                                                    }
                                                    else
                                                    {
                                                        newclass.class_start_time = tmp_val[tmp_cot] - '0';
                                                        if (tmp_val[tmp_cot + 3] != '周')
                                                        {
                                                            int end_time = tmp_val[tmp_cot + 2] - '0';
                                                            end_time = end_time * 10 + (tmp_val[tmp_cot + 3] - '0');
                                                            newclass.class_end_time = end_time;
                                                        }
                                                        else
                                                        {
                                                            newclass.class_end_time = tmp_val[tmp_cot + 2] - '0';
                                                        }
                                                    }
                                                }
                                                tmp_cot -= 6;
                                                //System.Diagnostics.Debug.WriteLine(tmp_val + cot_day + "||");
                                                newclass.class_name = tmp_val.Substring(0, tmp_cot);
                                                newclass.class_day = cot_day % 7;
                                                student.fifth.Insert(student.fifth.Count, newclass);
                                            }
                                            // //System.Diagnostics.Debug.WriteLine(tmp_val);
                                        }
                                    }
                                    cot_day++;
                                }
                            }
                            else if(cot_day < 42)
                            {
                                if (message[0] != '1')
                                {
                                    if (message[0] == '&')
                                    {
                                        class_message newclass = new class_message();
                                        newclass.flag = false;
                                    }
                                    else
                                    {
                                        //every single class
                                        string tmp_val = "";
                                        for (int j = 0; j < message.Length; ++j)
                                        {
                                            int start = j;
                                            while (j < message.Length && message[j] != ' ')
                                            { ++j; }
                                            if (j != start)
                                                tmp_val = message.Substring(start, j - start);
                                            else continue;
                                            if ('0' <= tmp_val[0] && tmp_val[0] <= '9')
                                            {
                                                student.sixth.Last().class_length = tmp_val[0] - '0';
                                                tmp_val = tmp_val.Substring(2);
                                            }
                                            if (tmp_val.Length != 0)
                                            {
                                                class_message newclass = new class_message();
                                                newclass.flag = true;
                                                int tmp_cot = tmp_val.Length - 2;
                                                while (tmp_cot >= 0 && tmp_val[tmp_cot] != 'A' && tmp_val[tmp_cot] != 'B')
                                                {
                                                    tmp_cot--;
                                                }
                                                tmp_cot -= 2;
                                                newclass.class_address = tmp_val.Substring(tmp_cot, 6);
                                                tmp_cot += 6;
                                                //start_time and end_time
                                                if (tmp_val.Length - tmp_cot == 2)
                                                {
                                                    newclass.class_start_time = tmp_val[tmp_cot] - '0';
                                                }
                                                else if (tmp_val.Length - tmp_cot == 3)
                                                {
                                                    newclass.class_start_time = (tmp_val[tmp_cot] - '0') * 10 + (tmp_val[tmp_cot + 1] - '0');
                                                }
                                                else
                                                {
                                                    if (tmp_val[tmp_cot + 1] != '-')
                                                    {
                                                        int start_time = tmp_val[tmp_cot] - '0';
                                                        start_time = start_time * 10 + (tmp_val[tmp_cot + 1] - '0');
                                                        newclass.class_start_time = start_time;
                                                        if (tmp_val[tmp_cot + 4] != '周')
                                                        {
                                                            int end_time = tmp_val[tmp_cot + 3] - '0';
                                                            end_time = end_time * 10 + (tmp_val[tmp_cot + 4] - '0');
                                                            newclass.class_end_time = end_time;
                                                        }
                                                        else
                                                        {
                                                            newclass.class_end_time = tmp_val[tmp_cot + 3] - '0';
                                                        }
                                                    }
                                                    else
                                                    {
                                                        newclass.class_start_time = tmp_val[tmp_cot] - '0';
                                                        if (tmp_val[tmp_cot + 3] != '周')
                                                        {
                                                            int end_time = tmp_val[tmp_cot + 2] - '0';
                                                            end_time = end_time * 10 + (tmp_val[tmp_cot + 3] - '0');
                                                            newclass.class_end_time = end_time;
                                                        }
                                                        else
                                                        {
                                                            newclass.class_end_time = tmp_val[tmp_cot + 2] - '0';
                                                        }
                                                    }
                                                }
                                                tmp_cot -= 6;
                                                //System.Diagnostics.Debug.WriteLine(tmp_val + cot_day + "||");
                                                newclass.class_name = tmp_val.Substring(0, tmp_cot);
                                                newclass.class_day = cot_day % 7;
                                                student.sixth.Insert(student.sixth.Count, newclass);
                                            }
                                            // //System.Diagnostics.Debug.WriteLine(tmp_val);
                                        }
                                    }
                                    cot_day++;
                                }
                            }
                            else
                            { break; }
                        }
                    }
                }
                /*System.Diagnostics.Debug.WriteLine("first:");
                for (int i=0; i<student.first.Count;++i)
                {
                    class_message tmp = student.first[i];
                    System.Diagnostics.Debug.WriteLine(tmp.class_name);
                    System.Diagnostics.Debug.WriteLine(tmp.class_start_time);
                }
                System.Diagnostics.Debug.WriteLine("sed:");
                for (int i = 0; i < student.second.Count; ++i)
                {
                    class_message tmp = student.second[i];
                    System.Diagnostics.Debug.WriteLine(tmp.class_name);
                    System.Diagnostics.Debug.WriteLine(tmp.class_start_time);
                }
                System.Diagnostics.Debug.WriteLine("thi:");
                for (int i = 0; i < student.third.Count; ++i)
                {
                    class_message tmp = student.third[i];
                    System.Diagnostics.Debug.WriteLine(tmp.class_name);
                    System.Diagnostics.Debug.WriteLine(tmp.class_start_time);
                }
                System.Diagnostics.Debug.WriteLine("fou:");
                for (int i = 0; i < student.fourth.Count; ++i)
                {
                    class_message tmp = student.fourth[i];
                    System.Diagnostics.Debug.WriteLine(tmp.class_name);
                    System.Diagnostics.Debug.WriteLine(tmp.class_start_time);
                }
                System.Diagnostics.Debug.WriteLine("fif:");
                for (int i = 0; i < student.fifth.Count; ++i)
                {
                    class_message tmp = student.fifth[i];
                    System.Diagnostics.Debug.WriteLine(tmp.class_name);
                    System.Diagnostics.Debug.WriteLine(tmp.class_start_time);
                }
                System.Diagnostics.Debug.WriteLine("six:");
                for (int i = 0; i < student.sixth.Count; ++i)
                {
                    class_message tmp = student.sixth[i];
                    System.Diagnostics.Debug.WriteLine(tmp.class_name);
                    System.Diagnostics.Debug.WriteLine(tmp.class_start_time);
                }*/
                //System.Diagnostics.Debug.WriteLine(student.institute + student.marjor + student.school_number+student.name);
            }
            return student;
        }
        private void show_message()
        {
            var border_week = new Border()
            { BorderBrush = new SolidColorBrush(Colors.LightGray), BorderThickness = new Thickness(1) };
            Grid.SetColumn(border_week, 0);
            Grid.SetRow(border_week, 0);
            tmp_week_box.VerticalAlignment = VerticalAlignment.Center;
            tmp_week_box.HorizontalAlignment = HorizontalAlignment.Center;
            tmp_week_box.Content = "第"+now_week+"周";
            tmp_week_box.Background = new SolidColorBrush(Colors.White); ;
            border_week.Child = tmp_week_box;
            tmp_week_box.Click += show_week_class_click;
            show_class_message.Children.Add(border_week);
            for (int i=1;i<8;++i)
            {
                var border = new Border()
                { BorderBrush = new SolidColorBrush(Colors.LightGray), BorderThickness = new Thickness(1) };
                Grid.SetColumn(border, i);
                Grid.SetRow(border, 0);
                var tmp_text_box = new TextBlock();
                tmp_text_box.VerticalAlignment = VerticalAlignment.Center;
                tmp_text_box.HorizontalAlignment = HorizontalAlignment.Center;
                border.Child = tmp_text_box;
                show_class_message.Children.Add(border);
                if(i==1)
                {
                    tmp_text_box.Text = "星期一";
                }
                else if (i == 2)
                {
                    tmp_text_box.Text = "星期二";
                }
                else if (i == 3)
                {
                    tmp_text_box.Text = "星期三";
                }
                else if (i == 4)
                {
                    tmp_text_box.Text = "星期四";
                }
                else if (i == 5)
                {
                    tmp_text_box.Text = "星期五";
                }
                else if (i == 6)
                {
                    tmp_text_box.Text = "星期六";
                }
                else
                {
                    tmp_text_box.Text = "星期日";
                }
            }
            for(int i=1;i<13;i+=2)
            {
                var border = new Border()
                { BorderBrush = new SolidColorBrush(Colors.LightGray), BorderThickness = new Thickness(1) };
                Grid.SetColumn(border, 0);
                Grid.SetRow(border, i);
                Grid.SetRowSpan(border,2);
                var tmp_text_box = new TextBlock();
                tmp_text_box.Text = (i ).ToString() + "\n/\n" + (i +1).ToString();
                tmp_text_box.VerticalAlignment = VerticalAlignment.Center;
                tmp_text_box.HorizontalAlignment = HorizontalAlignment.Center;
                border.Child = tmp_text_box;
                show_class_message.Children.Add(border);
            }
            for (int i = 1; i < 8; i++)
            {
                bool flag = false;
                for (int j = 0; j < student.first.Count; ++j)
                {
                    var border = new Border()
                    { BorderBrush = new SolidColorBrush(Colors.LightGray), BorderThickness = new Thickness(1) };
                    Grid.SetColumn(border, i);
                    Grid.SetRow(border, 1);
                    Grid.SetRowSpan(border, 2);
                    var tmp_text_box = new Button();
                    string show_class_name = "";
                    show_class_message.Children.Add(border);
                    while (j< student.first.Count&&student.first[j].class_day==i-1)
                    {
                        if (student.first[j].class_name.Length > 10)
                        {
                            string tmp_string = student.first[j].class_name.Substring(6);
                            student.first[j].class_name = student.first[j].class_name.Substring(0, 6) + "\n" + tmp_string;
                        }
                        if (flag)
                            show_class_name =show_class_name+ "\n" + student.first[j].class_name;
                        else
                            show_class_name = show_class_name + student.first[j].class_name ;
                        flag = true;
                        j++;
                    }
                    if (flag)
                    {
                        tmp_text_box.VerticalAlignment = VerticalAlignment.Center;
                        tmp_text_box.HorizontalAlignment = HorizontalAlignment.Center;
                        tmp_text_box.Background = new SolidColorBrush(Colors.White);
                        border.Child = tmp_text_box;
                        tmp_text_box.Content = show_class_name;
                        tmp_text_box.Click += class_block_click;
                        break;
                    }
                }
            }
            for (int i = 1; i < 8; i++)
            {
                bool flag = false;
                for (int j = 0; j < student.second.Count; ++j)
                {
                    var border = new Border()
                    { BorderBrush = new SolidColorBrush(Colors.LightGray), BorderThickness = new Thickness(1) };
                    Grid.SetColumn(border, i);
                    Grid.SetRow(border, 3);
                    Grid.SetRowSpan(border, 2);
                    var tmp_text_box = new Button();
                    string show_class_name = "";
                    show_class_message.Children.Add(border);
                    while (j < student.second.Count && student.second[j].class_day == i - 1)
                    {
                        if (student.second[j].class_name.Length > 10)
                        {
                            string tmp_string = student.second[j].class_name.Substring(6);
                            student.second[j].class_name = student.second[j].class_name.Substring(0, 6) + "\n" + tmp_string;
                        }
                        if (flag)
                            show_class_name = show_class_name + "\n" +student.second[j].class_name ;
                        else
                            show_class_name = show_class_name+ student.second[j].class_name ;
                        flag = true;
                        j++;
                    }
                    if (flag)
                    {
                        tmp_text_box.VerticalAlignment = VerticalAlignment.Center;
                        tmp_text_box.HorizontalAlignment = HorizontalAlignment.Center;
                        tmp_text_box.Background = new SolidColorBrush(Colors.White);
                        border.Child = tmp_text_box;
                        tmp_text_box.Content = show_class_name;
                        tmp_text_box.Click += class_block_click;
                        break;
                    }
                }
            }
            for (int i = 1; i < 8; i++)
            {
                bool flag = false;
                for (int j = 0; j < student.third.Count; ++j)
                {
                    var border = new Border()
                    { BorderBrush = new SolidColorBrush(Colors.LightGray), BorderThickness = new Thickness(1) };
                    Grid.SetColumn(border, i);
                    Grid.SetRow(border, 5);
                    Grid.SetRowSpan(border, 2);
                    var tmp_text_box = new Button();
                    string show_class_name = "";
                    show_class_message.Children.Add(border);
                    while (j < student.third.Count && student.third[j].class_day == i - 1)
                    {
                        if(student.third[j].class_name.Length>10)
                        {
                            string tmp_string = student.third[j].class_name.Substring(6);
                            student.third[j].class_name = student.third[j].class_name.Substring(0, 6)+"\n"+tmp_string;
                        }
                        if(flag)
                        show_class_name =  show_class_name + "\n" +student.third[j].class_name ;
                        else
                        show_class_name =  show_class_name +  student.third[j].class_name ;
                        flag = true;
                        j++;
                    }
                    if (flag)
                    {
                        tmp_text_box.VerticalAlignment = VerticalAlignment.Center;
                        tmp_text_box.HorizontalAlignment = HorizontalAlignment.Center;
                        tmp_text_box.Background = new SolidColorBrush(Colors.White);
                        border.Child = tmp_text_box;
                        tmp_text_box.Content = show_class_name;
                        tmp_text_box.Click += class_block_click;
                        break;
                    }
                }
            }
            for (int i = 1; i < 8; i++)
            {
                bool flag = false;
                for (int j = 0; j < student.fourth.Count; ++j)
                {
                    var border = new Border()
                    { BorderBrush = new SolidColorBrush(Colors.LightGray), BorderThickness = new Thickness(1) };
                    Grid.SetColumn(border, i);
                    Grid.SetRow(border, 7);
                    Grid.SetRowSpan(border, 2);
                    var tmp_text_box = new Button();
                    string show_class_name = "";
                    show_class_message.Children.Add(border);
                    while (j < student.fourth.Count && student.fourth[j].class_day == i - 1)
                    {
                        if (student.fourth[j].class_name.Length > 10)
                        {
                            string tmp_string = student.fourth[j].class_name.Substring(6);
                            student.fourth[j].class_name = student.fourth[j].class_name.Substring(0, 6) + "\n" + tmp_string;
                        }
                        if (flag)
                            show_class_name = show_class_name + "\n" + student.fourth[j].class_name;
                        else
                            show_class_name = show_class_name + student.fourth[j].class_name;
                        flag = true;
                        j++;
                    }
                    if (flag)
                    {
                        tmp_text_box.VerticalAlignment = VerticalAlignment.Center;
                        tmp_text_box.HorizontalAlignment = HorizontalAlignment.Center;
                        tmp_text_box.Background = new SolidColorBrush(Colors.White);
                        border.Child = tmp_text_box;
                        tmp_text_box.Content = show_class_name;
                        tmp_text_box.Click += class_block_click;
                        break;
                    }
                }
            }
            for (int i = 1; i < 8; i++)
            {
                bool flag = false;
                for (int j = 0; j < student.fifth.Count; ++j)
                {
                    var border = new Border()
                    { BorderBrush = new SolidColorBrush(Colors.LightGray), BorderThickness = new Thickness(1) };
                    Grid.SetColumn(border, i);
                    Grid.SetRow(border, 9);
                    Grid.SetRowSpan(border, 2);
                    var tmp_text_box = new Button();
                    string show_class_name = "";
                    show_class_message.Children.Add(border);
                    while (j < student.fifth.Count && student.fifth[j].class_day == i - 1)
                    {
                        if (student.fifth[j].class_name.Length > 10)
                        {
                            string tmp_string = student.fifth[j].class_name.Substring(6);
                            student.fifth[j].class_name = student.fifth[j].class_name.Substring(0, 6) + "\n" + tmp_string;
                        }
                        if (flag)
                            show_class_name = show_class_name + "\n" + student.fifth[j].class_name;
                        else
                            show_class_name = show_class_name + student.fifth[j].class_name;
                        flag = true;
                        j++;
                    }
                    if (flag)
                    {
                        tmp_text_box.VerticalAlignment = VerticalAlignment.Center;
                        tmp_text_box.HorizontalAlignment = HorizontalAlignment.Center;
                        tmp_text_box.Background = new SolidColorBrush(Colors.White);
                        border.Child = tmp_text_box;
                        tmp_text_box.Content = show_class_name;
                        tmp_text_box.Click += class_block_click;
                        break;
                    }
                }
            }
            for (int i = 1; i < 8; i++)
            {
                bool flag = false;
                for (int j = 0; j < student.sixth.Count; ++j)
                {
                    var border = new Border()
                    { BorderBrush = new SolidColorBrush(Colors.LightGray), BorderThickness = new Thickness(1) };
                    Grid.SetColumn(border, i);
                    Grid.SetRow(border, 11);
                    Grid.SetRowSpan(border, 2);
                    var tmp_text_box = new Button();
                    string show_class_name = "";
                    show_class_message.Children.Add(border);
                    while (j < student.sixth.Count && student.sixth[j].class_day == i - 1)
                    {
                        if (student.sixth[j].class_name.Length > 10)
                        {
                            string tmp_string = student.sixth[j].class_name.Substring(6);
                            student.sixth[j].class_name = student.sixth[j].class_name.Substring(0, 6) + "\n" + tmp_string;
                        }
                        if (flag)
                            show_class_name = show_class_name + "\n" + student.sixth[j].class_name;
                        else
                            show_class_name = show_class_name + student.sixth[j].class_name;
                        flag = true;
                        j++;
                    }
                    if (flag)
                    {
                        tmp_text_box.Content = show_class_name;
                        tmp_text_box.Background = new SolidColorBrush(Colors.White);
                        tmp_text_box.VerticalAlignment = VerticalAlignment.Center;
                        tmp_text_box.HorizontalAlignment = HorizontalAlignment.Center;
                        tmp_text_box.Click += class_block_click;
                        border.Child = tmp_text_box;
                        break;
                    }
                }
            }
            /*Grid.SetColumn(border, 2);
            Grid.SetRow(border, 2);
            Grid.SetColumnSpan(border, 2);
            show_class_message.Children.Add(border);*/
        }


        private void class_block_click(object sender, RoutedEventArgs e)
        {
            Button tmp_button = sender as Button;
            Border tmp_border = tmp_button.Parent as Border;
            int tmp_row = Grid.GetRow(tmp_border);
            int tmp_column = Grid.GetColumn(tmp_border);
            TextBlock flyoutContent = new TextBlock();
            Flyout flyout = new Flyout();
            flyout.Placement = FlyoutPlacementMode.Top;
            flyoutContent.Text=student.find_class(tmp_row,tmp_column);
            flyout.Content = flyoutContent;
            FlyoutBase.SetAttachedFlyout(tmp_button, flyout);
            FlyoutBase.ShowAttachedFlyout(tmp_button);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string)
            {
                if (e.Parameter.ToString() == "Layin")
                {
                    student = html_serializer();
                    show_message();
                }
                else
                {
                    student = html_serializer();
                    show_message();
                    now_week = e.Parameter.ToString();
                    tmp_week_box.Content = "第" + now_week + "周";
                }
            }
            else 
            {
            }
            base.OnNavigatedTo(e);
        }
        private void show_week_class_click(object sender, RoutedEventArgs e)
        {
            TextBlock flyoutContent = new TextBlock();
            Flyout flyout = new Flyout();
            flyout.Placement = FlyoutPlacementMode.Full;
            flyoutContent.Text = student.find_week_class(int.Parse(now_week));
            flyout.Content = flyoutContent;
            FlyoutBase.SetAttachedFlyout(tmp_week_box, flyout);
            FlyoutBase.ShowAttachedFlyout(tmp_week_box);
        }
    }
}