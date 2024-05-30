using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{

    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }

    public State state = State.IDLE;

    public float traceDist = 10.0f;

    public float attackDist = 2.0f;

    public bool isDie = false;
    
    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent agent;
    private Animator anim;
    private GameObject bloodEffect;

    // Animator �Ķ������ �ؽð� ����
    private readonly int hashTrace = Animator.StringToHash("isTrace");
    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");

    private int hp = 100;

    void OnEnable()
    {
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;
    }

    void OnDisable()
    {
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;    
    }

    void Start()
    {
        monsterTr = GetComponent<Transform>();
        
        // FindWithTag �Լ��� �÷��̾� ������Ʈ �ּ� ��ȯ�ް� ��������Ʈ�� Ʈ������ �Ҵ�, �ӵ��� ���� �����̹Ƿ� ������Ʈ������ ��� ����
        playerTr = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();
        
        agent = GetComponent<NavMeshAgent>();

        anim = GetComponent<Animator>();

        // ���� ����� ��ġ�� destination(������)�� ����
        //agent.destination = playerTr.position;

        //BloodSparyEffect ������ �ε�
        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");

        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CheckMonsterState()
    {
        while(!isDie)
        {
            // ����Ƽ �������� �����带 �Ѱ���
            yield return new WaitForSeconds(0.3f);

            if (state == State.DIE) yield break;

            float distance = Vector3.Distance(playerTr.position, monsterTr.position);
            if (distance < attackDist)
            {
                state = State.ATTACK;
            }
            else if (distance < traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }

        }

    }

    IEnumerator MonsterAction()
    {

        while (!isDie)
        {
            switch(state)
            {
                case State.IDLE:
                    // ���� ����
                    agent.isStopped = true;

                    anim.SetBool(hashTrace, false);
                    
                    break;

                case State.TRACE:
                    // �÷��̾����� ���� ����
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false;

                    anim.SetBool(hashTrace, true);
                    anim.SetBool(hashAttack, false);

                    break;

                case State.ATTACK:
                    anim.SetBool(hashAttack, true);
                    break;

                case State.DIE:
                    isDie = true;

                    agent.isStopped = true;

                    anim.SetTrigger(hashDie);
                    GetComponent<CapsuleCollider>().enabled = false;
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            Destroy(collision.gameObject);
            // �ǰ� ���׼� �ִϸ��̼� ����
            // Ʈ���� �Ķ���ʹ� �� Ư���� ģ����.
            // �� �� �����ϰ� �ٷ� ������? �׷� �ʳ���
            anim.SetTrigger(hashHit);

            Vector3 pos = collision.GetContact(0).point;

            Quaternion rot = Quaternion.LookRotation(-collision.GetContact(0).normal);
            ShowBloodEffect(pos, rot);

            hp -= 10;
            Debug.Log(hp);
            if (hp <= 0) 
            {
                state = State.DIE;
            }

        }    
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }

    void ShowBloodEffect(Vector3 pos, Quaternion rot)
    {
        // ���� ȿ�� ����
        GameObject blood = Instantiate(bloodEffect, pos, rot, monsterTr);
        Destroy(blood, 1.0f);
    }

    void OnPlayerDie()
    {
        StopAllCoroutines();

        agent.isStopped = true;

        anim.SetFloat(hashSpeed, UnityEngine.Random.Range(0.8f, 1.2f));

        anim.SetTrigger(hashPlayerDie);
    }

    void OnDrawGizmos()
    {
        if (state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, traceDist);
        }
        if (state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDist);
        }
    }
}
