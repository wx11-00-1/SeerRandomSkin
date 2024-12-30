using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace SeerRandomSkin
{
    internal class Utils
    {
        public static JObject TryGetJObject(string str)
        {
            try
            {
                return JObject.Parse(str);
            }
            catch
            {
                return new JObject();
            }
        }

        /// <summary>
        /// Json 字符串 转 对象，失败则创建新对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T TryJsonConvert<T>(string str)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(str);
            }
            catch (JsonReaderException)
            {
                return Activator.CreateInstance<T>();
            }
        }
    }
}
