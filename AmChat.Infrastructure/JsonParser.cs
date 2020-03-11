using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure
{
    public static class JsonParser<T>
    {
        public static List<T> JsonToManyObjects(string json)
        {
            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        public static T JsonToOneObjects(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string ManyObjectsToJson(List<T> objs)
        {
            return JsonConvert.SerializeObject(objs);
        }

        public static string OneObjectToJson(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
