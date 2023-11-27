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
    //    Vector3 boxSize = Vector3.Scale(transform.parent.localScale, boxCollider.size); // ������ ũ��
    //    float boxcastHeight = transform.position.y + boxSize.y * 1.5f - offset; // ���ڸ� �����ϴ� ����

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
    //    Vector3 boxSize = Vector3.Scale(transform.parent.localScale, boxCollider.size); ; // ������ ũ��
    //    float boxcastHeight = transform.position.y + boxSize.y * 1.5f + offset; // ���ڸ� �����ϴ� ����

    //    // Gizmo ���� ����
    //    Gizmos.color = Color.red;

    //    // Gizmo�� BoxCast ������ ���·� �׸�
    //    Gizmos.DrawWireCube(transform.position + Vector3.up * boxcastHeight - Vector3.up * boxSize.y / 2f, boxSize);
    //}

    public void SetPlacementPossible(bool isPossible)
    {
        if (isPossible)
        {
            Material[] materials = new Material[] { baseMaterial, overlayMaterial }; // �� ���� material�� �迭�� ����ϴ�.
            meshRenderer.materials = materials; // �� ���� material�� ������Ʈ�� �����մϴ�.
        }
        else
        {
            Material[] materials = meshRenderer.materials; // ���� ���� �迭�� �����ɴϴ�.
            List<Material> materialList = new List<Material>(materials); // �迭�� ����Ʈ�� ��ȯ�մϴ�.
            materialList.RemoveAt(1); // ����Ʈ���� �ش� �ε����� ������ �����մϴ�.
            meshRenderer.materials = materialList.ToArray(); // ����Ʈ�� �迭�� ��ȯ�Ͽ� �ٽ� �Ҵ��մϴ�.
        }
        arrangePossible = isPossible;
    }
}
