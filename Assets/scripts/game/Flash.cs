using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ProjectLine
{
    public class Flash : MonoBehaviour
    {
        private Image image;
        private enum State { Unactive, ToAlpha, ToZero };
        private State state;

        private float toAlphaTime = 0.1f;
        private float toZeroTime = 0.6f;

        private float alphaCount;
        void Awake()
        {
            image = GetComponent<Image>();
            image.enabled = false;
            alphaCount = 0;
            image.color = new Color(1f, 1f, 1f, 0);
            state = State.Unactive;
        }

        public void Run()
        {
            state = State.ToAlpha;
            image.enabled = true;
        }

        void Update()
        {
            switch (state)
            {
                case State.ToAlpha:

                    float rate = 1f / toAlphaTime;
                    alphaCount += Time.deltaTime * rate;
                    float t = (float)Mathf.Min(alphaCount, 1f);
                    image.color = new Color(1f, 1f, 1f, Mathf.Lerp(0, 0.85f, t));
                    if(t >= 1)
                    {
                        alphaCount = 0;
                        state = State.ToZero;
                    }
                    break;
                case State.ToZero:
                    rate = 1f / toZeroTime;
                    alphaCount += Time.deltaTime * rate;
                    t = (float)Mathf.Min(alphaCount, 1f);
                    image.color = new Color(1f, 1f, 1f, Mathf.Lerp(0.85f, 0, t));
                    if (t >= 1)
                    {
                        alphaCount = 0;
                        state = State.Unactive;
                        image.enabled = false;
                    }
                    break;
            }
        }
    }
}
