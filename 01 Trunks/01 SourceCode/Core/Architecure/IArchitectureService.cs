using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using Core.Authority;

namespace Core.Architecure
{
    public interface IArchitectureService
    {
        //===================软件系统===================================

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guidString"></param>
        /// <returns></returns>
        SoftSystem GetSoftSystem(string guidString);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        DataTable GetSoftSystem();

        //===================模块系统===================================


        //===================软件组件系统===================================

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleGuidString"></param>
        /// <returns></returns>
        IList GetSystemComponent(string moduleGuidString);

    }
}
