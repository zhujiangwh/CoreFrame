using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Metadata
{
    public interface IEntityService
    {
        bool SaveBusiEntity(BusiEntity busiEntity);

        BusiEntity LoadBusiEntity(string name);

    }
}
