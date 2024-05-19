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
            // 첫 번째 충돌 지점의 정보 추출
            ContactPoint cp = collision.GetContact(0);

            //충돌한 총알의 법선 벡터를 쿼터니언 타입으로 변환
            Quaternion rot = Quaternion.LookRotation(-cp.normal);

            // 쿼터니언은 유니티에서 사용하는 각도의 단위
            GameObject spark = Instantiate(sparkEffect, collision.transform.position, rot);

            Destroy(spark, 0.5f);

            // 충돌한 게임 오브젝트 삭제
            Destroy(collision.gameObject); 
        }
    }
}
