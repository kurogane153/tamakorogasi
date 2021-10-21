using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleDeployer : MonoBehaviour
{

    //半径
    [SerializeField]
    private float _radius;

    private void Awake()
    {
        Deploy();
    }

    //Inspectorの内容(半径)が変更された時に実行
    private void OnValidate()
    {
        Deploy();
    }

    //子を円状に配置する
    [ContextMenu("Deploy")]
    private void Deploy()
    {

        //子を取得
        List<GameObject> childList = new List<GameObject>();
        foreach (Transform child in transform)
        {
            childList.Add(child.gameObject);
        }

        //数値、アルファベット順にソート
        childList.Sort(
          (a, b) => {
              return string.Compare(a.name, b.name);
          }
        );

        //オブジェクト間の角度差
        float angleDiff = 360f / (float)childList.Count;

        //各オブジェクトを円状に配置
        for (int i = 0; i < childList.Count; i++)
        {
            Vector3 childPostion = transform.position;

            float angle = (90 - angleDiff * i) * Mathf.Deg2Rad;
            childPostion.x += _radius * Mathf.Cos(angle);
            childPostion.z += _radius * Mathf.Sin(angle);

            childList[i].transform.position = childPostion;
        }

    }

}
