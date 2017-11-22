using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;

namespace CommonLibrary
{
    /// <summary>
    /// 服务器的账户管理类，如果你采用这种方式存储，请参照本项目事例
    /// </summary>
    /// <typeparam name="T">账户类，该类必须派生自UserAccount类</typeparam>
    public class ServerAccounts<T> : HslCommunication.BasicFramework.SoftFileSaveBase where T : UserAccount, new()
    {
        #region Constructor

        /// <summary>
        /// 初始化构造方法
        /// </summary>
        public ServerAccounts()
        {

        }
        /// <summary>
        /// 初始化构造方法，将添加几个初始化账户
        /// </summary>
        public ServerAccounts(IEnumerable<T> accounts)
        {
            all_list_accounts.AddRange(accounts);
        }

        #endregion

        #region Private Member

        private List<T> all_list_accounts = new List<T>();
        /// <summary>
        /// 一个简单的混合锁，相比Lock速度更快
        /// </summary>
        private HslCommunication.Core.SimpleHybirdLock hybirdLock = new HslCommunication.Core.SimpleHybirdLock();

        #endregion

        #region Public Method

        /// <summary>
        /// 更新指定账户的密码
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        public void UpdatePassword(string name, string password)
        {
            hybirdLock.Enter();
            for (int i = 0; i < all_list_accounts.Count; i++)
            {
                if (name == all_list_accounts[i].UserName)
                {
                    all_list_accounts[i].Password = password;
                    ILogNet?.WriteInfo(SoftResources.StringResouce.AccountModifyPassword + name);
                    break;
                }
            }
            hybirdLock.Leave();
        }


        /// <summary>
        /// 更新指定账户的大小尺寸的头像MD5码
        /// </summary>
        /// <param name="name"></param>
        /// <param name="largePortraitMD5">大尺寸头像的MD5</param>
        /// <param name="smallPortraitMD5">小尺寸头像的MD5</param>
        public void UpdatePortraitMD5(string name, string smallPortraitMD5, string largePortraitMD5)
        {
            hybirdLock.Enter();
            for (int i = 0; i < all_list_accounts.Count; i++)
            {
                if (name == all_list_accounts[i].UserName)
                {
                    all_list_accounts[i].SmallPortraitMD5 = smallPortraitMD5;
                    all_list_accounts[i].LargePortraitMD5 = largePortraitMD5;
                    ILogNet?.WriteInfo(SoftResources.StringResouce.AccountUploadPortrait + name);
                    break;
                }
            }
            hybirdLock.Leave();
        }



        /// <summary>
        /// 筛选特定的账户信息
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public List<T> WhereAccounts(Func<T, bool> selector)
        {
            return all_list_accounts.Where(selector).ToList();
        }

        
        

        /// <summary>
        /// 检查账户信息，并返回账户对象
        /// </summary>
        /// <param name="name">需要验证的用户名</param>
        /// <param name="code">需要验证的密码</param>
        /// <param name="ipAddress">登录的ip地址</param>
        /// <returns>验证的结果对象</returns>
        public T CheckAccount(string name, string code, string ipAddress, string way)
        {
            T result = new T()
            {
                UserName = name,
                Password = code,
                ForbidMessage = "用户名不存在！",
            };

            hybirdLock.Enter();
            for (int i = 0; i < all_list_accounts.Count; i++)
            {
                T item = all_list_accounts[i];
                if (item.UserName == name)
                {
                    if (item.Password != code)
                    {
                        item.LoginFailedCount++;
                        result.ForbidMessage = "密码错误！";
                        break;
                    }
                    else
                    {
                        // 说明已经登录成功，需要进行进一步操作
                        item.LoginFrequency++;
                        result = item.DeepCopy<T>();
                        // 下面两个数据应该是旧的数据
                        item.LastLoginIpAddress = ipAddress;
                        item.LastLoginTime = DateTime.Now;
                        item.LastLoginWay = way;
                        break;
                    }
                }
            }

            hybirdLock.Leave();

            return result;
        }
        /// <summary>
        /// 新增一个账户，如果账户名称已经存在，则返回False，注册成功返回True
        /// </summary>
        /// <param name="json_account">账户对象的JSON表示方式</param>
        /// <returns>成功True，失败False</returns>
        public bool AddNewAccount(string json_account)
        {
            T account = JObject.Parse(json_account).ToObject<T>();
            return AddNewAccount(account);
        }
        /// <summary>
        /// 新增一个账户，如果账户名称已经存在，则返回False，注册成功返回True
        /// </summary>
        /// <param name="account">账户对象</param>
        /// <returns>成功<c>True</c>，失败<c>False</c></returns>
        public bool AddNewAccount(T account)
        {
            bool result = true;
            hybirdLock.Enter();

            // 账户名重复的时候不允许添加
            for (int i = 0; i < all_list_accounts.Count; i++)
            {
                if (all_list_accounts[i].UserName == account.UserName)
                {
                    result = false;
                    break;
                }
            }

            // 账户名为空的时候不允许添加
            if(string.IsNullOrEmpty(account.UserName))
            {
                result = false;
            }

            // 检测通过后进行添加账户名
            if (result)
            {
                all_list_accounts.Add(account);
                ILogNet?.WriteInfo(SoftResources.StringResouce.AccountAddSuccess + account.UserName);
            }

            hybirdLock.Leave();
            return result;
        }

