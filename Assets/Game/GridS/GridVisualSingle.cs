using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer gridRenderer;

    public void Show()
    {
        gridRenderer.enabled = true;
    }

    public void Hide()
    {
        gridRenderer.enabled = false;
    }
}
