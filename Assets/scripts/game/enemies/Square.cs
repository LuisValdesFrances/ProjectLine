using UnityEngine;
using System.Collections;

namespace ProjectLine
{
    public class Square : Enemy
    {
        protected override void Update()
        {
            base.Update();
            if (state == State.Run)
            {
                if (!IsInGameArea(transform.position))
                {
                    DoDestroy(false);
                }
            }
        }
    }
}