using UnityEngine;
using System.Collections;

namespace ProjectLine
{
    public abstract class PowerUpElement : GameElement
    {
        
        [SerializeField]
        protected SpriteRenderer sprite;

        protected bool isUsed = false;

        void Awake()
        {
            this.sprite.enabled = false;
            this.GetComponent<BoxCollider2D>().enabled = false;
        }

        public override void Init(Transform transform, Vector2 position)
        {
            base.Init(this.transform, position);
            this.sprite.enabled = true;
            this.GetComponent<BoxCollider2D>().enabled = true;
            this.angle = UnityEngine.Random.Range(0, 360);

            StartCoroutine(
                    DoToBig(this.transform, speedToBig, 1.75f,
                    () => StartCoroutine(DoToSmall(this.transform, speedToSmall, 1,
                        () => state = State.Run))));
        }

        public virtual void consecuence()
        {
            if(state == State.Run)
            {
                isUsed = true;
                PowerUpManager.Sustract();
            }
        }

        protected override void Update()
        {
            base.Update();
            switch (state)
            {
                case State.Init:
                    break;
                case State.Run:
                    if (!IsInGameArea(transform.position))
                    {
                        transform.position = lastPosition;
                        angle += (180 + UnityEngine.Random.Range(-45, 45)) % 360;
                    }
                    lastPosition = transform.position;
                    Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
                    transform.Translate(direction * currentSpeed * Time.deltaTime, Space.World);
                    break;
                case State.Dead:
                    break;
            }
        }

        public override void DoDestroy(bool instanceParticles)
        {
            if (state != State.Dead)
            {
                state = State.Dead;
                transform.localScale = new Vector3(1f, 1f, 1f);
                StopAllCoroutines();//TODO: Peligro

                StartCoroutine(
                    DoToBig(this.transform, speedToBig * 2, 2f,
                    () => StartCoroutine(
                        DoToSmall(this.transform, speedToSmall * 2, 0,
                        () => Destroy(this.gameObject)))));
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            //TODO: Es una mierda, pero de momento sirve
            if (state == State.Run || state == State.Pause)
            {
                if (other.gameObject.GetComponent<GameElement>())
                {
                    base.ChangeDirection();
                }
            }
        }
    }
}