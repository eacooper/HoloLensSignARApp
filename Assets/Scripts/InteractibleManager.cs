using HoloToolkit;
using UnityEngine;

/// <summary>
/// InteractibleManager keeps tracks of which GameObject
/// is currently in focus.
/// </summary>
public class InteractibleManager : Singleton<InteractibleManager>
{
    public GameObject TextBox;

    public GameObject FocusedGameObject { get; private set; }

    private GameObject oldFocusedGameObject = null;

    void Start()
    {
        FocusedGameObject = null;
    }

    void Update()
    {
        oldFocusedGameObject = FocusedGameObject;

        if (GazeManager.Instance.Hit)
        {
            RaycastHit hitInfo = GazeManager.Instance.HitInfo;
            if (hitInfo.collider != null)
            {
                // Assign the hitInfo's collider gameObject to the FocusedGameObject.
                FocusedGameObject = hitInfo.collider.gameObject;
            }
            else
            {
                FocusedGameObject = null;
            }
        }
        else
        {
            FocusedGameObject = null;
        }

        if (FocusedGameObject != oldFocusedGameObject)
        {
            ResetFocusedInteractible();

            if (FocusedGameObject != null)
            {
                if (FocusedGameObject.GetComponent<Interactible>() != null)
                {
                    // Send a GazeEntered message to the FocusedGameObject.
                    FocusedGameObject.SendMessage("GazeEntered");
                }
            }
        }
    }

    private void ResetFocusedInteractible()
    {
        if (oldFocusedGameObject != null)
        {
            if (oldFocusedGameObject.GetComponent<Interactible>() != null)
            {
                // Send a GazeExited message to the oldFocusedGameObject.
                oldFocusedGameObject.SendMessage("GazeExited");
            }
        }
    }

    /// <summary>
    /// Deselect current icon
    /// </summary>
    public void DeSelect()
    {
        GameObject selectedIcon = gameObject.GetComponent<IconManager>().SelectedIcon;

        if (selectedIcon != null)
        {
            selectedIcon.SetActive(true);
            gameObject.GetComponent<IconManager>().SelectedIcon.GetComponent<Interactible>().ShowOriginalMaterial();
            gameObject.GetComponent<IconManager>().SelectedIcon = null;
        }

        if (TextBox != null)
        {
            TextBox.SetActive(false);
        }
    }


    /// <summary>
    /// Select icon
    /// </summary>
    public void Select()
    {
        if (FocusedGameObject != null && FocusedGameObject.GetComponent<Interactible>() != null)
        {
            FocusedGameObject.GetComponent<Interactible>().OnSelect();
        }
    }
}