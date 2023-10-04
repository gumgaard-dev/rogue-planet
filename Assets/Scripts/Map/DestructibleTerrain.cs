using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleTerrain : MonoBehaviour
{
    public int maxHP = 1;
    private int curHP;

    float colorModOnHit;
    private void Awake()
    {
        curHP = maxHP;
        colorModOnHit = 1f / (float)maxHP;
    }

    public void Hit()
    {
        --this.curHP;
        if (this.curHP == 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                Color c = sr.color;
                Color newColor = new Color(c.r, c.g - colorModOnHit, c.b - colorModOnHit, c.a);
                sr.color = newColor;
            }
        }
    }
}
