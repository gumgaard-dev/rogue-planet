using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] private float speed = 1.5f;

    [SerializeField] private EnemyData data;

    private GameObject _target;

    void Start()
    {
        //TODO need a target character in the project 
        //using tag "Player" for testing only
        _target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Follow();
    }


    //For now, this will just make the enemies swarm towards the player on any axis
    private void Follow()
    {
        transform.position =
            Vector2.MoveTowards(transform.position, _target.transform.position, speed * Time.deltaTime);
    }

    //TODO need a target character in the project 
    //when an enemy collides with the target, deal damage
    private void OnTriggerEnter2D(Collider2D col)
    {
        
    }
}
