// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if WINDOWS_UWP
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.SpeechSynthesis;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
#endif

namespace HoloToolkit.Unity
{
    public static class UnityAudioHelper
    {
        /// <summary>
        /// Converts two bytes to one float in the range -1 to 1 
        /// </summary>
        /// <param name="firstByte">
        /// The first byte.
        /// </param>
        /// <param name="secondByte">
        /// The second byte.
        /// </param>
        /// <returns>
        /// The converted float.
        /// </returns>
        private static float BytesToFloat(byte firstByte, byte secondByte)
        {
            // Convert two bytes to one short (little endian)
            short s = (short)((secondByte << 8) | firstByte);

            // Convert to range from -1 to (just below) 1
            return s / 32768.0F;
        }

        /// <summary>
        /// Dynamically creates an <see cref="AudioClip"/> that represents raw Unity audio data.
        /// </summary>
        /// <param name="name">
        /// The name of the dynamically generated clip.
        /// </param>
        /// <param name="audioData">
        /// Raw Unity audio data.
        /// </param>
        /// <param name="sampleCount">
        /// The number of samples in the audio data.
        /// </param>
        /// <param name="frequency">
        /// The frequency of the audio data.
        /// </param>
        /// <returns>
        /// The <see cref="AudioClip"/>.
        /// </returns>
        internal static AudioClip ToClip(string name, float[] audioData, int sampleCount, int frequency)
        {
            // Create the audio clip
            var clip = AudioClip.Create(name, sampleCount, 1, frequency, false);

            // Set the data
            clip.SetData(audioData, 0);

            // Done
            return clip;
        }

        /// <summary>
        /// Converts raw WAV data into Unity formatted audio data.
        /// </summary>
        /// <param name="wavAudio">
        /// The raw WAV data.
        /// </param>
        /// <param name="sampleCount">
        /// The number of samples in the audio data.
        /// </param>
        /// <param name="frequency">
        /// The frequency of the audio data.
        /// </param>
        /// <returns>
        /// The Unity formatted audio data.
        /// </returns>
        internal static float[] ToUnityAudio(byte[] wavAudio, out int sampleCount, out int frequency)
        {
            // Determine if mono or stereo
            int channelCount = wavAudio[22];     // Speech audio data is always mono but read actual header value for processing

            // Get the frequency
            frequency = BitConverter.ToInt32(wavAudio, 24);

            // Get past all the other sub chunks to get to the data subchunk:
            int pos = 12;   // First subchunk ID from 12 to 16

            // Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))
            while (!(wavAudio[pos] == 100 && wavAudio[pos + 1] == 97 && wavAudio[pos + 2] == 116 && wavAudio[pos + 3] == 97))
            {
                pos += 4;
                int chunkSize = wavAudio[pos] + wavAudio[pos + 1] * 256 + wavAudio[pos + 2] * 65536 + wavAudio[pos + 3] * 16777216;
                pos += 4 + chunkSize;
            }
            pos += 8;

            // Pos is now positioned to start of actual sound data.
            sampleCount = (wavAudio.Length - pos) / 2;     // 2 bytes per sample (16 bit sound mono)
            if (channelCount == 2) sampleCount /= 2;      // 4 bytes per sample (16 bit stereo)

            // Allocate memory (supporting left channel only)
            float[] unityData = new float[sampleCount];

            // Write to double array/s:
            int i = 0;
            while (pos < wavAudio.Length)
            {
                unityData[i] = BytesToFloat(wavAudio[pos], wavAudio[pos + 1]);

                pos += 2;
                if (channelCount == 2)
                {
                    pos += 2;
                }
                i++;
            }

            // Done
            return unityData;
        }
#if WINDOWS_UWP
    internal static async Task<byte[]> SynthesizeToUnityDataAsync(
      string text,
      Func<string, IAsyncOperation<SpeechSynthesisStream>> speakFunc)
    {
      byte[] buffer = null;
 
      // Speak and get stream
      using (var speechStream = await speakFunc(text))
      {
        // Create buffer
        buffer = new byte[speechStream.Size];
 
        // Get input stream and the size of the original stream
        using (var inputStream = speechStream.GetInputStreamAt(0))
        {
          await inputStream.ReadAsync(buffer.AsBuffer(),
            (uint)buffer.Length, InputStreamOptions.None);
        }
      }
      return (buffer);
    }
#endif
    }
}