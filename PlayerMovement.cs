using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2.0f;   // Movement speed
    public float turnSpeed = 100.0f; // Turning speed
    public Transform cameraTransform; // Camera Transform reference

    public Transform playerModel;   // Elephant model
    public Vector3 modelOffset;

    void Update()
    {
        // Read the left controller thumbstick
        Vector2 leftThumbstick;
        bool leftThumbstickFound = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primary2DAxis, out leftThumbstick);

        // Read the right controller thumbstick
        Vector2 rightThumbstick;
        bool rightThumbstickFound = InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primary2DAxis, out rightThumbstick);

        if (leftThumbstickFound)
        {
            // Move using the left thumbstick axes
            Vector3 direction = transform.forward * leftThumbstick.y + transform.right * leftThumbstick.x;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        if (rightThumbstickFound)
        {
            // Rotate using the right thumbstick horizontal axis
            float rotation = rightThumbstick.x * turnSpeed * Time.deltaTime;
            transform.Rotate(0, rotation, 0);
        }

        // Update the player model position and rotation
        playerModel.position = cameraTransform.position + cameraTransform.rotation * modelOffset;
        playerModel.rotation = cameraTransform.rotation;
    }
}
