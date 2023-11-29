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
            Material[] materials = new Material[] { baseMaterial, arrangePossibleMaterial }; // �� ���� material�� �迭�� ����ϴ�.
            meshRenderer.materials = materials; // �� ���� material�� ������Ʈ�� �����մϴ�.
        }
        else if(isPossible && (materialType == TileMaterial.Attack))
        {
            Material[] materials = new Material[] { baseMaterial, attackPossibleMaterial }; // �� ���� material�� �迭�� ����ϴ�.
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
