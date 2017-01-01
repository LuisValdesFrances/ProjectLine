using UnityEngine;
using System.Collections;

namespace ProjectLine
{
    public class PowerUpManager : MonoBehaviour
    {
        [SerializeField]
        private Bomb bomb;
        [SerializeField]
        private TimeUp timeUp;
        [SerializeField]
        private DoubleUp doubleUp;

        public enum PowerUpType { Bomb = 0, TimeUp = 1, DoubleUp = 2 };

        #region Singletone Pattern
        private static PowerUpManager instance;

        public static PowerUpManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<PowerUpManager>();
                    ResetActiveNumber();
                }
                return instance;
            }
        }
        #endregion

        public static int activeNumber { get; protected set; }
        public static void ResetActiveNumber()
        {
            activeNumber = 0;
        }
        public static void Sustract()
        {
            if (activeNumber > 0)
                activeNumber--;
        }

        public void SpawnPowerUp(PowerUpType powerUpType, Vector2 v2)
        {
            GameElement powerUp = null;
            activeNumber++;
            switch (powerUpType)
            {
                case PowerUpType.Bomb:
                    powerUp = UnityEngine.Object.Instantiate<Bomb>(bomb);
                    break;
                case PowerUpType.TimeUp:
                    powerUp = UnityEngine.Object.Instantiate<TimeUp>(timeUp);
                    break;
                case PowerUpType.DoubleUp:
                    powerUp = UnityEngine.Object.Instantiate<DoubleUp>(doubleUp);
                    break;
            }
            if(powerUp)
                powerUp.Init(powerUp.transform, v2);
        }
    }
}