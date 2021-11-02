using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallControll : MonoBehaviour
{
    private Vector3 startPos;
    private Rigidbody rb;

    private int itemCount;

    public GameObject GameClearText;
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private GameObject _gameClearText;
    private GameManager gm;

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
        
    }

    void Update()
    {
        //if (Input.GetButtonDown("Cancel")) {
            //transform.position = startPos;
            //if (rb) {
                //rb.velocity = Vector3.zero;
            //}
        //}
    }

    void FixedUpdate()
    {
        if (itemCount == 12)
        {
            //GameClearText.SetActive(true);
            
            StartCoroutine(nameof(GameClear));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Item")
        {
            Debug.Log("すり抜けた！");
            itemCount++;
            gm.CoinCount = itemCount;
            other.gameObject.SetActive(false);
            Debug.Log(itemCount);
        }
        
    }
    public int GetItemCount()
    {
        return itemCount;
    }

    private IEnumerator GameClear()
    {
        _gameClearText.SetActive(true);
        yield return new WaitForSeconds(1f);

        _gameClearText.SetActive(false);
        _resultPanel.SetActive(true);
        Time.timeScale = 0;
    }

}
