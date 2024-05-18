using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            // 충돌한 게임 오브젝트 삭제
            Destroy(collision.gameObject); 
        }
    }
}
