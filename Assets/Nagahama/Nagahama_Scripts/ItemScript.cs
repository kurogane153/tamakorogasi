using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    [SerializeField] private GameObject _efffect;

    public void PlayCoinEffect()
    {
        Instantiate(_efffect, transform.position, Quaternion.identity);
    }
}