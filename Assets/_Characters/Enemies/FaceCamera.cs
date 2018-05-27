using UnityEngine;

namespace RPG.Characters
{
    public class FaceCamera : MonoBehaviour
    {
        Camera cameraToLookAt;

        void Start()
        {
            cameraToLookAt = Camera.main;
        }

        // LateUpdate is called after all Update functions have been called
        void LateUpdate()
        {
            transform.LookAt(cameraToLookAt.transform);
        }
    }
}
