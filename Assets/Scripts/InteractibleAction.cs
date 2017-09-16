using HoloToolkit;
using UnityEngine;

/// <summary>
/// InteractibleAction performs custom actions when you gaze at the holograms.
/// </summary>
public class InteractibleAction : MonoBehaviour
{
    [Tooltip("Drag the Tagalong prefab asset you want to display.")]
    public GameObject ObjectToTagAlong;

    void PerformTagAlong()
    {
        if (ObjectToTagAlong == null)
        {
            return;
        }

        // Recommend having only one tagalong.
        GameObject existingTagAlong = GameObject.FindGameObjectWithTag("TagAlong");
        if (existingTagAlong != null)
        {
            return;
        }

        GameObject instantiatedObjectToTagAlong = GameObject.Instantiate(ObjectToTagAlong);

        instantiatedObjectToTagAlong.SetActive(true);

        // AddComponent Billboard to instantiatedObjectToTagAlong.
        // So it's always facing the user as they move.
        instantiatedObjectToTagAlong.AddComponent<Billboard>();

        // AddComponent SimpleTagalong to instantiatedObjectToTagAlong.
        // So it's always following the user as they move.
        instantiatedObjectToTagAlong.AddComponent<SimpleTagalong>();

        // Set any public properties you wish to experiment with.
    }
}