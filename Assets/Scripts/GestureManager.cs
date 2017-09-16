using HoloToolkit;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

/// <summary>
/// GestureManager contains event handlers for subscribed gestures.
/// </summary>
public class GestureManager : Singleton<GestureManager>
{
    private GestureRecognizer gestureRecognizer;
    private SettingsManager SettingsManager;

    void Start()
    {
        SettingsManager = gameObject.GetComponent<SettingsManager>();

        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);

        gestureRecognizer.TappedEvent += (source, tapCount, ray) =>
        {
            if (source.ToString().Equals("Hand"))
            {
                Debug.Log("In hand");
                HandTapHandler();
            } else
            {
                Debug.Log("In Controller");
                ControllerTapHandler();
            }
        };

        gestureRecognizer.StartCapturingGestures();
    }

    void OnDestroy()
    {
        gestureRecognizer.StopCapturingGestures();
    }

    /// <summary>
    /// Responses to air taps
    /// </summary>
    void HandTapHandler()
    {
        GameObject focusedObject = InteractibleManager.Instance.FocusedGameObject;

        if (focusedObject != null && focusedObject.layer != 8)                      // Select icon (don't select SpatialMapping which is layer 8)
        {
            focusedObject.SendMessage("OnSelect");
        }
        else                                                                        // Deselect icon if you don't click on an icon
        {
            InteractibleManager.Instance.DeSelect();
        }
    }

    /// <summary>
    /// Response to clicker clicks
    /// </summary>
    void ControllerTapHandler()
    {
        GameObject focusedObject = InteractibleManager.Instance.FocusedGameObject;
        GameObject selectedIcon = GetComponent<IconManager>().SelectedIcon;

        if (focusedObject != null && focusedObject.layer != 8)                          // Select Icon if you're looking at an icon and not the spatial mapping (layer 8)
        {
            focusedObject.SendMessage("OnSelect");
        }
        else
        {
            if (selectedIcon != null)                                                   // Deselect Icon if you have selected an icon
            {
                InteractibleManager.Instance.DeSelect();
            }
            // Run OCR if you clicked and you haven't selected an icon
            else if (SettingsManager.OCRSetting == OCRRunSetting.Manual)  
            {
                GetComponent<CameraManager>().BeginManualPhotoMode();
            }
        }
    }
}