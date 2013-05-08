using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace JZT.Utility
{
    /*----------------------------------------------------------------
    // Copyright (C) 2010 九州通集团有限公司 版权所有
    // 
    // 文件名：UIUtils.cs
    // 文件功能描述：界面工具类
    // 
    // 创建标识：魏刚20101026
    // 
    // 修改标识：
    // 修改描述：
    // 
    // 修改标识：
    // 修改描述：
    //----------------------------------------------------------------*/
    public static class UIUtils
    {
        #region AutoTabIndex
        //------------------------------------------------------------------------------------------------
        /// <summary>
        /// 动态自动调整界面中的TabIndex（对于控件较多的情况下，性能可能会存在问题）。
        /// </summary>
        /// <param name="parent">父容器类控件，也就是调整该容器控件中子控件的TabIndex。</param>
        /// <param name="startIndex">TabIndex开始序号。</param>
        /// <param name="step">TabIndex步长（一般为1即可）</param>
        public static void AutoTabIndex(Control parent, int startIndex, int step)
        {
            AutoTabIndex(parent, startIndex, step, null, false, null);
        }

        /// <summary>
        /// 动态自动调整界面中的TabIndex（对于控件较多的情况下，性能可能会存在问题）。
        /// </summary>
        /// <param name="parent">父容器类控件，也就是调整该容器控件中子控件的TabIndex。</param>
        /// <param name="startIndex">TabIndex开始序号。</param>
        public static void AutoTabIndex(Control parent, int startIndex)
        {
            AutoTabIndex(parent, startIndex, 1, null, false, null);
        }

        /// <summary>
        /// 动态自动调整界面中的TabIndex（对于控件较多的情况下，性能可能会存在问题）。
        /// </summary>
        /// <param name="parent">父容器类控件，也就是调整该容器控件中子控件的TabIndex。</param>
        public static void AutoTabIndex(Control parent)
        {
            AutoTabIndex(parent, 1, 1, null, false, null);
        }

        /// <summary>
        /// 动态自动调整界面中的TabIndex（对于控件较多的情况下，性能可能会存在问题）。
        /// </summary>
        /// <param name="parent">父容器类控件，也就是调整该容器控件中子控件的TabIndex。</param>
        /// <param name="startIndex">TabIndex开始序号。</param>
        /// <param name="step">TabIndex步长（一般为1即可）</param>
        /// <param name="excludeControlNames">需要排除的控件名数组</param>
        public static void AutoTabIndex(Control parent, int startIndex, int step, string[] excludeControlNames)
        {
            AutoTabIndex(parent, startIndex, step, excludeControlNames, false, null);
        }

        /// <summary>
        /// 动态自动调整界面中的TabIndex（对于控件较多的情况下，性能可能会存在问题）。
        /// </summary>
        /// <param name="parent">父容器类控件，也就是调整该容器控件中子控件的TabIndex。</param>
        /// <param name="startIndex">TabIndex开始序号。</param>
        /// <param name="step">TabIndex步长（一般为1即可）</param>
        /// <param name="excludeControlNames">需要排除的控件名数组</param>
        /// <param name="includeType">是否包含（或者排除）后面参数 params Type[] types 提供的类型，true表示包含，false表示排除</param>
        /// <param name="types">包含（或者排除）的类型，依赖includeType。</param>
        public static void AutoTabIndex(Control parent, 
                                        int startIndex, 
                                        int step,
                                        string[] excludeControlNames ,
                                        bool includeType, 
                                        params Type[] types)
        {
            if (parent == null)
                return;

            if (step < 1)
                step = 1;
            
            if (startIndex < 1)
                startIndex = 1;

            String excludeControlNameString = null;

            if (excludeControlNames == null || excludeControlNames.Length == 0)
                excludeControlNameString = "";
            else
                excludeControlNameString = String.Join(",",excludeControlNames) + ",".ToLower();

            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            SortedList<String, Control> sortedControls = new SortedList<string, Control>();

            GetSortedControls(ref sortedControls,parent,0,0);
            
            // 存放需要设置 TabIndex 的控件。
            List<Control> needsControls = new List<Control>();

            foreach (Control control in sortedControls.Values)
            {
                // 排除无需设置 TabIndex 控件
                if (IsExcludeType(control))
                    continue;

                String controlLowerName = control.Name.ToLower();
                if (!String.IsNullOrEmpty(excludeControlNameString))
                {
                    // 排除指定名的控件
                    if (excludeControlNameString.IndexOf(controlLowerName+",") >= 0)
                        continue;
                }

                if (includeType) // 包括的类型
                {
                    if (ControlBelongType(control, types))
                    {
                        needsControls.Add(control);
                    }
                }
                else // 排除的类型
                {
                    if (!ControlBelongType(control, types))
                    {
                        needsControls.Add(control);
                    }
                }
            }

            int tabIdx = startIndex;
            // 设置 TabIndex
            foreach (Control control in needsControls)
            {
                // System.Diagnostics.Debug.Print("{0}:{1}\r\n", control.Name, control.Text);
                control.TabIndex = tabIdx; 
                tabIdx += step;
            }

            watch.Stop();
            System.Diagnostics.Debug.Print("AutoTabIndex:{0}毫秒\r\n", watch.ElapsedMilliseconds);
        }

        /// <summary>
        /// 是否为排除的类型，排除不需要TabIndex的类型控件。
        /// </summary>
        /// <param name="control"></param>
        /// <returns>true表示排除</returns>
        private static bool IsExcludeType(Control control)
        {
            Type controlType = control.GetType();

            if (controlType == typeof(Label) || 
                controlType == typeof(GroupBox) || 
                controlType == typeof(PictureBox) ||
                controlType == typeof(Panel))
                return true;

            return false;
        }

        /// <summary>
        /// 是否为容器控件
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private static bool IsContainer(Control control)
        {
            String typeName = control.GetType().Name;//.ToLower();
            String baseTypeName = control.GetType().BaseType.Name;

            switch (typeName)
            {
                case "GroupBox":
                case "PictureBox":
                case "Panel":
                    return true;

                default:
                    if (typeName.StartsWith("Edit") && typeName.EndsWith("Control"))
                        return true;

                    if (baseTypeName == "Form")
                        return true;

                    break;
            }

            return false;
        }
        /// <summary>
        /// 获取已排序的控件方便设置TabIndex，排序的规则为从左至右，从上至下。
        /// </summary>
        /// <param name="sortedControls">排序的控件放置到该列表。</param>
        /// <param name="parent">父容器控件。</param>
        /// <param name="top">父容器控件相对窗体的TOP值</param>
        /// <param name="left">父容器控件相对窗体的LEFT值</param>
        private static void GetSortedControls(
            ref SortedList<String, Control> sortedControls, 
            Control parent, 
            int top, 
            int left)
        { 
           
            foreach (Control control in parent.Controls)
            {
                if (control.Visible == false)
                    continue;

                if (IsContainer(control))
                {
                    GetSortedControls(ref sortedControls, control, control.Top + top, control.Left + left);
                }                
                else
                {
                    // 排序原理的关键位置。
                    // 根据控件的位置创建排序的Key。
                    // 可能存在的问题：就是控件没有横向对齐造成的TabIndex不正确，处理办法就是对齐。
                    String controlkey = String.Format("{0:00000}{1:00000}", control.Top + top, control.Left + left);

                    if (sortedControls.ContainsKey(controlkey) || IsExcludeType(control))
                        continue;

                    sortedControls.Add(controlkey, control);
                }                
            }
        }

        /// <summary>
        /// 控件是否属于所属类型
        /// </summary>
        /// <param name="control"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        private static bool ControlBelongType(Control control, params Type[] types)
        {
            if (types == null || types.Length == 0)
                return false;

            foreach (Type type in types)
            {
                if (control.GetType() == type)
                    return true;
            }

            return false;
        }
        //================================================================================================
        #endregion AutoTabIndex



        /// <summary>
        /// 获取当前用户界面grid的布局目录
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string GetUserLocalPath(string userName,string branchid)
        {
            string userLocalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"LoaclLayout");

            if (string.IsNullOrEmpty(userName))
            {
                userName = string.Empty;
            }

            string companyPath = Path.Combine(userLocalPath, branchid);
            string sessionPath = Path.Combine(companyPath, companyPath);


            if(!Directory.Exists(userLocalPath))
            {
                Directory.CreateDirectory(userLocalPath);
            }
            if (!Directory.Exists(companyPath))
            {
                Directory.CreateDirectory(companyPath);
            }
            if (!Directory.Exists(sessionPath))
            {
                Directory.CreateDirectory(sessionPath);
            }
            return sessionPath;
        }
        public static string GetUserMessagePath(string userName, string branchid)
        {
            string userPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Message");
            string companyPath = Path.Combine(userPath, branchid);
            string sessionPath = Path.Combine(companyPath, userName);
            if (!Directory.Exists(userPath))
            {
                Directory.CreateDirectory(userPath);
            }
            if (!Directory.Exists(companyPath))
            {
                Directory.CreateDirectory(companyPath);
            }
            if (!Directory.Exists(sessionPath))
            {
                Directory.CreateDirectory(sessionPath);
            }
            return sessionPath;
        }
    }
}
