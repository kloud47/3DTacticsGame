using UnityEngine;

public class Unit : MonoBehaviour
{
   private Vector3 targetPosition;

   private void Update()
   {
      float stoppingDistance = 0.1f;
      if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
      {
         Vector3 moveDirection = (targetPosition - transform.position).normalized;
         float moveSpeed = 5f;
         transform.position += moveDirection * Time.deltaTime;
      }

      if (Input.GetMouseButton(0))
      {
         Move(MouseMovement.GetPosition());
      }
   }
   
   private void Move(Vector3 targetPosition)
   {
      this.targetPosition = targetPosition;
   }
}
