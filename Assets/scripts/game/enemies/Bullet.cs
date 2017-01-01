using UnityEngine;
using System.Collections;

namespace ProjectLine
{
    public class Bullet : Enemy
    {
        public void Init(Vector2 position, float angle)
        {
            base.Init(this.transform, position);
            this.sprite.transform.eulerAngles = new Vector3(0, 0, angle);
            this.angle = angle;
        }

        protected override void Update()
        {
            base.Update();
            if(state == State.Run)
            {
                if (!IsInGameArea(transform.position))
                {
                    StopAllCoroutines();
                    state = State.Dead;
                    StartCoroutine(
                                DoToSmall(transform, speedToSmall/2f, 0, () => DestroyGameObject()));
                }
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (state == State.Run || state == State.Pause)
            {
                if (other.GetComponent<Good>() != null)
                {
                    StopAllCoroutines();
                    state = State.Dead;
                    StartCoroutine(
                            DoToSmall(transform, speedToSmall/2f, 0, () => DestroyGameObject()));
                }
                else if (other.GetComponent<LineCollision>())
                {
                    SendCollisionLineEvent();
                }
            }
        }

    }



}
