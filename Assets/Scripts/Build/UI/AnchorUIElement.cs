using Capstone.Build.Characters.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorUIElement : MonoBehaviour
{

    public Transform ObjectToFollow;
    public Vector3 Offset;

    private void Update()
    {
        if (!ObjectToFollow) return;
        transform.position = ObjectToFollow.position + Offset;
    }
}
