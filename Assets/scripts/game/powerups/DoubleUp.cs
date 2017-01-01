using UnityEngine;
using System.Collections;

namespace ProjectLine
{
    public class DoubleUp : PowerUpElement
    {
        public override void consecuence()
        {
            if (!isUsed)
            {
                base.consecuence();

                ScoreManager.Instance.activateDouble();
                FXManager.Instance.playDoublePoint();
                DoDestroy(false);
            }
        }
    }
}
