using UnityEngine;

namespace RPG.CameraUI
{
    [RequireComponent(typeof(CameraRaycaster))]
    public class CursorAffordance : MonoBehaviour
    {
        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D targetCursor = null;
        [SerializeField] Texture2D unknownCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        const int walkableLayerNumber = 8;
        const int enemyLayerNumber = 9;
        const int uiLayerNumber = 5;

        CameraRaycaster cameraRaycaster;

        // Use this for initialization
        void Start()
        {
            cameraRaycaster = GetComponent<CameraRaycaster>();
            cameraRaycaster.notifyLayerChangeObservers += OnLayerChanged; // registering
        }

        void OnLayerChanged(int newLayer)
        {
            switch (newLayer)
            {
                case uiLayerNumber:
                // fall through
                case walkableLayerNumber:
                    Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                    break;
                case enemyLayerNumber:
                    Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
                    break;
            }
        }
    }
}
