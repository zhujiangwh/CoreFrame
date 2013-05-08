using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Core.Metadata
{
    public partial class EditEntityControl : UserControl
    {
        public EditEntityControl()
        {
            InitializeComponent();

            BindEnum(Column1);
        }

        public void BindEnum(DataGridViewComboBoxColumn combo)
        {
            //DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
            combo.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            combo.DataSource = Enum.GetValues(typeof(BusinessType));
            //string[] names = Enum.GetNames(typeof(BusinessType));
            //Dictionary<string, int> dictionary = new Dictionary<string, int>();

            //foreach (string name in names)
            //{
            //    BusinessType parse = (BusinessType)Enum.Parse(typeof(BusinessType), name);
            //    dictionary.Add(EnumDescription.GetEnumDescription(parse), (int)parse);
            //}
            //combo.DataSource = new BindingSource(dictionary, null);
            //combo.DisplayMember = "Key";
            //combo.ValueMember = "Value";
        }


        public void SetEditedObject(object editedObject)
        {
            BusiEntity entity = editedObject as BusiEntity;
            bsEntity.DataSource = entity;
            bsDataitem.DataSource = entity.DataItemList;
        }

        public object GetEditedObject()
        {
            bsEntity.EndEdit();
            bsDataitem.EndEdit();
            return bsEntity.DataSource; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bsDataitem.AddNew();
        }

        private void bsDataitem_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = new BusiDataItem();
        }
        
    }
}
