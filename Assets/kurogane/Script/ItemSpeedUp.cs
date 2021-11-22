using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpeedUp : MonoBehaviour
{
    [SerializeField] private GameObject _efffect;
    [SerializeField] private float _ballSpeedUpReduceTime = 7.5f;

    private BallControll ballControll;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            ballControll = other.GetComponent<BallControll>();
            StartCoroutine(nameof(Poison));
        }
    }

    private IEnumerator Poison()
    {
        transform.position = new Vector3(10000, 10000, 10000);
        ballControll.isSpeedUp = true;
        yield return new WaitForSeconds(_ballSpeedUpReduceTime);
        ballControll.isSpeedUp = false;
    }

    public void PlayCoinEffect()
    {
        Instantiate(_efffect, transform.position, Quaternion.identity);
    }

}
