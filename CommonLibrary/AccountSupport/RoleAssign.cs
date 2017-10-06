using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HslCommunication;
using Newtonsoft.Json.Linq;

namespace CommonLibrary
{

    /**********************************************************************************
     * 
     *    说明：本系统的角色设计
     *    
     *    角色功能的设计就是针对一些特殊功能权限的账户设计的，我们希望实现对某一个功能的角色可以
     *    任意的配置账户
     * 
     **********************************************************************************/
    


    public class RoleAssign : HslCommunication.BasicFramework.SoftFileSaveBase
    {
        #region Constructor

        public RoleAssign()
        {
            // 添加一个初始化的测试例子
            m_roles.Add(new RoleItem()
            {
                RoleName = "审计员",
                Accounts = new List<string>()
                {
                    "admin"
                }
            });
        }

        #endregion

        #region Override Method

        public override string ToSaveString()
        {
            string json = string.Empty;
            hybirdLock.Enter();
            json = JArray.FromObject(m_roles).ToString();
            hybirdLock.Leave();
            return json;
        }

        public override void LoadByString(string content)
        {
            hybirdLock.Enter();
            m_roles = JArray.Parse(content).ToObject<List<RoleItem>>();
            hybirdLock.Leave();
        }

        #endregion
        
        #region Public Method

        public bool IsAllowAccountOperate(string role,string name)
        {
            bool result = false;
            hybirdLock.Enter();

            for (int i = 0; i < m_roles.Count; i++)
            {
                if (m_roles[i].RoleName == role)
                {
                    if(m_roles[i].Accounts.Contains(name))
                    {
                        result = true;
                    }
                }
            }

            hybirdLock.Leave();
            return result;
        }


        #endregion
        
        #region Private Members

        private List<RoleItem> m_roles = new List<RoleItem>();
        private SimpleHybirdLock hybirdLock = new SimpleHybirdLock();

        #endregion
    }


    public class RoleItem
    {
        public string RoleName { get; set; }
        public List<string> Accounts { get; set; } = new List<string>();

        public override string ToString()
        {
            return RoleName;
        }
    }
}
