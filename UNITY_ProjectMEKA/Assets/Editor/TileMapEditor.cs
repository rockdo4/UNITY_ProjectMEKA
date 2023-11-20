using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.Tilemaps;
using System.Drawing;

[CustomEditor(typeof(TileMap))]
public class TileMapEditor : Editor
{
    private TileMap tileMap;
    private TileBrush brush;
    private Vector3 mouseHitPos;

    public void OnEnable() // �ν����� Ȱ��ȭ
    {
        Debug.Log("OnEnable");
        tileMap = target as TileMap;
        Tools.current = Tool.View;

        //UpdateTileMap();
        CreateBrush();
    }

    public void OnDisable() // �ν����� ��Ȱ��ȭ(�ٸ� �ν�����â���� �ٲ�)
    {
        Debug.Log("OnDisable");
        tileMap = null;

        if (brush != null)
        {
            DestroyImmediate(brush.gameObject); // 
            //Destroy(brush.gameObject);
        }
    }

    public override void OnInspectorGUI() // �ν������� ������ ���ŵ� ������ ȣ��
    {
        // �⺻ ����
        // base.OnInspectorGUI();
        // DrawDefaultInspector();
        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Our custom editor"); // �б⸸ ����

        var prevMapSize = tileMap.mapSize;
        tileMap.mapSize = EditorGUILayout.Vector2Field("Map Size", tileMap.mapSize);

        var prevTexture2D = tileMap.texture2D;
        tileMap.texture2D = EditorGUILayout.ObjectField("Texture2D: ", tileMap.texture2D, typeof(Texture2D), false) as Texture2D;

        if (prevMapSize != tileMap.mapSize || prevTexture2D != tileMap.texture2D)
        {
            UpdateTileMap();
        }

        if (tileMap.texture2D == null)
        {
            EditorGUILayout.HelpBox("�ؽ��ĸ� �����ϼ���.", MessageType.Warning); // MessageType.Warning : ���� ��
        }
        else
        {
            EditorGUILayout.LabelField($"Tile Size: {tileMap.tileSize}");
            EditorGUILayout.LabelField($"Grid Size: {tileMap.gridSize}"); // ���忡���� ������
            EditorGUILayout.LabelField($"Pixel To Unit: {tileMap.pixelToUnits}");
        }

        if(GUILayout.Button("Clear Tiles"))
        {
            if(EditorUtility.DisplayDialog("Clear map's tiles?", "Are you sure?", "Clear", "Do not clear"))
            {
                ClearTiles();
            }
        }

        EditorGUILayout.EndVertical();
        EditorUtility.SetDirty(tileMap); // �ش� ���ӿ�����Ʈ�� TileMap������Ʈ�� �� ����
    }

    private void CreateBrush()
    {
        if(brush != null)
        {
            DestroyImmediate(brush.gameObject);
        }

        if(tileMap.texture2D == null)
        {
            return;
        }

        var sprite = tileMap.sprites[TileMap.SelectedTileIndex];
        if (sprite == null)
        {
            return;
        }

        var newGo = new GameObject("Brush");
        newGo.transform.SetParent(tileMap.transform);

        brush = newGo.AddComponent<TileBrush>();
        brush.spriteRenderer = newGo.AddComponent<SpriteRenderer>();
        brush.brushSize = new Vector2(sprite.textureRect.width, sprite.textureRect.height);
        brush.brushSize /= tileMap.pixelToUnits;
        brush.spriteRenderer.sortingOrder = 999;

        brush.UpdateBrush(sprite);
    }

    private void UpdateTileMap()
    {
        if (tileMap.texture2D == null)
        {
            return;
        }

        var path = AssetDatabase.GetAssetPath(tileMap.texture2D);
        //Debug.Log(path);
        var array = AssetDatabase.LoadAllAssetsAtPath(path);
        //Debug.Log(array.Length);

        tileMap.sprites = new Sprite[array.Length - 1];
        for (int i = 1; i < array.Length; i++)
        {
            tileMap.sprites[i - 1] = array[i] as Sprite;
            //Debug.Log(tileMap.sprites[i - 1].name);
        }
        var sampleSprite = tileMap.sprites[0];
        var w = sampleSprite.textureRect.width;
        var h = sampleSprite.textureRect.height;
        tileMap.tileSize = new Vector2(w, h);
        tileMap.pixelToUnits = (int)(w / sampleSprite.bounds.size.x);

        tileMap.gridSize = new Vector2(w * tileMap.mapSize.x, h * tileMap.mapSize.y);
        tileMap.gridSize /= tileMap.pixelToUnits;

        ClearTiles();

        if (brush == null)
        {
            CreateBrush();
        }

        EditorUtility.SetDirty(tileMap);
    }

