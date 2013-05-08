using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Core.UI
{
    public interface ISelectObjectService
    {
        bool SaveSelectObjectUIC(SelectObjectUIC selectObjectUIC);

        SelectObjectUIC LoadSelectObjectUIC(  string name);

        DataTable GetDataTable( string name);
    }
}
