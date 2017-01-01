using UnityEngine;
using System.Collections;
using System;

namespace ProjectLine
{
    public class Portal : GameElement
    {
        private GameElement[] listGameElements;
        [SerializeField]
        private SpriteRenderer sprite = null;

        private bool spawn;
        private int elementIndex;
        public enum Type { Red, Yellow };
        public Type type { get; protected set; }

        public void Init(GameElement[] elements, Vector2 position, Type type)
        {
            base.Init(sprite.transform, position);
            this.type = type;
            this.listGameElements = elements;

            StartCoroutine(
                   DoToBig(sprite.transform, speedToBig, 1.75f, 
                   () => state = State.Run));
            elementIndex = 0;
            spawn = false;
        }

        protected override void Update()
        {
           base.Update();


            switch (state)
            {
                case State.Pause:
                    if (this.tag == "PortalRed" &&  TimeManager.Instance.pauseTime <= 0)
                    {
                        state = lastState;
                    }
                    break;
                case State.Run:
                    if (elementIndex < listGameElements.Length)
                    {
                        if (!spawn)
                        {
                            spawn = true;
                            listGameElements[elementIndex].
                                Init(listGameElements[elementIndex].transform, transform.position);
                        }
                        else
                        {
                            if (listGameElements[elementIndex].state == GameElement.State.Run
                                ||
                                listGameElements[elementIndex].state == GameElement.State.Dead)
                            {
                                elementIndex++;
                                spawn = false;
                            }
                        }
                    }
                    else
                    {
                        state = State.Dead;
                        StartCoroutine(
                                DoToSmall(sprite.transform, speedToSmall, 0, () => Destroy()));
                    }


                    if (this.tag == "PortalRed" && TimeManager.Instance.pauseTime > 0 && state == State.Run)
                    {
                        lastState = state;
                        state = State.Pause;
                    }
                    break;
                case State.Dead:
                    break;
            }


            {
                angle += 500 * Time.deltaTime;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
                    () => StartCoroutine(DoToSmall(this.transform, speedToSmall * 2, 0,
                        () => Destroy()))));
            }
        }
    }
}
