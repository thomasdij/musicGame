using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableNPC : DamagableCharacter
{
    public Pathfinding.AIPath AIPath;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Die()
    {
        AIPath.enabled = false;
        Debug.Log("NPC died");
    }
}
