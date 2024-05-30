using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private Transform tr;
    private Animation anim;

    public float moveSpeed = 10.0f;
    public float turnSpeed = 80.0f;

    private readonly float initHp = 100.0f;

    public float currentHp;

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    IEnumerator Start()
    {
        currentHp = initHp;
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();

        anim.Play("Idle");

        turnSpeed = 0.0f;
        yield return new WaitForSeconds(0.3f);
        turnSpeed = 80.0f;
        //OnPlayerDie = GameObject.FindGameObjectWithTag("MONSTER").GetComponent<MonsterCtrl>();

    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");

        Vector3 MoveDir = (Vector3.forward * v) + (Vector3.right * h);

        tr.Translate(MoveDir.normalized * moveSpeed * Time.deltaTime);

        tr.Rotate(Vector3.up * turnSpeed * Time.deltaTime * r);

        PlayerAnim(h, v);

    }

    void PlayerAnim(float h, float v)
    {
        if(v >= 1.0f)
        {
            anim.CrossFade("RunF", 0.25f);
        }
        else if(v <= -0.1f)
        {
            anim.CrossFade("RunB", 0.25f);    
        }
        else if (h >= 0.1f)
        {
            anim.CrossFade("RunR", 0.25f);
        }
        else if(h <= -0.1f)
        {
            anim.CrossFade("RunL", 0.25f);
        }
        else 
        {
            anim.CrossFade("Idle", 0.25f);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (currentHp >= 0.0f && collider.CompareTag("PUNCH"))
        {
            currentHp -= 10.0f;
            Debug.Log($"Player hp = {currentHp/initHp}");

            if (currentHp <= 0.0f)
            {
                PlayerDie();
            }
        }    
    }

    void PlayerDie()
    {
        Debug.Log("Player Die!");
        //GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");
        //foreach(GameObject monster in monsters)
        //{
        //    monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}

        OnPlayerDie();
    }
}
