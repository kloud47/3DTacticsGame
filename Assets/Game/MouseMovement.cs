using System;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    private static MouseMovement instance; // Making a permanent instance in the world:
    [SerializeField] private LayerMask tilePlaneLayerMask;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        transform.position = MouseMovement.GetPosition();
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, float.MaxValue, instance.tilePlaneLayerMask);
        return hit.point;
    }
}
