using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public static int SelectedTileIndex { get; set; }

    public Vector2 mapSize = new Vector2(20, 10);
    public Texture2D texture2D; // 타일 전체 이미지
    public Vector2 tileSize; // 타일 하나의 사이즈
    public Vector2 tileOffset;
    public Vector2 tilePadding { get; set; } = new Vector2(2f,2f);
    public Sprite[] sprites; // slice된 타일 하나씩 배열로 추가
    public Vector2 gridSize; // 전체 사이즈
    public int pixelToUnits;

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
