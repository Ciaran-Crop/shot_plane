using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] GameObject[] enemyList;
    [SerializeField] GameObject[] bossList;
    GameObject player;
    [SerializeField] bool selfGenerate = true;
    [SerializeField] int[] enemy1Attr;

    [SerializeField] int[] enemy2Attr;
    [SerializeField] int[] enemy3Attr;
    [SerializeField] int[] bossAttr;
    [SerializeField] float bossFactor = 0.35f;
    [SerializeField] float healthFactor = 100f;
    [SerializeField] float waitForShowUITime = 3f;
    List<List<Dictionary<string, int>>> waveList;
    List<GameObject> nowWaveList;
    int nowWaveListCount => nowWaveList.Count;
    int curWave = 1;
    WaitUntil waitUntilWaveEnd;
    WaitUntil waitUntilEnemyLastOne;
    WaitForSeconds waitForShowUI;
    [SerializeField] int[] randomLevel1;
    [SerializeField] int[] randomLevel2;
    [SerializeField] int[] randomLevel3;
    [SerializeField] WaveUIController waveUIController;
    [SerializeField] GameObject bossUI;
    public bool OnlyHasOneEnemy => nowWaveListCount == 0;
    public bool isBoss = false;

    IEnumerator SelfWaveStartCoroutine()
    {
        if (GameManager.GameMode == GameMode.Survive) yield return SurviveMode();
        else if (GameManager.GameMode == GameMode.Endless) yield return EndlessMode();
        else if (GameManager.GameMode == GameMode.Buff) yield return BuffMode();
    }

    IEnumerator BuffMode()
    {
        while (true)
        {
            if (GameManager.IsGameOver) yield break;
            if (curWave % 5 == 0)
            {
                isBoss = true;
                SetBossUI();
                yield return waitForShowUI;
                RemoveBossUI();  
                CreateOneBoss(curWave / 5);
            }
            else
            {
                isBoss = false;
                SetWaveUI(curWave);
                yield return waitForShowUI;
                RemoveWaveUI();
                createOneWaveFromSetting(getOneWave(curWave));
            }
            curWave++;
            yield return StartCoroutine(BuffManager.Instance.RandomOneBuff());
            yield return waitUntilWaveEnd;

        }
    }

    IEnumerator EndlessMode()
    {
        while (true)
        {
            if (GameManager.IsGameOver) yield break;
            if (curWave % 5 == 0)
            {
                isBoss = true;
                CreateOneBoss(curWave / 5);
                curWave++;
                yield return waitUntilEnemyLastOne;
            }
            else
            {
                isBoss = false;
                createOneWaveFromSetting(getOneWave(curWave));
                curWave++;
                yield return waitUntilEnemyLastOne;
            }
        }
    }

    IEnumerator SurviveMode()
    {
        while (true)
        {
            if (GameManager.IsGameOver) yield break;
            if (curWave % 5 == 0)
            {
                isBoss = true;
                SetBossUI();
                yield return waitForShowUI;
                RemoveBossUI();
                CreateOneBoss(curWave / 5);
                curWave++;
                yield return waitUntilWaveEnd;
            }
            else
            {
                isBoss = false;
                SetWaveUI(curWave);
                yield return waitForShowUI;
                RemoveWaveUI();
                createOneWaveFromSetting(getOneWave(curWave));
                curWave++;
                yield return waitUntilWaveEnd;
            }
        }
    }

    List<Dictionary<string, int>> getOneWave(int waveNum)
    {
        if (waveNum <= 6)
        {
            return waveList[waveNum - 1];
        }
        else
        {
            return RandomOneWave();
        }
    }


    void SetBossUI()
    {
        bossUI.SetActive(true);
    }

    void RemoveBossUI() => bossUI.SetActive(false);

    void SetWaveUI(int value)
    {
        waveUIController.gameObject.SetActive(true);
        waveUIController.UpdateText(value);
    }

    void RemoveWaveUI() => waveUIController.gameObject.SetActive(false);

    override protected void Awake()
    {
        base.Awake();
        waveList = new List<List<Dictionary<string, int>>>();
        waitUntilWaveEnd = new WaitUntil(() => nowWaveListCount == 0);
        waitUntilEnemyLastOne = new WaitUntil(() => (!isBoss && nowWaveListCount <= 1) || (isBoss && nowWaveListCount == 0));
        waitForShowUI = new WaitForSeconds(waitForShowUITime);
        nowWaveList = new List<GameObject>();
        CreateWaveSettings();
        player = GetPlayer();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (selfGenerate)
        {
            StartCoroutine(nameof(SelfWaveStartCoroutine));
        }
    }

    void OnDisable()
    {
        StopCoroutine(nameof(SelfWaveStartCoroutine));
    }

    public void createOneWaveFromSetting(List<Dictionary<string, int>> waveSetting)
    {
        for (int i = 0; i < waveSetting.Count; i++)
        {
            var dictionary = waveSetting[i];
            int enemyType = dictionary["type"];
            int powerLevel = dictionary["power"];
            int der = dictionary["der"];
            int maxHealth = dictionary["maxHealth"];
            var enemy = PoolManager.Release(enemyList[enemyType]);
            var enemyController = enemy.GetComponent<EnemyController>();
            enemyController.setPowerLevel(powerLevel);
            var healthSystem = enemy.GetComponent<HealthSystem>();
            healthSystem.setDer(der);
            healthSystem.setMaxHealth(maxHealth);
            nowWaveList.Add(enemy);
            enemy.SetActive(true);
        }
    }

    public GameObject RandomEnemyTarget()
    {
        return nowWaveListCount == 0 ? null : nowWaveList[Random.Range(0, nowWaveListCount)];
    }

    public GameObject GetPlayer()
    {
        player = player == null ? GameObject.FindGameObjectWithTag("Player") : player;
        return player;
    }

    internal void RemoveOneEnemy(GameObject enemy)
    {
        nowWaveList.Remove(enemy);
    }

    Dictionary<string, int> createOneEnemySetting(int type, int power, int der, int maxHealth)
    {
        Dictionary<string, int> setting = new Dictionary<string, int>();
        setting["type"] = type;
        setting["power"] = power;
        setting["der"] = der;
        setting["maxHealth"] = maxHealth;
        return setting;
    }

    List<Dictionary<string, int>> CreateOneList()
    {
        return new List<Dictionary<string, int>>();
    }

    void CreateWaveSettings()
    {
        // wave 1
        List<Dictionary<string, int>> newWaveList = CreateOneList();
        for (int i = 0; i < 5; i++)
        {
            newWaveList.Add(createOneEnemySetting(0, enemy1Attr[0], enemy1Attr[1], enemy1Attr[2]));
        }
        waveList.Add(newWaveList);
        // wave 2
        newWaveList = CreateOneList();
        for (int i = 0; i < 5; i++)
        {
            newWaveList.Add(createOneEnemySetting(0, enemy1Attr[3], enemy1Attr[4], enemy1Attr[5]));
        }
        waveList.Add(newWaveList);
        // wave 3
        newWaveList = CreateOneList();
        for (int i = 0; i < 5; i++)
        {
            newWaveList.Add(createOneEnemySetting(1, enemy2Attr[0], enemy2Attr[1], enemy2Attr[2]));
        }
        waveList.Add(newWaveList);
        // wave 4
        newWaveList = CreateOneList();
        for (int i = 0; i < 5; i++)
        {
            newWaveList.Add(createOneEnemySetting(1, enemy2Attr[3], enemy2Attr[4], enemy2Attr[5]));
        }
        waveList.Add(newWaveList);
        // wave 5
        newWaveList = CreateOneList();
        newWaveList.Add(createOneEnemySetting(0, enemy1Attr[6], enemy1Attr[7], enemy1Attr[8]));
        newWaveList.Add(createOneEnemySetting(0, enemy1Attr[6], enemy1Attr[7], enemy1Attr[8]));
        newWaveList.Add(createOneEnemySetting(1, enemy2Attr[6], enemy2Attr[7], enemy1Attr[8]));
        newWaveList.Add(createOneEnemySetting(1, enemy2Attr[6], enemy2Attr[7], enemy1Attr[8]));
        newWaveList.Add(createOneEnemySetting(2, enemy3Attr[0], enemy3Attr[1], enemy3Attr[2]));
        waveList.Add(newWaveList);
        // wave 6
        waveList.Add(RandomOneWave());
    }

    List<Dictionary<string, int>> RandomOneWave()
    {
        List<Dictionary<string, int>> newWaveList = CreateOneList();
        // random enemy1 number
        int randomLevel1Number = Random.Range(randomLevel1[0], randomLevel1[1] + 1) + curWave / 10;
        for (int i = 0; i < randomLevel1Number; i++)
        {
            int power = enemy1Attr[Random.Range(1, 2 + 1) * 3 + 0];
            int der = enemy1Attr[Random.Range(1, 2 + 1) * 3 + 1];
            int maxHealth = enemy1Attr[Random.Range(1, 2 + 1) * 3 + 2];
            newWaveList.Add(createOneEnemySetting(0, power, der, maxHealth));
        }
        // random enemy2 number
        int randomLevel2Number = Random.Range(randomLevel2[0], randomLevel2[1] + 1) + +curWave / 15;
        for (int i = 0; i < randomLevel2Number; i++)
        {
            int power = enemy2Attr[Random.Range(1, 2 + 1) * 3 + 0];
            int der = enemy2Attr[Random.Range(1, 2 + 1) * 3 + 1];
            int maxHealth = enemy2Attr[Random.Range(1, 2 + 1) * 3 + 2];
            newWaveList.Add(createOneEnemySetting(1, power, der, maxHealth));
        }
        // random enemy3 number
        int randomLevel3Number = Random.Range(randomLevel3[0], randomLevel3[1] + 1) + +curWave / 20;
        for (int i = 0; i < randomLevel3Number; i++)
        {
            int power = enemy3Attr[Random.Range(1, 2 + 1) * 3 + 0];
            int der = enemy3Attr[Random.Range(1, 2 + 1) * 3 + 1];
            int maxHealth = enemy3Attr[Random.Range(1, 2 + 1) * 3 + 2];
            newWaveList.Add(createOneEnemySetting(2, power, der, maxHealth));
        }
        return newWaveList;
    }

    void CreateOneBoss(int id)
    {
        int level = (id + 1) / 2;
        // int index = (id + 1) % 2;
        int maxHealth = Mathf.FloorToInt(bossAttr[0] + healthFactor * level * level);
        int der = Mathf.FloorToInt(maxHealth * bossFactor);
        var boss = PoolManager.Release(bossList[0]);
        var healthSystem = boss.GetComponent<HealthSystem>();
        healthSystem.setDer(der);
        healthSystem.setMaxHealth(maxHealth);
        nowWaveList.Add(boss);
        boss.SetActive(true);
        boss.GetComponent<BossController>().StartSkill();
    }
}
