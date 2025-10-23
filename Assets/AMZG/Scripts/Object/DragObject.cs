using System;
using UnityEngine;
using System.Collections;

public class DragObject3D : MonoBehaviour
{
    public Tray tray;
    public bool IsDraggable = true;

    [Header("Touch Settings")]
    public bool useTouch;
    public int touchIndex = 0;

    [Header("Drag Settings")]
    public float fixedY = 0.3f;          // luôn giữ Y cố định
    public float dragLerpSpeed = 15f;    // tốc độ mượt khi kéo
    public float dragDelay = 0.05f;      // độ trễ sau khi nhấn chuột
    public float releaseDuration = 0.15f; // thời gian trượt về cell khi thả

    [Header("Grid Info")]
    public Vector2Int currentCell;        // tọa độ ô hiện tại (x,z)
    public Vector2Int objectSize = Vector2Int.one; // kích thước object (theo số ô)
    public bool autoDetectSize = true;    // tự tính size dựa trên localScale

    private Camera cam;
    private bool isDragging = false;
    private float dragStartTime;
    private Vector3 lastValidPos;
    private Vector3 targetPos;
    private LevelController level;

    private void Start()
    {
        cam = CameraController.Instance.GameCamera;
        tray = GetComponent<Tray>();
        level = LevelController.Instance;

        if (level == null || level.grid == null)
        {
            Debug.LogError("❌ LevelController hoặc Grid chưa được gán!");
            enabled = false;
            return;
        }

        if (autoDetectSize)
            CalculateObjectSize();

        lastValidPos = transform.position;

        // Căn đúng vị trí ban đầu trên grid
        UpdatePositionFromGrid();
    }

    private void CalculateObjectSize()
    {
        // Tự tính số ô mà object chiếm theo kích thước thật
        float cell = level.grid.cellSize;
        Vector3 scale = transform.localScale;

        int xSize = Mathf.RoundToInt(scale.x / cell);
        int zSize = Mathf.RoundToInt(scale.z / cell);

        xSize = Mathf.Max(1, xSize);
        zSize = Mathf.Max(1, zSize);

        objectSize = new Vector2Int(xSize, zSize);
    }

    private Vector3 FingerPosition
    {
        get
        {
            if (useTouch && Input.touchCount > touchIndex)
                return Input.GetTouch(touchIndex).position;
            else
                return Input.mousePosition;
        }
    }

    private void OnMouseDown()
    {
        if (!IsDraggable) return;
        dragStartTime = Time.time;
        isDragging = true;
        Debug.Log("OnMouseDown");
    }

    private void OnMouseDrag()
    {
        if (!IsDraggable) return;
        if (Time.time - dragStartTime < dragDelay) return;

        Ray ray = cam.ScreenPointToRay(FingerPosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            hitPoint.y = fixedY;

            transform.position = Vector3.Lerp(transform.position, hitPoint, Time.deltaTime * dragLerpSpeed);
            lastValidPos = transform.position;
        }
    }

    private void OnMouseUp()
    {
        if (!isDragging) return;
        isDragging = false;
        Debug.Log("OnMouseUp");
        Release();
    }

    private void Release()
    {
        if (!IsDraggable) return;

        // Tính cell gần nhất từ vị trí hiện tại
        Vector2Int gridPos = level.GetGridPosition(lastValidPos);

        // Giới hạn trong phạm vi grid (để không tràn)
        gridPos.x = Mathf.Clamp(gridPos.x, 0, level.grid.size.x - objectSize.x);
        gridPos.y = Mathf.Clamp(gridPos.y, 0, level.grid.size.y - objectSize.y);

        currentCell = gridPos;

        // ✅ Lấy đúng vị trí góc dưới-trái của ô
        targetPos = level.GetWorldPosition(gridPos.x, gridPos.y);
        targetPos.y = fixedY;

        StopAllCoroutines();
        StartCoroutine(SmoothMove(targetPos, releaseDuration));
    }

    private void UpdatePositionFromGrid()
    {
        Vector3 basePos = level.GetWorldPosition(currentCell.x, currentCell.y);
        transform.position = new Vector3(basePos.x, fixedY, basePos.z);
    }

    private IEnumerator SmoothMove(Vector3 target, float duration)
    {
        Vector3 start = transform.position;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }
        transform.position = target;
    }
}