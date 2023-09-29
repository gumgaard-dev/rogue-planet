using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class player_controller_spaceship : MonoBehaviour
{
    public GameObject weaponPrefab;
    private GameObject weaponInstance;

    void Start()
    {
        weaponInstance = Instantiate(weaponPrefab, transform.position, quaternion.identity, transform);
    }
}
