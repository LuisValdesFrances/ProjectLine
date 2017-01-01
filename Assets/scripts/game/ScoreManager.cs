using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ProjectLine
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField]
        private Text scoreText;
        [SerializeField]
        private Image doubleImage;
        [SerializeField]
        private Text x2;
        private int score = 0;
        public bool doubleScore;
        private float timeDoubleScore = 0;
        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score += value;
                scoreText.text = "Puntos : " + score.ToString();
                if(value>0) FeedBackScoreTime.Instance.flashingScore();
            }
        }

        public int rescue = 0;
        public int killed = 0;

        #region Singletone Pattern

        private static ScoreManager instance;

        public static ScoreManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<ScoreManager>();
                }
                return instance;
            }
        }

        #endregion

        public void activateDouble()
        {
            x2.gameObject.SetActive(true);
            doubleScore = true;
            timeDoubleScore = Constants.TIME_DOUBLE_SCORE;
        }

        public void deactivateDouble()
        {
            x2.gameObject.SetActive(false);
            doubleScore = false;
            timeDoubleScore = 0;
        }

        void Update()
        {
            if (timeDoubleScore > 0)
            {
                timeDoubleScore -= Time.deltaTime;
                doubleImage.fillAmount = Mathf.Clamp01(timeDoubleScore/Constants.TIME_DOUBLE_SCORE);
                if (timeDoubleScore <= 0)
                {
                    deactivateDouble();
                }
            }
        }
    }
}