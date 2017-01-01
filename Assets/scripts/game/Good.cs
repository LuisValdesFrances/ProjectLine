using UnityEngine;
using System.Collections;

namespace ProjectLine
{
    public class Good : GameElement
    {
        [SerializeField]
        private SpriteRenderer unSelect = null;
        [SerializeField]
        private SpriteRenderer select = null;

        private bool isSelect;

        public void SetPause(bool pause)
        {
            isSelect = pause;
            unSelect.enabled = !pause;
            select.enabled = pause;
        }

        void Awake()
        {
            this.unSelect.enabled = false;
            this.select.enabled = false;
            this.GetComponent<Collider2D>().enabled = false;
        }

        public override void Init(Transform transform, Vector2 position)
        {
            base.Init(this.transform, position);
            this.GetComponent<Collider2D>().enabled = true;
            unSelect.enabled = true;
            select.enabled = false;
            isSelect = false;
            StartCoroutine(
                    DoToBig(this.transform, speedToBig, 1.75f,
                    () => StartCoroutine(
                        DoToSmall(this.transform, speedToSmall, 1,
                        () => state = State.Run))));
        }

        protected override void Update()
        {
            base.Update();
            switch (state)
            {
                case State.Init:
                    break;
                case State.Run:
                    if(!isSelect)
                    {
                        if (!base.IsInGameArea(transform.position))
                        {
                            base.ChangeDirection();
                        }
                        lastPosition = transform.position;
                        Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
                        transform.Translate(direction * currentSpeed * Time.deltaTime, Space.World);
                    }
                    break;
                case State.Dead:
                    break;
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(state == State.Run)
            {
                //TODO: Es una mierda, pero de momento sirve
                if (
                    (currentSpeed == endSpeed 
                    && other.gameObject.GetComponent<Portal>())
                    /*|| other.gameObject.GetComponent<Good>()*/)
                {
                    base.ChangeDirection();
                }
                else
                {
                    if (other.GetComponent<LineCollision>() != null)
                    {
                        SetPause(true);
                    }
                    else if (other.GetComponent<Enemy>() != null)
                    {
                        ParticlesSpawner.Instance.generateParticleGreenDie(transform.position);
                        DoDestroy(false);
                    }
                }
            }
        }

        public override void DoDestroy(bool instanceParticles)
        {
            if (state != State.Dead)
            {
                if(instanceParticles)
                    ParticlesSpawner.Instance.generateParticleGreen(transform.position);
                FXManager.Instance.playGood();

                StopAllCoroutines();//TODO: Peligro
                transform.localScale = new Vector3(1f, 1f, 1f);
                state = State.Dead;
                StartCoroutine(
                    DoToBig(this.transform, speedToBig*2, 2f,
                    () => StartCoroutine(
                        DoToSmall(this.transform, speedToSmall * 2, 0,
                        () => Destroy()))));
            }
        }
    }
}
