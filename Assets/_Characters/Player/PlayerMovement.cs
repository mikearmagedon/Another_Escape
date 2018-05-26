using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace RPG.Characters
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {
        ThirdPersonCharacter thirdPersonCharachter;

        private void Start()
        {
            thirdPersonCharachter = GetComponent<ThirdPersonCharacter>();
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            ProcessKeyboardMovement();
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
    }
}
