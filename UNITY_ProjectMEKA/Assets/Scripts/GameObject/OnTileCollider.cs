using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class OnTileCollider : MonoBehaviour
{
    Tile tileController;

    private void Awake()
    {
        tileController = transform.parent.GetComponentInChildren<Tile>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���̰ų�, ĳ���͸� tileController�� targetList�� �߰�
        var isEnemyBody = other.gameObject.tag == Tags.enemyCollider;
        var isCharacterBody = other.gameObject.tag == Tags.playerCollider;
        if (isEnemyBody || isCharacterBody)
        {
            if(!tileController.objectsOnTile.Contains(other.transform.parent.gameObject))
            {
                tileController.objectsOnTile.Add(other.transform.parent.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var isEnemyBody = other.gameObject.layer == LayerMask.NameToLayer(Layers.enemyCollider);
        var isCharacterBody = other.gameObject.layer == LayerMask.NameToLayer(Layers.playerCollider);
        if (isEnemyBody || isCharacterBody)
        {
            if (tileController.objectsOnTile.Contains(other.transform.parent.gameObject))
            {
                tileController.objectsOnTile.Remove(other.transform.parent.gameObject);
            }
        }

    }
}
