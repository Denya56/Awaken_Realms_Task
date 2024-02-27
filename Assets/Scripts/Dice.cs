using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class Dice : MonoBehaviour
{
    #region Dice side

    // class for storing diceSide info
    [System.Serializable]
    private class DiceSideInfo
    {
        public string sideText;
        [HideInInspector]
        public Transform sidePosition;
    }

    [SerializeField] private List<DiceSideInfo> diceSides = new List<DiceSideInfo>();
    private Transform canvas;

#if UNITY_EDITOR

    // automativally add sides depending on the dice
    // expects the dice to have canvas "DiceCanvas" with child objects tagged "DiceSide"
    // amount of objects = amount of dice sides
    private void OnValidate()
    {
        canvas = transform.Find("DiceCanvas");
        if (canvas != null)
        {
            AddSidesText();
            // allows editing text on dice sides in the inspector via the dice script
            SetSidesEditorText();
        }
    }

    private void AddSidesText()
    {
        if (canvas.transform.childCount != diceSides.Count)
        {
            diceSides.Clear();

            foreach (Transform side in canvas)
            {
                if (side.CompareTag("DiceSide"))
                    diceSides.Add(new DiceSideInfo());
            }
        }
    }

    private void SetSidesEditorText()
    {
        for (int i = 0; i < canvas.childCount; i++)
        {
            TMP_Text textComponent = canvas.GetChild(i).gameObject.GetComponent<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = diceSides[i].sideText;
            }
        }
    }

#endif
    #endregion

    private Rigidbody rb;
    private Draggable draggable;

    private DiceSideInfo resutSide;

    private Vector3 dir;
    private bool thrown = false;
    private bool buttonThrown = false;

    [SerializeField] private float power;

    private Vector3[] lastPositions = new Vector3[5];

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        draggable = GetComponent<Draggable>();


        // get dice sides global position for detecting the result
        for (int i = 0; i < canvas.childCount; i++)
        {
            diceSides[i].sidePosition = canvas.GetChild(i).gameObject.transform;
        }

        resutSide = diceSides[0];

        if (draggable)
        {
            EventManager.Instance.OnDragStart += OnDragStart;
            EventManager.Instance.OnDragged += HandleDragged;
            EventManager.Instance.OnDragEnd += OnDragEnd;
        }

        EventManager.Instance.OnButtonThrow += OnButtonRoll;
    }

    private void FixedUpdate()
    {

        if (thrown)
        {
            thrown = false;
            // add random torque for natral feeling
            rb.AddTorque(new Vector3(UnityEngine.Random.Range(1f, 5f), UnityEngine.Random.Range(1f, 5f), UnityEngine.Random.Range(1f, 5f)) * power);
            rb.AddForce(dir * power, ForceMode.Impulse);
        }

        if (buttonThrown)
        {
            buttonThrown = false;
            rb.AddTorque(new Vector3(UnityEngine.Random.Range(1f, 5f), UnityEngine.Random.Range(1f, 5f), UnityEngine.Random.Range(1f, 5f)) * power);
            rb.AddForce(dir * power, ForceMode.Impulse);
        }
    }

    private void OnDragStart()
    {
        // remove velocity and rotation when the dice is picked up
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void HandleDragged(Vector3 pos, Vector3 dragOffset)
    {
        // save last position to calculate throwing direction
        // use multiple save points for avoiding throwing the dice right after save
        // which results in lastPosition = transform.position (no throw)
        for (int i = 0; i < lastPositions.Length - 1; i++)
        {
            lastPositions[i] = lastPositions[i + 1];
        }
        lastPositions[lastPositions.Length - 1] = transform.position;

        gameObject.transform.position = new Vector3(pos.x, dragOffset.y, pos.z);
    }

    private void OnDragEnd()
    {
        dir = (transform.position - lastPositions[0]).normalized;
        // only for physics update
        thrown = true;

        // if the difference is too small don't calculate the result of the throw
        if (Mathf.Abs(dir.x) >= 0.1f || Mathf.Abs(dir.z) >= 0.1f)
        {
            EventManager.Instance.HandleOnRollStart();
            StartCoroutine(DiceRoll());
        }
    }

    // used for rolling with UI button
    private void OnButtonRoll()
    {
        // get random throw direction
        // could improve to prevent direction pointing down the table
        dir = UnityEngine.Random.onUnitSphere;
        buttonThrown = true;
        EventManager.Instance.HandleOnRollStart();
        StartCoroutine(DiceRoll());
    }

    private IEnumerator DiceRoll()
    {
        // if velocity doesn't eqaul zero (dice moving) do nothing
        // if thrown with button do nothing (velocity will be zero in the frame when button is pressed
        // used to wait until FixUpdate adds force)
        while (!rb.velocity.Equals(Vector3.zero) || buttonThrown)
        {
            yield return null;
        }

        // when the dice stopped find the dice side with the highest global position.y and set it to result
        foreach (var side in diceSides)
        {
            if (side.sidePosition.position.y > resutSide.sidePosition.position.y)
            {
                resutSide = side;
            }
        }
        EventManager.Instance.HandleOnRollEnd(resutSide.sideText);
    }
}
