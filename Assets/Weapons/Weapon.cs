﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Weapon")]
public class Weapon : ScriptableObject
{
    public Transform weaponGrip;

    [SerializeField] GameObject weaponPrefab;
    [SerializeField] Animation attackAnimation;

    public GameObject GetWeaponPrefab()
    {
        return weaponPrefab;
    }
}
