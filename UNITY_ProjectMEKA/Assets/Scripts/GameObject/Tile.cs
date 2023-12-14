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
            Material[] materials = new Material[] { baseMaterial, arrangePossibleMaterial }; // 두 개의 material을 배열로 만듭니다.
            meshRenderer.materials = materials; // 두 개의 material을 오브젝트에 적용합니다.
        }
       if (materialType == TileMaterial.Attack)
        {
            Material[] materials = new Material[] { baseMaterial, attackPossibleMaterial }; // 두 개의 material을 배열로 만듭니다.
            meshRenderer.materials = materials; // 두 개의 material을 오브젝트에 적용합니다.
        }
    }

    public void ClearTileMesh()
    {
        Material[] materials = meshRenderer.materials; // 기존 재질 배열을 가져옵니다.
        List<Material> materialList = new List<Material>(materials); // 배열을 리스트로 변환합니다.
        if (materialList.Count > 1)
        {
            materialList.RemoveAt(1); // 리스트에서 해당 인덱스의 재질을 제거합니다.
        }
        meshRenderer.materials = materialList.ToArray(); // 리스트를 배열로 변환하여 다시 할당합니다.
    }
}
