using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Metadata
{
    [Serializable]
    public class Domain : BaseDataItem
    {
        public Domain()
        {
            ClassPropertyDefine = new Core.Metadata.CSharpDesign.ClassPropertyDefine() ;
        }

        public Core.Metadata.CSharpDesign.ClassPropertyDefine ClassPropertyDefine { get; set; }
    }
}
