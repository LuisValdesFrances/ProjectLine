using UnityEngine;
using System.Collections;

namespace ProjectLine
{
    public class Constants
    {
        /*Game dimensions*/
        public const float MARGIN = 0.5f;
        public const float MARGIN_TOP = 1f;
        public const float GAME_AREA_MARGIN = 0.25f;
        public const float SPAWN_DIMENSIONS_MARGIN = 0.5f;

        public const int PLAYER_DAMAGE = 1;
        public const int TIME_DOUBLE_SCORE = 6;
        public const float TIME_PAUSEUP = 2.5f;

        public const int SCORE_BOMB = 10;
        public const int SCORE_GREEN = 20;
        public const int SCORE_RED = 60;
        public const float TIME_GREEN = 0.0012f;

        public const int MIN_COLLECTED_POWERUP = 6;

        public const int MAX_POWERUP_ACTIVE = 3;

        public const int TIME_GAME_HARCODED = 60; //Cada nivel deberia tener su tiempo
    }
}
