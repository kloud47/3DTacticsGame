using Game.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UnitWorldUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI actionPointsText;
        [SerializeField] private Unit unit;
        [SerializeField] private Image healthBarImage;
        [SerializeField] private HealthSysem healthSystem;

        private void Start()
        {
            Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
            healthSystem.HealthChanged += healthSystem_OnHealthChanged;
            UpdateActionPointsText();
            UpdateHealthBar();
        }

        private void UpdateActionPointsText()
        {
            actionPointsText.text = unit.GetActionPoints().ToString();
        }

        private void Unit_OnAnyActionPointsChanged(object sender, System.EventArgs e)
        {
            UpdateActionPointsText();
        }

        private void UpdateHealthBar()
        {
            healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
        }

        private void healthSystem_OnHealthChanged(object sender, System.EventArgs e)
        {
            UpdateHealthBar();
        }
    }
}
