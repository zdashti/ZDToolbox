using Newtonsoft.Json;

namespace ZDToolbox.Helpers
{
    public static class SerializerHelper
    {
        public static TResult SerializeDeserialize<TResult, TRequest>(TRequest user)
        {
            return JsonConvert.DeserializeObject<TResult>(JsonConvert.SerializeObject(user));
        }
    }
}
