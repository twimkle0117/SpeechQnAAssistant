using NAudio.MediaFoundation;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace SS.Common.AI
{
    public class ConvertAudioHelper
    {
        /// <summary>
        /// 将wav转为MP3
        /// </summary>
        /// <param name="sourceFile">in wav文件</param>
        /// <param name="desFile">out MP3文件</param>
        public static void ConvertWAVtoMP3(string sourceFile, string desFile)
        {
            MediaFoundationApi.Startup();
            using (var reader = new WaveFileReader(sourceFile))
            {
                MediaFoundationEncoder.EncodeToMp3(reader, desFile);
            }
        }

        /// <summary>
        /// 将mp3转为wav
        /// </summary>
        /// <param name="sourceFile">in mp3文件</param>
        /// <param name="desFile">out wav文件</param>
        public static void ConvertMP3toWAV(string sourceFile, string desFile)
        {
            using (var reader = new Mp3FileReader(sourceFile))
            {
                WaveFileWriter.CreateWaveFile(desFile, reader);
            }
        }
    }
}
