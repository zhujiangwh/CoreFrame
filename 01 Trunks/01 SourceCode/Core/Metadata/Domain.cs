using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Metadata.CSharpDesign;

namespace Core.Metadata
{
    [Serializable]
    public class Domain : BaseDataItem
    {
        public Domain()
        {
            ClassPropertyDefine = new ClassPropertyDefine() ;
        }

        public ClassPropertyDefine ClassPropertyDefine { get; set; }
    }
}
