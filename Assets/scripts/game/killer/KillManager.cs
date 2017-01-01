using UnityEngine;
using System.Collections;

namespace ProjectLine
{
    public class KillManager : MonoBehaviour
    {
        Vector3 newPoint;
        private bool hasPoint = false;

        void Update()
        {
            //For Mouse
            if (Input.GetMouseButton(0))
            {
                pointDetecting();
            }
            else
            {
                if (hasPoint)
                {
                    pointNODetecting();
                }
            }

            //For Touch
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                {
                    pointDetecting();
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    pointNODetecting();
                }
            }
        }


        private void pointDetecting()
        {
            if (!hasPoint && Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0)
            {
                hasPoint = true;
                newPoint = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                transform.position = newPoint;
            }
        }

        private void pointNODetecting()
        {
            transform.position = new Vector2(-100, transform.position.y);
            hasPoint = false;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PowerUpElement>())
            {
                PowerUpElement pe = other.gameObject.transform.GetComponent<PowerUpElement>();
                pe.consecuence();
            }
            else
            {
                Enemy enemy = other.gameObject.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DoDamage(Constants.PLAYER_DAMAGE);
                    if (enemy.currentHealth <= 0)
                    {
                        ScoreManager.Instance.killed++;
                        int plus = Constants.SCORE_RED * ((ScoreManager.Instance.doubleScore) ? 2 : 1);
                        ScoreManager.Instance.Score = plus;
                        TimeManager.Instance.Timer = plus * Constants.TIME_GREEN;
                    }
                }
            }
        }
    }
}