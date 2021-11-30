using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // 爆発までの時間
    [SerializeField] private float _explosionTime = 5f;

    // 爆発前兆演出を爆発の何秒前から開始するか
    [SerializeField] private float _aoESignEffectTime = 1.5f;

    // 爆発力
    [SerializeField] private float _explosionForce = 3f;

    // 爆発の範囲、子要素のものを入れる
    [SerializeField] private SphereCollider _explosionRange;

    // 爆発エリア描画用MeshRenderer
    [SerializeField] private MeshRenderer _aoEMeshRenderer;

    // 爆発エフェクト
    [SerializeField] private GameObject _explosionEffect;

    private MeshRenderer myMeshRenderer;
    private bool isAoESignEffect;   // 爆発前兆演出をするか

    void Start()
    {
        myMeshRenderer = GetComponent<MeshRenderer>();
        StartCoroutine(nameof(ExplosionStart));
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball")) {
            Rigidbody ballRB = other.GetComponent<Rigidbody>();
            Vector3 vec = (ballRB.transform.position - transform.position).normalized;
            ballRB.AddForce((vec + Vector3.up) * _explosionForce, ForceMode.Impulse);
        }
    }

    private IEnumerator ExplosionStart()
    {
        SoundManager.Instance.PlaySE(SE.Fuse);

        float waitTime = 0;

        while(waitTime < _explosionTime) {
            waitTime += Time.deltaTime;

            if((_explosionTime - _aoESignEffectTime) <= waitTime && !isAoESignEffect) {
                StartCoroutine(nameof(MaterialFadein));
                isAoESignEffect = true;
            }

            yield return new WaitForFixedUpdate();
        }

        _explosionRange.enabled = true;
        myMeshRenderer.enabled = false;
        _aoEMeshRenderer.enabled = false;
        GetComponent<SphereCollider>().enabled = false;
        StopCoroutine(nameof(MaterialFadein));
        StopCoroutine(nameof(MaterialFadeout));

        SoundManager.Instance.PlaySE(SE.Explosion);

        yield return new WaitForSeconds(0.1f);
        _explosionRange.enabled = false;

        // 爆発エフェクト処理書く
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);

    }

    private IEnumerator MaterialFadein()
    {
        float waitTime = 0;

        while (waitTime < 0.2f) {
            waitTime += Time.deltaTime;

            Color newcolor = _aoEMeshRenderer.material.color;

            newcolor.a = Mathf.Lerp(0f, 0.4f, waitTime * 5);

            _aoEMeshRenderer.material.color = newcolor;

            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(nameof(MaterialFadeout));
    }

    private IEnumerator MaterialFadeout()
    {
        float waitTime = 0;

        while (waitTime < 0.2f) {
            waitTime += Time.deltaTime;

            Color newcolor = _aoEMeshRenderer.material.color;

            newcolor.a = Mathf.Lerp(0.4f, 0f, waitTime * 5);

            _aoEMeshRenderer.material.color = newcolor;

            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(nameof(MaterialFadein));
    }

    
}
