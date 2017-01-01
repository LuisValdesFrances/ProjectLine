using UnityEngine;
using System.Collections;
using System;

namespace ProjectLine
{
    public abstract class GameElement : MonoBehaviour
    {
        [SerializeField]
        protected float startSpeed = 3;
        [SerializeField]
        protected float endSpeed = 0.2f;
        [SerializeField]
        protected float timeDecelerarion = 1.5f;
        [SerializeField]
        protected float speedToBig = 4f;
        [SerializeField]
        protected float speedToSmall = 8f;

        protected float angle;
        protected float currentSpeed;
        protected float randomAngleDelta = 90;
        protected Vector2 lastPosition;

        public delegate void DestroyEventElement(GameElement element);
        public event DestroyEventElement OnDestroyed;//Event is a delegate type and it will call width a delegate param

        public enum State { Init, Run, Pause, Dead};
        public State state;//{ get;  protected set; }
        public State lastState;//{ get;  protected set; }

        private float speedInterpolation;

        public virtual void Init(Transform transform, Vector2 position)
        {
            //Debug.Log("Call Init: " + this.GetType());
            this.gameObject.SetActive(true);
            this.state = State.Init;
            this.lastState = state;
            this.speedInterpolation = 0;
            this.transform.position = position;

            Vector2 diference = Camera.main.transform.position - transform.position;
            float sign = (Camera.main.transform.position.y < transform.position.y) ? -1.0f : 1.0f;
            this.angle = Vector2.Angle(Vector2.right, diference) * sign;

            this.angle += UnityEngine.Random.Range(-randomAngleDelta, randomAngleDelta);

            transform.localScale = new Vector2(0, 0);
        }

        public virtual void DoDestroy(bool instanceParticles)
        {

        }

        protected void Destroy()
        {
            this.gameObject.SetActive(false);
            if (OnDestroyed != null)
            {
                OnDestroyed(this);
            }
        }

        protected virtual void Update()
        {

            switch(state)
            {
                case State.Pause:
                    break;
                case State.Run:
                    float rate = 1f / timeDecelerarion;
                    speedInterpolation += Time.deltaTime * rate;

                    float t = (float)Mathf.Min(speedInterpolation, 1f);

                    currentSpeed = Mathf.Lerp(startSpeed, endSpeed, t);
                    break;
            }
        }

        protected IEnumerator DoToBig(Transform transform, float speed, float targetScale, Action callback)
        {
            while(transform.localScale.x < targetScale)
            {
                transform.localScale = 
                    new Vector2(transform.localScale.x + speed * Time.deltaTime, transform.localScale.y + speed * Time.deltaTime);
                yield return null;
            }
            transform.localScale = new Vector2(targetScale, targetScale);
            if(callback != null)
            {
                callback();
            }
        }

        protected IEnumerator DoToSmall(Transform transform, float speed, float targetScale, Action callback)
        {
            while (transform.localScale.x > targetScale)
            {
                transform.localScale =
                    new Vector2(transform.localScale.x - speed * Time.deltaTime, transform.localScale.y - speed * Time.deltaTime);
                yield return null;
            }
            transform.localScale = new Vector2(targetScale, targetScale);
            if (callback != null)
            {
                callback();
            }

        }

        protected bool IsInGameArea(Vector2 position)
        {
            return GameManager.GetInstance.
                GetRectGameDimension(Constants.GAME_AREA_MARGIN).Contains(position);
        }

        protected void ChangeDirection()
        {
            transform.position = lastPosition;
            angle += (180 + UnityEngine.Random.Range(-45, 45)) % 360;
            speedInterpolation = 0;
        }
    }
}
