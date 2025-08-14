using UnityEngine;

public class SpinAction : MonoBehaviour
{
    private float totalSpinAmt;
    private bool isActive;
    
    private void Update()
    {
        if (!isActive) return;
        
        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
        totalSpinAmt += spinAddAmount;
        if (totalSpinAmt >= 360f)
        {
            isActive = false;
        }
    }

    public void Spin()
    {
        isActive = true;
        totalSpinAmt = 0f;
    }
}
