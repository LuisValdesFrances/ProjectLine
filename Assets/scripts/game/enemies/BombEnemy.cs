using UnityEngine;
using System.Collections;
using System;

namespace ProjectLine
{
    public class BombEnemy : Enemy
    {
        private float detonationCount;

        private float explosionTime = 1f;
        private float contractionTime = 0.25f;

        [SerializeField]
        protected SpriteRenderer sprite2;
        [SerializeField]
        protected SpriteRenderer sprite1;

        private SpriteRenderer[] sprites;


        private float explosionCount;
        private float explosionRadius;

        private bool idleScale;

        public bool isWaiting{ get; protected set; }

        [SerializeField]
        private SpriteRenderer explosionSprite = null;

        private CircleCollider2D explosionCollider = null;

        public override void Init(Transform transform, Vector2 position)
        {
            base.Init(transform, position);
            this.angle += UnityEngine.Random.Range(0,360);
            this.explosionCollider = GetComponent<CircleCollider2D>();
            this.explosionRadius = this.explosionCollider.radius;
            this.explosionCollider.radius = 0;
            this.explosionSprite.transform.localScale = new Vector2(0, 0);
            this.sprite2.enabled = false;
            this.sprite1.enabled = false;
            isWaiting = true;
            sprites = new SpriteRenderer[3];
            sprites[0] = sprite;
            sprites[1] = sprite2;
            sprites[2] = sprite1;
            detonationCount = 0;
        }

        protected override void Update()
        {
            base.Update();
            if(isWaiting && state == State.Run)
            {
                if(detonationCount < sprites.Length)
                {
                    detonationCount += Time.deltaTime;
                    ShowSprite((int)detonationCount);
                    UpdateScale();
                }
                else
                {
                    isWaiting = false;
                    ShowSprite(-1);
                    StartCoroutine(Expansion());
                    FXManager.Instance.playBomb();
                }
            }
        }

        private void UpdateScale()
        {
            if (idleScale)
            {
                if (transform.localScale.x < 1.2f)
                {
                    transform.localScale += transform.localScale / 64f;
                }
                else
                {
                    idleScale = !idleScale;
                }
            }
            else
            {
                if (transform.localScale.x > 1f)
                {
                    transform.localScale -= transform.localScale / 64f;
                }
                else
                {
                    idleScale = !idleScale;
                }
            }
        }

        private IEnumerator Expansion()
        {
            float t = 0;
            while (explosionCount < explosionTime)
            {
                float rate = 1f / explosionTime;
                explosionCount += Time.deltaTime * rate;
                t = (float)Mathf.Min(explosionCount, 1f);

                explosionSprite.transform.localScale = Vector2.Lerp(explosionSprite.transform.localScale, new Vector2(1, 1), t);
                explosionCollider.radius = Mathf.Lerp(0, explosionRadius, t);
                explosionSprite.color = new Color(1f, 1f, 1f, Mathf.Lerp(1, 0, t));
                yield return null;
            }
            DestroyGameObject();
        }

        private IEnumerator Contraction()
        {
            explosionCount = 0;
            explosionCollider.enabled = false;
            float t = 0;
            while (t < 1f)
            {
                float rate = 1f / contractionTime;
                explosionCount += Time.deltaTime * rate;
                t = (float)Mathf.Min(explosionCount, 1f);

                explosionSprite.transform.localScale = Vector2.Lerp(new Vector2(1, 1), new Vector2(0, 0), t);


                yield return null;
            }
            DestroyGameObject();
        }

        private void ShowSprite(int index)
        {
            for(int i = 0; i < sprites.Length; i++)
            {
                sprites[i].enabled = i == index;
            }
        }

        private SpriteRenderer GetCurrentSprite()
        {
            foreach(SpriteRenderer s in sprites)
            {
                if (s.enabled)
                {
                    return s;
                }
            }
            return null;
        }


        public override void DoDamage(int damage)
        {
            if (isWaiting)
            {
                base.DoDamage(damage);
            }
        }

    }
}
