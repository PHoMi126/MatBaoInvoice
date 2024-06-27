using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization;

namespace MatBaoInvoice.Invoice
{
    /// <summary>
    /// Convert Object(s) into another format (XML, JSON, ...)
    /// </summary>
    class SerializeUtil
    {
        private const DateTimeZoneHandling dateTimeZoneHandling = DateTimeZoneHandling.Local; //Using local timezone

        public static string SerializeObject(object data) //Object -> JSON
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = dateTimeZoneHandling;
            return JsonConvert.SerializeObject(data, settings);
        }

        public static T DeserializeObject<T>(string data) //JSON -> Object
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = dateTimeZoneHandling;
            return JsonConvert.DeserializeObject<T>(data, settings);
        }

        public static object DeserializeObject(string data, Type objectType) //JSON -> Object
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = dateTimeZoneHandling;
            return JsonConvert.DeserializeObject(data, objectType, settings);
        }

        /// <summary>
        /// Convert the data string to the nearest correct type.
        /// Use to convert report parameters to the correct type
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object ParseClientParamValue(string value)
        {
            if (Guid.TryParse(value, out Guid gValue))
            {
                return gValue;
            }

            return value;
        }
    }
}
