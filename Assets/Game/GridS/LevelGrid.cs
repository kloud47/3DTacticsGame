using System.Collections.Generic;
using Game.Grids;
using UnityEngine;

/// <summary>
/// This class uses gridsystem and Unit information to perform visual changes and show the grids:
/// </summary>
public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }
    
    [SerializeField] private Transform gridDebugObjPrefab;
    [SerializeField] private int gridSize = 10;
    [SerializeField] private int cellSize = 10;
    private GridSystem gridSystem;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one instance of LevelGrid found!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // Initializing the Grid System:
        gridSystem = new GridSystem(gridSize, gridSize, cellSize);
        gridSystem.CreateDebugObjects(gridDebugObjPrefab);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnitList(unit);
    }

    public List<Unit> GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnitList(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition formGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(formGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
    }
    
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    
    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }
}
