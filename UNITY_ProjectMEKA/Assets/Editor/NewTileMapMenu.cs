using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NewTileMapMenu
{
    [MenuItem("GameObject/Tile Map")] // �� �޴��� Ŭ������ �� �Ʒ��� static �Լ� ȣ��
    public static void CreateTileMap()
    {
        var go = new GameObject("Tile Map"); // �����Ǵ� ���ӿ�����Ʈ�� �̸�
        go.AddComponent<TileMap>();
    }
}
