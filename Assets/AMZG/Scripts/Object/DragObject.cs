using System;
using UnityEngine;
using System.Collections;

public class DragObject3D : MonoBehaviour
{
    public Tray tray;
    public bool IsDraggable;

    [Header("Touch Settings")]
    public bool useTouch;
    public int touchIndex = 0;

    [Header("Drag Settings")]
    public float fixedY = 0.3f;          // luôn giữ Y cố định
    public float dragLerpSpeed = 15f;    // tốc độ mượt khi kéo
    public float dragDelay = 0.05f;      // độ trễ sau khi nhấn chuột
    public float releaseDuration = 0.4f; // thời gian trượt về cell khi thả

    private Camera cam;
    private bool isDragging = false;
    private float dragStartTime;
    private Vector3 lastValidPos;

    private void Start()
    {
        IsDraggable = true;
        cam = CameraController.Instance.GameCamera;
        tray = GetComponent<Tray>();
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
        Plane plane = new Plane(Vector3.up, new Vector3(0, 0, 0));

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            hitPoint.y = fixedY;
            transform.position = Vector3.Lerp(transform.position, hitPoint, Time.deltaTime * dragLerpSpeed);
            //lastValidPos = transform.position;
            lastValidPos = new Vector3(transform.position.x, 0.3f, transform.position.z);
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