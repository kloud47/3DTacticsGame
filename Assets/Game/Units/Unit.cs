using UnityEngine;

public class Unit : MonoBehaviour
{
   [SerializeField] private Animator unitAnimator;
   private Vector3 targetPosition;
   [SerializeField] float moveSpeed = 5f;

   private void Awake()
   {
      targetPosition = transform.position;
   }
   
   private void Update()
   {
      float stoppingDistance = 0.1f;
      if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
      {
         Vector3 moveDirection = (targetPosition - transform.position).normalized;
         transform.position += moveDirection * (Time.deltaTime * moveSpeed);
         float rotationSpeed = 15f;
         transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
         unitAnimator.SetBool("IsWalking", true);
      }
      else
      {
         unitAnimator.SetBool("IsWalking", false);
      }
   }
   
   public void Move(Vector3 targetPosition)
   {
      this.targetPosition = targetPosition;
   }
}
