using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject expEffect;

    public Texture[] textures;

    //폭발 반경
    public float radius = 10.0f;

    private new MeshRenderer renderer; 

    private Transform tr;
    private Rigidbody rb;

    private int hitCount = 0;
    
    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        renderer = GetComponentInChildren<MeshRenderer>();
        
        //난수 발생
        int idx = Random.Range(0, textures.Length);

        // 텍스처 지정
        renderer.material.mainTexture = textures[idx];
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            if (++hitCount == 3)
            {
                ExpBarrel();
            }
        }
    }
    
    void ExpBarrel()
    {
        // 폭발 효과 파티클 생성
        GameObject exp = Instantiate(expEffect, tr.position, Quaternion.identity);

        Destroy(exp, 3.0f);

        rb.mass = 1.0f;
        rb.AddForce(Vector3.up * 1500.0f);

        // rkswjq vhrqkffur wjsekf
        IndirectDamage(tr.position);

        Destroy(gameObject, 3.0f);
    }

    void IndirectDamage(Vector3 pos)
    {
        // 주변 드럼통 추출
        Collider[] colls = Physics.OverlapSphere(pos, radius, 1 << 3);
        
        foreach(var coll in colls)
        {
            // 폭발 범위에 포함된 드럼통 rb 추출
            rb = coll.GetComponent<Rigidbody>();

            // 드럼통 무게 가볍게
            rb.mass = 1.0f;
            
            // freeze rotation 제한 해제
            rb.constraints = RigidbodyConstraints.None;

            // 폭발력 전달
            rb.AddExplosionForce(2500.0f, pos, radius, 1200.0f);
        }
    }

}
