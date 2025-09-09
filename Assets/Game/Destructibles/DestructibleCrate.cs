using System;
using Game.Grids;
using UnityEngine;

namespace Game.Destructibles
{
    public class DestructibleCrate : MonoBehaviour
    {
        public static event EventHandler OnAnyDestroyed;

        [SerializeField] private Transform crateDestroyedPrefab;
        // [SerializeField] private float debrisLifeTime = 3f;
    
        private GridPosition gridPosition;

        private void Start()
        {
            gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        }

        public GridPosition GetGridPosition()
        {
            return gridPosition;
        }
        public void Damage()
        {
            // Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);
        
            // ApplyExplosionToChildren(crateDestroyedTransform, 250f, transform.position, 10f);
        
            Destroy(gameObject);
        
            //  Start debris destruction timer:
            // Destroy(crateDestroyedTransform.gameObject, debrisLifeTime);
        
            OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
        }
    
        private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
        {
            foreach (Transform child in root)
            {
                if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
                {
                    childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
                }

                ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
            }
        }
    }
}
