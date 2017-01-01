using UnityEngine;
using System.Collections;
using System;

namespace ProjectLine
{
    public class Triangle : Enemy
    {
        [SerializeField]
        private float speedRotation = 120;
        [SerializeField]
        private float timeCandence = 3;
        [SerializeField]
        private Bullet bullet = null;
        /*
        [SerializeField]
        [Range(-1, 1)]
        private int timeVariationCandence = 1;
        */

        private float spriteAngle;

        public override void Init(Transform transform, Vector2 position)
        {
            base.Init(transform, position);
            StartCoroutine(Fire());
        }

        protected override void Update()
        {
            base.Update();
            if(state == State.Run)
            {
                if (!IsInGameArea(transform.position))
                {
                    DoDestroy(false);
                }
                spriteAngle = (spriteAngle + (speedRotation * Time.deltaTime)) % 360;
                sprite.transform.eulerAngles = new Vector3(0, 0, spriteAngle);
            }
        }

        protected IEnumerator Fire()
        {
            while (true)
            {
                if((state == State.Run))
                {
                    Bullet bulletInstance = UnityEngine.Object.Instantiate<Bullet>(bullet);
                    bulletInstance.Init(transform.position, spriteAngle);
                }
                yield return new WaitForSeconds(timeCandence + UnityEngine.Random.Range(-1, 1));

            }
        }
    }
}
