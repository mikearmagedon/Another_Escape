using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System;
using RPG.Characters;

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

    public void Get()
    {
        health = HealthSystem.FindObjectOfType<HealthSystem>().currentHealtPoints;
        energy = AbilitySystem.FindObjectOfType<AbilitySystem>().currentEnergyPoints;
        Vector3 position = PlayerController.FindObjectOfType<PlayerController>().position;
        positionX = position.x;
        positionY = position.y;
        positionZ = position.z;
    }

    public void Set()
    {
        HealthSystem.FindObjectOfType<HealthSystem>().currentHealtPoints = health;
        AbilitySystem.FindObjectOfType<AbilitySystem>().currentEnergyPoints = energy;
        PlayerController.FindObjectOfType<PlayerController>().position = new Vector3(positionX,positionY,positionZ);
        Debug.Log("Done");
    }
}