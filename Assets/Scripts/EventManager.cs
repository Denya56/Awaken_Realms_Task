using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public Action<Vector3, Vector3> OnDragged;
    public Action<string> OnRollEnd;
    public Action OnDragStart, OnDragEnd, OnButtonThrow, OnRollStart;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void HandleMouseDown()
    {
        OnDragStart?.Invoke();
    }

    public void HandleMouseUp()
    {
        OnDragEnd?.Invoke();
    }

    public void HandleOnDragged(Vector3 pos, Vector3 dragOffset)
    {
        OnDragged?.Invoke(pos, dragOffset);
    }

    public void HandleOnRollStart()
    {
        OnRollStart?.Invoke();
    }

    public void HandleOnRollEnd(string resultText)
    {
        OnRollEnd?.Invoke(resultText);
    }

    public void HandleOnButtonThrow()
    {
        OnButtonThrow?.Invoke();
    }
}
