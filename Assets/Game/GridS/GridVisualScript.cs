using System;
using Game.Grids;
using UnityEngine;

public class GridVisualScript : MonoBehaviour
{
    [SerializeField] private Transform gridSystemVisualPrefab;

    private void Start()
    {
        // gridSystemVisualSingleArray = new GridSystemVisualSingle[
        //     LevelGrid.Instance.GetWidth(),
        //     LevelGrid.Instance.GetHeight()
        // ];

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                Transform gridSystemVisualSingleTransform = 
                    Instantiate(gridSystemVisualPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                // gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }

    }
}
