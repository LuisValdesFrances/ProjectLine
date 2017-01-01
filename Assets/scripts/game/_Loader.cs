using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ProjectLine
{
    public class _Loader : MonoBehaviour
    {
        public void retry()
        {
            StartCoroutine(waitSound());
        }

        IEnumerator waitSound()
        {
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}