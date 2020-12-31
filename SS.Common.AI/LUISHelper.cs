using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Authoring;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Authoring.Models;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Newtonsoft.Json;

namespace SS.Common.AI
{
    public class LUISHelper
    {
        public static string authoringKey = "authoringKey";
        public static string predictionKey = "predictionKey";
        public static string authoringResourceName = "authoringResourceName";
        public static string predictionResourceName = "predictionResourceName";
        public static string authoringEndpoint = String.Format("https://{0}.cognitiveservices.azure.com/", authoringResourceName);
        public static string predictionEndpoint = String.Format("https://{0}.cognitiveservices.azure.com/", predictionResourceName);
        public static string appName = "appName";
        public static Guid appId = Guid.Parse("appId");
        public static string versionId = "0.1";


        public static async Task<string> GetResult(string input)
        {
            var credentials = new Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.ApiKeyServiceClientCredentials(predictionKey);
            var runtimeClient = new LUISRuntimeClient(credentials) { Endpoint = predictionEndpoint };
            var request = new PredictionRequest { Query = input };
            var prediction = await runtimeClient.Prediction.GetSlotPredictionAsync(appId, "Production", request);
            var result = JsonConvert.SerializeObject(prediction, Formatting.Indented);
            return result;
        }

        public static async Task<PredictionResponse> GetObjectResult(string input)
        {
            var credentials = new Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.ApiKeyServiceClientCredentials(predictionKey);
            var runtimeClient = new LUISRuntimeClient(credentials) { Endpoint = predictionEndpoint };
            var request = new PredictionRequest { Query = input };
            var prediction = await runtimeClient.Prediction.GetSlotPredictionAsync(appId, "Production", request);
            return prediction;
        }
    }
}
