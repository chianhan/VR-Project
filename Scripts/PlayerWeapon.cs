using UnityEngine;
using UnityEngine.XR;

public class PlayerWeapon : MonoBehaviour
{
    public float heightOffset = 0.2f; // Height threshold for hand detection
    public Transform vrCharacterPos; // VR character position
    public Transform leftHandTransform; // Left-hand Transform assigned in the Inspector
    public Transform rightHandTransform; // Right-hand Transform assigned in the Inspector
    public GameObject prefab_Bullet; // Bullet prefab
    public Transform muzzle; // Bullet spawn point

    public bool isLeftHandReady = false; // Left-hand height state
    public bool isRightHandReady = false; // Right-hand height state

    public bool oneShoot = true; // True for single shots; false for continuous fire
    private bool onTriggerButtonClicked; // Tracks whether the trigger has been pressed
    private bool leftHandTriggerClicked; // Tracks left-hand trigger input
    private bool rightHandTriggerClicked; // Tracks right-hand trigger input
    public bool weaponCanUse; // Whether the weapon is available
    public Animator anim; // Animator reference

    public float shootRate = 0.1f; // Interval between shots in continuous-fire mode
    public float currentShootTime; // Elapsed time since the last shot

    void Update()
    {
        CheckHandHeight(leftHandTransform, "¥ª¤â", ref isLeftHandReady, XRNode.LeftHand, ref leftHandTriggerClicked);
        CheckHandHeight(rightHandTransform, "¥k¤â", ref isRightHandReady, XRNode.RightHand, ref rightHandTriggerClicked);

        // Update the trunk animation based on hand height
        if (isLeftHandReady || isRightHandReady)
        {
            if (!weaponCanUse) // Trigger the animation if the weapon is not yet enabled
            {
                anim.SetBool("NoseUp", true);
            }
        }
        else
        {
            if (weaponCanUse) // Lower the trunk if the weapon is currently enabled
            {
                anim.SetBool("NoseUp", false);
            }
        }

        // Update the firing timer
        if (!oneShoot)
        {
            if (currentShootTime < shootRate)
            { currentShootTime += Time.deltaTime; }
        }
    }
    void CheckHandHeight(Transform handTransform, string handName, ref bool isHandHigh, XRNode handNode, ref bool handTriggerClicked)
    {
        float cameraHeight = vrCharacterPos.position.y; // Get the camera height
        if (handTransform != null)
        {
            float handHeight = handTransform.position.y; // Get the hand height
            if (handHeight >= cameraHeight - heightOffset)
            {
                if (!isHandHigh) // Hand has reached the height threshold
                {
                    isHandHigh = true;
                }

                // Check trigger input while the hand is raised
                InputDevice handDevice = InputDevices.GetDeviceAtXRNode(handNode);
                bool triggerValue;
                if (handDevice.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue))
                {
                    if (oneShoot)
                    {
                        if (triggerValue)
                        {
                            if (!handTriggerClicked)
                            {
                                // Trigger was just pressed
                                Fire();
                            }
                        }
                        else
                        {
                            onTriggerButtonClicked = false;
                        }
                        handTriggerClicked = triggerValue;
                    }
                    else
                    {
                        if (triggerValue)
                        {
                            Fire();
                        }
                    }
                }
            }
            else
            {
                if (isHandHigh) // Hand has dropped below the height threshold
                {
                    isHandHigh = false;
                }
            }
        }
    }


    void Fire()
    {
        if (oneShoot)
        {
            // Fire one shot per trigger press
            if (!onTriggerButtonClicked)
            {
                ShootBullet();
                onTriggerButtonClicked = true;
            }
        }
        else
        {
            if (currentShootTime >= shootRate)
            {
                ShootBullet();
                currentShootTime = 0; // Reset the firing timer
            }
        }
    }

    public void ShootBullet()
    {
        GameObject bullet = Instantiate(prefab_Bullet, muzzle.position, muzzle.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = muzzle.forward * 20f; // Set the initial bullet velocity
    }

    public void OnNoseUp()
    {
        weaponCanUse = true;
    }

    public void OnNoseDown()
    {
        weaponCanUse = false;
    }
}
