using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Core.UI
{
    public partial class EditObjectControl : UserControl, IObjectEditer
    {
        public EditObjectControl()
        {
            InitializeComponent();
        }

        #region IObjectEditer 成员

        public object EditedObject
        {
            get
            {
                return propertyGrid.SelectedObject;
            }
            set
            {
                propertyGrid.SelectedObject = value;
             }
        }

        #endregion
    }
}
