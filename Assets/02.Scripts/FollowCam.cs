using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    // 따라가야 할 대상을 연결할 변수

    [SerializeField] private Transform targetTr;

    private Transform camTr;
    
    [Range(2.0f, 20.0f)]
    public float distance = 5.0f;

    [Range(0.0f, 10.0f)]
    public float height = 2.0f;

    public float damping = 10.0f;

    private Vector3 velocity = Vector3.zero;

    public float targetOffset = 2.0f;


    void Start()
    {
        // 카메라 자신의 트랜스폼 컴포넌트 추출;
        camTr = GetComponent<Transform>();        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        // 추적해야 할 대상의 뒤쪽으로 distance만큼 이동
        // 높이를 height만큼 이동
        
        // 플레이어가 바라보는 방향의 뒤를 바라보는 방향벡터 생성
        Vector3 pos = targetTr.position
                            + (-targetTr.forward * distance)
                            + (Vector3.up * height);

        camTr.position = Vector3.SmoothDamp(camTr.position, // 현재 위치
                                            pos,            // 타겟 위치
                                            ref velocity,   // 그냥 속도
                                            damping);       // 목표 위치까지 도달할 시간       
                            


        //camTr.position = Vector3.Slerp( camTr.position,             // 시작 위치
        //                                pos,                        // 목표 위치
        //                                Time.deltaTime * 10.0f);    // 시간 t

        // 카메라가 해당 포지션을 바라보도록 회전. 피벗 좌표가 기준임
        camTr.LookAt(targetTr.position + (targetTr.up * targetOffset));
    }
}
