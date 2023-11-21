using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

public class TilemapManager : MonoBehaviour
{
    public static TilemapManager Instacne;
    public Tilemap tilemap;
    public TileBase brickTile;
    public TileBase fillTile;

    public static Dictionary<Vector3, int> wallIsBlocked;
    public static Dictionary<Vector3, int> target;

    private void Awake()
    {
        wallIsBlocked = new Dictionary<Vector3, int>();
        target = new Dictionary<Vector3, int>();
        if(Instacne != null) 
            Debug.LogWarning("Only 1 TilemapManager be allowed exist! ");
        else
        {
            Instacne = this;
        }
        LoadComponent();
    }

    void Start()
    {
        FindBrickPositions();
        FindTargetPositions();
        // FindPush();
    }

    protected virtual void LoadComponent()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        
        // Lưu ý: Thay "BrickTile" bằng tên của prefab hay resource bạn đang sử dụng
        brickTile = Resources.Load<TileBase>("tileset/wall");
        fillTile = Resources.Load<TileBase>("tileset/fill");
        if (tilemap == null)
        {
            Debug.LogError("Tilemap not found!");
        }

        if (brickTile == null)
        {
            Debug.LogError("BrickTile not found!");
        }

        if (fillTile == null)
        {
            Debug.LogError("FillTile not found!");
        }
    }

    void FindBrickPositions()
    {
        BoundsInt bounds = tilemap.cellBounds;

        foreach (var position in bounds.allPositionsWithin)
        {
            Vector3Int cellPosition = new Vector3Int(position.x, position.y, position.z);

            if (tilemap.GetTile(cellPosition) == brickTile)
            {
                wallIsBlocked[tilemap.GetCellCenterWorld(cellPosition)] = 1;
            }
        }
    }
    
    void FindTargetPositions()
    {
        BoundsInt bounds = tilemap.cellBounds;

        foreach (var position in bounds.allPositionsWithin)
        {
            Vector3Int cellPosition = new Vector3Int(position.x, position.y, position.z);

            if (tilemap.GetTile(cellPosition) == fillTile)
            {
                target[tilemap.GetCellCenterWorld(cellPosition)] = 1;
            }
        }
    }

    protected virtual void FindPush()
    {
        Test pushNormal = FindObjectOfType<Test>();
        PushSmart pushSmart = FindObjectOfType<PushSmart>();
        if (pushNormal != null)
        {
            wallIsBlocked[pushNormal.transform.Find("obstacle").transform.position] = 1;
            wallIsBlocked[pushNormal.transform.Find("push").transform.position] = 1;
        }

        if (pushSmart != null)
        {
            wallIsBlocked[pushSmart.transform.Find("obstacle").transform.position] = 1;
            wallIsBlocked[pushSmart.transform.Find("push").transform.position] = 1;
        }
    }
}