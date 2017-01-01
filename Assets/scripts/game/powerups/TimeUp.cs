using UnityEngine;
using System.Collections;

namespace ProjectLine
{
    public class TimeUp : PowerUpElement
    {
        public override void consecuence()
        {
            if (!isUsed)
            {
                base.consecuence();

                GameElement[] allElements = UnityEngine.Object.FindObjectsOfType<GameElement>();
                foreach (GameElement element in allElements)
                {
                    if((element is Enemy) || (element is Portal && element.tag == "PortalRed"))
                    {
                        TimeManager.Instance.pauseTime = Constants.TIME_PAUSEUP;
                    }
                }
                FXManager.Instance.playFreeze();
                DoDestroy(false);
            }
        }
    }
}
