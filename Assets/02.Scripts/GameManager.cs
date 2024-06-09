using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TextMesh Pro 관련 컴포넌트에 접근하기 위해 선언
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

    // 싱글턴 인스턴스 선언
    public static GameManager instance = null;

    public TMP_Text scoreText;
    private int totalScore = 0;

    private void Awake()
    {
        // 처음 생성하는 인스턴스일 경우
        if (instance == null) 
        {
            instance = this;
        }

        // instance에 할당된 클래스의 인스턴스가 다를 경우 새로 생성된 클래스를 의미함
        // 이미 만들어져있는 인스턴스 있으면 해당 게임매니저 오브젝트 부숴버리겠다.
        else if (instance != this) 
        {
            Destroy(this.gameObject);
        }

        // 다른 씬으로 넘어가더라도 삭제하지 않고 유지함.
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        CreateMonsterPool();
        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

        Debug.Log(spawnPointGroup.position);

        // SpawnPointGroup 하위에 있는 모든 차일드 게임오브젝트의 Transform 컴포넌트 추출
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
            // 몬스터 생성
            var _monster = Instantiate<GameObject>(monster);

            // 몬스터 이름 지정
            _monster.name = $"Monster_{i:00}";

            // 몬스터 비활성화
            _monster.SetActive(false);

            monsterPool.Add(_monster);
        }
    }

    public GameObject GetMonsterInPool()
    {
        foreach (var _monster in monsterPool)
        {
            // 게임 오브젝트가 비활성화이면 갖고오기
            if (_monster.activeSelf == false)
            {
                // 몬스터 반환
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
