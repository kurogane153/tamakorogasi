using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScaleUp : MonoBehaviour
{
    [SerializeField] private GameObject _efffect;
    [SerializeField] private float _ballSceleUpReduceTime = 7.5f;

    private BallControll ballControll;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            ballControll = other.GetComponent<BallControll>();
            StartCoroutine(nameof(Scale));
        }
    }

    private IEnumerator Scale()
    {
        transform.position = new Vector3(10000, 10000, 10000);
        ballControll.isScaleUp = true;
        yield return new WaitForSeconds(_ballSceleUpReduceTime);
        ballControll.isScaleUp = false;
    }

    public void PlayCoinEffect()
    {
        Instantiate(_efffect, transform.position, Quaternion.identity);
    }
}
