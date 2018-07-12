using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System;
using RPG.Characters;
using System.Collections.Generic;

public class SaveLoad : MonoBehaviour
{
    PlayerData data = new PlayerData();

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/savedGame.gd"); //you can call it anything you want

        data.Get();

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGame.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGame.gd", FileMode.Open);
            data = (PlayerData)bf.Deserialize(file);
            file.Close();

            data.Set();
        }
    }
}

[Serializable]
class PlayerData
{
    public float health;
    public float energy;
    public float positionX;
    public float positionY;
    public float positionZ;
    public List<CheckPointData> checkPoints = new List<CheckPointData>();

    public void Get()
    {
        health = HealthSystem.FindObjectOfType<HealthSystem>().currentHealtPoints;
        energy = AbilitySystem.FindObjectOfType<AbilitySystem>().currentEnergyPoints;
        Vector3 position = PlayerController.FindObjectOfType<PlayerController>().position;
        positionX = position.x;
        positionY = position.y;
        positionZ = position.z;

        CheckPoint[] checkPoint = CheckPoint.FindObjectsOfType<CheckPoint>();
        for (int i = 0; i < checkPoint.Length; i++)
        {
            CheckPointData checkPointData = new CheckPointData();
            checkPointData.Get(i);
            checkPoints.Add(checkPointData);
        }
    }

    public void Set()
    {
        for (int i = 0; i < checkPoints.Count; i++)
        {
            checkPoints[i].Set();
        }

        HealthSystem.FindObjectOfType<HealthSystem>().currentHealtPoints = health;
        AbilitySystem.FindObjectOfType<AbilitySystem>().currentEnergyPoints = energy;
        PlayerController.FindObjectOfType<PlayerController>().transform.position = new Vector3(positionX, positionY, positionZ);
    }
}

[Serializable]
class CheckPointData
{
    public bool triggered;
    public string nameCheckpoint;

    public void Get(int i)
    {
        CheckPoint[] checkPoints = CheckPoint.FindObjectsOfType<CheckPoint>();
        nameCheckpoint = checkPoints[i].nameCheckpoint;
        triggered = checkPoints[i].triggered;
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
