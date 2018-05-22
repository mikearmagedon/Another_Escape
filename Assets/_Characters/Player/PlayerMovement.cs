using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using RPG.CameraUI; // TODO consider re-wiring

namespace RPG.Characters
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {
        const int walkableLayerNumber = 8;
        const int enemyLayerNumber = 9;

        ThirdPersonCharacter thirdPersonCharachter;
        CameraRaycaster cameraRaycaster;
        //Vector3 currentClickTarget;

        private void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            thirdPersonCharachter = GetComponent<ThirdPersonCharacter>();
            //currentClickTarget = transform.position;

            cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
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

        private void ProcessMouseClick(RaycastHit raycastHit, int layerHit)
        {
            switch (layerHit)
            {
                case walkableLayerNumber:
                    break;
                case enemyLayerNumber:
                    //currentClickTarget = raycastHit.point;
                    break;
                default:
                    Debug.LogWarning("Don't know how to handle mouse click for player movement");
                    break;
            }
        }
    }
}
