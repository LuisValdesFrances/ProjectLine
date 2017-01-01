using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ProjectLine
{
    public class GameOverManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject panel;
        [SerializeField]
        private Text rescue;
        [SerializeField]
        private Text killed;
        [SerializeField]
        private Text points;
        [SerializeField]
        private Image colorStar1;
        [SerializeField]
        private Image colorStar2;
        [SerializeField]
        private Image colorStar3;
        [SerializeField]
        private Image colorTrophy;

        private int harcodedOneStarLevel1 = 5;
        private int harcodedTwoStarLevel1 = 10;
        private int harcodedThreeStarLevel1 = 15;
        private int harcodedTrophyLevel1 = 30;

        #region Singletone Pattern

        private static GameOverManager instance;

        public static GameOverManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<GameOverManager>();
                }
                return instance;
            }
        }

        #endregion

        public void Init()
        {

            rescue.text = "Rescatados   :   " + ScoreManager.Instance.rescue;
            killed.text = "Asesinados   :   " + ScoreManager.Instance.killed;
            points.text = "Puntos   :   " + ScoreManager.Instance.Score;

            Color colorBad = new Color(colorStar1.color.r, colorStar1.color.g, colorStar1.color.b, 0.35f);

            if (ScoreManager.Instance.Score < harcodedOneStarLevel1)
            {
                colorStar1.color = colorBad;
                colorStar2.color = colorBad;
                colorStar3.color = colorBad;
                colorTrophy.color = colorBad;
            }
            else if (ScoreManager.Instance.Score < harcodedTwoStarLevel1)
            {
                colorStar2.color = colorBad;
                colorStar3.color = colorBad;
                colorTrophy.color = colorBad;
            }
            else if (ScoreManager.Instance.Score < harcodedThreeStarLevel1)
            {
                colorStar3.color = colorBad;
                colorTrophy.color = colorBad;
            }
            else if (ScoreManager.Instance.Score < harcodedTrophyLevel1)
            {
                colorTrophy.color = colorBad;
            }

            panel.SetActive(true);
        }
    }
}
