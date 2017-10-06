using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{


    /*************************************************************************************
     * 
     *    说明：本来考虑使用特性来给账户标注，但是考虑到以后将要实现多语言版本，那么特性支持就
     *          不是个好选择，所以放弃了，保留了特性类
     * 
     *************************************************************************************/








    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class HslDisplayAttribute : Attribute
    {
        public string m_display;


        public HslDisplayAttribute(string display)
        {
            m_display = display;
        }
    }
}
