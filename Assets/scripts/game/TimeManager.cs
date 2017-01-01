using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ProjectLine
{
    public class TimeManager : MonoBehaviour
    {
        public float pauseTime = 0;
        [SerializeField]
        private Text timeText;
        [SerializeField]
        private Image TimeUpImage;
        [SerializeField]
        private Image PauseUpImage;
        private float time = Constants.TIME_GAME_HARCODED;
        private bool pauseUp = false;

        public float Timer
        {
            get
            {
                return time;
            }
            set
            {
                time += value;
                timeText.text = "Tiempo : " + ((int)time).ToString();

                if (value > (Constants.TIME_GREEN * (Constants.SCORE_GREEN * 3))) FeedBackScoreTime.Instance.flashingTime();
            }
        }

        #region Singletone Pattern

        private static TimeManager instance;

        public static TimeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<TimeManager>();
                }
                return instance;
            }
        }

        #endregion

        void Update()
        {
            if (GameManager.GetInstance.isGameRunning)
            {
                if (pauseTime <= 0)
                {
                    if (pauseUp)
                    {
                        TimeUpImage.fillAmount = 0;
                        pauseUp = false;
                        PauseUpImage.gameObject.SetActive(pauseUp);
                    }
                    if (time > 0)
                    {
                        time -= Time.deltaTime;
                        timeText.text = "Tiempo : " + ((int)time).ToString();
                        if (time <= 0)
                        {
                            GameManager.GetInstance.EndGame();
                        }
                    }
                }
                else
                {
                    TimeUpUpdate();
                    pauseTime -= Time.deltaTime;
                }
            }
        }    
        
        private void TimeUpUpdate()
        {
            if (!pauseUp)
            {
                pauseUp = true;
                PauseUpImage.gameObject.SetActive(pauseUp);
            }
            TimeUpImage.fillAmount = Mathf.Clamp01(pauseTime / Constants.TIME_PAUSEUP);
        }

        void Start()
        {
            CheckWarning();
        }

        private void CheckWarning()
        {
            if(time<6)
            {
                if (GameManager.GetInstance.isGameRunning)
                    FXManager.Instance.playWarning();
            }
            if(time>0)
            {
                StartCoroutine(waiter());
            }
        }

        private IEnumerator waiter()
        {
            yield return new WaitForSeconds(0.8f);
            CheckWarning();
        }
    }
}