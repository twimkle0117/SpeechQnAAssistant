using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SS.Common.AI
{
    public class SpeechHelper
    {
        public static string subscriptionKey = "subscriptionKey";
        public static string region = "region";

        /// <summary>文字转语音 输出到文件</summary> 
        public static async Task SynthesisToAudioMP3Async(string inputText, string outputFileName)
        {
            var config = SpeechConfig.FromSubscription(subscriptionKey, region);
            config.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Audio16Khz32KBitRateMonoMp3);
            using var audioConfig = AudioConfig.FromWavFileOutput(outputFileName);
            using var synthesizer = new SpeechSynthesizer(config, audioConfig);
            await synthesizer.SpeakTextAsync(inputText);
        }

        /// <summary>文字转语音 输出到文件</summary> 
        public static async Task SynthesisToAudioAsync(string inputText, string outputFileName)
        {
            var config = SpeechConfig.FromSubscription(subscriptionKey, region);
            using var audioConfig = AudioConfig.FromWavFileOutput(outputFileName);
            using var synthesizer = new SpeechSynthesizer(config, audioConfig);
            await synthesizer.SpeakTextAsync(inputText);
        }

        /// <summary>文字转语音 输出到扬声器</summary> 
        public static async Task SynthesisToSpeakerAsync(string inputText)
        {
            var config = SpeechConfig.FromSubscription(subscriptionKey, region);
            using var synthesizer = new SpeechSynthesizer(config);
            await synthesizer.SpeakTextAsync(inputText);
        }

        /// <summary>文字转语音 输出到内存流</summary> 
        public static async Task<AudioDataStream> SynthesisToStreamAsync(string inputText)
        {
            var config = SpeechConfig.FromSubscription(subscriptionKey, region);
            using var synthesizer = new SpeechSynthesizer(config, null);
            var result = await synthesizer.SpeakTextAsync("inputText");
            using var stream = AudioDataStream.FromResult(result);
            return stream;
        }


        /// <summary>语音转文字 从文件识别</summary> 
        public static async Task<string> RecognizeFromAudioMP3Async(string inputFileName)
        {
            var config = SpeechConfig.FromSubscription(subscriptionKey, region);
            using var audioConfig = AudioConfig.FromWavFileInput(inputFileName);
            using var recognizer = new SpeechRecognizer(config, audioConfig);
            var result = await recognizer.RecognizeOnceAsync();
            return result.Text;
        }


        /// <summary>语音转文字 从文件识别</summary> 
        public static async Task<string> RecognizeFromAudioAsync(string inputFileName)
        {
            var config = SpeechConfig.FromSubscription(subscriptionKey, region);
            using var audioConfig = AudioConfig.FromWavFileInput(inputFileName);
            using var recognizer = new SpeechRecognizer(config, audioConfig);
            var result = await recognizer.RecognizeOnceAsync();
            return result.Text;
        }


        /// <summary>语音转文字 从内存流识别</summary> 
        public static async Task<string> RecognizeFromStreamAsync(string inputFileName)
        {
            var config = SpeechConfig.FromSubscription(subscriptionKey, region);

            var reader = new BinaryReader(File.OpenRead(inputFileName));
            using var audioInputStream = AudioInputStream.CreatePushStream();
            using var audioConfig = AudioConfig.FromStreamInput(audioInputStream);
            using var recognizer = new SpeechRecognizer(config, audioConfig);

            byte[] readBytes;
            do
            {
                readBytes = reader.ReadBytes(1024);
                audioInputStream.Write(readBytes, readBytes.Length);
            } while (readBytes.Length > 0);

            var result = await recognizer.RecognizeOnceAsync();
            return result.Text;
        }


        /// <summary>语音转文字 从麦克风识别</summary> 
        public static async Task<string> RecognizeFromSpeechAsync()
        {
            var config = SpeechConfig.FromSubscription(subscriptionKey, region);

            using (var recognizer = new SpeechRecognizer(config))
            {
                var result = await recognizer.RecognizeOnceAsync();

                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine($"We recognized: {result.Text}");
                    return result.Text;
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = CancellationDetails.FromResult(result);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                        Console.WriteLine($"CANCELED: Did you update the subscription info?");
                    }
                }
                return "";
            }
        }


    }
}
