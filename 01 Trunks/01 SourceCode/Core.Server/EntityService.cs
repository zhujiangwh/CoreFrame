using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Server;
using Core.Metadata;
using Core.Serialize.XML;

namespace Core
{
    public class EntityService : RemotingService, IEntityService
    {
        private XmlSerializeService xmlSerializeService;

        private string fullClassName = @"Core.Metadata.BusiEntity";

        public EntityService()
        {
            xmlSerializeService = new  XmlSerializeService();
        }

        #region IEntityService 成员

        public bool SaveBusiEntity(BusiEntity busiEntity)
        {
            xmlSerializeService.SaveToFile(busiEntity);

            return true ;
        }

        public BusiEntity LoadBusiEntity(string name)
        {
            string filename = xmlSerializeService.GetFileName(fullClassName, name);
            return xmlSerializeService.LoadFormFile(fullClassName, filename) as BusiEntity;
        }

        #endregion
    }
}
