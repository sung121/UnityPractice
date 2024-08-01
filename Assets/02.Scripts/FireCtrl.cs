using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// �ݵ�� �ʿ��� ������Ʈ ������ �ش� ������Ʈ�� �����Ǵ� ���� �����ϴ� ��Ʈ����Ʈ
// 로컬 저장소에서 건듬~~~~~~
[RequireComponent (typeof (AudioSource))]

public class FireCtrl : MonoBehaviour
{
    // �Ѿ� ������
    public GameObject bullet;

    // �Ѿ� �߻� ��ǥ
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
        // Bullet �������� �������� ����(������ ��ü, ��ġ, ȸ��)
        //Instantiate(bullet, fireTransform.position, fireTransform.rotation);

        audio.PlayOneShot(fireSfx, 1.0f);

        StartCoroutine(ShowMuzzleFlash());

    }

        // �ڷ�ƾ �Լ���.
    IEnumerator ShowMuzzleFlash()
    {
        // ������ ��ǩ���� ���� �Լ��� ����
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;

        muzzleFlash.material.mainTextureOffset = offset;

        // MuzzleFlash�� ȸ�� ����
        float angle = Random.Range(0, 360);
        // Euler(���Ϸ���� �ǹ� ) = ���Ϸ����� ���ʹϾ� ��ȯ���ִ� �Լ�
        muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);

        // MuzzleFlash ũ�� ����
        float scale = Random.Range(1.0f, 2.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        muzzleFlash.enabled = true;

        // �ѱ��� �����ִ� �ð�.
        yield return new WaitForSeconds(0.2f);

        muzzleFlash.enabled = false;
    }
}