        /// <summary>
        /// 删除一个账户信息，
        /// </summary>
        /// <param name="name">需要删除的账户的名称</param>
        public void DeleteAccount(string name)
        {
            hybirdLock.Enter();

            for (int i = 0; i < all_list_accounts.Count; i++)
            {
                if (name == all_list_accounts[i].UserName)
                {
                    all_list_accounts.RemoveAt(i);
                    ILogNet?.WriteInfo(SoftResources.StringResouce.AccountDeleteSuccess + name);
                    break;
                }
            }

            hybirdLock.Leave();
        }

        /// <summary>
        /// 检查账户对象并返回账户的JSON字符串
        /// </summary>
        /// <param name="name">登录的用户名</param>
        /// <param name="code">登录的密码</param>
        /// <param name="ipAddress">检查的客户端的登录的ip地址</param>
        /// <returns></returns>
        public string CheckAccountJson(string name, string code, string ipAddress, string way)
        {
            T result = CheckAccount(name, code, ipAddress, way);
            return JObject.FromObject(result).ToString();
        }


        /// <summary>
        /// 获取所有的账户的JSON字符串
        /// </summary>
        /// <returns></returns>
        public string GetAllAccountsJson()
        {
            string result = string.Empty;
            hybirdLock.Enter();
            result = JArray.FromObject(all_list_accounts).ToString();
            hybirdLock.Leave();
            return result;
        }

        /// <summary>
        /// 获取所有的账户的JSON字符串
        /// </summary>
        /// <returns></returns>
        /// <param name="factory">获取一个分厂的所有账户</param>
        public string GetAllAccountsJson(string factory)
        {
            string result = string.Empty;
            hybirdLock.Enter();
            result = JArray.FromObject(all_list_accounts.Where(m => m.Factory == factory)).ToString();
            hybirdLock.Leave();
            return result;
        }

        /// <summary>
        /// 获取选定账户的JSON字符串
        /// </summary>
        /// <returns></returns>
        /// <param name="names">选择的用户名</param>
        public string GetAllAccountsJson(string[] names)
        {
            string result = string.Empty;
            hybirdLock.Enter();
            result = JArray.FromObject(all_list_accounts.Where(m => names.Contains(m.UserName))).ToString();
            hybirdLock.Leave();
            return result;
        }


        /// <summary>
        /// 获取账户的别名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetAccountAlias(string name)
        {
            string result = string.Empty;
            hybirdLock.Enter();
            
            for (int i = 0; i < all_list_accounts.Count; i++)
            {
                if (name == all_list_accounts[i].UserName)
                {
                    result = all_list_accounts[i].NameAlias;
                }
            }

            hybirdLock.Leave();
            return result;
        }

        /// <summary>
        /// 获取账户的部门
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetAccountFactory(string name)
        {
            string result = string.Empty;
            hybirdLock.Enter();
            
            for (int i = 0; i < all_list_accounts.Count; i++)
            {
                if (name == all_list_accounts[i].UserName)
                {
                    result = all_list_accounts[i].Factory;
                }
            }

            hybirdLock.Leave();
            return result;
        }

        /// <summary>
        /// 获取所有的账户的用户名的JSON字符串
        /// </summary>
        /// <returns></returns>
        public string GetAccountsNamesJson()
        {
            string result = string.Empty;
            hybirdLock.Enter();
            result = JArray.FromObject(all_list_accounts.ConvertAll(m => m.UserName)).ToString();
            hybirdLock.Leave();
            return result;
        }

        /// <summary>
        /// 从所有的账户的json数据加载账户
        /// </summary>
        /// <param name="json"></param>
        public void LoadAllAccountsJson(string json)
        {
            hybirdLock.Enter();
            try
            {
                all_list_accounts = JArray.Parse(json).ToObject<List<T>>();
            }
            catch (Exception ex)
            {
                ILogNet?.WriteException(SoftResources.StringResouce.AccountLoadFailed, ex);
            }
            hybirdLock.Leave();
        }

        #endregion

        #region Override Method



        /// <summary>
        /// 从字符串加载数据内容
        /// </summary>
        /// <param name="content"></param>
        public override void LoadByString(string content)
        {
            LoadAllAccountsJson(content);
        }
        /// <summary>
        /// 获取需要保存的数据内容
        /// </summary>
        /// <returns></returns>
        public override string ToSaveString()
        {
            return GetAllAccountsJson();
        }
        /// <summary>
        /// 使用加密规则从文件加载
        /// </summary>
        public override void LoadByFile()
        {
            LoadByFile(m => m);
        }
        /// <summary>
        /// 使用加密规则保存到文件
        /// </summary>
        public override void SaveToFile()
        {
            SaveToFile(m => m);
        }


        #endregion
    }
}
