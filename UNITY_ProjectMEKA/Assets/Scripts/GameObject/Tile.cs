using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Material overlayMaterial;
    private Material baseMaterial;
    private MeshRenderer meshRenderer;
    public float height;
    private BoxCollider boxCollider;
    //private float offset;

    public bool arrangePossible;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        baseMaterial = GetComponent<MeshRenderer>().material;
        boxCollider = GetComponent<BoxCollider>();
        height = boxCollider.bounds.size.y;
        Debug.Log(height);
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
        else
        {
            Material[] materials = meshRenderer.materials; // 기존 재질 배열을 가져옵니다.
            List<Material> materialList = new List<Material>(materials); // 배열을 리스트로 변환합니다.
            materialList.RemoveAt(1); // 리스트에서 해당 인덱스의 재질을 제거합니다.
            meshRenderer.materials = materialList.ToArray(); // 리스트를 배열로 변환하여 다시 할당합니다.
        }
        arrangePossible = isPossible;
    }
}
