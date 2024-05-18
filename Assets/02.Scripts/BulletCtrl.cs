using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float damage = 20.0f;

    public float force = 1500.0f;

    private Rigidbody rb;

    void Start()
    {
        // Rigidbody 컴포넌트 추출
        rb = GetComponent<Rigidbody>();

        // 총알의 전진방향으로 힘 가하기
        rb.AddForce(transform.forward * force);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
