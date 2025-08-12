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
        UnitConstrolSystem.Instance.OnSelectedUnitChanged += UnitControlSystem_OnSelectedUnitChanged;
        UpdateVisual();
    }

    private void UnitControlSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (UnitConstrolSystem.Instance.GetSelectedUnit() == unit)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
}
