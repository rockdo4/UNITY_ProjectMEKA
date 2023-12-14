using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class Tile : MonoBehaviour
{
    public enum TileMaterial
    {
        None,
        Arrange,
        Attack
    }

    public TileType tileType = TileType.None;
    public Material arrangePossibleMaterial;
    public Material attackPossibleMaterial;
    private Material baseMaterial;
    public MeshRenderer meshRenderer;
    [HideInInspector]
    public float height;
    private BoxCollider boxCollider;
    public bool arrangePossible = true;
    public bool attackPossible;
    public bool isSomthingOnTile;

    private void Awake()
    {
        if(tileType == TileType.Obstacle)
        {
            arrangePossible = false;
        }

        meshRenderer = GetComponent<MeshRenderer>();
        baseMaterial = GetComponent<MeshRenderer>().material;
        boxCollider = GetComponent<BoxCollider>();
        height = boxCollider.bounds.size.y;
        arrangePossible = true;

        RaycastHit hit;
        var tempPos = new Vector3(transform.parent.position.x, 100f, transform.parent.position.z);
        if (Physics.Raycast(tempPos, Vector3.down, out hit))
        {
            if (hit.transform.gameObject.layer != gameObject.layer)
            {
                arrangePossible = false;
                isSomthingOnTile = true;
            }
        }
    }

    private void Update()
    {
        if (arrangePossible && tileType == TileType.Obstacle)
        {
            arrangePossible = false;
        }
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
