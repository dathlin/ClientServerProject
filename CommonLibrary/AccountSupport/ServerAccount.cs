using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;

namespace CommonLibrary
{
    /// <summary>
    /// 服务器的账户管理类，如果你采用这种方式存储，必须仔细阅读说明手册
    /// </summary>
    /// <typeparam name="T">账户类，该类必须派生自UserAccount类</typeparam>
    public class ServerAccounts<T> where T : UserAccount, new()
    {
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

        /// <summary>
        /// 所有的帳戶信息存儲的位置，只有設置了才進行保存
        /// </summary>
        public string FileSavePath { get; set; } = "";

        private List<T> all_list_accounts = new List<T>();

        private object lock_list_accounts = new object();


        /// <summary>
        /// 更新指定账户的密码
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        public void UpdatePassword(string name, string password)
        {
            lock (lock_list_accounts)
            {
                for (int i = 0; i < all_list_accounts.Count; i++)
                {
                    if (name == all_list_accounts[i].UserName)
                    {
                        all_list_accounts[i].Password = password;
                        break;
                    }
                }
            }
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
        /// 将所有账户信息转换成另一种元素，并返回列表
        /// </summary>
        /// <typeparam name="TResult">目标类型</typeparam>
        /// <param name="converter">转换方法</param>
        /// <returns>转换后的结果列表</returns>
        public List<TResult> ConvertAll<TResult>(Converter<T, TResult> converter)
        {
            return all_list_accounts.ConvertAll(converter);
        }

        /// <summary>
        /// 检查账户信息，并返回账户对象
        /// </summary>
        /// <param name="name">需要验证的用户名</param>
        /// <param name="code">需要验证的密码</param>
        /// <param name="ipAddress">登录的ip地址</param>
        /// <returns>验证的结果对象</returns>
        public T CheckAccount(string name, string code, string ipAddress)
        {
            T result = new T()
            {
                UserName = name,
                Password = code,
                ForbidMessage = "用户名不存在！",
            };

            lock (lock_list_accounts)
            {
                for (int i = 0; i < all_list_accounts.Count; i++)
                {
                    T item = all_list_accounts[i];
                    if (item.UserName == name)
                    {
                        if (item.Password != code)
                        {
                            result.ForbidMessage = "密码错误！";
                            break;
                        }
                        else
                        {
                            //说明已经登录成功，需要进行进一步操作
                            item.LoginFrequency++;
                            result = item.DeepCopy<T>();
                            //下面两个数据应该是旧的数据
                            item.LastLoginIpAddress = ipAddress;
                            item.LastLoginTime = DateTime.Now;
                            break;
                        }
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 新增一个账户，如果账户名称已经存在，则返回False，注册成功返回True
        /// </summary>
        /// <param name="json_account">账户对象的JSON表示方式</param>
        /// <returns>成功True，失败False</returns>
        public bool AddNewAccount(string json_account)
        {
            try
            {
                T account = JObject.Parse(json_account).ToObject<T>();
                return AddNewAccount(account);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 新增一个账户，如果账户名称已经存在，则返回False，注册成功返回True
        /// </summary>
        /// <param name="account">账户对象</param>
        /// <returns>成功True，失败False</returns>
        public bool AddNewAccount(T account)
        {
            lock (lock_list_accounts)
            {
                for (int i = 0; i < all_list_accounts.Count; i++)
                {
                    if (all_list_accounts[i].UserName == account.UserName)
                    {
                        return false;
                    }
                }
                all_list_accounts.Add(account);
            }
            return true;
        }

        /// <summary>
        /// 删除一个账户信息，
        /// </summary>
        /// <param name="name">需要删除的账户的名称</param>
        public void DeleteAccount(string name)
        {
            lock (lock_list_accounts)
            {
                for (int i = 0; i < all_list_accounts.Count; i++)
                {
                    if (name == all_list_accounts[i].UserName)
                    {
                        all_list_accounts.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 检查账户对象并返回账户的JSON字符串
        /// </summary>
        /// <param name="name">登录的用户名</param>
        /// <param name="code">登录的密码</param>
        /// <param name="ipAddress">检查的客户端的登录的ip地址</param>
        /// <returns></returns>
        public string CheckAccountJson(string name, string code, string ipAddress)
        {
            T result = CheckAccount(name, code, ipAddress);
            return JObject.FromObject(result).ToString();
        }

        /// <summary>
        /// 获取所有的账户的JSON字符串
        /// </summary>
        /// <returns></returns>
        public string GetAllAccountsJson()
        {
            string result = string.Empty;
            lock (lock_list_accounts)
            {
                result = JArray.FromObject(all_list_accounts).ToString();
            }
            return result;
        }

        /// <summary>
        /// 从所有的账户的json数据加载账户
        /// </summary>
        /// <param name="json"></param>
        public void LoadAllAccountsJson(string json)
        {
            lock (lock_list_accounts)
            {
                try
                {
                    all_list_accounts = JArray.Parse(json).ToObject<List<T>>();
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// 使用Base64编码将所有的账户信息保存到文件
        /// </summary>
        public void SaveFile()
        {
            SaveFile(m => Convert.ToBase64String(Encoding.Unicode.GetBytes(m)));
        }

        /// <summary>
        /// 使用自定义的加密方法将所有账户信息保存到文件
        /// </summary>
        /// <param name="encrypt">加密的方式</param>
        public void SaveFile(Converter<string, string> encrypt)
        {
            if (FileSavePath != "")
            {
                string result = GetAllAccountsJson();
                StreamWriter sw = new StreamWriter(FileSavePath, false, Encoding.Default);
                sw.Write(encrypt(result));
                sw.Flush();
                sw.Dispose();
            }
        }

        /// <summary>
        /// 使用Base64编码从文件加载所有的账户
        /// </summary>
        public void LoadByFile()
        {
            LoadByFile(m => Encoding.Unicode.GetString(Convert.FromBase64String(m)));
        }
        /// <summary>
        /// 使用自定义的解密方法加载所有的账户
        /// </summary>
        public void LoadByFile(Converter<string, string> decrypt)
        {
            if (FileSavePath != "")
            {
                if (File.Exists(FileSavePath))
                {
                    StreamReader sr = new StreamReader(FileSavePath, Encoding.Default);
                    string result = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                    result = decrypt(result);
                    LoadAllAccountsJson(result);
                }
            }
        }


        /**************************************************************
         * 
         *    日志记录块，保存运行中的所有的异常信息，方便追踪系统异常
         * 
         *************************************************************/
        /// <summary>
        /// 日志记录对象
        /// </summary>
        public HslCommunication.Enthernet.HslLogHelper LogHelper { get; set; }
    }
}
