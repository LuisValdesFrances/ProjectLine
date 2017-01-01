using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ProjectLine
{
    public class CountDownStart : MonoBehaviour
    {
        [SerializeField]
        private Text CountDownText;
        [SerializeField]
        private Image HideImage;

        private int secondsCountdown = 3;
        private float timeCount;

        void Start()
        {
            timeCount = secondsCountdown;
            StartCoroutine(StartCountDown());
            CountDownText.text = ""+timeCount;
        }

        private IEnumerator StartCountDown()
        {
            FXManager.Instance.playCountDown();
            while (timeCount > 0)
            {
                yield return new WaitForSeconds(1f);
                timeCount--;
                CountDownText.text = "" + timeCount;
                FXManager.Instance.playCountDown();
            }
            CountDownText.gameObject.SetActive(false);
            HideImage.gameObject.SetActive(false);
            GameManager.GetInstance.StartGame();
        }
    }
}