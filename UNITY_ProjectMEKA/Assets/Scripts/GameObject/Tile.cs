using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;
using static Defines;

public class Tile : MonoBehaviour
{
    public enum TileMaterial
    {
        None,
        Arrange,
        Attack,
        Skill
    }

    public TileType tileType = TileType.None;
    public Material arrangePossibleMaterial;
    public Material attackPossibleMaterial;
    public Material skillAttackPossibleMaterial;
    private Material baseMaterial;
    public MeshRenderer meshRenderer;
    [HideInInspector]
    public float height;
    private BoxCollider boxCollider;
    public bool arrangePossible = true;
    public bool attackPossible;
    public bool isSomthingOnTile;
    public Vector3Int index;

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
            var hitLayer = hit.transform.gameObject.layer;
            var isSameLayer = hitLayer == gameObject.layer;

            var houseMask = LayerMask.NameToLayer(Layers.house);
            var gateMask = LayerMask.NameToLayer(Layers.gate);
            var isGateOrHouse = hitLayer == houseMask || hitLayer == gateMask;
            if (!isSameLayer && !isGateOrHouse)
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
        Material[] materials = new Material[2];
        switch (materialType)
        {
            case TileMaterial.Arrange:
                materials = new Material[] { baseMaterial, arrangePossibleMaterial }; // �� ���� material�� �迭�� ����ϴ�.
                break;
            case TileMaterial.Attack:
                materials = new Material[] { baseMaterial, attackPossibleMaterial }; // �� ���� material�� �迭�� ����ϴ�.
                break;
            case TileMaterial.Skill:
                materials = new Material[] { baseMaterial, skillAttackPossibleMaterial }; // �� ���� material�� �迭�� ����ϴ�.
                break;
        }
        meshRenderer.materials = materials;
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
