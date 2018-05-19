using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkMoveStopRadius = 1.5f;

    ThirdPersonCharacter thirdPersonCharachter;
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;
    bool isInKeyboardMovementMode = false;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharachter = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.K)) // TODO add to controls menu
        {
            isInKeyboardMovementMode = !isInKeyboardMovementMode;
            currentClickTarget = transform.position; // clear current click target
        }

        if (isInKeyboardMovementMode)
        {
            ProcessKeyboardMovement();
        }
        else
        {
            ProcessMouseMovement();
        }
    }

    private void ProcessKeyboardMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // calculate camera relative direction to move:
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

        thirdPersonCharachter.Move(movement, false, false);
    }

    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0)) // Left mouse button
        {
            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Walkable:
                    currentClickTarget = cameraRaycaster.hit.point;
                    // If hold down button to move
                    //m_Character.Move(currentClickTarget - transform.position, false, false);
                    break;
                case Layer.Enemy:
                    break;
                case Layer.RaycastEndStop:
                    break;
                default:
                    break;
            }
        }

        // Prevent player animation from twitching when reaching the target
        var playertoClickPoint = currentClickTarget - transform.position;
        if (playertoClickPoint.magnitude >= walkMoveStopRadius)
        {
            thirdPersonCharachter.Move(playertoClickPoint, false, false);
        }
        else
        {
            thirdPersonCharachter.Move(Vector3.zero, false, false);
        }
    }
}

