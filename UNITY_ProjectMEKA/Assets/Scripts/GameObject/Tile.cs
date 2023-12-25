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
        Skill,
        UnActive
    }

    public TileType tileType = TileType.None;
    public TileMaterial currentTileMaterial = TileMaterial.None;
    public Material arrangePossibleMaterial;
    public Material attackPossibleMaterial;
    public Material skillAttackPossibleMaterial;
    public Material unActiveMaterial;
    private Material baseMaterial;
    public MeshRenderer meshRenderer;
    [HideInInspector]
    public float height;
    private BoxCollider boxCollider;
    public bool arrangePossible = true;
    public bool attackPossible;
    public bool isSomthingOnTile; // for check attackable tile
    public Vector3Int index;
    public List<GameObject> objectsOnTile = new List<GameObject>(); // have to change to LinkedList

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
        var tileColliderMask = 1 << LayerMask.NameToLayer(Layers.onTile);
        var tileMask = gameObject.layer;
        var houseMask = LayerMask.NameToLayer(Layers.house);
        var gateMask = LayerMask.NameToLayer(Layers.gate);
        var layerMask = ~tileColliderMask;
        if (Physics.Raycast(tempPos, Vector3.down, out hit, 100f, layerMask))
        {
            var hitLayer = hit.transform.gameObject.layer;
            var isGateOrHouse = hitLayer == houseMask || hitLayer == gateMask;
            var isSelf = hitLayer == tileMask;

            if (!isGateOrHouse && !isSelf) // when highTile is on this tile
            {
                arrangePossible = false;
                isSomthingOnTile = true;
            }
            else if(isGateOrHouse) // when gate or house is on this tile
            {
                arrangePossible = false;
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
        currentTileMaterial = materialType;
        Material[] materials = new Material[2];
        switch (materialType)
        {
            case TileMaterial.Arrange:
                materials = new Material[] { baseMaterial, arrangePossibleMaterial }; // 두 개의 material을 배열로 만듭니다.
                break;
            case TileMaterial.Attack:
                materials = new Material[] { baseMaterial, attackPossibleMaterial }; // 두 개의 material을 배열로 만듭니다.
                break;
            case TileMaterial.Skill:
                materials = new Material[] { baseMaterial, skillAttackPossibleMaterial }; // 두 개의 material을 배열로 만듭니다.
                break;
            case TileMaterial.UnActive:
                materials = new Material[] { baseMaterial, unActiveMaterial };
                break;
        }
        meshRenderer.materials = materials;
    }

    public void ClearTileMesh()
    {
        currentTileMaterial = TileMaterial.None;
        Material[] materials = meshRenderer.materials; // 기존 재질 배열을 가져옵니다.
        List<Material> materialList = new List<Material>(materials); // 배열을 리스트로 변환합니다.
        if (materialList.Count > 1)
        {
            materialList.RemoveAt(1); // 리스트에서 해당 인덱스의 재질을 제거합니다.
        }
        meshRenderer.materials = materialList.ToArray(); // 리스트를 배열로 변환하여 다시 할당합니다.
    }
}
