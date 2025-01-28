using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTDimmerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger Enter: {other.gameObject.name}");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"Trigger Exit: {other.gameObject.name}");
    }
}
