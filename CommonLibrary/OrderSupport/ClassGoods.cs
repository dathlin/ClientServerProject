using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    /********************************************************************************
     * 
     *     订单类的商品类，是单个的商品对象
     * 
     * 
     * 
     * 
     ********************************************************************************/

    /// <summary>
    /// 订单的商品类
    /// </summary>
    public class ItemGoods
    {
        /// <summary>
        /// 商品的编号，也可以是条形码编号
        /// </summary>
        public string ItemCode { get; set; } = string.Empty;
        /// <summary>
        /// 商品的名称
        /// </summary>
        public string ItemName { get; set; } = string.Empty;
        /// <summary>
        /// 购买商品的数量
        /// </summary>
        public int ItemCount { get; set; } = 1;
        /// <summary>
        /// 商品的单个的基础价格
        /// </summary>
        public double ItemSinglePrice { get; set; }


        public double GetNormalPrice()
        {
            if (ItemCount >= 0)
                return ItemSinglePrice * ItemCount;
            else throw new Exception("数量错误");
        }
        /// <summary>
        /// 根据具体的优惠条件来获取最终的价格，有可能来自会员的固定折扣，也有可能来自活动折扣
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public double GetPayPrice(Func<double, int, double> method)
        {
            return method.Invoke(ItemSinglePrice, ItemCount);
        }
    }
}
