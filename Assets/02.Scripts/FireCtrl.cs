using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    // 총알 프리팹
    public GameObject bullet;

    // 총알 발사 좌표
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
        // Bullet 프리팹을 동적으로 생성(생성할 객체, 위치, 회전)
        Instantiate(bullet, fireTransform.position, fireTransform.rotation);

    }
}
