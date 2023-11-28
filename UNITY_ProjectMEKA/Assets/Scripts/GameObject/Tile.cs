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
    private MeshRenderer meshRenderer;
    [HideInInspector]
    public float height;
    private BoxCollider boxCollider;
    public bool arrangePossible;
    public bool attackPossible;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        baseMaterial = GetComponent<MeshRenderer>().material;
        boxCollider = GetComponent<BoxCollider>();
        height = boxCollider.bounds.size.y;
    }

    public void SetTileMaterial(bool isPossible, TileMaterial materialType)
    {
        if (isPossible && (materialType == TileMaterial.Arrange))
        {
            Material[] materials = new Material[] { baseMaterial, arrangePossibleMaterial }; // 두 개의 material을 배열로 만듭니다.
            meshRenderer.materials = materials; // 두 개의 material을 오브젝트에 적용합니다.
        }
        else if(isPossible && (materialType == TileMaterial.Attack))
        {
            Material[] materials = new Material[] { baseMaterial, attackPossibleMaterial }; // 두 개의 material을 배열로 만듭니다.
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
