using System;
using UnityEngine;

namespace Resources
{
    public class MouseCursor : MonoBehaviour
    {
        private void Start()
        {
            Cursor.visible = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                Cursor.visible = false;
            CursorMoving();
        }
        private void CursorMoving()
        {
            transform.position = Input.mousePosition;
        }
    }
}
