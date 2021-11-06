using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    [SerializeField] private GameObject _efffect;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnDisable()
    {
        Instantiate(_efffect, transform.position, Quaternion.identity);
    }
}
