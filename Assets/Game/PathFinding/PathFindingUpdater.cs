using System;
using Game.Destructibles;
using UnityEngine;

public class PathFindingUpdater : MonoBehaviour
{
    private void Start()
    {
        DestructibleCrate.OnAnyDestroyed += DestructibleCrate_OnAnyDestroyed;
    }

    private void DestructibleCrate_OnAnyDestroyed(object sender, EventArgs e)
    {
        Debug.Log("Pathfinding working or not");
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;
        if (destructibleCrate != null)
            Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(), true);
    }
}
