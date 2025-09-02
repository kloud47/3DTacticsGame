using System;
using Game.Units;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform ragdollPrefab;
    [SerializeField] private Transform originalRootBone;
    
    
    private HealthSysem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSysem>();

        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        UnitRagdollBehav unitRagdoll = ragdollTransform.GetComponent<UnitRagdollBehav>();
        unitRagdoll.Setup(originalRootBone);
    }
}
