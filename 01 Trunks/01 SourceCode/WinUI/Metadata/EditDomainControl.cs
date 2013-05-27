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
    public partial class EditDomainControl : UserControl, IObjectEditer
    {
        public EditDomainControl()
        {
            InitializeComponent();
        }

        #region IObjectEditer 成员

        public object EditedObject
        {
            get
            {
                Domain domain = bsDomain.DataSource as Domain;

                domain.BusiType = (BusinessType)comboBox1.SelectedIndex ;

                return bsDomain.DataSource;
            }
            set
            {
                Domain domain = value as Domain;

                if (domain != null)
                {
                    
                    bsDomain.DataSource = domain;

                    comboBox1.SelectedIndex = (int)domain.BusiType;

                    editCSharpPropertyControl1.EditedObject = domain.ClassPropertyDefine;
                }
            }
        }

        #endregion

        private void EditDomainControl_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = System.Enum.GetNames(typeof(BusinessType));
        }
    }
}
