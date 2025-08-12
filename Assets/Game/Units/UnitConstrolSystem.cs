using UnityEngine;

public class UnitConstrolSystem : MonoBehaviour
{
    [SerializeField] private Unit selectedUnit;
    
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            selectedUnit.Move(MouseMovement.GetPosition());
        }
    }
}
