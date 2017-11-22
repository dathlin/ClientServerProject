using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using HslCommunication.Core;

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
    

    /// <summary>
    /// 角色分配的管理器
    /// </summary>
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

        /// <summary>
        /// 检测一个账户名是否有当前角色的权限
        /// </summary>
        /// <param name="roleCode">角色代号</param>
        /// <param name="name">用户名</param>
        /// <returns></returns>
        public bool IsAllowAccountOperate(string roleCode, string name)
        {
            bool result = false;
            hybirdLock.Enter();

            for (int i = 0; i < m_roles.Count; i++)
            {
                if (m_roles[i].RoleCode == roleCode)
                {
                    if (m_roles[i].Accounts != null)
                    {
                        if (m_roles[i].Accounts.Contains(name))
                        {
                            result = true;
                        }
                    }
                    break;
                }
            }

            hybirdLock.Leave();
            return result;
        }

        /// <summary>
        /// 获取一个用户名的所有角色名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string[] GetRolesByUserName(string name)
        {
            List<string> list = new List<string>();

            hybirdLock.Enter();

            for (int i = 0; i < m_roles.Count; i++)
            {
                if (m_roles[i].Accounts.Contains(name))
                {
                    list.Add(m_roles[i].RoleName);
                }
            }

            hybirdLock.Leave();

            return list.ToArray();
        }


        /// <summary>
        /// 获取一个角色的所有的用户名称
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        public string[] GetUsernamesByRolename(string roleCode)
        {
            string[] result = null;
            hybirdLock.Enter();

            for (int i = 0; i < m_roles.Count; i++)
            {
                if (m_roles[i].RoleCode == roleCode)
                {
                    if (m_roles[i].Accounts != null)
                    {
                        result = m_roles[i].Accounts.ToArray();
                    }
                    else
                    {
                        result = new string[0];
                    }
                    break;
                }
            }

            hybirdLock.Leave();

            if (result == null) result = new string[0]; // 角色不存在的情况
            return result;
        }


        #endregion
        
        #region Private Members

        private List<RoleItem> m_roles = new List<RoleItem>();
        private SimpleHybirdLock hybirdLock = new SimpleHybirdLock();

        #endregion
    }


    /// <summary>
    /// 单个角色对象
    /// </summary>
    public class RoleItem
    {
        #region Public Property


        /// <summary>
        /// 角色的唯一代码
        /// </summary>
        public string RoleCode { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 关联的账户列表
        /// </summary>
        public List<string> Accounts { get; set; } = new List<string>();


        #endregion
        
        #region Object Override

        /// <summary>
        /// 返回对象的字符串标识形式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return RoleName;
        }
        
        #endregion

    }
}
