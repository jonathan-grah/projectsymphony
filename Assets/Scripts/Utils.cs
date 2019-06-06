using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utils
{
    static Texture2D _texture;
    public static Texture2D Texture
    {
        get
        {
            if (_texture == null) // ensure only one
            {
                _texture = new Texture2D(1, 1);
                _texture.SetPixel(0, 0, Color.white);
                _texture.Apply();
            }

            return _texture;
        }
    }

    public static void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, Texture);
        GUI.color = Color.white;
    }

    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color); // top
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color); // left
        Utils.DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color); // right
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color); // bottom
    }

    public static Rect GetScreenRect(Vector3 initialPosition, Vector3 newPosition)
    {
        // move origin from bottom left to top left
        initialPosition.y = Screen.height - initialPosition.y;
        newPosition.y = Screen.height - newPosition.y;
        // corners
        var topLeft = Vector3.Min(initialPosition, newPosition);
        var bottomRight = Vector3.Max(initialPosition, newPosition);

        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    public static Bounds GetViewportBounds(Camera camera, Vector3 initialPosition, Vector3 newPosition)
    {
        var v1 = Camera.main.ScreenToViewportPoint(initialPosition);
        var v2 = Camera.main.ScreenToViewportPoint(newPosition);
        var min = Vector3.Min(v1, v2); var max = Vector3.Max(v1, v2);
        min.z = camera.nearClipPlane; max.z = camera.farClipPlane;
        var bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }

    public static bool IsPointerOverUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, hits);

        return hits.Count > 1;
    }
}
