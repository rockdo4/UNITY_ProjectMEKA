using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NewTileMapMenu
{
    [MenuItem("GameObject/Tile Map")] // 이 메뉴를 클릭했을 때 아래의 static 함수 호출
    public static void CreateTileMap()
    {
        var go = new GameObject("Tile Map"); // 생성되는 게임오브젝트의 이름
        go.AddComponent<TileMap>();
    }
}
