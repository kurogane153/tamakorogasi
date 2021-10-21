using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetItemText : MonoBehaviour
{

    public GameObject ball;

    private TextMeshProUGUI itemCountText;

    void Start()
    {
        itemCountText = GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        itemCountText.SetText("{00}/12",ball.GetComponent<BallControll>().GetItemCount());
    }
}
