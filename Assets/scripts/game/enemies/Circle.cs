using UnityEngine;
using System.Collections;
namespace ProjectLine
{
    public class Circle : Enemy
    {
        [SerializeField]
        private int timeCandence = 3;
        [SerializeField]
        private BombEnemy bomb = null;

        public override void Init(Transform transform, Vector2 position)
        {
            base.Init(transform, position);
            StartCoroutine(Fire());
        }

        protected override void Update()
        {
            base.Update();
            if (state == State.Run)
            {
                if (!IsInGameArea(transform.position))
                {
                    DoDestroy(false);
                }
            }
        }

        protected IEnumerator Fire()
        {
            while (true)
            {
                if (state == State.Run)
                {
                    BombEnemy bombInstance = UnityEngine.Object.Instantiate<BombEnemy>(bomb);
                    bombInstance.Init(bombInstance.transform, transform.position);
                }
                yield return new WaitForSeconds(timeCandence + UnityEngine.Random.Range(-1, 1));

            }
        }
    }
}