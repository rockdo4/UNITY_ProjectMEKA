using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum tileType // 타일 sprite와 타일 3d 객체 1:1 매칭
{
    None,
    LowFloor,
    HighFloor,
    LowGate,
    HighGate,
    House,
    Obstacle // 세분화 예정
}

public class TileMap : MonoBehaviour
{
    // 타일 윈도우
    public static int SelectedTileIndex { get; set; }
    public Vector2 mapSize = new Vector2(8, 5);
    public Texture2D texture2D; // 타일윈도우용 전체 이미지
    //public Sprite[] sprites; // slice된 타일 하나씩 배열로 추가
    public Dictionary<tileType, Sprite> sprites = new Dictionary<tileType, Sprite>();
    public Vector2 tileSize; // 타일 하나의 사이즈 for scaleUp
    public int pixelToUnits;
    public Vector2 tilePadding { get; set; } = new Vector2(2f,2f);

    // 타일맵
    public static tileType selectedTileType;
    public Vector2 gridSize; // 전체 사이즈
    public Dictionary<tileType, GameObject> tiles = new Dictionary<tileType, GameObject>();


    private void OnDrawGizmosSelected()
    {
        var pos = transform.position;
        var center = new Vector2(pos.x + gridSize.x * 0.5f, pos.y - gridSize.y * 0.5f);
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(center, gridSize);

        Gizmos.color = Color.cyan;

        for (int i = 1; i < mapSize.x; ++i)
        {
            var x = pos.x + tileSize.x * i;
            Gizmos.DrawLine(new Vector2(x, pos.y), new Vector2(x, pos.y - gridSize.y));
        }
        for (int i = 1; i < mapSize.y; ++i)
        {
            var y = pos.y - tileSize.y * i;
            Gizmos.DrawLine(new Vector2(pos.x, y), new Vector2(pos.x + gridSize.x, y));
        }
    }

    public static string GetTileId(int r, int c)
    {
        return $"Tile (${r}, {c})";
    }
}
