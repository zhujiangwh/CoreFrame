using System;
using System.Collections.Generic;
using System.Text;

namespace JZT.Utility
{

 /*----------------------------------------------------------------
 // Copyright (C) 2011 九州通集团有限公司
 // 版权所有。 
 //
 // 文件名：MyComments.cs
 // 文件功能描述：我的注释小帮手。
 //     
 // 
 // 创建标识：朱江 20110128

 //----------------------------------------------------------------*/

    public static class ConversionDouble
    {
        /// <summary>
        /// 把数字转换为大写
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static string CovertDouble(string amount)
        {
            string[] _price = amount.Split('.');
            string zheng = _price[0];
            string xiaos = _price[1];

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < zheng.Length; i++)
            {
                //先把数字和单位转换过来
                sb.Append(To_Upper(zheng[i]).ToString() + To_Mon(zheng.Length + 1 - i).ToString());
            }
            StringBuilder sb1 = new StringBuilder();
            for (int i = 0; i < xiaos.Length; i++)
            {
                //先把数字和单位转换过来
                sb.Append(To_Upper(xiaos[i]).ToString() + To_Mon(xiaos.Length - 1 - i).ToString());
            }
            //去掉多于的单位
            string dx = replace(sb.ToString());
            return dx.Equals("") ? "零" : dx;
        }

        /// <summary>
        /// 把数字转换为大写字符
        /// </summary>
        /// <param name="one"></param>
        /// <returns></returns>
        public static char To_Upper(char one)
        {
            switch (one)
            {
                case '0': return '零';
                case '1': return '壹';
                case '2': return '贰';
                case '3': return '叁';
                case '4': return '肆';
                case '5': return '伍';
                case '6': return '陆';
                case '7': return '柒';
                case '8': return '捌';
                case '9': return '玖';
                default: return one;
            }
        }

        /// <summary>
        /// 给后面加单位
        /// </summary>
        /// <param name="one"></param>
        /// <returns></returns>
        private static string To_Mon(int one)
        {
            switch (one)
            {
                case 0: return "分";
                case 1: return "角";
                case 2: return "元";
                case 3: return "拾";
                case 4: return "佰";
                case 5: return "仟";
                case 6: return "万";
                case 7: return "拾";
                case 8: return "佰";
                case 9: return "仟";
                case 10: return "亿";
                case 11: return "十亿";
                case 12: return "佰亿";
                default: return " ";

            }
        }

        /// <summary>
        /// 转换单位
        /// </summary>
        /// <param name="one"></param>
        /// <returns></returns>
        private static string replace(string one)
        {
            one = one.Replace("零分", "");
            one = one.Replace("零角", "零");
            int left = 0;
            while (true)
            {
                left = one.Length;
                one = one.Replace("零零", "零");
                one = one.Replace("零元", "元");
                one = one.Replace("零拾", "零");
                one = one.Replace("零佰", "零");
                one = one.Replace("零仟", "零");
                one = one.Replace("零万", "万零");
                one = one.Replace("零亿", "亿");
                one = one.Replace("亿万", "亿零");
                one = one.Replace("万仟", "仟零");
                one = one.Replace("仟佰", "仟零");
                one = one.Replace("仟亿", "仟");
                one = one.Replace("佰亿", "佰");
                one = one.Replace("十亿", "十");
                //如果这一字符串的长度没有变化.说明所有的条件都已经达到了..就跳出循环
                if (one.Length == left)
                {
                    break;
                }
            }
            //去掉最后的零
            if (one.Length > 1)
            {
                if (one[one.Length - 1].Equals('零'))
                {
                    one = one.Substring(0, one.Length - 1);
                }
                //如果没有小数位就在最后加一个'整'
                if (!one[one.Length - 1].Equals('角') && !one[one.Length - 1].Equals('分') && !one[one.Length - 1].Equals('零'))
                {
                    one = one + "整";
                }
            }
            return one;
        }
    }
}

