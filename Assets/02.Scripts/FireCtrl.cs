using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    // �Ѿ� ������
    public GameObject bullet;

    // �Ѿ� �߻� ��ǥ
    public Transform fireTransform;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Fire();
        }
    }

    void Fire()
    {
        // Bullet �������� �������� ����(������ ��ü, ��ġ, ȸ��)
        Instantiate(bullet, fireTransform.position, fireTransform.rotation);

    }
}
