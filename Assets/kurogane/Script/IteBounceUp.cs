using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IteBounceUp : MonoBehaviour
{
    [SerializeField] private GameObject _efffect;
    [SerializeField] private float _ballBounceUpReduceTime = 7.5f;

    private BallControll ballControll;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            ballControll = other.GetComponent<BallControll>();
            StartCoroutine(nameof(Bounce));
        }
    }

    private IEnumerator Bounce()
    {
        transform.position = new Vector3(10000, 10000, 10000);
        ballControll.isBounceUp = true;
        SoundManager.Instance.PlaySE(SE.BounceUpItemGet);

        yield return new WaitForSeconds(_ballBounceUpReduceTime);
        ballControll.isBounceUp = false;
    }

    public void PlayCoinEffect()
    {
        Instantiate(_efffect, transform.position, Quaternion.identity);
    }
}
