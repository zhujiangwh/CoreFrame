using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Core.Architecure;
using Core.Common;
using Core.Metadata;

namespace Core.UI
{
    public partial class TestObjectForm : Form
    {
        public ICommonObjectService CommonObjectService { get; set; }

        public TestObjectForm()
        {
            InitializeComponent();

            RemotingServer remotingServer = new RemotingServer("tcp", "127.0.0.1:8545");
            CommonObjectService = remotingServer.CreateRemotingInterface<ICommonObjectService>("CommonObjectService");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            BusiEntity softComponent = new BusiEntity();

            softComponent.EntityCode = "Code5";
            softComponent.EntityName = "Name5";
            softComponent.EntityCatalog = "Catalog";
            softComponent.EntityScripe = "scripe";
            softComponent.DeleteFlag = true;
            softComponent.ModuleGuidString = "ModuleGuidString";

            BusiDataItem item = new BusiDataItem();
            item.BusiEntity = softComponent;

            item.Caption = "caption";

            softComponent.DataItemList.Add(item);






            CommonObjectService.Create(softComponent);

        }
    }
}
