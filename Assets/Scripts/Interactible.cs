using UnityEngine;

/// <summary>
/// The Interactible class flags a Game Object as being "Interactible".
/// Determines what happens when an Interactible is being gazed at.
/// </summary>
public class Interactible : MonoBehaviour
{
    [Tooltip("Audio clip to play when interacting with this hologram.")]
    public AudioClip TargetFeedbackSound;
    [Tooltip("Material to show by default.")]
    public Material DefaultMaterial;
    [Tooltip("Material to show that the icon is confident in the text it holds.")]
    public Material ValidMaterial;
    [Tooltip("Material to show that the icon is slight not confident in the text it holds.")]
    public Material WarningMaterial;
    [Tooltip("Material to show that the icon is not confident in the text it holds.")]
    public Material ErrorMaterial;
    [Tooltip("Material to show that the icon is gazed upon.")]
    public Material TargetedMaterial;
    [Tooltip("Interval for flashing (in seconds)")]
    public float Interval;

    public Material OriginalMaterial { get; set; }

    private AudioSource audioSource;

    private Material[] defaultMaterials;

    private Renderer renderer;

    private int flashCounter;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        defaultMaterials = GetComponent<Renderer>().materials;

        if (renderer != null)
        {
            renderer.material = OriginalMaterial;
        }

        // Add a BoxCollider if the interactible does not contain one.
        Collider collider = GetComponentInChildren<Collider>();
        if (collider == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }

        EnableAudioHapticFeedback();

        // Flash icon
        flashCounter = 0;
        InvokeRepeating("FlashIcon", 0f, Interval);
    }

    private void EnableAudioHapticFeedback()
    {
        // If this hologram has an audio clip, add an AudioSource with this clip.
        if (TargetFeedbackSound != null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.clip = TargetFeedbackSound;
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1;
            audioSource.dopplerLevel = 0;
        }
    }

    void GazeEntered()
    {
        // Change material when gaze entered
        ShowTargetedMaterial();
    }

    void GazeExited()
    {
        // Change material back when gaze exited
        ShowOriginalMaterial();
    }

    public void ShowTargetedMaterial()
    {
        renderer.material = TargetedMaterial;
    }

    public void ShowOriginalMaterial()
    {
        renderer.material = OriginalMaterial;
    }

    /// <summary>
    /// Call functions in IconAction.cs
    /// </summary>
    public void OnSelect()
    {
        Debug.Log("OnSelect");
        GameObject.Find("Managers").GetComponent<InteractibleManager>().DeSelect();
        GameObject.Find("Managers").GetComponent<IconManager>().SelectedIcon = gameObject;

        // Play the audioSource feedback when we gaze and select a hologram.
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        // Handle the OnSelect by sending a PerformTagAlong message.
        this.SendMessage("PerformTagAlong");
        this.SendMessage("HideIcon");
    }

    /// <summary>
    /// Flash icon by creating a counter that indicates when it should be the original material and when it should be the other material
    /// </summary>
    void FlashIcon()
    {
        //Debug.Log("Flashing Icon");
        GameObject currIconSelected = GameObject.Find("Managers").GetComponent<InteractibleManager>().FocusedGameObject;
        if (currIconSelected != null && currIconSelected.GetInstanceID() == gameObject.GetInstanceID())
            return;

        if (flashCounter % 2 == 0)
        {
            renderer.material = DefaultMaterial;
        }
        else
        {
            renderer.material = OriginalMaterial;
        }

        flashCounter++;
    }
}