using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DB.NHibernate;
using Core.Common;

namespace Core.Server
{


    public class BusiObject : ICommonBusiObject
    {
        /// <summary>
        /// 业务对象中的 NH 服务类 不由自己创建 ，由对外的接口服务类创建。
        /// </summary>
        public NHiberanteService NHService { get; set; }

        public void SetNHiberanteService(NHiberanteService nhService)
        {
            NHService = nhService;
        }


        #region ICommonBusiObject 成员

        public virtual void OnBeforeSave(object obj)
        {
        }

        public virtual void OnAfterSave(object obj)
        {
        }

        public virtual void OnBeforeUpdate(object obj)
        {
        }

        public virtual  void OnAfterUpdate(object obj)
        {
        }

        public virtual void OnBeforeDelete(object obj)
        {
        }

        public virtual void OnAfterDelete(object obj)
        {
        }

        #endregion
    }
}
