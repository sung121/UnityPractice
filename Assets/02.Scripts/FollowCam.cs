using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    // ���󰡾� �� ����� ������ ����

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
        // ī�޶� �ڽ��� Ʈ������ ������Ʈ ����;
        camTr = GetComponent<Transform>();        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        // �����ؾ� �� ����� �������� distance��ŭ �̵�
        // ���̸� height��ŭ �̵�
        
        // �÷��̾ �ٶ󺸴� ������ �ڸ� �ٶ󺸴� ���⺤�� ����
        Vector3 pos = targetTr.position
                            + (-targetTr.forward * distance)
                            + (Vector3.up * height);

        camTr.position = Vector3.SmoothDamp(camTr.position, // ���� ��ġ
                                            pos,            // Ÿ�� ��ġ
                                            ref velocity,   // �׳� �ӵ�
                                            damping);       // ��ǥ ��ġ���� ������ �ð�       
                            


        //camTr.position = Vector3.Slerp( camTr.position,             // ���� ��ġ
        //                                pos,                        // ��ǥ ��ġ
        //                                Time.deltaTime * 10.0f);    // �ð� t

        // ī�޶� �ش� �������� �ٶ󺸵��� ȸ��. �ǹ� ��ǥ�� ������
        camTr.LookAt(targetTr.position + (targetTr.up * targetOffset));
    }
}
