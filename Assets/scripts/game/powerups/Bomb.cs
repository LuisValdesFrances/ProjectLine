using UnityEngine;
using System.Collections;

namespace ProjectLine
{
    public class Bomb : PowerUpElement
    {

        public override void consecuence()
        {
            if(!isUsed)
            {
                base.consecuence();
                Handheld.Vibrate();
                Enemy[] allEnemies = UnityEngine.Object.FindObjectsOfType<Enemy>();
                int score = allEnemies.Length;
                foreach (Enemy enemy in allEnemies)
                {
                    enemy.DoDestroy(true);
                }

                ScoreManager.Instance.killed += score;
                int plus = (score + (Constants.SCORE_BOMB * score)) * ((ScoreManager.Instance.doubleScore) ? 2 : 1);
                ScoreManager.Instance.Score = plus;
                TimeManager.Instance.Timer = plus * Constants.TIME_GREEN;
                FXManager.Instance.playBomb();
                GameManager.GetInstance.Flash();
                DoDestroy(false);
            }
        }
    }
}
