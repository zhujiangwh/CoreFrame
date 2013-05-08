using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Architecure;
using Core.Common;
using Core.Serialize.DB;
using System.Data;
using System.Collections;

namespace Core.UI
{

    /// <summary>
    /// 界面实现接口.
    /// </summary>
    public interface IObjectDisplay
    {
        //显示对象.
        void Dispaly(object obj);

        CommonUIinteractive CommonUIinteractive { get; set; }


    }

    /// <summary>
    /// 用于对象与用户在界面上界面操作管理的类.
    /// </summary>
    [Serializable]
    public class CommonUIinteractive : UIinteractive
    {

        public CommonUIinteractive()
        {
            EditedObjectDefine = new ObjectDefine();
            EditFormDefine = new ObjectDefine();
            EditModuleFormDefine = new ObjectDefine();
        }


        public virtual  void Initial()
        {
            EditForm = EditFormDefine.CreateObject() as IObjectDisplay;
            EditForm.CommonUIinteractive = this;

            EditModuleForm = EditModuleFormDefine.CreateObject() as IObjectDisplay;
            EditModuleForm.CommonUIinteractive = this;
        }


        public ICommonObjectService CommonObjectService { get; set; }

        /// <summary>
        /// 被编辑的对象.
        /// </summary>
        public ObjectDefine EditedObjectDefine { get; set; }


        public ObjectDefine DisplayFormDefine { get; set; } 

        public IObjectDisplay DisplayForm {get;set;}


        public ObjectDefine EditFormDefine { get; set; } 
        public IObjectDisplay EditForm { get; set; }

        public ObjectDefine EditModuleFormDefine { get; set; }
        public IObjectDisplay EditModuleForm { get; set; }


        public object EditedObject { get; set; }

        public SoftModule NewSoftModule(SoftSystem softSystem)
        {
            SoftModule softModule = softSystem.CreateSubModule();

            //显示窗口编辑软件模块 。
            EditModuleForm.Dispaly(softModule);




            //保存

            CommonObjectService.Create(softModule);

            //向用户反馈保存信息。



            //更新界面







            return softModule;
        }



        public SoftModule NewSoftModule(SoftModule parentSoftModule)
        {
            SoftModule softModule = parentSoftModule.CreateSubModule();

            //显示窗口编辑软件模块 。
            EditModuleForm.Dispaly(softModule);

            //保存
            CommonObjectService.Create(softModule);

            //向用户反馈保存信息。


            //更新界面


            return softModule;
        }

        /// <summary>
        /// 创建一个新对象.
        /// </summary>
        public void NewObject()
        { 
            //创建新对象.
            EditedObject = EditedObjectDefine.CreateObject();

            //在弹出窗口中，编辑新系统。
            //如果保存窗口，则显示 ，否则。
            EditForm.Dispaly(EditedObject);

            //显示新对象.
            if (EditedObject != null)
            {
                DisplayForm.Dispaly(EditedObject);
            }
            
        }

        //保存对象。
        public void Save()
        {
            try
            {
                EditedObject = CommonObjectService.Create(EditedObject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { 
            }
        }

        public void GetObject(string guid)
        {
            //创建新对象.
            RemotingServer remotingServer = new RemotingServer("tcp", "127.0.0.1:8545");
            IArchitectureService service = remotingServer.CreateRemotingInterface<IArchitectureService>("ArchitectureService");
            EditedObject = service.GetSoftSystem(guid);


            DisplayForm.Dispaly(EditedObject);

 
        }
    }
}
