using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SS.Common.AI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace SpeechChatAssistant.Demo.Web.Controllers
{
    public class LogicController : Controller
    {
        private readonly ILogger<LogicController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public LogicController(ILogger<LogicController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public class InputClass
        {
            public string InputText { set; get; }
            public int OutputType { set; get; }
        }

        [HttpPost]
        public async Task<IActionResult> Handle([FromBody] InputClass input)
        {
            return await handle(input?.InputText, input?.OutputType == 2);
        }

        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            var inputFile = Request.Form.Files["fileUploader"];
            var outputType = Request.Form["OutputType"].ToString();
            var fileObj = await Upload(inputFile);
            var text = await SpeechHelper.RecognizeFromAudioMP3Async(fileObj.FilePath);
            System.IO.File.Delete(fileObj.FilePath);
            return await handle(text, outputType != "1");
        }


        private async Task<IActionResult> handle(string input, bool returnAudio = false)
        {
            var inputText = input?.Trim();
            if (string.IsNullOrWhiteSpace(inputText))
            {
                return Json(new { ResultType = "QnA", Result = "Empty input", InputText = inputText });
            }
            var luisResult = await LUISHelper.GetObjectResult(inputText);
            if (luisResult.Prediction.TopIntent == "None" || luisResult.Prediction.Intents.FirstOrDefault().Value.Score < 0.5)
            {
                var qnaResult = await QnAMakerHelper.GetAnswer(inputText);
                if (returnAudio)
                {
                    var rootPath = _hostingEnvironment.WebRootPath;
                    var filePath = "Audio/Out/" + Guid.NewGuid() + ".mp3";
                    var fullPath = rootPath + "/" + filePath;
                    var dir = Path.GetDirectoryName(fullPath);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    await SpeechHelper.SynthesisToAudioMP3Async(qnaResult, fullPath);
                    return Json(new { ResultType = "Audio", Result = filePath, InputText = inputText, ResultText = qnaResult });
                }
                else
                {
                    return Json(new { ResultType = "QnA", Result = qnaResult, InputText = inputText });
                }
            }
            else
            {
                return Json(new { ResultType = "Luis", Result = luisResult, InputText = inputText });
            }
        }




        [HttpPost]
        public async Task<string> TTS(string inputText)
        {
            var rootPath = _hostingEnvironment.ContentRootPath;
            var filePath = rootPath + "\\Audio\\" + Guid.NewGuid() + ".wav";
            await SpeechHelper.SynthesisToAudioAsync(inputText, filePath);
            return filePath;
        }

        [HttpPost]
        public async Task<string> STT(string inputFilePath)
        {
            var rootPath = _hostingEnvironment.ContentRootPath;
            var text = await SpeechHelper.RecognizeFromAudioAsync(rootPath + "\\" + inputFilePath);
            return text;
        }




        private static Random _random = new Random();
        public async Task<UploadFile> Upload(IFormFile inputFile)
        {
            var time = DateTime.UtcNow.ToString("yyMMddHHmmssffffff") + "-" + _random.Next(100000, 999999);
            var ext = inputFile.FileName.Substring(inputFile.FileName.LastIndexOf("."));

            string FileUrl = "Audio/Upload/" + time + ext;

            var filePath = _hostingEnvironment.WebRootPath + "/" + FileUrl;
            var dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            MemoryStream memoryStream = new MemoryStream();
            await inputFile.CopyToAsync(memoryStream);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                memoryStream.WriteTo(stream);
            }
            var newFilePath = filePath.Replace(".mp3", ".wav");
            ConvertAudioHelper.ConvertMP3toWAV(filePath, newFilePath);
            System.IO.File.Delete(filePath);
            return new UploadFile
            {
                FileUrl = FileUrl,
                FilePath = newFilePath,
                FileName = inputFile.FileName,
                ContentType = inputFile.ContentType,
                FileSize = inputFile.Length
            };
        }

        public class UploadFile
        {
            public string FileUrl { set; get; }
            public string FilePath { set; get; }
            public string FileName { set; get; }
            public string ContentType { set; get; }
            public long FileSize { set; get; }
        }

    }
}
