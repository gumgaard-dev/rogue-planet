using UnityEngine;

public class DebugLog
{

    public static void ComponentMissing(string className, string componentName)
    {
        Debug.Log(className + ": no " + componentName + "set in inspector!");
    }
}

