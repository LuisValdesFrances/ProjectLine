using UnityEngine;
using System.Collections;

namespace ProjectLine
{
    public class particlesRed : MonoBehaviour
    {
        private ParticleSystem ps;

        void Start()
        {
            ps = GetComponent<ParticleSystem>();
        }

        void Update()
        {
            if (ps)
            {
                if (!ps.IsAlive())
                {
                    Destroy(gameObject);
                }
            }
        }

        public void init(Vector2 position)
        {
            transform.position = position;
        }
    }
}
