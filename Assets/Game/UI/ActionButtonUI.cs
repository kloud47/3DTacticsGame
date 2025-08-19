using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextMesh;
    [SerializeField] private Button Button;
    [SerializeField] private GameObject selectedGameObject;

    private BaseAction selectedAction;
    public void SetBaseAction(BaseAction baseAction)
    {
        this.selectedAction = baseAction;
        TextMesh.text = baseAction.GetActionName().ToUpper();
        Button.onClick.AddListener(() =>
        {
            UnitControlSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitControlSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(selectedAction == selectedBaseAction);
    }
}
