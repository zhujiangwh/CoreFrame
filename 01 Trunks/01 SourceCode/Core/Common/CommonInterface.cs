using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Common
{
    public interface IUpdateTime
    {
        DateTime CreateTime { get; set; }
        DateTime LastModifyTime { get; set; }
    }

    public interface IGuidStringKey
    {
        string GuidString { get; set; }

        bool DeleteFlag { get; set; }
    }

    public interface IDelteteFlag
    {
        bool DeleteFlag { get; set; }
    }


    public interface IVersion
    {
        int Version { get; set; }
    }



}
