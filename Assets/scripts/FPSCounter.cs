using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour 
{
    [SerializeField]
    private Text fpsText;
    private float fpsCount;

    private float timeCount;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Update()
    {
        timeCount += Time.deltaTime;
        fpsCount++;
        if(timeCount >= 1f)
        {
            fpsText.text = "FPS: " + fpsCount;
            fpsCount = 0;
            timeCount = 0f;
        }
    }
}
