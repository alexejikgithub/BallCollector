using System;
using UnityEngine;

namespace BallCollector.Gameplay
{
    public class CollectableItem : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        [SerializeField] private MeshFilter _meshFilter;

        [SerializeField] private float _volume;

        public float Volume => _volume;


        public void BecomeCollected()
        {
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            Debug.Log("!!!!!!!!!!!");
            if (_meshFilter != null)
            {
                var localScale = transform.localScale;
                _volume = VolumeOfMesh(_meshFilter.mesh) * (localScale.x + localScale.y + localScale.z) / 3;
            }
        }

        float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float v321 = p3.x * p2.y * p1.z;
            float v231 = p2.x * p3.y * p1.z;
            float v312 = p3.x * p1.y * p2.z;
            float v132 = p1.x * p3.y * p2.z;
            float v213 = p2.x * p1.y * p3.z;
            float v123 = p1.x * p2.y * p3.z;
            return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
        }

        float VolumeOfMesh(Mesh mesh)
        {
            float volume = 0;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                Vector3 p1 = vertices[triangles[i + 0]];
                Vector3 p2 = vertices[triangles[i + 1]];
                Vector3 p3 = vertices[triangles[i + 2]];
                volume += SignedVolumeOfTriangle(p1, p2, p3);
            }

            return Mathf.Abs(volume);
        }

#endif
    }
}