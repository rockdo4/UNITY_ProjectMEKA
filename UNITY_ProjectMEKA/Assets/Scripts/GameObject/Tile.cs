using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Material overlayMaterial;
    private Material baseMaterial;
    private MeshRenderer meshRenderer;
    //private BoxCollider boxCollider;
    //private float offset;

    private bool arrangePossible;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        baseMaterial = GetComponent<Material>();
        //offset = transform.parent.parent.localPosition.y * transform.parent.localScale.y;
        //Debug.Log(offset);
    }

    //public void CheckTileArrangePossible()
    //{
    //    Vector3 boxSize = Vector3.Scale(transform.parent.localScale, boxCollider.size); // 상자의 크기
    //    float boxcastHeight = transform.position.y + boxSize.y * 1.5f - offset; // 상자를 생성하는 높이

    //    RaycastHit hit;
    //    if (!Physics.BoxCast(transform.position + Vector3.up * boxcastHeight, boxSize, Vector3.down, out hit, Quaternion.identity, boxSize.y))
    //    {
    //        arrangePossible = true;
    //    }
    //}

    //void OnDrawGizmos()
    //{
    //    boxCollider = GetComponent<BoxCollider>();
    //    offset = transform.parent.parent.localPosition.y * transform.parent.localScale.y;
    //    Vector3 boxSize = Vector3.Scale(transform.parent.localScale, boxCollider.size); ; // 상자의 크기
    //    float boxcastHeight = transform.position.y + boxSize.y * 1.5f + offset; // 상자를 생성하는 높이

    //    // Gizmo 색상 설정
    //    Gizmos.color = Color.red;

    //    // Gizmo를 BoxCast 상자의 형태로 그림
    //    Gizmos.DrawWireCube(transform.position + Vector3.up * boxcastHeight - Vector3.up * boxSize.y / 2f, boxSize);
    //}

    public void SetPlacementPossible(bool isPossible)
    {
        if (isPossible)
        {
            Material[] materials = new Material[] { baseMaterial, overlayMaterial }; // 두 개의 material을 배열로 만듭니다.
            meshRenderer.materials = materials; // 두 개의 material을 오브젝트에 적용합니다.
        }
    }
}
