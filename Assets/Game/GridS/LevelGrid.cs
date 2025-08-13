using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }
    
    [SerializeField] private Transform gridDebugObjPrefab;
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
        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateDebugObjects(gridDebugObjPrefab);
    }

    public void SetUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.SetUnit(unit);
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    public void ClearUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.SetUnit(null);
    }
    
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
}
