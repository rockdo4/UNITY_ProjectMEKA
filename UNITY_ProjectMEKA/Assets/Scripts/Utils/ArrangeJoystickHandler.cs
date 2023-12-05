using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Defines;

public class ArrangeJoystickHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    private List<GameObject> directions = new List<GameObject>();
    private Bounds backgroundBounds;
    private List<Bounds> bounds;
    private GameObject currentTile;
    private GameObject prevTile;
    private float half;
    public float radius;

    public bool cancelButtonOn;

    private StageManager stageManager;
    private ArrangeJoystick joystick;
    private CharacterInfoUIManager characterInfoUIManager;

    private void Awake()
    {
        InitOnce();
    }

    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        cancelButtonOn = false;
        Camera.main.GetComponent<PhysicsRaycaster>().eventMask = ~0;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(ray, out hit))
            {
                //if(hit.transform.gameObject != null)
                Debug.Log(hit.transform.gameObject.name, hit.transform.gameObject);
            }
        }
    }

    public void Init()
    {
        transform.localPosition = Vector3.zero;
        currentTile = null;
        Camera.main.GetComponent<PhysicsRaycaster>().eventMask = 1 << LayerMask.NameToLayer(Layers.handler);
    }

    public void InitOnce()
    {
        joystick = transform.parent.GetComponent<ArrangeJoystick>();
        characterInfoUIManager = GameObject.FindGameObjectWithTag(Tags.characterInfoUIManager).GetComponent<CharacterInfoUIManager>();
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();

        var boxCollider = GetComponent<BoxCollider>();
        half = boxCollider.bounds.size.x / 2f;
        backgroundBounds = new Bounds(Vector3.zero, new Vector3(2f, 2f, 0f));

        bounds = new List<Bounds>
        {
            new Bounds(new Vector3(0.5f, 0.5f, 0f), new Vector3(1f, 1f, 0f)),
            new Bounds(new Vector3(0.5f, -0.5f, 0f), new Vector3(1f, 1f, 0f)),
            new Bounds(new Vector3(-0.5f, -0.5f, 0f), new Vector3(1f, 1f, 0f)),
            new Bounds(new Vector3(-0.5f, 0.5f, 0f), new Vector3(1f, 1f, 0f))
        };

        var parent = transform.parent;
        for (int i = 0; i < (int)Defines.RotationDirection.Count; ++i)
        {
            directions.Add(parent.GetChild(i).gameObject);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("on pointer down");
        OnDrag(eventData);


        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //int layerMask = 1 << LayerMask.NameToLayer(Layers.handler);
        //if (Physics.Raycast(ray, Mathf.Infinity, layerMask))
        //{
        //    if (!stageManager.currentPlayer.stateManager.secondArranged)
        //    {
        //        OnDrag(eventData);
        //    }
        //}
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!stageManager.currentPlayer.stateManager.secondArranged)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position);
            float enter;
            if (plane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                transform.position = hitPoint;
                transform.localPosition = backgroundBounds.ClosestPoint(transform.localPosition);
            }

            prevTile = currentTile;
            for (int i = 0; i < (int)Defines.RotationDirection.Count; ++i)
            {
                if (bounds[i].Contains(transform.localPosition))
                {
                    currentTile = directions[i];
                    if (prevTile != currentTile)
                    {
                        cancelButtonOn = false;
                    }
                    break;
                }
            }

            RotateHandler(currentTile.transform, false);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        characterInfoUIManager.ClearTileMesh();

        if (!stageManager.currentPlayer.stateManager.secondArranged)
        {
            var playerCenterPos = stageManager.currentPlayer.transform.position;
            playerCenterPos.y = transform.position.y;

            if (Vector3.Distance(transform.position, playerCenterPos) < radius)
            {
                cancelButtonOn = true;
                transform.localPosition = Vector3.zero;
            }
            else
            {
                RotateHandler(currentTile.transform, true);
                joystick.ArrangeDone.Invoke();
            }
        }
    }

    public void RotateHandler(Transform currentTileParent, bool snap)
    {
        var pos = currentTileParent.position;
        var go = currentTileParent.gameObject;
        var playerState = stageManager.currentPlayer.stateManager.currentBase as PlayableArrangeState;
        if (go == directions[(int)Defines.RotationDirection.Up])
        {
            if (snap)
            {
                pos.z += -half;
            }
            else
            {
                playerState.RotatePlayer(Defines.RotationDirection.Up);
                stageManager.currentPlayer.AttackableTileSet(stageManager.currentPlayer.state.occupation);
                characterInfoUIManager.ChangeAttackableTileMesh();
            }
        }
        else if (go == directions[(int)Defines.RotationDirection.Right])
        {
            if (snap)
            {
                pos.x += -half;
            }
            else
            {
                playerState.RotatePlayer(Defines.RotationDirection.Right);
                stageManager.currentPlayer.AttackableTileSet(stageManager.currentPlayer.state.occupation);
                characterInfoUIManager.ChangeAttackableTileMesh();
            }
        }
        else if (go == directions[(int)Defines.RotationDirection.Down])
        {
            if (snap)
            {
                pos.z += half;
            }
            else
            {
                playerState.RotatePlayer(Defines.RotationDirection.Down);
                stageManager.currentPlayer.AttackableTileSet(stageManager.currentPlayer.state.occupation);
                characterInfoUIManager.ChangeAttackableTileMesh();
            }
        }
        else
        {
            if (snap)
            {
                pos.x += half;
            }
            else
            {
                playerState.RotatePlayer(Defines.RotationDirection.Left);
                stageManager.currentPlayer.AttackableTileSet(stageManager.currentPlayer.state.occupation);
                characterInfoUIManager.ChangeAttackableTileMesh();
            }
        }

        if (snap)
        {
            transform.position = pos;
        }
    }
}
