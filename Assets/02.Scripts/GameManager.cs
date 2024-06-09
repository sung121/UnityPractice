using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TextMesh Pro ���� ������Ʈ�� �����ϱ� ���� ����
using TMPro;
using System.Drawing;
public class GameManager : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();
    
    public GameObject monster;

    public List<GameObject> monsterPool = new List<GameObject>();

    public int maxMonsters = 10;

    public float createTime = 3.0f;

    private bool isGameOver;


    public bool IsGameOver
    {
        get { return isGameOver; }
        set {
            isGameOver = value; 
            if (isGameOver)
            {
                CancelInvoke("CreateMonster");
            }
        }
    }

    // �̱��� �ν��Ͻ� ����
    public static GameManager instance = null;

    public TMP_Text scoreText;
    private int totalScore = 0;

    private void Awake()
    {
        // ó�� �����ϴ� �ν��Ͻ��� ���
        if (instance == null) 
        {
            instance = this;
        }

        // instance�� �Ҵ�� Ŭ������ �ν��Ͻ��� �ٸ� ��� ���� ������ Ŭ������ �ǹ���
        // �̹� ��������ִ� �ν��Ͻ� ������ �ش� ���ӸŴ��� ������Ʈ �ν������ڴ�.
        else if (instance != this) 
        {
            Destroy(this.gameObject);
        }

        // �ٸ� ������ �Ѿ���� �������� �ʰ� ������.
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        CreateMonsterPool();
        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

        Debug.Log(spawnPointGroup.position);

        // SpawnPointGroup ������ �ִ� ��� ���ϵ� ���ӿ�����Ʈ�� Transform ������Ʈ ����
        //spawnPointGroup?.GetComponentsInChildren<Transform>(points);

        foreach (Transform point in spawnPointGroup)
        {
            points.Add(point);
        }

        InvokeRepeating("CreateMonster", 2.0f, createTime);
        totalScore += PlayerPrefs.GetInt("TOT_SCORE");
        DisplayeScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateMonster()
    {
        int idx = Random.Range(0, points.Count);

        //Instantiate(monster, points[idx].position, points[idx].rotation);

        GameObject _monster = GetMonsterInPool();

        _monster?.transform.SetPositionAndRotation( points[idx].position,
                                                    points[idx].rotation);

        _monster?.SetActive(true);
    }
    void CreateMonsterPool()
    {
        for (int i = 0; i < maxMonsters; i++) 
        {
            // ���� ����
            var _monster = Instantiate<GameObject>(monster);

            // ���� �̸� ����
            _monster.name = $"Monster_{i:00}";

            // ���� ��Ȱ��ȭ
            _monster.SetActive(false);

            monsterPool.Add(_monster);
        }
    }

    public GameObject GetMonsterInPool()
    {
        foreach (var _monster in monsterPool)
        {
            // ���� ������Ʈ�� ��Ȱ��ȭ�̸� �������
            if (_monster.activeSelf == false)
            {
                // ���� ��ȯ
                return _monster;
            }
        }

        return null;
    }

    public void DisplayeScore(int score)
    {
        totalScore += score;
        scoreText.text = $"<color=#00ff00>SCORE :</color><color=#ff0000>{totalScore: #, ##0}</color>";
        PlayerPrefs.SetInt("TOT_SCORE", totalScore);
    }

}
