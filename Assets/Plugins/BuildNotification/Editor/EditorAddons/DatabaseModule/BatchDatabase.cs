using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BuildNotification.EditorAddons.DatabaseModule.RequestWrappers;
using BuildNotification.EditorAddons.Interfaces;
using Newtonsoft.Json;
using HttpMessageContent = BuildNotification.EditorAddons.DatabaseModule.Extensions.HttpMessageContent;

namespace BuildNotification.EditorAddons.DatabaseModule
{
    public class BatchDatabase : Database
    {
        private const string Boundary = "subrequestboundary";

        public BatchDatabase(ISendData cmData)
        {
            sendData = cmData;
        }

        private protected override async Task<Wrapper> ParseResponseMessage<TResponse>(
            HttpResponseMessage responseMessage)
        {
            var parsed = await responseMessage.Content.ReadAsStringAsync();
            var matches = Regex.Matches(parsed, @"{({*[^{}]*}*)}").ToList();
            var respondList = matches.Select(match => JsonConvert.DeserializeObject<TResponse>(match.ToString())).ToList();

            return new ListWrapper<TResponse>(respondList);
        }

        private protected override async Task<HttpResponseMessage> SendRequest(HttpClient client, HttpContent content)
        {
            return await client.PostAsync(sendData.RequestBatchUrl, content);
        }

        private protected override HttpContent GenerateContent<TRequest>(Wrapper requestWrapper)
        {
            if (requestWrapper is not ListWrapper<TRequest> list) throw new InvalidDataException();
            var content = new MultipartContent("mixed", Boundary);
            foreach (var request in list.Data)
            {
                var serializeObject = JsonConvert.SerializeObject(request, Formatting.Indented,
                    new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                var requestMessage = new HttpRequestMessage(HttpMethod.Post,
                    new Uri(new Uri(sendData.BaseUrl), sendData.RequestUrl))
                {
                    Content = new StringContent(serializeObject)
                };

                var messageContent = new HttpMessageContent(
                    requestMessage
                );


                messageContent.Headers.ContentType = new MediaTypeHeaderValue(sendData.GetContentType());
                messageContent.Headers.TryAddWithoutValidation("Content-Transfer-Encoding", "binary");
                messageContent.Headers.TryAddWithoutValidation("Authorization", $"Bearer {sendData.GenerateToken()}");
                messageContent.Headers.TryAddWithoutValidation("Accept", sendData.GetContentType());

                content.Add(messageContent);
            }

            return content;
        }
    }
}