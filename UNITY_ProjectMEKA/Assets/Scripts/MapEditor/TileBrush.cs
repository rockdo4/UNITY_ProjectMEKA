using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBrush : MonoBehaviour
{
    public Vector2 brushSize = Vector2.zero;
    //public SpriteRenderer spriteRenderer;
    public Vector2Int gridIndex;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f,0f,0.5f,0.5f);
        Gizmos.DrawCube(transform.position, brushSize);
    }
    
    //public void UpdateBrush(Sprite sprite)
    //{
    //    spriteRenderer.sprite = sprite;
    //}
}
