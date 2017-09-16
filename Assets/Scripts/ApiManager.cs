using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using System;

public class ApiManager : MonoBehaviour {
    /// <summary>
    /// Send image to Google VIsion API, get response JSON and send to IconManager
    /// </summary>
    public IEnumerator GoogleRequest(byte[] image)
    {
        // Convert to Bse64 String
        string base64Image = Convert.ToBase64String(image);

        DownloadHandler download = new DownloadHandlerBuffer();

        // Create JSON
        string json = "{\"requests\": [{\"image\": {\"content\": \"" + base64Image + "\"},\"features\": [{\"type\": \"TEXT_DETECTION\",\"maxResults\": 1}]}]}";
        byte[] content = Encoding.UTF8.GetBytes(json);

        // Enter the url to your google vision api account here:
        // string url = "https://vision.googleapis.com/v1/images:annotate?key=YourKeyHere";

        // Request to API
        var header = new Dictionary<string, string>() {
            { "Content-Type", "application/json" }
        };

        var data = Encoding.UTF8.GetBytes(json);
        WWW www = new WWW(url, data, header);

        // Send API
        yield return www;

        if (www.error != null)
        {
            if (gameObject.GetComponent<SettingsManager>().OCRSetting == OCRRunSetting.Manual)
            {
                GetComponent<TextToSpeechManager>().SpeakText("Error Occurred");
            }
        }
        else
        {
            string respJson = www.text;

            // Indicate when no text is detected
            if (!respJson.Contains("textAnnotations") || respJson.Contains("error"))
            {
                if (gameObject.GetComponent<SettingsManager>().OCRSetting == OCRRunSetting.Manual )
                {
                    GetComponent<TextToSpeechManager>().SpeakText("No Text Was Detected");
                }
            }
            else
            {
                GetComponent<IconManager>().CreateIcons(respJson);
            }
        }

        // Update indicators
        GetComponent<CameraManager>().detecting = false;

    }
}
