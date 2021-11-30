using System.Collections;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] private float _firstSpan = 5f;
    [SerializeField] private float _spawnSpan = 10f;
    [SerializeField] private GameObject _bombPrefab;

    void Start()
    {
        StartCoroutine(BombSpawn(_firstSpan));
    }

    private IEnumerator BombSpawn(float spawnTime)
    {
        yield return new WaitForSeconds(spawnTime);
        Quaternion instantRot = Quaternion.Euler(-45, 90, 0);
        Instantiate(_bombPrefab, transform.position, instantRot);

        StartCoroutine(BombSpawn(_spawnSpan));
    }
}