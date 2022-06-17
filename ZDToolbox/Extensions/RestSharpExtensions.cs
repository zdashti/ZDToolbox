using Newtonsoft.Json;
using RestSharp;
using System.ComponentModel.DataAnnotations;


namespace ZDToolbox.Extensions
{
    public enum ContentTypeEnum
    {
        [Display(Name = "application/json")]
        ApplicationJson,
        [Display(Name = "application/x-www-form-urlencoded")]
        ApplicationXWwwFormUrlencoded
    }
    public static class RestSharpExtension
    {
        public static IRestResponse SendPostRequest(this RestClient client, ContentTypeEnum contentType, object param, List<KeyValue<string, string>> headerParams)
        {
            var request = new RestRequest(Method.POST)
                .ManipulateRequestParameters(contentType, param)
                .AddHeader("Content-Type", contentType.DisplayName());
            if (headerParams != null)
                foreach (var item in headerParams)
                    request.AddHeader(item.Key, item.Value);
            //throw new DomainException("don't run!!!!!");
            return client.Execute(request);
        }

        public static IRestResponse SendGetRequest(this RestClient client, List<KeyValue<string, string>> headerParams)
        {
            var request = new RestRequest(Method.GET);
            foreach (var item in headerParams)
                request.AddHeader(item.Key, item.Value);
            return client.Execute(request);
        }

        public static RestRequest ManipulateRequestParameters(this RestRequest request, ContentTypeEnum contentType, object body)
        {
            switch (contentType)
            {
                case ContentTypeEnum.ApplicationJson:
                    request.AddParameter(ContentTypeEnum.ApplicationJson.DisplayName(), JsonConvert.SerializeObject(body, Formatting.Indented), ParameterType.RequestBody);
                    break;
                case ContentTypeEnum.ApplicationXWwwFormUrlencoded:
                    if (!(body is List<KeyValue<string, object>>))
                        throw new Exception("Send Body Param as List<KeyValue<string, object>>");
                    foreach (var pair in (List<KeyValue<string, object>>)body)
                        request.AddParameter(pair.Key, pair.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null);
            }

            return request;
        }

        public static RestClient CreateClient(string baseUrl, string methodName = "", int timeOut = -1)
        {
            return new RestClient(baseUrl + methodName) { Timeout = timeOut };
        }
    }
}
