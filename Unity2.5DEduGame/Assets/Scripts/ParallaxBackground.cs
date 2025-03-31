using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform transform;
        public float parallaxFactor;
        public Vector3 startPosition;
    }

    public ParallaxLayer[] layers;
    public Transform cameraTransform;
    public Vector3 lastCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        lastCameraPosition = cameraTransform.position;

        // Initialize layer start positions
        foreach (var layer in layers)
        {
            layer.startPosition = layer.transform.position;
        }
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        
        foreach (var layer in layers)
        {
            // Calculate parallax movement (inverse of parallax factor)
            float parallax = (1 - layer.parallaxFactor);
            Vector3 newPos = layer.startPosition + cameraTransform.position * parallax;
            
            // Apply Z offset for 2.5D depth
            newPos.z = layer.startPosition.z + (cameraTransform.position.y * 0.1f);
            
            layer.transform.position = newPos;
        }

        lastCameraPosition = cameraTransform.position;
    }

    public void ResetPositions()
    {
        foreach (var layer in layers)
        {
            layer.transform.position = layer.startPosition;
        }
        lastCameraPosition = cameraTransform.position;
    }
}