    private void ClearTiles()
    {
        int count = tileMap.transform.childCount;
        for (int i = count - 1; i >= 0; --i)
        {
            // �귯���� ����� �ȵ�
            var child = tileMap.transform.GetChild(i).gameObject;
            if (child == brush.gameObject)
            {
                continue;
            }
            DestroyImmediate(child);
        }
    }

    private void OnSceneGUI() // UPDATE �Լ�ó�� ��
    {
        if (brush != null)
        {
            var sprite = tileMap.sprites[TileMap.SelectedTileIndex];
            if (brush.spriteRenderer.sprite != sprite)
            {
                brush.UpdateBrush(sprite);
            }

            UpdateHitPosition();
            MoveBrush();

            if(Event.current.shift)
            {
                Draw();
            }

            if(Event.current.control)
            {
                Eraise();
            }
        }
    }

    private void UpdateHitPosition()
    {
        var mousePos = Event.current.mousePosition;
        var ray = HandleUtility.GUIPointToWorldRay(mousePos);
        var p = new Plane(tileMap.transform.TransformDirection(Vector3.forward), Vector3.zero); // ��� ����

        var hit = Vector3.zero;
        if (p.Raycast(ray, out float distance))
        {
            hit = ray.origin + ray.direction.normalized * distance;
        }
        mouseHitPos = tileMap.transform.InverseTransformPoint(hit); // ���� -> ����
    }

    private void MoveBrush()
    {
        var col = Mathf.FloorToInt(mouseHitPos.x / (int)tileMap.tileSize.x);
        var row = - Mathf.FloorToInt(mouseHitPos.y / (int)tileMap.tileSize.y) - 1;

        col = Mathf.Clamp(col, 0, (int)tileMap.mapSize.x - 1);
        row = Mathf.Clamp(row, 0, (int)tileMap.mapSize.y - 1);

        brush.gridIndex.x = col;
        brush.gridIndex.y = row;

        var x = col * tileMap.tileSize.x;
        var y = -(row + 1) * tileMap.tileSize.y;
        x += tileMap.tileSize.x * 0.5f;
        y += tileMap.tileSize.y * 0.5f;

        brush.transform.localPosition = new Vector3(x, y, tileMap.transform.position.z);
    }

    public void Draw()
    {
        if(brush == null)
        {
            return;
        }

        // Ÿ���� �̸��� ��Ģ�� �ְ� ��� �̸����� ã�ƿ���
        GameObject tileGo = null;
        var tileId = TileMap.GetTileId(brush.gridIndex.y, brush.gridIndex.x);
        var tileTr = tileMap.transform.Find(tileId); // �ڽĿ�����Ʈ���� �� �̸��� ���� ���ӿ�����Ʈ ����
        if(tileTr != null)
        {
            tileGo = tileTr.gameObject;
        }

        if(tileGo == null)
        {
            tileGo = new GameObject(tileId);
            tileGo.transform.SetParent(tileMap.transform);
            tileGo.transform.position = brush.transform.position;
            tileGo.AddComponent<SpriteRenderer>();
        }

        var ren = tileGo.GetComponent<SpriteRenderer>();
        ren.sprite = brush.spriteRenderer.sprite;
        EditorUtility.SetDirty(tileMap);
    }

    public void Eraise()
    {
        var tileId = TileMap.GetTileId(brush.gridIndex.y, brush.gridIndex.x);
        var tileTr = tileMap.transform.Find(tileId); // �ڽĿ�����Ʈ���� �� �̸��� ���� ���ӿ�����Ʈ ����
        if (tileTr != null)
        {
            DestroyImmediate(tileTr.gameObject);
        }
        EditorUtility.SetDirty(tileMap);
    }
}
