using UnityEngine;

namespace RPG.CameraUI
{
    public class CursorAffordance : MonoBehaviour
    {
        [SerializeField] Texture2D uiCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        void Start()
        {
            Cursor.SetCursor(uiCursor, cursorHotspot, CursorMode.Auto);
        }
    }
}
