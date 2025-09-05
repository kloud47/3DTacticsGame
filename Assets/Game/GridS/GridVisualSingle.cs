using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer gridRenderer;

    public void Show(Material material)
    {
        gridRenderer.enabled = true;
        gridRenderer.material = material;
    }

    public void Hide()
    {
        gridRenderer.enabled = false;
    }
}
