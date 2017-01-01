using UnityEngine;
using System.Collections;

namespace ProjectLine
{
    public class Enemy : GameElement
    {
        [SerializeField]
        protected SpriteRenderer sprite;
        [SerializeField]
        private int healt = 1;
        
        public int currentHealth{ get; protected set; }

        public delegate void ColisionLine();
        public event ColisionLine OnColisionLine;

        void Awake()
        {
            this.sprite.enabled = false;
            this.GetComponent<Collider2D>().enabled = false;
        }

        public override void Init(Transform tansform, Vector2 position)
        {
            base.Init(transform, position);
            OnColisionLine += GameManager.GetLineManagerInstance().RemoveLine;
            OnColisionLine += GameManager.GetLineManagerInstance().lineCollision.clearLine;
            this.sprite.enabled = true;
            this.GetComponent<Collider2D>().enabled = true;
            this.currentHealth = healt;
            StartCoroutine(
                    DoToBig(this.transform, speedToBig, 1.75f, () => StartCoroutine(
                        DoToSmall(this.transform, speedToSmall, 1,
                        () => state = State.Run))));
        }

        protected override void Update()
        {
            base.Update();
            switch (state)
            {
                case State.Pause:
                    if (TimeManager.Instance.pauseTime <= 0)
                    {
                        state = lastState;
                    }
                    break;
                case State.Run:
                    lastPosition = transform.position;
                    Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
                    transform.Translate(direction * currentSpeed * Time.deltaTime, Space.World);


                    if (TimeManager.Instance.pauseTime > 0 && state == State.Run)
                    {
                        lastState = state;
                        state = State.Pause;
                    }
                    break;
                case State.Dead:
                    break;
            }
        }

        public virtual void DoDamage(int damage)
        {
            if (state == State.Run || state == State.Pause)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                StopAllCoroutines();//TODO: Peligro
                currentHealth -= damage;
                if(currentHealth <= 0)
                {
                    DoDestroy(true);
                }
                else
                {
                    state = State.Pause;
                    StartCoroutine(
                    DoToBig(this.transform, speedToBig*3f, 1.5f, 
                    () => StartCoroutine(
                        DoToSmall(this.transform, speedToSmall * 3f, 1f, 
                        () => state = State.Run))));
                }
            }
        }

        public override void DoDestroy(bool instanceParticles)
        {
            if (state != State.Dead)
            {
                if (instanceParticles)
                    ParticlesSpawner.Instance.generateParticleRed(transform.position);

                state = State.Dead;
                transform.localScale = new Vector3(1f, 1f, 1f);
                StopAllCoroutines();//TODO: Peligro

                OnColisionLine -= GameManager.GetLineManagerInstance().RemoveLine;
                OnColisionLine -= GameManager.GetLineManagerInstance().lineCollision.clearLine;
                
                StartCoroutine(
                    DoToBig(this.transform, speedToBig * 2, 2f,
                    () => StartCoroutine(DoToSmall(this.transform, speedToSmall * 2, 0,
                        () => Destroy()))));
            }
        }

        protected void DestroyGameObject()
        {
            OnColisionLine -= GameManager.GetLineManagerInstance().RemoveLine;
            OnColisionLine -= GameManager.GetLineManagerInstance().lineCollision.clearLine;
            Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            //TODO: Es una mierda, pero de momento sirve
            if (state == State.Run || state == State.Pause)
            {
                if (other.GetComponent<BombEnemy>())
                {
                    if(!other.GetComponent<BombEnemy>().isWaiting)
                        DoDestroy(true);
                }
                else if (
                    (currentSpeed == endSpeed
                    && other.gameObject.GetComponent<Portal>())
                    || 
                    (other.gameObject.GetComponent<Enemy>() && !other.gameObject.GetComponent<Bullet>())
                    )
                {
                    base.ChangeDirection();
                }
                else if(other.GetComponent<LineCollision>())
                {
                    SendCollisionLineEvent();
                }
            }
        }

        protected void SendCollisionLineEvent()
        {
            if (OnColisionLine != null)
                OnColisionLine();
        }
    }
}