using Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker;
using Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SS.Common.AI
{
    public class QnAMakerHelper
    {
        public static string authoringKey = "authoringKey";
        public static string resourceName = "resourceName";
        public static string authoringURL = $"https://{resourceName}.cognitiveservices.azure.com";
        public static string queryingURL = $"https://{resourceName}-Web.azurewebsites.net";
        public static string kbId = "knowledgeBaseId";

        public static async Task<String> GetAnswer(string question)
        {
            var client = new QnAMakerClient(new ApiKeyServiceClientCredentials(authoringKey))
            { Endpoint = authoringURL };

            var primaryQueryEndpointKey = await GetQueryEndpointKey(client);
            var runtimeClient = new QnAMakerRuntimeClient(new EndpointKeyServiceClientCredentials(primaryQueryEndpointKey))
            { RuntimeEndpoint = queryingURL };
            var answer = await GenerateAnswer(runtimeClient, question);
            return answer;
        }

        private static async Task<String> GetQueryEndpointKey(IQnAMakerClient client)
        {
            var endpointKeysObject = await client.EndpointKeys.GetKeysAsync();
            return endpointKeysObject.PrimaryEndpointKey;
        }

        private static async Task<String> GenerateAnswer(IQnAMakerRuntimeClient runtimeClient, string question)
        {
            var response = await runtimeClient.Runtime.GenerateAnswerAsync(kbId, new QueryDTO { Question = question });
            return response.Answers[0].Answer;
        }
    }
}
