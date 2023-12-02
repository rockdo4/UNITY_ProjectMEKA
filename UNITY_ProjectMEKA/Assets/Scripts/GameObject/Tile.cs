using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileMaterial
    {
        None,
        Arrange,
        Attack
    }

    public Material arrangePossibleMaterial;
    public Material attackPossibleMaterial;
    private Material baseMaterial;
    public MeshRenderer meshRenderer;
    [HideInInspector]
    public float height;
    private BoxCollider boxCollider;
    public bool arrangePossible = true;
    public bool attackPossible;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        baseMaterial = GetComponent<MeshRenderer>().material;
        boxCollider = GetComponent<BoxCollider>();
        height = boxCollider.bounds.size.y;
        arrangePossible = true;

        // �̶� ���� ���� ���� �� ������ POSSIBLE FALSE
        RaycastHit hit;
        var tempPos = new Vector3(transform.parent.position.x, 100f, transform.parent.position.z);
        if (Physics.Raycast(tempPos, Vector3.down, out hit))
        {
            //Debug.Log($"{hit.transform.gameObject.name}�� ����");
            if (hit.transform.gameObject.layer != gameObject.layer)
            {
                arrangePossible = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        RaycastHit hit;
        var tempPos = new Vector3(transform.parent.position.x, 100f, transform.parent.position.z);
        if (Physics.Raycast(tempPos, Vector3.down, out hit))
        {
            //if (hit.transform.gameObject.layer != gameObject.layer)
            //{
            //    Debug.Log($"���� {hit} ����");
            //    arrangePossible = false;
            //}
        }
        Gizmos.DrawLine(tempPos, tempPos + Vector3.down);
    }

    public void SetTileMaterial(TileMaterial materialType)
    {
       if (materialType == TileMaterial.Arrange)
        {
            Material[] materials = new Material[] { baseMaterial, arrangePossibleMaterial }; // �� ���� material�� �迭�� ����ϴ�.
            meshRenderer.materials = materials; // �� ���� material�� ������Ʈ�� �����մϴ�.
        }
       if (materialType == TileMaterial.Attack)
        {
            Material[] materials = new Material[] { baseMaterial, attackPossibleMaterial }; // �� ���� material�� �迭�� ����ϴ�.
            meshRenderer.materials = materials; // �� ���� material�� ������Ʈ�� �����մϴ�.
        }
    }

    public void ClearTileMesh()
    {
        Material[] materials = meshRenderer.materials; // ���� ���� �迭�� �����ɴϴ�.
        List<Material> materialList = new List<Material>(materials); // �迭�� ����Ʈ�� ��ȯ�մϴ�.
        if (materialList.Count > 1)
        {
            materialList.RemoveAt(1); // ����Ʈ���� �ش� �ε����� ������ �����մϴ�.
        }
        meshRenderer.materials = materialList.ToArray(); // ����Ʈ�� �迭�� ��ȯ�Ͽ� �ٽ� �Ҵ��մϴ�.
    }
}
