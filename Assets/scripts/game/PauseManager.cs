using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ProjectLine
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField]
        private Image pauseButton;

        public bool pause { get; private set; }

        #region Singletone Pattern

        private static PauseManager instance;

        public static PauseManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<PauseManager>();
                }
                return instance;
            }
        }

        #endregion

        public void Pause()
        {
            pause = !pause;

            if (pause)
            {
                FXManager.Instance.PauseMusic();
                Time.timeScale = 0;
                pauseButton.color = Color.red;
            }
            else
            {
                FXManager.Instance.PlayMusic();
                Time.timeScale = 1;
                pauseButton.color = Color.white;
            }
        }
    }
}