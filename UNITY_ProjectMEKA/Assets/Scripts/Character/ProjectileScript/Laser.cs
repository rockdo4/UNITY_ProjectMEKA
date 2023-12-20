using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Laser : MonoBehaviour
{
    
    public float speed = 0.1f; // 레이저의 이동 속도
    public float maxDistance = 100.0f; // 레이저가 이동할 최대 거리

    private LineRenderer lineRenderer;
    public PlayerController player;
    private Vector3 endPosition;
    private Vector3 startPos;
    private RaycastHit hit;
    private Vector3 direction;
    private bool isOne;
    private GameObject obj;
    private LayerMask layerMask;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        layerMask = LayerMask.GetMask("LowTile", "HighTile");
    }
    private void OnEnable()
    {
        if(player!= null)
        {
            endPosition = player.transform.position;
            startPos = DetermineClosestDirection(player.transform.forward);
        }
        isOne = false;
    }

    void Update()
    {
        // 레이저의 끝점을 전방으로 이동
        float distance = speed * Time.deltaTime;
        endPosition += startPos * distance;
        lineRenderer.SetPosition(0, player.FirePosition.transform.position);
        lineRenderer.SetPosition(1, endPosition);

        if (Physics.Raycast(player.FirePosition.transform.position, endPosition, out hit, layerMask)&& !isOne)
        {
            
            isOne = true;
            obj = ObjectPoolManager.instance.GetGo("BlueBeamEffect");
            obj.transform.position = hit.point;
            obj.SetActive(false);
            obj.SetActive(true);
            obj.GetComponent<PoolAble>().ReleaseObject(0.6f);
            
        }
        if (obj != null)
        {

            obj.transform.position = endPosition;
        }
        transform.position = endPosition;
    }
    private Vector3 DetermineClosestDirection(Vector3 forward)
    {
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        Vector3 closestDirection = Vector3.forward;
        float maxDot = -Mathf.Infinity;

        foreach (var direction in directions)
        {
            float dot = Vector3.Dot(forward, direction);
            if (dot > maxDot)
            {
                maxDot = dot;
                closestDirection = direction;
            }
        }
        
        return closestDirection;
    }
}
