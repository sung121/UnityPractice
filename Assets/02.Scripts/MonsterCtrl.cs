using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        monsterTr = GetComponent<Transform>();
        
        // FindWithTag �Լ��� �÷��̾� ������Ʈ �ּ� ��ȯ�ް� ��������Ʈ�� Ʈ������ �Ҵ�, �ӵ��� ���� �����̹Ƿ� ������Ʈ������ ��� ����
        playerTr = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();
        
        agent = GetComponent<NavMeshAgent>();
        
        // ���� ����� ��ġ�� destination(������)�� ����
        //agent.destination = playerTr.position;

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
            yield return new WaitForSeconds(0.3f);

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
        while(!isDie)
        {
            switch(state)
            {
                case State.IDLE:
                    // ���� ����
                    agent.isStopped = true;
                    break;

                case State.TRACE:
                    // �÷��̾����� ���� ����
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false;
                    break;

                case State.ATTACK:
                    break;

                case State.DIE:
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }

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
