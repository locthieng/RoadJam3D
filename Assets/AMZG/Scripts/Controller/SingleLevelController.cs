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
    public BaseGrid grid;               // Grid chứa thông tin size + cellSize
    public List<Tray> trays = new List<Tray>();  // Danh sách tray sẽ được random

    [Header("Tray Settings")]
    public float trayY = 0.3f;          // Chiều cao cố định của tray trên mặt phẳng

    // -------------------------
    public virtual void SetUp()
    {
        Instance = this;

        if (grid == null)
        {
            Debug.LogError("❌ Grid chưa được gán trong SingleLevelController!");
            return;
        }

        if (trays == null || trays.Count == 0)
        {
            Debug.LogWarning("⚠️ Chưa có tray nào trong danh sách trays!");
            return;
        }

        // ✅ Tạo danh sách tất cả ô trong grid
        List<Vector2Int> availableCells = new List<Vector2Int>();
        for (int x = 0; x < grid.size.x; x++)
        {
            for (int z = 0; z < grid.size.y; z++)
            {
                availableCells.Add(new Vector2Int(x, z));
            }
        }

        // ✅ Dọn danh sách occupied trước khi setup lại
        LevelController.Instance.occupiedCells.Clear();

        // ✅ Random vị trí cho từng Tray (không trùng nhau)
        foreach (Tray tray in trays)
        {
            if (availableCells.Count == 0)
            {
                Debug.LogWarning("⚠️ Hết ô trống để đặt Tray!");
                break;
            }

            int index = Random.Range(0, availableCells.Count);
            Vector2Int cell = availableCells[index];
            availableCells.RemoveAt(index);

            // ✅ Đặt vào world position của cell
            Vector3 worldPos = LevelController.Instance.GetWorldPosition(cell.x, cell.y);
            worldPos.y = 0.3f;

            tray.transform.position = worldPos;

            // ✅ Nếu có DragObject3D, đồng bộ cell
            DragObject3D drag = tray.GetComponent<DragObject3D>();
            if (drag != null)
            {
                drag.currentCell = cell;
                drag.lastValidPos = worldPos;
            }

            // ✅ Đánh dấu ô này là "đã bị chiếm"
            LevelController.Instance.occupiedCells.Add(cell);
        }

        Debug.Log($"✅ Đã random {trays.Count} tray, {LevelController.Instance.occupiedCells.Count} ô bị chiếm.");
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