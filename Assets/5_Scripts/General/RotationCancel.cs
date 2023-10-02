using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCancel : MonoBehaviour
{
    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        transform.rotation = initialRotation;
    }
}
