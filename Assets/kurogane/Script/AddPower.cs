using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPower : MonoBehaviour
{
    private Rigidbody rb;
    private BounceCalc bounceCalc;
    public float power = 1;    // 発射時の力

    // Use this for initialization
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        bounceCalc = this.GetComponent<BounceCalc>();
    }

    // Update is called once per frame
    void Update()
    {
        // 発射時のvelocityを取得
        bounceCalc.afterReflectVero = rb.velocity;
    }
}
