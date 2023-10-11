using UnityEngine;

public class Cooldown
{
    private float cooldownDuration;
    private float lastActivatedTime;

    public Cooldown(float duration)
    {
        cooldownDuration = duration;
    }

    // Call this method when the action is activated
    public void Activate()
    {
        lastActivatedTime = Time.time;
    }

    // Check if the action is currently on cooldown
    public bool IsOnCooldown()
    {
        return (Time.time - lastActivatedTime) < cooldownDuration;
    }

    // Check if the action is available (not on cooldown)
    public bool IsAvailable()
    {
        return !IsOnCooldown();
    }

    // Returns the remaining cooldown time
    public float RemainingTime()
    {
        if (IsOnCooldown())
        {
            return cooldownDuration - (Time.time - lastActivatedTime);
        }
        return 0;
    }
}