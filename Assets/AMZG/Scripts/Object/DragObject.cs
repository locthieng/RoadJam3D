using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NgoUyenNguyen.GridSystem;

public class DragObject3D : MonoBehaviour
{
    public Tray tray;
    public bool IsDraggable = true;

    [Header("Touch Settings")]
    public bool useTouch;
    public int touchIndex = 0;
    public BoxCollider col;
    [Header("Drag Settings")]
    public float fixedY = 0.3f;
    public float dragDelay = 0.05f;
    public float releaseDuration = 0.15f;
    public float dragLerpSpeed = 15f; // tốc độ mượt khi kéo

    [Header("Grid Info")]
    public Transform currentCell;
    private float startY;

    private Camera cam;
    private bool isDragging;
    private float dragStartTime;
    private Vector3 lastValidPos;
    private Vector3 targetPos;

    private void Start()
    {
        cam = CameraController.Instance.GameCamera;
        tray = GetComponent<Tray>();
        lastValidPos = transform.position;
        startY = transform.position.y;

        currentCell = SingleLevelController.Instance.GetNearestCell(transform.position);
        if (currentCell != null)
        {
            var state = currentCell.GetComponent<Cell>();
            if (state == null) state = currentCell.gameObject.AddComponent<Cell>();
            state.isBlocked = true;
        }
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
        Debug.Log(tray.truePosition);
        if (!IsDraggable) return;
        dragStartTime = Time.time;
        isDragging = true;

        if (currentCell != null)
        {
            var state = currentCell.GetComponent<Cell>();
            if (state != null) state.isBlocked = false;
        }
    }

    private void OnMouseUp()
    {
        if (!isDragging) return;

        Release();
    }

    private void Update()
    {
        if (!isDragging) return;
        if (Time.time - dragStartTime < dragDelay) return;

        Ray ray = cam.ScreenPointToRay(FingerPosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0, fixedY, 0));

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hit = ray.GetPoint(distance);

            Vector3 desiredPos;

            desiredPos = new Vector3(hit.x, fixedY, hit.z);

            Transform nearest = SingleLevelController.Instance.GetNearestCell(desiredPos);

            if (nearest == null)
            {
                transform.position = Vector3.Lerp(transform.position, lastValidPos, Time.deltaTime * dragLerpSpeed);
                return;
            }

            var state = nearest.GetComponent<Cell>();

            if (state != null && state.isBlocked && nearest != currentCell)
            {
                //transform.position = Vector3.Lerp(transform.position, lastValidPos, Time.deltaTime * dragLerpSpeed);
                Release();
                return;
            }

            targetPos = new Vector3(nearest.position.x, fixedY, nearest.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * dragLerpSpeed);

            lastValidPos = transform.position;
        }
    }

    private void Release()
    {
        if (!IsDraggable) return;
        isDragging = false;

        Transform nearest = SingleLevelController.Instance.GetNearestCell(transform.position);
        if (nearest != null)
        {
            var state = nearest.GetComponent<Cell>();
            bool blocked = state != null && state.isBlocked;

            if (!blocked)
            {
                Vector3 snapPos = new Vector3(nearest.position.x, startY, nearest.position.z);
                StartCoroutine(SmoothMove(snapPos, releaseDuration));
                lastValidPos = snapPos;

                if (state == null) state = nearest.gameObject.AddComponent<Cell>();
                state.isBlocked = true;

                currentCell = nearest;

                if (tray.truePosition.x == nearest.position.x && tray.truePosition.z == nearest.position.z)
                {
                    SingleLevelController.Instance.trays.Remove(tray);
                    col.enabled = false;
                    tray.isActive = true;
                }    
            }
            else
            {
                StartCoroutine(SmoothMove(lastValidPos, releaseDuration));
                if (currentCell != null)
                {
                    var curState = currentCell.GetComponent<Cell>();
                    if (curState == null) curState = currentCell.gameObject.AddComponent<Cell>();
                    curState.isBlocked = true;
                }
            }
        }
        else
        {
            StartCoroutine(SmoothMove(lastValidPos, releaseDuration));
            if (currentCell != null)
            {
                var curState = currentCell.GetComponent<Cell>();
                if (curState == null) curState = currentCell.gameObject.AddComponent<Cell>();
                curState.isBlocked = true;
            }
        }
    }

    IEnumerator SmoothMove(Vector3 target, float duration)
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

    public void CheckTray()
    {
        if (currentCell == null) return;

        Cell cell = currentCell.GetComponent<Cell>();
        if (cell == null)
            cell = currentCell.gameObject.AddComponent<Cell>();

        cell.isBlocked = false;
        currentCell = null;
    }
}