using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;
    
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        // Subscribing to the event:
        UnitControlSystem.Instance.OnSelectedUnitChanged += UnitControlSystem_OnSelectedUnitChanged;
        UpdateVisual();
    }

    private void UnitControlSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (UnitControlSystem.Instance.GetSelectedUnit() == unit)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
}
