using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleTerrain : MonoBehaviour
{
    public int maxHP = 1;
    private int curHP;
    private float delayBeforeHeal = 1f;
    private float timeBetweenHeal = 0.2f;
    private float lastHitTime;
    private int healAmount = 1;
    private Coroutine healingCoroutine;
    

    float colorModOnHit;
    
    
    private void Awake()
    {
        curHP = maxHP;
        colorModOnHit = 1f / (float)maxHP;
    }

    public void Hit()
    {
        lastHitTime = Time.time;
        --this.curHP;
        if (this.curHP == 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            UpdateSpriteColor();
            // Start the healing process if not already started
            if (healingCoroutine == null)
            {
                healingCoroutine = StartCoroutine(HealOverTime());
            }
        }
    }
    IEnumerator HealOverTime()
    {
        // delay for some time after last hit
        yield return new WaitUntil(() => Time.time - lastHitTime > delayBeforeHeal);

        while (curHP < maxHP)
        {
            // control heal rate
            yield return new WaitForSeconds(timeBetweenHeal);
            curHP += healAmount;

            UpdateSpriteColor();
        }

        // Stop the coroutine once fully healed
        StopCoroutine(healingCoroutine);
        healingCoroutine = null;
    }

    private void UpdateSpriteColor()
    {
        SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            float curColorMod = (float)(maxHP - curHP) * colorModOnHit;
            Color newColor = new Color(1f, 1f - curColorMod, 1f - curColorMod, 1f);
            sr.color = newColor;
        }
    }
}
