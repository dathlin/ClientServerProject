using CommonLibrary;
using HslCommunication;
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
        /// 缓存的在线客户端信息
        /// </summary>
        public string ClientsOnlineCache { get => m_ClientsOnlineCache; }


        #region Private Member

        private List<NetAccount> OnlineClients = new List<NetAccount>();       // 所有在线客户端的列表
        private SimpleHybirdLock hybirdLock = new SimpleHybirdLock();          // 操作列表的混合锁
        private string m_ClientsOnlineCache = "[]";                              // 在线客户端的缓存

        #endregion
    }
}
