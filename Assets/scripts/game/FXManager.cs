using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ProjectLine
{
    public class FXManager : MonoBehaviour
    {
        bool mute = false;

        [SerializeField]
        private Image soundImage;

        private float volumeFX = 1;

        [SerializeField]
        private AudioClip bad;


        [SerializeField]
        private AudioClip bomb;

        [SerializeField]
        private AudioClip doublePoint;

        [SerializeField]
        private AudioClip freeze;

        [SerializeField]
        private AudioClip gameover;

        [SerializeField]
        private AudioClip good;

        [SerializeField]
        private AudioClip play;

        [SerializeField]
        private AudioClip union;

        [SerializeField]
        private AudioClip countDown;

        [SerializeField]
        private AudioClip warning;

        [SerializeField]
        private AudioSource musicGame;

        #region Singletone Pattern

        private static FXManager instance;

        public static FXManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<FXManager>();
                }
                return instance;
            }
        }

        #endregion

        public void PlayMusic()
        {
            musicGame.Play();
        }

        public void PauseMusic()
        {
            musicGame.Pause();
        }

        public void StopMusic()
        {
            musicGame.Stop();
        }

        public void playBad()
        {
            AudioSource.PlayClipAtPoint(bad, transform.position, volumeFX);
        }

        public void playBomb()
        {
            AudioSource.PlayClipAtPoint(bomb, transform.position, volumeFX);
        }

        public void playDoublePoint()
        {
            AudioSource.PlayClipAtPoint(doublePoint, transform.position, volumeFX);
        }

        public void playFreeze()
        {
            AudioSource.PlayClipAtPoint(freeze, transform.position, volumeFX);
        }

        public void playGameover()
        {
            AudioSource.PlayClipAtPoint(gameover, transform.position, volumeFX);
        }

        public void playGood()
        {
            AudioSource.PlayClipAtPoint(good, transform.position, volumeFX);
        }

        public void playPlay()
        {
            AudioSource.PlayClipAtPoint(play, transform.position, volumeFX);
        }

        public void playUnion()
        {
            AudioSource.PlayClipAtPoint(union, transform.position, volumeFX);
        }

        public void playCountDown()
        {
            AudioSource.PlayClipAtPoint(countDown, transform.position, volumeFX);
        }

        public void playWarning()
        {
            AudioSource.PlayClipAtPoint(warning, transform.position, volumeFX);
        }

        public void muter()
        {
            mute = !mute;

            if(mute)
            {
                soundImage.color = Color.red;
                AudioListener.volume = 0;
            }
                
            else
            {
                soundImage.color = Color.white;
                AudioListener.volume = 1;
            } 
        }
    }
}
