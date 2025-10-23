using UnityEngine;
using NgoUyenNguyen.GridSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Kết hợp hệ thống Grid (BaseGrid) với quản lý Level.
/// Mỗi Level giờ có thể có lưới (Grid) riêng.
/// </summary>
[ExecuteAlways]
public class LevelGridController : LevelController
{
    [Header("GRID SETTINGS")]
    public BaseGrid grid;          // Grid được gắn vào level
    public GameObject cellPrefab;  // Prefab của ô (Cell)
    public Vector2Int gridSize = new Vector2Int(10, 10);
    public float cellSize = 1f;
    public GridAlignment gridAlignment = GridAlignment.BottomLeft;
    public GridSpace gridSpace = GridSpace.Horizontal;
    public CellLayout cellLayout = CellLayout.Square;
    public static new LevelGridController Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

#if UNITY_EDITOR
    [ContextMenu("Create Grid")]
    public void EditorCreateGrid()
    {
        if (cellPrefab == null)
        {
            Debug.LogWarning("⚠️ Hãy gán Cell Prefab trước khi tạo Grid!");
            return;
        }

        // Nếu chưa có grid => tạo mới
        if (grid == null)
        {
            GameObject gridObj = new GameObject("Grid");
            //gridObj.transform.SetParent(transform);
            gridObj.transform.SetParent(Level.transform);
            grid = gridObj.AddComponent<BaseGridSquare>();
        }

        grid.cellSize = cellSize;
        grid.size = gridSize;
        grid.alignment = gridAlignment;
        grid.space = gridSpace;
        grid.layout = cellLayout;

        grid.Create(gridSize.x, gridSize.y, cellPrefab);
        Debug.Log($"✅ Grid {gridSize.x}x{gridSize.y} created for level {name}");

    }

    [ContextMenu("Clear Grid")]
    public void EditorClearGrid()
    {
        if (grid == null)
        {
            Debug.LogWarning("⚠️ Không có grid để xóa!");
            return;
        }

        for (int i = grid.transform.childCount - 1; i >= 0; i--)
        {
            var child = grid.transform.GetChild(i);
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }
        Debug.Log("🗑️ Cleared all cells in grid.");
    }
#endif
}