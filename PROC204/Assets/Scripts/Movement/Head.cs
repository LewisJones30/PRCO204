﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    [SerializeField] float slipSpeed = 5f;
    float heightOffset;
    private void Awake()
    {
        heightOffset = GetComponentInParent<CharacterController>().height / 2f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (transform.parent == other.transform) return;        

        var charPhysics = other.gameObject.GetComponent<CharacterPhysics>();
        if (charPhysics == null) return;

        float otherHeight = other.gameObject.transform.position.y;
        float headHeight = transform.parent.position.y + heightOffset;

        if (otherHeight < headHeight) return;

        charPhysics.SlipMove(slipSpeed);
    }

    private void OnTriggerExit(Collider other)
    {
        var charPhysics = other.gameObject.GetComponent<CharacterPhysics>();
        if (charPhysics == null) return;

        charPhysics.FinishSlip();
    }
}
