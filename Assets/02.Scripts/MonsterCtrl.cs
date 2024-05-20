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


    // Start is called before the first frame update
    void Start()
    {
        monsterTr = GetComponent<Transform>();

        // �÷��̾��� ������Ʈ ����. �±׷� ����.
        playerTr = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();
    
        // NavMeshAgent ������Ʈ �Ҵ�
        agent = GetComponent<NavMeshAgent>();

        // ���� ����� ��ġ�� �����ϰ� �ٷ� ���� ����;
        agent.destination = playerTr.position;

        StartCoroutine(CheckMonsterState());
    
    }

    IEnumerator CheckMonsterState()
    {
        yield return new WaitForSeconds(0.3f);

        float distance = Vector3.Distance(playerTr.position, monsterTr.position);

        if (distance < attackDist)
        {
            state = State.ATTACK;
        }
        else if (distance <= traceDist)
        {
            state = State.TRACE;
        }
        else
        {
            state = State.DIE;
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
            Gizmos.DrawSphere(transform.position, attackDist);
        }
    }

}
