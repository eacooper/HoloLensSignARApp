using HoloToolkit;
using UnityEngine;

/// <summary>
/// GazeManager determines the location of the user's gaze, hit position and normals.
/// </summary>
public class GazeManager : Singleton<GazeManager>
{
    [Tooltip("Maximum gaze distance for calculating a hit.")]
    public float MaxGazeDistance = 5.0f;

    [Tooltip("Select the layers raycast should target first.")]
    public LayerMask PrimaryRaycastLayerMask = Physics.DefaultRaycastLayers;

    [Tooltip("Select the layers raycast should target second.")]
    public LayerMask SecondaryRaycastLayerMask = Physics.DefaultRaycastLayers;

    [Tooltip("Reports the position of the ray cast")]
    public Vector3 PosRay;

    [Tooltip("Reports the normal of the ray cast")]
    public Vector3 NormRay;

    /// <summary>
    /// Physics.Raycast result is true if it hits a Hologram.
    /// </summary>
    public bool Hit { get; private set; }

    /// <summary>
    /// HitInfo property gives access
    /// to RaycastHit public members.
    /// </summary>
    public RaycastHit HitInfo { get; private set; }

    /// <summary>
    /// Position of the user's gaze.
    /// </summary>
    public Vector3 Position { get; private set; }

    /// <summary>
    /// RaycastHit Normal direction.
    /// </summary>
    public Vector3 Normal { get; private set; }

    private GazeStabilizer gazeStabilizer;
    private Vector3 gazeOrigin;
    private Vector3 gazeDirection;

    void Awake()
    {
        // GetComponent GazeStabilizer and assign it to gazeStabilizer.
        gazeStabilizer = GetComponent<GazeStabilizer>();
    }

    private void Update()
    {
        // Assign Camera's main transform position to gazeOrigin.
        gazeOrigin = Camera.main.transform.position;

        // Assign Camera's main transform forward to gazeDirection.
        gazeDirection = Camera.main.transform.forward;

        // Using gazeStabilizer, call function UpdateHeadStability.
        // Pass in gazeOrigin and Camera's main transform rotation.
        gazeStabilizer.UpdateHeadStability(gazeOrigin, Camera.main.transform.rotation);

        // Using gazeStabilizer, get the StableHeadPosition and
        // assign it to gazeOrigin.
        gazeOrigin = gazeStabilizer.StableHeadPosition;

        UpdateRaycast();
    }

    /// <summary>
    /// Calculates the Raycast hit position and normal.
    /// </summary>
    private void UpdateRaycast()
    {
        if (HitLayer(PrimaryRaycastLayerMask))
        {
            // If raycast hit an icon...
            // Assign property Position to be the hitInfo point.
            Position = HitInfo.point;
            // Assign property Normal to be the hitInfo normal.
            Normal = HitInfo.normal;
        } 
        else if (HitLayer(SecondaryRaycastLayerMask))
        {
            // If raycast hit a spatial mapping...
            // Assign property Position to be the hitInfo point.
            Position = HitInfo.point;
            // Assign property Normal to be the hitInfo normal.
            Normal = HitInfo.normal;
        }
        else
        {
            // Assign Position to be gazeOrigin plus MaxGazeDistance times gazeDirection.
            Position = gazeOrigin + (gazeDirection * MaxGazeDistance);
            // Assign Normal to be the user's gazeDirection.
            Normal = gazeDirection;
        }

        PosRay = Position;
        NormRay = Normal;
    }

    private bool HitLayer(LayerMask layerToHit)
    {
        // Create a variable hitInfo of type RaycastHit.
        RaycastHit hitInfo;

        // Perform a Unity Physics Raycast.
        // Collect return value in public property Hit.
        // Pass in origin as gazeOrigin and direction as gazeDirection.
        // Collect the information in hitInfo.
        // Pass in MaxGazeDistance and RaycastLayerMask.
        Hit = Physics.Raycast(gazeOrigin,
                       gazeDirection,
                       out hitInfo,
                       MaxGazeDistance,
                       layerToHit);

        // Assign hitInfo variable to the HitInfo public property 
        // so other classes can access it.
        HitInfo = hitInfo;

        return Hit;
    }
}