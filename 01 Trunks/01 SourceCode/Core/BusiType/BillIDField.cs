using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.BusiType
{
    public class BillIDField : Core.Metadata.Domain
    {
        public BillIDField()
        {
            BusiType = Core.Metadata.BusinessType.String;  //本类型为 字符串型 。

            ClassPropertyDefine = new Core.Metadata.CSharpDesign.ClassPropertyDefine(this); //传入属性名。

        }
    }
}
