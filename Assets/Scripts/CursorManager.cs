using HoloToolkit;
using UnityEngine;

/// <summary>
/// CursorManager class takes Cursor GameObjects.
/// One that is on Holograms and another off Holograms.
/// Shows the appropriate Cursor when a Hologram is hit.
/// Places the appropriate Cursor at the hit position.
/// Matches the Cursor normal to the hit surface.
/// </summary>
public class CursorManager : Singleton<CursorManager>
{
    [Tooltip("Drag the Cursor object to show when it hits a hologram.")]
    public GameObject CursorOnHolograms;

    [Tooltip("Drag the Cursor object to show when it does not hit a hologram.")]
    public GameObject CursorOffHolograms;

    void Awake()
    {
        if (CursorOnHolograms == null || CursorOffHolograms == null)
        {
            return;
        }

        // Hide the Cursors to begin with.
        CursorOnHolograms.SetActive(false);
        CursorOffHolograms.SetActive(false);
    }

    void Update()
    {
        if (GazeManager.Instance == null || CursorOnHolograms == null || CursorOffHolograms == null)
        {
            return;
        }

        bool isGazeNull = !GazeManager.Instance.Hit || GazeManager.Instance.HitInfo.transform == null || GazeManager.Instance.HitInfo.transform.gameObject == null;
        if (!isGazeNull &&  GazeManager.Instance.HitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Holograms"))
        { 
            // SetActive true the CursorOnHolograms to show cursor.
            CursorOnHolograms.SetActive(true);
            // SetActive false the CursorOffHolograms hide cursor.
            CursorOffHolograms.SetActive(false);
        }
        else
        {
            // SetActive true CursorOffHolograms to show cursor.
            CursorOffHolograms.SetActive(true);
            // SetActive false CursorOnHolograms to hide cursor.
            CursorOnHolograms.SetActive(false);
        }

        // Assign gameObject's transform position equals GazeManager's instance Position.
        gameObject.transform.position = GazeManager.Instance.Position;

        // Assign gameObject's transform up vector equals GazeManager's instance Normal.
        gameObject.transform.up = GazeManager.Instance.Normal;
    }
}