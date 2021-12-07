using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotation : MonoBehaviour
{
    [SerializeField] private bool _autoRotation = true;
    [SerializeField] private float _rotateSpeed = 0.1f;

    private void FixedUpdate()
    {
        if (_autoRotation) {
            transform.Rotate(new Vector3(0, 0, _rotateSpeed));
        }
    }
}
