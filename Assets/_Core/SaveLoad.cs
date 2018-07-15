using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System;
using RPG.Characters;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveLoad : MonoBehaviour
{
    PlayerData data;
    [HideInInspector] public CheckPoint[] checkPoint;
    [HideInInspector] public EnemyAI[] enemy;
    [HideInInspector] public PickupSFX[] pickup;

    private void Start()
    {
        data = new PlayerData();
        checkPoint = CheckPoint.FindObjectsOfType<CheckPoint>();
        enemy = EnemyAI.FindObjectsOfType<EnemyAI>();
        pickup = PickupSFX.FindObjectsOfType<PickupSFX>();
        LoadDataFromFile();
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/savedGame.gd"); //you can call it anything you want

        data.Clear();

        data.Get();

        bf.Serialize(file, data);
        file.Close();
    }

    public IEnumerator ReloadScene()
    {
        //LoadDataFromFile();
        SceneManager.LoadSceneAsync(data.sceneIndex, LoadSceneMode.Additive);
        yield return new WaitForSeconds(3f);
        Load();
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
    }

    //public IEnumerator LoadSavedScene()
    //{
    //    SceneManager.LoadScene(data.sceneIndex);
    //    yield return new WaitForSeconds(3f);
    //}

    void Load()
    {
        data.Set();
    }

    void LoadDataFromFile()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGame.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGame.gd", FileMode.Open);
            data = (PlayerData)bf.Deserialize(file);
            file.Close();
        }
    }
}

[Serializable]
class PlayerData
{
    public int sceneIndex;
    public float health;
    public float energy;
    public float positionX;
    public float positionY;
    public float positionZ;
    public int score;
    public List<CheckPointData> checkPoints = new List<CheckPointData>();
    public List<EnemyData> enemys = new List<EnemyData>();
    public List<PickupData> pickups = new List<PickupData>();

    public void Get()
    {
        sceneIndex = GameManager.instance.currentSceneIndex;
        health = HealthSystem.FindObjectOfType<HealthSystem>().currentHealtPoints;
        energy = AbilitySystem.FindObjectOfType<AbilitySystem>().currentEnergyPoints;
        Vector3 position = PlayerController.FindObjectOfType<PlayerController>().position;
        positionX = position.x;
        positionY = position.y;
        positionZ = position.z;
        score = GameManager.instance.score;

        for (int i = 0; i < SaveLoad.FindObjectOfType<SaveLoad>().checkPoint.Length; i++)
        {
            CheckPointData checkPointData = new CheckPointData();
            checkPointData.Get(i);
            checkPoints.Add(checkPointData);
        }

        for (int i = 0; i < SaveLoad.FindObjectOfType<SaveLoad>().enemy.Length; i++)
        {
            EnemyData enemyData = new EnemyData();
            enemyData.Get(i);
            enemys.Add(enemyData);
        }

        for (int i = 0; i < SaveLoad.FindObjectOfType<SaveLoad>().pickup.Length; i++)
        {
            PickupData pickupData = new PickupData();
            pickupData.Get(i);
            pickups.Add(pickupData);
        }
    }

    public void Set()
    {
        //GameManager.instance.currentSceneIndex = sceneIndex;

        for (int i = 0; i < checkPoints.Count; i++)
        {
            checkPoints[i].Set();
        }

        for (int i = 0; i < enemys.Count; i++)
        {
            enemys[i].Set();
        }

        for (int i = 0; i < pickups.Count; i++)
        {
            pickups[i].Set();
        }

        GameManager.instance.score = score;
        HealthSystem.FindObjectOfType<HealthSystem>().currentHealtPoints = health;
        AbilitySystem.FindObjectOfType<AbilitySystem>().currentEnergyPoints = energy;
        PlayerController.FindObjectOfType<PlayerController>().transform.position = new Vector3(positionX, positionY, positionZ);
    }

    public void Clear()
    {
        checkPoints.Clear();
        enemys.Clear();
        pickups.Clear();
    }
}

[Serializable]
class CheckPointData
{
    public bool triggered;
    public string nameCheckpoint;

    public void Get(int i)
    {
        nameCheckpoint = SaveLoad.FindObjectOfType<SaveLoad>().checkPoint[i].nameCheckpoint;
        triggered = SaveLoad.FindObjectOfType<SaveLoad>().checkPoint[i].triggered;
    }

    public void Set()
    {
        CheckPoint[] newCheckpoint = CheckPoint.FindObjectsOfType<CheckPoint>();

        for (int j = 0; j < newCheckpoint.Length; j++)
        {
            if (nameCheckpoint == newCheckpoint[j].nameCheckpoint)
            {
                if (triggered)
                {
                    newCheckpoint[j].gameObject.SetActive(false);
                }
            }
        }
    }
}

[Serializable]
class EnemyData
{
    public bool isDead;
    public string enemyName;

    public void Get(int i)
    {
        isDead = SaveLoad.FindObjectOfType<SaveLoad>().enemy[i].isDead;
        enemyName = SaveLoad.FindObjectOfType<SaveLoad>().enemy[i].enemyName;
    }

    public void Set()
    {
        EnemyAI[] newEnemy = EnemyAI.FindObjectsOfType<EnemyAI>();

        for (int j = 0; j < newEnemy.Length; j++)
        {
            if (enemyName == newEnemy[j].enemyName)
            {
                if (isDead)
                {
                    newEnemy[j].gameObject.SetActive(false);
                }
            }
        }
    }
}

[Serializable]
class PickupData
{
    public bool isActive;
    public string pickupName;

    public void Get(int i)
    {
        isActive = SaveLoad.FindObjectOfType<SaveLoad>().pickup[i].isActive;
        pickupName = SaveLoad.FindObjectOfType<SaveLoad>().pickup[i].pickupName;
    }

    public void Set()
    {
        PickupSFX[] newPickup = PickupSFX.FindObjectsOfType<PickupSFX>();

        for (int j = 0; j < newPickup.Length; j++)
        {
            if (pickupName == newPickup[j].pickupName)
            {
                if (!isActive)
                {
                    newPickup[j].gameObject.SetActive(false);
                }
            }
        }
    }
}
