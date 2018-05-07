using CommonLibrary;
using HslCommunication.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 软件系统服务端模版.BasicSupport
{

    /******************************************************************************************
     * 
     *    时间：2017年10月16日 21:17:00
     *    功能：一个服务器端的在线客户端管理器
     * 
     * ****************************************************************************************/


    /// <summary>
    /// 所有在线客户端的管理器，该在线模型是基于NetAccount类扩展的
    /// </summary>
    public class NetAccountManager
    {

        #region Constructor


        /// <summary>
        /// 实例化一个默认的对象
        /// </summary>
        public NetAccountManager()
        {
            OnlineClients = new List<NetAccount>( );
            hybirdLock = new SimpleHybirdLock( );
            m_ClientsOnlineCache = "[]";
        }

        #endregion

        #region Public Method

        /// <summary>
        /// 获取所有在线客户端的信息
        /// </summary>
        /// <returns></returns>
        public string[] GetOnlineInformation()
        {
            string[] result = null;

            hybirdLock.Enter( );
            result = new string[OnlineClients.Count];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = OnlineClients[i].ToString( );
            }

            hybirdLock.Leave( );

            return result;
        }


        /// <summary>
        /// 新增一个在线的客户端
        /// </summary>
        /// <param name="account"></param>
        public void AddOnlineClient(NetAccount account)
        {
            hybirdLock.Enter();
            OnlineClients.Add(account);
            m_ClientsOnlineCache = JArray.FromObject(OnlineClients).ToString();
            hybirdLock.Leave();

        }

        /// <summary>
        /// 根据在线客户端的唯一ID进行移除指定的客户端
        /// </summary>
        /// <param name="uniqueId"></param>
        public void RemoveOnlineClient(string uniqueId)
        {
            hybirdLock.Enter();

            int index = -1;
            for (int i = 0; i < OnlineClients.Count; i++)
            {
                if (OnlineClients[i].UniqueId == uniqueId)
                {
                    index = i;
                    break;
                }
            }

            if (index >= 0)
            {
                OnlineClients.RemoveAt(index);
            }

            m_ClientsOnlineCache = JArray.FromObject(OnlineClients).ToString();

            hybirdLock.Leave();
        }

        /// <summary>
        /// 判断指定名字的客户端是否在线
        /// </summary>
        /// <param name="userName">用户名字</param>
        /// <returns>在线就返回<c>True</c>，否则返回<c>Flase</c></returns>
        public bool IsClientOnline(string userName)
        {
            bool result = false;
            hybirdLock.Enter( );

            for (int i = 0; i < OnlineClients.Count; i++)
            {
                if (OnlineClients[i].UserName == userName)
                {
                    result = true;
                    break;
                }
            }

            hybirdLock.Leave( );
            return result;
        }


        #endregion

        #region Public Properties


        /// <summary>
        /// 缓存的在线客户端信息
        /// </summary>
        public string ClientsOnlineCache { get => m_ClientsOnlineCache; }

        #endregion

        #region Private Member

        private List<NetAccount> OnlineClients;                         // 所有在线客户端的列表
        private SimpleHybirdLock hybirdLock;                            // 操作列表的混合锁
        private string m_ClientsOnlineCache;                            // 在线客户端的缓存

        #endregion
    }
}
