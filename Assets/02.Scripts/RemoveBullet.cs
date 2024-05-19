using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{

    public GameObject sparkEffect;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            // ù ��° �浹 ������ ���� ����
            ContactPoint cp = collision.GetContact(0);

            //�浹�� �Ѿ��� ���� ���͸� ���ʹϾ� Ÿ������ ��ȯ
            Quaternion rot = Quaternion.LookRotation(-cp.normal);

            // ���ʹϾ��� ����Ƽ���� ����ϴ� ������ ����
            GameObject spark = Instantiate(sparkEffect, collision.transform.position, rot);

            Destroy(spark, 0.5f);

            // �浹�� ���� ������Ʈ ����
            Destroy(collision.gameObject); 
        }
    }
}
