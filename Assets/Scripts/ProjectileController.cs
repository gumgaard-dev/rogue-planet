using Build.Component;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 2f;

    private void Start()
    {
        //destroy projectile after set time
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        //move projectile forward
        transform.Translate(Vector3.up * (WeaponController.ProjectileSpeed * Time.deltaTime));
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        var targetHealth = col.GetComponent<HealthData>();

        if (targetHealth != null)
        {
            //apply damage to target
            targetHealth.Damage(gameObject.GetComponentInParent<AttackData>().AttackPower);  
        }
    }
}
