using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ProjectLine
{
    public class LineCollision : MonoBehaviour
    {
        public List<Good> listGood = new List<Good>();

        public delegate void SpawnPoints(Vector2 position, int quantity);
        public event SpawnPoints OnSpawnPoints;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Good>())
            {
                FXManager.Instance.playUnion();
                listGood.Add(other.gameObject.GetComponent<Good>());
            }
        }

        public void FinisCollision()
        {
            if (listGood.Count>0)
            {
                FXManager.Instance.playGood();
                CheckPowerUps();
            }
            int combo = listGood.Count;

            if (combo > 1) FeedBackScoreTime.Instance.flashingCombo(combo);
            int plus = ((combo*((combo>1)?combo-1:1)*Constants.SCORE_GREEN)*((ScoreManager.Instance.doubleScore)?2:1));
            ScoreManager.Instance.Score = plus;
            ScoreManager.Instance.rescue += combo;
            TimeManager.Instance.Timer = plus * Constants.TIME_GREEN;
            foreach (Good good in listGood)
            {
                good.DoDestroy(true);
            }
            if (OnSpawnPoints != null && listGood.Count > 0)
                OnSpawnPoints(listGood[listGood.Count - 1].transform.position, plus);
            clearLine();
        }

        public void clearLine()
        {
            listGood.Clear();
        }

        private void CheckPowerUps()
        {
            PolygonCollider2D polygonCollider = this.transform.GetComponent<PolygonCollider2D>();
            int i = 0;
            for (int powerUp = (int)Mathf.Ceil(listGood.Count / Constants.MIN_COLLECTED_POWERUP); powerUp > 0; powerUp--)
            {
                if (PowerUpManager.activeNumber < Constants.MAX_POWERUP_ACTIVE)
                {
                    int r = UnityEngine.Random.Range(0, 9);
                    if (r < 4)
                    {
                        PowerUpManager.Instance.SpawnPowerUp(PowerUpManager.PowerUpType.Bomb, listGood[i].transform.position);
                    }
                    else if (r < 6)
                    {
                        PowerUpManager.Instance.SpawnPowerUp(PowerUpManager.PowerUpType.TimeUp, listGood[i].transform.position);
                    }
                    else
                    {
                        PowerUpManager.Instance.SpawnPowerUp(PowerUpManager.PowerUpType.DoubleUp, listGood[i].transform.position);
                    }
                }
                else
                {
                    return;
                }
            }
        }
    }
}
