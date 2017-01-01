using UnityEngine;
using System.Collections;

namespace ProjectLine
{
    public class ParticlesSpawner : MonoBehaviour
    {
        [SerializeField]
        particlesRed particlesred;
        [SerializeField]
        particlesGreen particlesgreen;
        [SerializeField]
        particlesGreenDie particlesgreenDie;

        #region Singletone Pattern

        private static ParticlesSpawner instance;

        public static ParticlesSpawner Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<ParticlesSpawner>();
                }
                return instance;
            }
        }

        #endregion

        public void generateParticleRed(Vector2 pos)
        {
            particlesRed p = UnityEngine.Object.Instantiate<particlesRed>(particlesred);
            p.init(new Vector3(pos.x, pos.y, particlesred.transform.position.x));
        }

        public void generateParticleGreen(Vector2 pos)
        {
            particlesGreen p = UnityEngine.Object.Instantiate<particlesGreen>(particlesgreen);
            p.init(new Vector3(pos.x, pos.y, particlesgreen.transform.position.x));
        }

        public void generateParticleGreenDie(Vector2 pos)
        {
            particlesGreenDie p = UnityEngine.Object.Instantiate<particlesGreenDie>(particlesgreenDie);
            p.init(new Vector3(pos.x, pos.y, particlesgreenDie.transform.position.x));
        }
    }
}