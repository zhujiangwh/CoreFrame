using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.UI
{
    public class BaseControlAdapter
    {
        //public BaseControlAdapter()
        //{
        //    InitializeComponent();
        //}

        //public BaseControlAdapter(IContainer container)
        //{
        //    container.Add(this);

        //    InitializeComponent();
        //}

        public UiDisplayManager UiDisplayManager { get; set; }

        public virtual void SetIsMulSelect(bool isMulSelect)
        {
        }

        public virtual void Display()
        {
        }

        public virtual void MoveFrior()
        { }

        public virtual void MoveNext()
        { }


        public virtual void Clear()
        { }


        public virtual void NewUiDisplayObject(UIDisplayObject uiDisplayObject)
        { }

        public virtual void InsertUiDisplayObject(UIDisplayObject uiDisplayObject, UIDisplayObject curUIDisplayObject)
        { }

        public virtual void DelUiDisplayObject(UIDisplayObject uiDisplayObject)
        { }

        public virtual void UpdateUiDisplayObject(UIDisplayObject uiDisplayObject)
        { }


        public virtual void FocusNode(UIDisplayObject uiDisplayObject)
        {
        }

        public virtual void SetChecked(UIDisplayObject uiDisplayObject, bool checkedFlag)
        {
        }

    }
}
