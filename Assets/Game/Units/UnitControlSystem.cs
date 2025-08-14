using System;
using Game.Grids;
using UnityEngine;

// This class handles all the logic for Unit selection and providing a base for all its other properties:
public class UnitControlSystem : MonoBehaviour
{
    public static UnitControlSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChanged;
    
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one instance of UnitConstrolSystem found!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // check for tile or unit selection:
            if (TryHandleUnitSelection()) return;
            GridPosition mouseSelectedGridPosition = LevelGrid.Instance.GetGridPosition(MouseMovement.GetPosition());
            if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseSelectedGridPosition))
            {
                selectedUnit?.GetMoveAction().Move(mouseSelectedGridPosition); // Elvis operator(for null checks):   
            }
        }
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, unitLayerMask))
        {
            if (hit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
