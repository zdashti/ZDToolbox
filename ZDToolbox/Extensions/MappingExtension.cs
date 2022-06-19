using Newtonsoft.Json;

namespace ZDToolbox.Extensions
{
    public static class MappingExtension
    {
        public static IDictionary<string, string> AsDictionary(this object source)
        {
            var dic = new Dictionary<string, string>();
            var json = JsonConvert.SerializeObject(source);
            if (!string.IsNullOrWhiteSpace(json))
            {
                json = json.Replace("{", "").Replace("}", "").Replace("\"", "");
                var array = json.Split(",");
                foreach (var item in array)
                {
                    var keyValue = item.Split(":");
                    if (string.IsNullOrWhiteSpace(keyValue[1]) || keyValue[1].ToLower() == "null") continue;
                    dic.Add(keyValue[0], keyValue[1].ToString());
                }
            }
            return dic;
        }
    }
}
