using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject expEffect;

    public Texture[] textures;

    //���� �ݰ�
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
        
        //���� �߻�
        int idx = Random.Range(0, textures.Length);

        // �ؽ�ó ����
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
        // ���� ȿ�� ��ƼŬ ����
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
        // �ֺ� �巳�� ����
        Collider[] colls = Physics.OverlapSphere(pos, radius, 1 << 3);
        
        foreach(var coll in colls)
        {
            // ���� ������ ���Ե� �巳�� rb ����
            rb = coll.GetComponent<Rigidbody>();

            // �巳�� ���� ������
            rb.mass = 1.0f;
            
            // freeze rotation ���� ����
            rb.constraints = RigidbodyConstraints.None;

            // ���߷� ����
            rb.AddExplosionForce(2500.0f, pos, radius, 1200.0f);
        }
    }

}
