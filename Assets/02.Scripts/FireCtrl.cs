using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 반드시 필요한 컴포넌트 명시해 해당 컴포넌트가 삭제되는 것을 방지하는 어트리뷰트
[RequireComponent (typeof (AudioSource))]

public class FireCtrl : MonoBehaviour
{
    // 총알 프리팹
    public GameObject bullet;

    // 총알 발사 좌표
    public Transform fireTransform;

    public AudioClip fireSfx;

    private new AudioSource audio;

    private MeshRenderer muzzleFlash;

    private RaycastHit hit;

    void Start()
    {
        audio = GetComponent<AudioSource>();

        muzzleFlash = fireTransform.GetComponentInChildren<MeshRenderer>();

        muzzleFlash.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(fireTransform.position, fireTransform.forward * 10.0f, Color.green);

        if (Input.GetMouseButtonDown(0)) 
        {
            Fire();

            if(Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, 10.0f, LayerMask.GetMask("MONSTER_BODY", "BARREL") ));
            {
                if (hit.transform == null)
                {
                    return;
                }
                GameObject hitThing = hit.transform.gameObject;

                if (hitThing.tag == "MONSTER")
                {
                    Debug.Log("Hit");
                    hitThing.GetComponent<MonsterCtrl>()?.OnDamage(hit.point, hit.normal);
                }
                Debug.Log(LayerMask.LayerToName(hit.transform.gameObject.layer));
                if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "BARREL")
                {
                    Debug.Log("Hit");
                    hitThing.GetComponent<BarrelCtrl>()?.OnDamage();
                }
            }
        }
    }

    void Fire()
    {
        // Bullet 프리팹을 동적으로 생성(생성할 객체, 위치, 회전)
        //Instantiate(bullet, fireTransform.position, fireTransform.rotation);

        audio.PlayOneShot(fireSfx, 1.0f);

        StartCoroutine(ShowMuzzleFlash());

    }

        // 코루틴 함수임.
    IEnumerator ShowMuzzleFlash()
    {
        // 오프셋 좌푯값을 랜덤 함수로 생성
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;

        muzzleFlash.material.mainTextureOffset = offset;

        // MuzzleFlash의 회전 변경
        float angle = Random.Range(0, 360);
        // Euler(오일러라는 의미 ) = 오일러각을 쿼터니언 변환해주는 함수
        muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);

        // MuzzleFlash 크기 조절
        float scale = Random.Range(1.0f, 2.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        muzzleFlash.enabled = true;

        // 총구를 보여주는 시간.
        yield return new WaitForSeconds(0.2f);

        muzzleFlash.enabled = false;
    }
}
