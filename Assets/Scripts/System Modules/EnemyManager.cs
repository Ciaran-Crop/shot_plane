using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyList;
    [SerializeField] bool selfGenerate = true;
    [SerializeField] int[] enemy1Attr;

    [SerializeField] int[] enemy2Attr;
    [SerializeField] int[] enemy3Attr;
    [SerializeField] float waitForShowUITime = 3f;
    List<List<Dictionary<string, int>>> waveList;
    int nowWaveListCount;
    int curWave = 1;
    WaitUntil waitUntilWaveEnd;
    WaitForSeconds waitForShowUI;
    [SerializeField] int[] randomLevel1;
    [SerializeField] int[] randomLevel2;
    [SerializeField] int[] randomLevel3;
    [SerializeField] WaveUIController waveUIController;

    IEnumerator SelfWaveStartCoroutine()
    {
        while (true)
        {
            SetWaveUI(curWave);
            yield return waitForShowUI;
            RemoveWaveUI();
            nowWaveListCount = createOneWaveFromSetting(getOneWave(curWave));
            curWave++;
            yield return waitUntilWaveEnd;
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

    void SetWaveUI(int value)
    {
        waveUIController.gameObject.SetActive(true);
        waveUIController.UpdateText(value);
        // Debug.Log(string.Format("Wave {0} Start !!!", curWave));
    }

    void RemoveWaveUI()
    {
        waveUIController.gameObject.SetActive(false);
    }

    void Awake()
    {
        waveList = new List<List<Dictionary<string, int>>>();
        waitUntilWaveEnd = new WaitUntil(() => nowWaveListCount == 0);
        waitForShowUI = new WaitForSeconds(waitForShowUITime);
        CreateWaveSettings();
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

    public int createOneWaveFromSetting(List<Dictionary<string, int>> waveSetting)
    {
        int enemyLen = waveSetting.Count;
        for (int i = 0; i < enemyLen; i++)
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
        }

        return enemyLen;
    }

    internal void RemoveOneEnemy()
    {
        nowWaveListCount--;
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
        int randomLevel1Number = Random.Range(randomLevel1[0], randomLevel1[1] + 1);
        for (int i = 0; i < randomLevel1Number; i++)
        {
            int power = enemy1Attr[Random.Range(1, 2 + 1) * 3 + 0];
            int der = enemy1Attr[Random.Range(1, 2 + 1) * 3 + 1];
            int maxHealth = enemy1Attr[Random.Range(1, 2 + 1) * 3 + 2];
            newWaveList.Add(createOneEnemySetting(0, power, der, maxHealth));
        }
        // random enemy2 number
        int randomLevel2Number = Random.Range(randomLevel2[0], randomLevel2[1] + 1);
        for (int i = 0; i < randomLevel2Number; i++)
        {
            int power = enemy2Attr[Random.Range(1, 2 + 1) * 3 + 0];
            int der = enemy2Attr[Random.Range(1, 2 + 1) * 3 + 1];
            int maxHealth = enemy2Attr[Random.Range(1, 2 + 1) * 3 + 2];
            newWaveList.Add(createOneEnemySetting(1, power, der, maxHealth));
        }
        // random enemy3 number
        int randomLevel3Number = Random.Range(randomLevel3[0], randomLevel3[1] + 1);
        for (int i = 0; i < randomLevel3Number; i++)
        {
            int power = enemy3Attr[Random.Range(1, 2 + 1) * 3 + 0];
            int der = enemy3Attr[Random.Range(1, 2 + 1) * 3 + 1];
            int maxHealth = enemy3Attr[Random.Range(1, 2 + 1) * 3 + 2];
            newWaveList.Add(createOneEnemySetting(2, power, der, maxHealth));
        }
        return newWaveList;
    }
}
