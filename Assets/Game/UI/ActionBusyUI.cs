using UnityEngine;

namespace Game.UI
{
    public class ActionBusyUI : MonoBehaviour
    {
        private void Start()
        {
            UnitControlSystem.Instance.OnBusyChanged += UnitControlSystem_OnBusyChanged;
            Hide();
        
        }
    
        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void UnitControlSystem_OnBusyChanged(object sender, bool isBusy)
        {
            if (isBusy)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
    }
}
