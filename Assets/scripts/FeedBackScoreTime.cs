using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ProjectLine
{
    public class FeedBackScoreTime : MonoBehaviour
    {
        [SerializeField]
        private Text scoreText;
        private bool scoreTextBusy = false;

        [SerializeField]
        private Text timeText;
        private bool timeTextBusy = false;

        [SerializeField]
        private Text comboText;
        private bool comboTextBusy = false;


        #region Singletone Pattern

        private static FeedBackScoreTime instance;

        public static FeedBackScoreTime Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<FeedBackScoreTime>();
                }
                return instance;
            }
        }

        #endregion

        public void flashingCombo(int combo)
        {
            if(!comboTextBusy)
            {
                comboText.text = "COMBO x " + combo.ToString();
                comboTextBusy = true;
                StartCoroutine(flash(comboText, new Color(0,0,0,0)));
            }
        }

        public void flashingScore()
        {
            if(!scoreTextBusy)
            {
                scoreTextBusy = true;
                StartCoroutine(flash(scoreText, Color.white));
            }
        }

        public void flashingTime()
        {
            if (!timeTextBusy)
            {
                timeTextBusy = true;
                StartCoroutine(flash(timeText, Color.white));
            }
        }

        private IEnumerator flash(Text text, Color originalColor)
        {
            float waitTime = 0.055f;
            text.color = Color.green;
            text.fontSize += 1;

            for(int i=0; i<7; i++)
            {
                yield return new WaitForSeconds(waitTime);
                if (i<3)
                {
                    text.fontSize += 1;
                }
                else
                {
                    text.fontSize -= 1;
                }
            }

            yield return new WaitForSeconds(waitTime);
            text.color = originalColor;

            scoreTextBusy = false;
            timeTextBusy = false;
            comboTextBusy = false;
        }
    }
}
