using UnityEngine;

[ExecuteAlways] 
public class CameraFollow : MonoBehaviour
{
    [Header("Target to Follow")]
    public Transform target;

    [Header("Map Boundaries")]
    public float minX = -11f;
    public float maxX = 11f;
    public float minY = -10.5f;
    public float maxY = 7f;

    [Header("Camera Settings")]
    [Range(0f, 1f)]
    public float smoothSpeed = 0.125f; 

    private float cameraHalfWidth;
    private float cameraHalfHeight;
    private Camera cam;

    void Awake()
    {
        
        cam = GetComponent<Camera>();

        if (cam == null)
        {
            Debug.LogError("CameraFollow script requires a Camera component on the same GameObject.");
        }

        if (!cam.orthographic)
        {
            Debug.LogWarning("CameraFollow script is designed for Orthographic cameras.");
        }

        CalculateCameraDimensions();
    }

    void LateUpdate()
    {
        if (cam == null)
            return;

        if (target == null)
        {
            Debug.LogWarning("CameraFollow script requires a target to follow.");
            return;
        }

        
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

       
        float clampedX = Mathf.Clamp(desiredPosition.x, minX + cameraHalfWidth, maxX - cameraHalfWidth);
        float clampedY = Mathf.Clamp(desiredPosition.y, minY + cameraHalfHeight, maxY - cameraHalfHeight);

        Vector3 clampedPosition = new Vector3(clampedX, clampedY, desiredPosition.z);

        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }

    
    void CalculateCameraDimensions()
    {
        if (cam == null)
            return;

        if (cam.orthographic)
        {
            cameraHalfHeight = cam.orthographicSize;
            cameraHalfWidth = cameraHalfHeight * cam.aspect;
        }
        else
        {
            
            cameraHalfHeight = cam.orthographicSize; 
            cameraHalfWidth = cameraHalfHeight * cam.aspect; 
        }
    }

    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 topLeft = new Vector3(minX, maxY, transform.position.z);
        Vector3 topRight = new Vector3(maxX, maxY, transform.position.z);
        Vector3 bottomLeft = new Vector3(minX, minY, transform.position.z);
        Vector3 bottomRight = new Vector3(maxX, minY, transform.position.z);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }

    
    void OnValidate()
    {
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }

        CalculateCameraDimensions();
    }
}
