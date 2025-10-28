using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NgoUyenNguyen.GridSystem;

public struct StageTransform
{
    public string Name;
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;
}

public class SingleLevelController : MonoBehaviour
{
    public static SingleLevelController Instance { get; private set; }

    [Header("Level References")]
    public BaseGrid grid;               
    public List<Tray> trays = new List<Tray>();
    public List<Cell> cells = new List<Cell>();
    public List<PathTray> pathTrays = new List<PathTray>();

    [Header("Tray Settings")]
    public float trayY = 0.3f;
    public Transform tfListTray;
    private bool isWin;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!isWin)
        {
            CheckWin();
        }    
    }

    public virtual void SetUp()
    {
        for (int i = 0; i < cells.Count; i++ )
        {
            cells[i].isBlocked = false;
        }    

        List<Vector2Int> availableCells = new List<Vector2Int>();
        for (int x = 0; x < grid.size.x; x++)
        {
            for (int z = 0; z < grid.size.y; z++)
            {
                availableCells.Add(new Vector2Int(x, z));
            }
        }

        for (int i = 0; i < trays.Count; i++)
        {
            Tray tray = trays[i];
            tray.SetUp();
            if (availableCells.Count == 0)
            {
                Debug.LogWarning("⚠️ Hết ô trống để đặt Tray!");
                break;
            }

            int index = Random.Range(0, availableCells.Count);
            Vector2Int cell = availableCells[index];
            availableCells.RemoveAt(index);

            Vector3 worldPos = LevelController.Instance.GetWorldPosition(cell.x, cell.y);
            worldPos.y = 0.3f;

            tray.transform.position = worldPos;

            DragObject3D drag = tray.GetComponent<DragObject3D>();
            if (drag != null)
            {
                Cell c = GetCell(drag.transform.position);
                if (c != null)
                {
                    c.isBlocked = true;
                }
            }
        }


    }

    public Cell GetCell(Vector3 pos)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            if (pos == cells[i].transform.position)
            {
                return cells[i];
            }    
        }
        return null;
    }    

    public Transform GetNearestCell(Vector3 pos)
    {
        Transform nearest = null;
        float minDist = float.MaxValue;

        foreach (var cell in cells)
        {
            if (cell == null) continue;
            float dist = Vector3.Distance(pos, cell.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = cell.transform;
            }
        }
        return nearest;
    }

    public void CheckWin()
    {
        Debug.Log("CheckWin");

        for (int i = 0; i < pathTrays.Count; i++)
        {
            if (pathTrays[i].listCars.Count > 0)
            {
                Debug.Log("CheckWin1");
                return;
            }

            if (i == pathTrays.Count - 1)
            {
                Debug.Log("CheckWin2");
                isWin = true;
                StageController.Instance.End(isWin);
            }    
        }
    }

    public virtual void StartLevel() { }

    public virtual void ResetLevel() { }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public virtual void ShowSingleFinishEffects() { }

    public virtual void RefreshCharacterSkins() { }

    public virtual void SetCharacterSkin(ShopItemData data) { }
}