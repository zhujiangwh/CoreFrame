using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace JZT.Utility.Message.MessageFomatter
{
    internal class GenericTextFormatter : IFormatter
    {


        private string m_textTemplater;

        #region IFormatter 成员

        /// <summary>
        /// formatter 字符串，如text 不为null 就以hash表中的参数为foramtter 对象
        /// </summary>
        /// <param name="MsEntry"></param>
        /// <returns></returns>
        public object Formatter(string title,Hashtable variant)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(title);
            sb.AppendLine(ParseData(this.FormatterTemplate, variant));
            return sb.ToString();
         
        }

        public string FormatterTemplate
        {
            get
            {
               return m_textTemplater;
            }
            set
            {
               m_textTemplater = value;
            }
        }
        private  string ParseData(string input, Hashtable hash)
        {
            if (hash == null || hash.Count == 0)
                return input;
            if (null != input && input.Length > 0)
            {

                Regex r = new Regex("\\u007B[^\\u007D]+\\u007D");
                Match m = r.Match(input);
                while (m.Success)
                {
                    string variantName = input.Substring(m.Index + 1, m.Length - 2);
                    try
                    {
                        if (hash.ContainsKey(variantName))
                        {
                            input = input.Substring(0, m.Index) + hash[variantName] + input.Substring(m.Index + m.Length);

                        }
                        else
                        {
                            input = "";
                        }

                    }
                    catch
                    {
                        throw new Exception(variantName + "变量" + "未找到");
                    }


                    m = r.Match(input);
                }
            }

            return input;
        }
        #endregion
    }
}
