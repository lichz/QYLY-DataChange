using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace RoadFlow.Utility
{
    /// <summary>
    /// 短信
    /// </summary>
   public class SMS
    {
       protected  string Userid{ get; set; }

       private  string Account{ get; set; }

       private  string Password { get; set; }
       private  string PostUrl { get; set; }

       public SMS()
       {
            PostUrl = ConfigurationManager.AppSettings["SMS.PostUrl"];
           string users = ConfigurationManager.AppSettings["SMS.User"];
           Userid = users.Split(',')[0];
           Account = users.Split(',')[1];
           Password = users.Split(',')[2];
       }

       /// <summary>
       /// 发送短信
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public string SendSMS(SMSMessage model)
       {
           //return "";
           string postStrTpl = "userid={0}&account={1}&password={2}&mobile={3}&content={4}&sendTime={5}";
           UTF8Encoding encoding = new UTF8Encoding();
           byte[] postData = encoding.GetBytes(string.Format(postStrTpl, Userid, Account, Password, model.Mobile, model.Content, model.SendTime<DateTime.Now?"":model.SendTime.ToString("yyyy-MM-dd HH:mm:ss")));

           HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(PostUrl);
           myRequest.Method = "POST";
           myRequest.ContentType = "application/x-www-form-urlencoded";
           myRequest.ContentLength = postData.Length;

           Stream newStream = myRequest.GetRequestStream();
           // Send the data.
           newStream.Write(postData, 0, postData.Length);
           newStream.Flush();
           newStream.Close();

           HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
           if (myResponse.StatusCode == HttpStatusCode.OK)
           {
               StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
               return reader.ReadToEnd();
               //反序列化upfileMmsMsg.Text
               //实现自己的逻辑
           }
           else
           {
               return "访问失败";
           }
       }
    }

    public class SMSMessage
    {
        /// <summary>
        /// 发信发送的目的号码.多个号码之间用半角逗号隔开 
        /// </summary>
        public string Mobile{ get; set; }
        /// <summary>
        /// 系统做有判断和运营商规定，短信发送时内容签名请添加签名，签名长度：3-8个汉字内，例如：【志愿中国】+内容
        /// </summary>
        public string Content{ get; set; }
       public DateTime SendTime { get; set; }
    }

}
