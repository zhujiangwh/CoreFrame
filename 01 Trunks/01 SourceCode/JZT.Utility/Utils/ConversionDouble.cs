using System;
using System.Collections.Generic;
using System.Text;

namespace JZT.Utility
{

 /*----------------------------------------------------------------
 // Copyright (C) 2011 ����ͨ�������޹�˾
 // ��Ȩ���С� 
 //
 // �ļ�����MyComments.cs
 // �ļ������������ҵ�ע��С���֡�
 //     
 // 
 // ������ʶ���콭 20110128

 //----------------------------------------------------------------*/

    public static class ConversionDouble
    {
        /// <summary>
        /// ������ת��Ϊ��д
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
                //�Ȱ����ֺ͵�λת������
                sb.Append(To_Upper(zheng[i]).ToString() + To_Mon(zheng.Length + 1 - i).ToString());
            }
            StringBuilder sb1 = new StringBuilder();
            for (int i = 0; i < xiaos.Length; i++)
            {
                //�Ȱ����ֺ͵�λת������
                sb.Append(To_Upper(xiaos[i]).ToString() + To_Mon(xiaos.Length - 1 - i).ToString());
            }
            //ȥ�����ڵĵ�λ
            string dx = replace(sb.ToString());
            return dx.Equals("") ? "��" : dx;
        }

        /// <summary>
        /// ������ת��Ϊ��д�ַ�
        /// </summary>
        /// <param name="one"></param>
        /// <returns></returns>
        public static char To_Upper(char one)
        {
            switch (one)
            {
                case '0': return '��';
                case '1': return 'Ҽ';
                case '2': return '��';
                case '3': return '��';
                case '4': return '��';
                case '5': return '��';
                case '6': return '½';
                case '7': return '��';
                case '8': return '��';
                case '9': return '��';
                default: return one;
            }
        }

        /// <summary>
        /// ������ӵ�λ
        /// </summary>
        /// <param name="one"></param>
        /// <returns></returns>
        private static string To_Mon(int one)
        {
            switch (one)
            {
                case 0: return "��";
                case 1: return "��";
                case 2: return "Ԫ";
                case 3: return "ʰ";
                case 4: return "��";
                case 5: return "Ǫ";
                case 6: return "��";
                case 7: return "ʰ";
                case 8: return "��";
                case 9: return "Ǫ";
                case 10: return "��";
                case 11: return "ʮ��";
                case 12: return "����";
                default: return " ";

            }
        }

        /// <summary>
        /// ת����λ
        /// </summary>
        /// <param name="one"></param>
        /// <returns></returns>
        private static string replace(string one)
        {
            one = one.Replace("���", "");
            one = one.Replace("���", "��");
            int left = 0;
            while (true)
            {
                left = one.Length;
                one = one.Replace("����", "��");
                one = one.Replace("��Ԫ", "Ԫ");
                one = one.Replace("��ʰ", "��");
                one = one.Replace("���", "��");
                one = one.Replace("��Ǫ", "��");
                one = one.Replace("����", "����");
                one = one.Replace("����", "��");
                one = one.Replace("����", "����");
                one = one.Replace("��Ǫ", "Ǫ��");
                one = one.Replace("Ǫ��", "Ǫ��");
                one = one.Replace("Ǫ��", "Ǫ");
                one = one.Replace("����", "��");
                one = one.Replace("ʮ��", "ʮ");
                //�����һ�ַ����ĳ���û�б仯.˵�����е��������Ѿ��ﵽ��..������ѭ��
                if (one.Length == left)
                {
                    break;
                }
            }
            //ȥ��������
            if (one.Length > 1)
            {
                if (one[one.Length - 1].Equals('��'))
                {
                    one = one.Substring(0, one.Length - 1);
                }
                //���û��С��λ��������һ��'��'
                if (!one[one.Length - 1].Equals('��') && !one[one.Length - 1].Equals('��') && !one[one.Length - 1].Equals('��'))
                {
                    one = one + "��";
                }
            }
            return one;
        }
    }
}

