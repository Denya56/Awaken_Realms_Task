using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Draggable : MonoBehaviour
{
    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private LayerMask desktopLayerMask;
    [SerializeField] private float raycastDistance = 100f;
    [SerializeField] private Vector3 dragOffset = new Vector3(0, 2f, 0);

    private bool isMouseOver = false;
    private bool isDragging = false;

    private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        DetectMouseOver();
        DetectMouseDown();
        DetectMouseUp();
        DetectDragging();
    }

    private void DetectMouseOver()
    {
        var ray = camera.ScreenPointToRay(Input.mousePosition);

        // if raycast hit and hit this gameobject
        if(Physics.Raycast(ray, out var hitInfo, raycastDistance, raycastLayerMask)
            && hitInfo.collider.gameObject == gameObject)
        {
            Debug.DrawLine(ray.origin, hitInfo.point);
            // if mouse over for the first time
            if (!isMouseOver) HandleMouseEntered();
        }
        // if doesn't hit anything or hit somthing but not this game object
        else 
        {
            // if was hitting something before
            if (isMouseOver) HandleMouseExited();
        }
    }

    private void DetectMouseDown()
    {
        if(Input.GetMouseButtonDown(0) && isMouseOver)
        {
            HandleMouseDown();
        }
    }

    private void DetectMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            HandleMouseUp();
        }
    }

    private void DetectDragging()
    {
        if (isDragging)
        {
             // detect hit on desktop 
             var ray = camera.ScreenPointToRay(Input.mousePosition);
             if(Physics.Raycast(ray, out var hitInfo, raycastDistance, desktopLayerMask))
             {
                // if desktop hit - dragg the dice
                EventManager.Instance.HandleOnDragged(ray.GetPoint(hitInfo.distance), dragOffset);
             }
             else
             {
                // if nothing is hit or desktop not hit handle release of mouse button
                HandleMouseUp();
             }
        }
    }

    private void HandleMouseEntered()
    {
        isMouseOver = true;
    }

    private void HandleMouseExited()
    {
        isMouseOver = false;
    }

    void HandleMouseDown()
    {
        // prevent concurrent event calling if already dragging
        if (!isDragging)
        {
            isDragging = true;
            EventManager.Instance.HandleMouseDown();
        }
    }

    void HandleMouseUp()
    {
        // prevent concurrent event calling if already stopped dragging
        if (isDragging)
        {
            isDragging = false;
            EventManager.Instance.HandleMouseUp();
        }
    }
}
