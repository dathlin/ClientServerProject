using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    /*****************************************************************
     * 
     *    订单类，本系统提供一个标准的订单设计准则代码 
     * 
     * 
     * 
     * 
     *****************************************************************/


    public class Order
    {
        /// <summary>
        /// 订单的状态，0：创建，1：被取消，2：支付成功。
        /// </summary>
        public int Status { get; set; } = 0;
        /// <summary>
        /// 订单的流水号
        /// </summary>
        public string OrderNumber { get; set; } = string.Empty;
        /// <summary>
        /// 订单的生成时间，应该使用服务器的时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 订单的会员账号，如果为空，说明不是会员请求的
        /// </summary>
        public string VIPNumber { get; set; } = string.Empty;
        /// <summary>
        /// 支付方式，有可能现金，银行汇款，支付宝，微信等等
        /// </summary>
        public int PaymentWay { get; set; } = 0;
        /// <summary>
        /// 支付时间，可用来统计分析每个单子的滞留时间
        /// </summary>
        public DateTime PaymentTime { get; set; } = DateTime.Now;
        


        /// <summary>
        /// 获取所有商品普通的金额总和
        /// </summary>
        /// <returns></returns>
        public double GetNormalPrices()
        {
            return 0d;
        }
        /// <summary>
        /// 获取所有商品最后付款金额，减去了会员价或是活动价之后的金额
        /// </summary>
        /// <returns></returns>
        public double GetPayPrices()
        {
            return 0d;
        }

    }
}
