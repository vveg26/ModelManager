using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelManager
{
    class People
    {
        //固有属性
        public string card_id =" ";  //证件号码  
        public string name = " ";     //证件姓名
        public int card_type = 0; //证件类型,0为身份证，1为护照，2为驾照
        public int gender = 0; //0为女，1为男
        public byte[] pic;//证件图片byte数组

        //扩展属性
        public string address = "";//地址
        public string contact = "";//通讯录
        public string race = "";//种族

        //标识码
        public string SN = "";


    }
}
