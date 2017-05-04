using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Schedule
{
    public class class_message
    {
        public bool flag;
        public string class_name="";
        public int class_length=0;
        public string class_teacher="";
        public int class_start_time=0;
        public int class_end_time = 0;
        public string class_address="";
        public int class_day = -1;
    }
    public class people
    {
        public string name;
        public string school;
        public string marjor;
        public string institute;
        public string school_number;
        public string learn_year;
        public string class_number;
        public List<class_message> first = new List<class_message>();
        public List<class_message> second = new List<class_message>();
        public List<class_message> third = new List<class_message>();
        public List<class_message> fourth = new List<class_message>();
        public List<class_message> fifth = new List<class_message>();
        public List<class_message> sixth = new List<class_message>();
        public string find_class(int row,int column)
        {
            string tmp_string = "";
            if(row == 1|| row == 2)
            {
                bool flag = false;
                for(int i=0;i<first.Count;++i)
                {
                    if(first[i].class_day==column-1)
                    {
                        if (flag) tmp_string += "\n";
                        else flag = true;
                         tmp_string += "名字:"+first[i].class_name+"\n";
                        tmp_string += "地点:"+first[i].class_address + "\n";
                        tmp_string += "开课时间:"+first[i].class_start_time +"周-" + first[i].class_end_time + "周\n";
                    }
                }
            }
            else if (row == 3 || row == 4)
            {
                bool flag = false;
                for (int i = 0; i < second.Count; ++i)
                {
                    if (second[i].class_day == column-1)
                    {
                        if (flag) tmp_string += "\n";
                        else flag = true;
                        tmp_string += "名字:" + second[i].class_name + "\n";
                        tmp_string += "地点:" + second[i].class_address + "\n";
                        tmp_string += "开课时间:" + second[i].class_start_time + "周-" + second[i].class_end_time + "周\n";
                    }
                }
            }
            else if (row == 5 || row == 6)
            {
                bool flag = false;
                for (int i = 0; i < third.Count; ++i)
                {
                    if (third[i].class_day == column-1)
                    {
                        if (flag) tmp_string += "\n";
                        else flag = true;
                        tmp_string += "名字:" + third[i].class_name + "\n";
                        tmp_string += "地点:" + third[i].class_address + "\n";
                        tmp_string += "开课时间:" + third[i].class_start_time + "周-" + third[i].class_end_time + "周\n";
                    }
                }
            }
            else if (row == 7 || row == 8)
            {
                bool flag = false;
                for (int i = 0; i < fourth.Count; ++i)
                {
                    if (fourth[i].class_day == column-1)
                    {
                        if (flag) tmp_string += "\n";
                        else flag = true;
                        tmp_string += "名字:" + fourth[i].class_name + "\n";
                        tmp_string += "地点:" + fourth[i].class_address + "\n";
                        tmp_string += "开课时间:" + fourth[i].class_start_time + "周-" + fourth[i].class_end_time + "周\n";
                    }
                }
            }
            else if (row == 9 || row == 10)
            {
                bool flag = false;
                for (int i = 0; i < fifth.Count; ++i)
                {
                    if (fifth[i].class_day == column-1)
                    {
                        if (flag) tmp_string += "\n";
                        else flag = true;
                        tmp_string += "名字:" + fifth[i].class_name + "\n";
                        tmp_string += "地点:" + fifth[i].class_address + "\n";
                        tmp_string += "开课时间:" + fifth[i].class_start_time + "周-" + fifth[i].class_end_time + "周\n";
                    }
                }
            }
            else if (row == 11 || row == 12)
            {
                bool flag = false;
                for (int i = 0; i < sixth.Count; ++i)
                {
                    if (sixth[i].class_day == column-1)
                    {
                        if (flag) tmp_string += "\n";
                        else flag = true;
                        tmp_string += "名字:" + sixth[i].class_name + "\n";
                        tmp_string += "地点:" + sixth[i].class_address + "\n";
                        tmp_string += "开课时间:" + sixth[i].class_start_time + "周-" +sixth[i].class_end_time + "周\n";
                    }
                }
            }
            return tmp_string;
        }
        public string find_week_class(int now_week)
        {
            string tmp = "";
            bool flag = false;
            for(int i=0;i<first.Count;++i)
            {
                if(first[i].class_start_time<=now_week&& first[i].class_end_time >= now_week)
                {
                    if (!flag) tmp += "1/2节:\n";
                    else tmp += "\n";
                    tmp += first[i].class_name + "\n"+first[i].class_address +"\n"+ get_day(first[i].class_day);
                    flag = true;
                }
            }
            flag = false;
            for (int i = 0; i < second.Count; ++i)
            {
                if (second[i].class_start_time <= now_week && second[i].class_end_time >= now_week)
                {
                    if (!flag) tmp += "1/2节:\n";
                    else tmp += "\n";
                    tmp += second[i].class_name + "\n" + second[i].class_address + "\n" + get_day(second[i].class_day);
                    flag = true;
                }
            }
            flag = false;
            for (int i = 0; i < third.Count; ++i)
            {
                if (third[i].class_start_time <= now_week && third[i].class_end_time >= now_week)
                {
                    if (!flag) tmp += "1/2节:\n";
                    else tmp += "\n";
                    tmp +=third[i].class_name + "\n" + third[i].class_address + "\n" + get_day(third[i].class_day);
                    flag = true;
                }
            }
            flag = false;
            for (int i = 0; i < fourth.Count; ++i)
            {
                if (fourth[i].class_start_time <= now_week && fourth[i].class_end_time >= now_week)
                {
                    if (!flag) tmp += "1/2节:\n";
                    else tmp += "\n";
                    tmp += fourth[i].class_name + "\n" + fourth[i].class_address + "\n" + get_day(fourth[i].class_day);
                    flag = true;
                }
            }
            flag = false;
            for (int i = 0; i < fifth.Count; ++i)
            {
                if (fifth[i].class_start_time <= now_week && fifth[i].class_end_time >= now_week)
                {
                    if (!flag) tmp += "1/2节:\n";
                    else tmp += "\n";
                    tmp += fifth[i].class_name + "\n" + fifth[i].class_address + "\n" + get_day(fifth[i].class_day);
                    flag = true;
                }
            }
            flag = false;
            for (int i = 0; i < sixth.Count; ++i)
            {
                if (sixth[i].class_start_time <= now_week && sixth[i].class_end_time >= now_week)
                {
                    if (!flag) tmp += "1/2节:\n";
                    else tmp += "\n";
                    tmp += sixth[i].class_name + "\n" + sixth[i].class_address + "\n" + get_day(sixth[i].class_day);
                    flag = true;
                }
            }
            return tmp;
        }
        public string get_day(int i)
        {
            if (i == 0)
            {
                return "周一";
            }
            else if (i == 1)
            {
                return "周二";
            }
            else if (i == 2)
            {
                return "周三";
            }
            else if (i == 3)
            {
                return "周四";
            }
            else if (i == 4)
            {
                return "周五";
            }
            else if (i == 5)
            {
                return "周六";
            }
            else return "周日";
        }
    }
}
