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
    private Animator anim;

    // Animator 파라미터의 해시값 추출
    private readonly int hashTrace = Animator.StringToHash("isTrace");
    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");

    void Start()
    {
        monsterTr = GetComponent<Transform>();
        
        // FindWithTag 함수로 플레이어 오브젝트 주소 반환받고 겟컴포넌트로 트랜스폼 할당, 속도가 느린 연산이므로 업데이트에서는 사용 ㄴㄴ
        playerTr = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();
        
        agent = GetComponent<NavMeshAgent>();

        anim = GetComponent<Animator>();

        // 추적 대상의 위치를 destination(목적지)에 설정
        //agent.destination = playerTr.position;

        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update is Called");
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
                    // 추적 중지
                    agent.isStopped = true;

                    anim.SetBool(hashTrace, false);
                    
                    break;

                case State.TRACE:
                    // 플레이어한테 추적 시작
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false;

                    anim.SetBool(hashTrace, true);
                    anim.SetBool(hashAttack, false);

                    break;

                case State.ATTACK:

                    anim.SetBool(hashAttack, true);
                    break;

                case State.DIE:
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
            // 피격 리액션 애니메이션 실행
            anim.SetTrigger(hashHit);
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
