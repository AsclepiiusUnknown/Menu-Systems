using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairs : MonoBehaviour
{
    public LayerMask whatIsTarget;
    public SpriteRenderer dot;
    public Color dotHighlightColor;
    Color originalColor;

    private void Start()
    {
        originalColor = dot.color;
        Cursor.visible = false;
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * 40 * Time.deltaTime);
    }

    public void DetectTargets(Ray ray)
    {
        if (Physics.Raycast(ray, 100, whatIsTarget))
        {
            dot.color = dotHighlightColor;
        }
        else
        {
            dot.color = originalColor;
        }
    }
}
