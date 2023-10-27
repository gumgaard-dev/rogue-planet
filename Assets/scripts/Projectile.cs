using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    
    void Start()
    {
        //destroy projectile after set time
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        //move projectile forward
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
