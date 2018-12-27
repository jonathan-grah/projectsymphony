using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    void Update()
    {
        float angle = gameObject.transform.parent.transform.rotation.eulerAngles.y;
        if (angle > 0 && angle <= 90) // right
        {
            GetComponent<SpriteRenderer>().flipY = false;
            GetComponent<SpriteRenderer>().flipX = false;
        } else if (angle > 90 && angle <= 180) // down
        {
            GetComponent<SpriteRenderer>().flipX = false;
            GetComponent<SpriteRenderer>().flipY = true;
        } else if (angle > 180 && angle <= 270) // left
        {
            GetComponent<SpriteRenderer>().flipX = true;
        } else if (angle > 270 && angle <= 360) // up
        {
            GetComponent<SpriteRenderer>().flipY = false;
        }
    }
}
