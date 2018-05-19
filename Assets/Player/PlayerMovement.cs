using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkMoveStopRadius = 0.2f;

    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;
    bool isInKeyboardMovementMode = false;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.K)) // add to controls menu
        {
            isInKeyboardMovementMode = !isInKeyboardMovementMode;
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
        Vector3 m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 m_Move = v * m_CamForward + h * Camera.main.transform.right;

        m_Character.Move(m_Move, false, false);
    }

    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0)) // Left mouse button
        {
            switch (cameraRaycaster.layerHit)
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
            m_Character.Move(playertoClickPoint, false, false);
        }
        else
        {
            m_Character.Move(Vector3.zero, false, false);
        }
    }
}

