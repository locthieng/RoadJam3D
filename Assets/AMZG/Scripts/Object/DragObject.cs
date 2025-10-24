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
    public float fixedY = 0.3f;
    public float dragLerpSpeed = 15f;
    public float dragDelay = 0.05f;
    public float releaseDuration = 0.15f;

    [Header("Grid Info")]
    public Vector2Int currentCell;
    public Vector2Int objectSize = Vector2Int.one;
    public bool autoDetectSize = true;

    private Camera cam;
    private bool isDragging = false;
    private float dragStartTime;
    public Vector3 lastValidPos;
    private Vector3 targetPos;
    private LevelController level;
    private Vector2Int originalCell;

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

        lastValidPos = transform.position;
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
        if (!IsDraggable || !tray.isActive) return;

        dragStartTime = Time.time;
        isDragging = true;
        originalCell = currentCell;

        // 🔹 Giải phóng ô cũ tạm thời để có thể thả lại
        level.occupiedCells.Remove(currentCell);
    }

    private void OnMouseDrag()
    {
        if (!IsDraggable || !tray.isActive) return;
        if (Time.time - dragStartTime < dragDelay) return;

        Ray ray = cam.ScreenPointToRay(FingerPosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            hitPoint.y = fixedY;

            Vector2Int gridPos = level.GetGridPosition(hitPoint);

            gridPos.x = Mathf.Clamp(gridPos.x, 0, level.grid.size.x - objectSize.x);
            gridPos.y = Mathf.Clamp(gridPos.y, 0, level.grid.size.y - objectSize.y);

            if (gridPos != currentCell)
            {
                currentCell = gridPos;
                Vector3 cellWorldPos = level.GetWorldPosition(gridPos.x, gridPos.y);
                cellWorldPos.y = fixedY;

                transform.position = cellWorldPos;
                lastValidPos = cellWorldPos;
            }
        }
    }

    private void OnMouseUp()
    {
        if (!IsDraggable || !tray.isActive) return;
        isDragging = false;
        Release();
    }

    private void Release()
    {
        if (!IsDraggable || !tray.isActive) return;

        Vector2Int gridPos = level.GetGridPosition(lastValidPos);

        gridPos.x = Mathf.Clamp(gridPos.x, 0, level.grid.size.x - objectSize.x);
        gridPos.y = Mathf.Clamp(gridPos.y, 0, level.grid.size.y - objectSize.y);

        if (level.occupiedCells.Contains(gridPos))
        {
            targetPos = level.GetWorldPosition(originalCell.x, originalCell.y);
            level.occupiedCells.Add(originalCell); 
            currentCell = originalCell;
        }
        else
        {
            currentCell = gridPos;
            level.occupiedCells.Add(gridPos);
            targetPos = level.GetWorldPosition(gridPos.x, gridPos.y);
        }

        targetPos.y = fixedY;
        StopAllCoroutines();
        StartCoroutine(SmoothMove(targetPos, releaseDuration));

        if (targetPos == tray.truePosition)
        {
            tray.isActive = false;
        }
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