using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallControll : MonoBehaviour
{
    private Vector3 startPos;
    private Rigidbody rb;
    private Rigidbody rbbuffer;

    private int itemCount;

    public GameObject GameClearText;
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private GameObject _gameClearText;
    [SerializeField] private ParticleSystem _clearConfetti;
    [SerializeField] private GameObject _shockWave;
    private GameManager gm;

    public bool isSpeedReduceHalf;  // ボールの速度を半減させるか
    public bool isSpeedUp;
    public bool isScaleUp;
    public bool isBounceUp;

    private bool scaleupflg = false;

    private GUIStyle style;                 // デバッグ表示用

    void Start()
    {
        startPos = transform.position;
        gm = GameManager.Instance;
        rb = GetComponent<Rigidbody>();
        itemCount = 0;
        _gameClearText.SetActive(false);
        if (rb) {
            Debug.Log("ボールにRigidbodyがアタッチされている");
        }
        style = new GUIStyle();
        style.fontSize = 20;

    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        if (itemCount == 12)
        {
            //GameClearText.SetActive(true);
            
            StartCoroutine(nameof(GameClear));
        }

        if (isSpeedReduceHalf) {
            rb.velocity = new Vector3(rb.velocity.x * 0.85f, rb.velocity.y, rb.velocity.z * 0.85f);
        }

        if (isSpeedUp)
        {
        }

        //サイズアップアイテムを取った時の処理
        if (isScaleUp)
        {
            if (!scaleupflg)
            {
                this.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                scaleupflg = true;
            }
        }
        else
        {
            this.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            scaleupflg = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 壁に衝突したとき、速度が一定以上なら、衝撃波エフェクトと衝突SEを鳴らす
        if (collision.gameObject.CompareTag("Wall") && rb.velocity.magnitude >= 0.35f) {
            Instantiate(_shockWave, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySE(SE.Conflict);
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Item")
        {
            Debug.Log("すり抜けた！");
            itemCount++;
            gm.CoinCount = itemCount;
            other.GetComponent<ItemScript>().PlayCoinEffect();
            other.gameObject.SetActive(false);
            SoundManager.Instance.PlaySE(SE.Coin);
            Debug.Log(itemCount);
        }
        
    }

    public int GetItemCount()
    {
        return itemCount;
    }

    private IEnumerator GameClear()
    {
        _clearConfetti.Play();
        _gameClearText.SetActive(true);
        yield return new WaitForSeconds(1f);

        GetComponent<AudioSource>().enabled = false;
        _gameClearText.SetActive(false);
        _resultPanel.SetActive(true);
        GameObject.FindObjectOfType<FloorControll>().enabled = false;

        foreach(var bmsp in FindObjectsOfType<BombSpawner>()) {
            bmsp.enabled = false;
        }
    }

    private void OnGUI()
    {
        //GUI.Label(new Rect(0, 180, 500, 300), "magnitude : " + rb.velocity.magnitude, style);
        //GUI.Label(new Rect(0, 180, 500, 300), "isSpeedUp : " + isSpeedUp, style);
        //GUI.Label(new Rect(0, 220, 500, 300), "rb.velocity : " + rb.velocity, style);
    }

}
