using UnityEngine;
using System.Collections;
namespace ProjectLine
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private LevelSpawner levelSpawner = null;
        [SerializeField]
        private PointsSpawner pointsSpawner = null;
        [SerializeField]
        private SpriteRenderer backGround = null;
        [SerializeField]
        private Flash flash = null;

        public float gameWidth { get; private set; }
        public float gameHeight { get; private set; }
       

        private static LineManager lineManager = null;
        public static LineManager GetLineManagerInstance()
        {
            if(lineManager == null)
            {
                lineManager = GameObject.Find("LineManager").GetComponent<LineManager>();
            }
            return lineManager;
        }

        #region GameDificult
        [Header("Portals spawner")]
        [SerializeField]
        private float minFrequencyPortalEnemiesSpawner = 12f;
        [SerializeField]
        private float maxFrequencyPortalEnemiesSpawner = 1f;
        [SerializeField]
        private float minFrequencyPortalGoodsSpawner = 8f;
        [SerializeField]
        private float maxFrequencyPortalGoodsSpawner = 2f;

        [Header("Enemies quantity")]
        [SerializeField]
        private float minNumberEnemiesHordeStart = 2;
        [SerializeField]
        private float maxNumberEnemiesHordeStart = 6;
        [SerializeField]
        private float minNumberEnemiesHordeEnd = 6;
        [SerializeField]
        private float maxNumberEnemiesHordeEnd = 12;

        [Header("Goods quantity")]
        [SerializeField]
        private float minNumberGoodsHordeStart = 2;
        [SerializeField]
        private float maxNumberGoodsHordeStart = 6;
        [SerializeField]
        private float minNumberGoodsHordeEnd = 6;
        [SerializeField]
        private float maxNumberGoodsHordeEnd = 12;

        [SerializeField]
        private float totalSessionSeconds = 180f;
        #endregion GameDificult

        public bool isGameRunning { get; private set; }

        private static GameManager gameManager;

        public static GameManager GetInstance
        {
            get
            {
                if (gameManager == null)
                {
                    gameManager = GameObject.FindObjectOfType<GameManager>();
                }
                return gameManager;
            }
        }

        private class SessionTimeManager
        {
            public float currentSessionTime { get; private set; }
            private float totalSessionSeconds;
            public SessionTimeManager(float totalSessionSeconds)
            {
                this.totalSessionSeconds = totalSessionSeconds;
                this.currentSessionTime = 0;
            }
            public void Update(float delta)
            {
                float rate = 1f / totalSessionSeconds;
                currentSessionTime += delta * rate;
            }
        }
        private SessionTimeManager sessionTime;

        private float GetDificultValue(float startValue, float endValue, bool random)
        {
            float currentValue = startValue;
            if(sessionTime != null)
            {
                float t = (float)Mathf.Min(sessionTime.currentSessionTime, 1f);
                currentValue = Mathf.Lerp(startValue, endValue, t);
                endValue = currentValue; 
            }
            if(!random)
                return currentValue;
            else
                return UnityEngine.Random.Range(Mathf.Min(startValue, endValue), Mathf.Max(startValue, endValue));
        }

        void Awake()
        {
            isGameRunning = false;
            gameWidth = backGround.bounds.max.x - backGround.bounds.min.x;
            gameHeight = backGround.bounds.max.y - backGround.bounds.min.y;
        }

        public void StartGame()
        {
            isGameRunning = true;
            StartCoroutine(SpawnGoodsPortals());
            StartCoroutine(SpawnEnemiesPortals());
            sessionTime = new SessionTimeManager(this.totalSessionSeconds);
            GetLineManagerInstance().lineCollision.OnSpawnPoints += SpawnPoints;
            FXManager.Instance.PlayMusic();
            FXManager.Instance.playPlay();
        }

        public void EndGame()
        {

            isGameRunning = false;
            GetLineManagerInstance().RemoveLine();
            GetLineManagerInstance().lineCollision.OnSpawnPoints -= SpawnPoints;
            DestroyAllElements();
            FXManager.Instance.StopMusic();
            StartCoroutine(PlayEndGame());
        }

        private void DestroyAllElements()
        {
            GameElement[] all = GameObject.FindObjectsOfType<GameElement>();
            foreach (GameElement e in all)
            {
                e.DoDestroy(true);
            }
        }

        IEnumerator PlayEndGame()
        {
            yield return new WaitForSeconds(2f);
            FXManager.Instance.playGameover();
            GameOverManager.Instance.Init();
        }

        void Update()
        {

            if(isGameRunning)
            {
                sessionTime.Update(Time.deltaTime);
            }

            Rect r = GetRectGameDimension(Constants.GAME_AREA_MARGIN);
            Debug.DrawLine(new Vector3(r.xMin, r.yMax, 0), new Vector3(r.xMax, r.yMax, 0), Color.white);//Top
            Debug.DrawLine(new Vector3(r.xMin, r.yMax, 0), new Vector3(r.xMin, r.yMin, 0), Color.white);//Left
            Debug.DrawLine(new Vector3(r.xMax, r.yMax, 0), new Vector3(r.xMax, r.yMin, 0), Color.white);//Right
            Debug.DrawLine(new Vector3(r.xMin, r.yMin, 0), new Vector3(r.xMax, r.yMin, 0), Color.white);//Button

            Rect r2 = GetRectGameDimension(Constants.SPAWN_DIMENSIONS_MARGIN);
            Debug.DrawLine(new Vector3(r2.xMin, r2.yMax, 0), new Vector3(r2.xMax, r2.yMax, 0), Color.magenta);//Top
            Debug.DrawLine(new Vector3(r2.xMin, r2.yMax, 0), new Vector3(r2.xMin, r2.yMin, 0), Color.magenta);//Left
            Debug.DrawLine(new Vector3(r2.xMax, r2.yMax, 0), new Vector3(r2.xMax, r2.yMin, 0), Color.magenta);//Right
            Debug.DrawLine(new Vector3(r2.xMin, r2.yMin, 0), new Vector3(r2.xMax, r2.yMin, 0), Color.magenta);//Button
        }

        void SpawnPoints(Vector2 position, int quantity)
        {
            PointsSpawner p = Instantiate(pointsSpawner);
            p.Spawn(position, quantity);
        }

        private IEnumerator SpawnEnemiesPortals()
        {
            
            {
                while (isGameRunning)
                {
                    int type = UnityEngine.Random.Range(0, 3);
                    int currentMinQuantity = (int)GetDificultValue(minNumberEnemiesHordeStart, minNumberEnemiesHordeEnd, false);
                    int currentMaxQuantity = (int)GetDificultValue(maxNumberEnemiesHordeStart, maxNumberEnemiesHordeEnd, false);

                    float quantity = UnityEngine.Random.Range(currentMinQuantity, currentMaxQuantity);
                    
                    if (type == 0)//Cricle->Bomb
                    {
                        quantity = quantity * 0.5f < 1 ? 1 : quantity * 0.5f;
                    }
                    else if(type == 1)//Square->Big
                    {
                        quantity = (quantity *0.75f < 1 ? 1 : quantity * 0.75f);
                    }
                    else//Trinagle->Bullet
                    {

                    }
                    

                    levelSpawner.SpawnPortalEnemies(type, (int)quantity);

                    yield return new WaitForSeconds(
                        GetDificultValue(
                            minFrequencyPortalEnemiesSpawner,
                            maxFrequencyPortalEnemiesSpawner,
                            false
                        ));
                }
            }
        }

        private IEnumerator SpawnGoodsPortals()
        {
            
            {
                while (isGameRunning)
                {
                    int currentMinQuantity = (int)GetDificultValue(minNumberGoodsHordeStart, minNumberGoodsHordeEnd, false);
                    int currentMaxQuantity = (int)GetDificultValue(maxNumberGoodsHordeStart, maxNumberGoodsHordeEnd, false);

                    int quantity = UnityEngine.Random.Range(currentMinQuantity, currentMaxQuantity);

                    levelSpawner.SpawnPortalGoods(quantity);

                    yield return new WaitForSeconds(
                        GetDificultValue(
                            minFrequencyPortalGoodsSpawner,
                            maxFrequencyPortalGoodsSpawner,
                            false
                        ));
                }
            }
        }

        public Vector2 GetRandonPosition()
        {
            Rect r = GetRectGameDimension(Constants.SPAWN_DIMENSIONS_MARGIN);
            return new Vector2(UnityEngine.Random.Range(r.xMin, r.xMax), UnityEngine.Random.Range(r.yMin, r.yMax));
        }

        public Rect GetRectGameDimension(float distanceMod)
        {
            Vector2 topLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
            /*
            Vector2 bottomRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            Rect r = new Rect(
                topLeft.x, 
                topLeft.y, 
                bottomRight.x - topLeft.x - Constants.MARGIN*2 - distanceMod*2, 
                bottomRight.y - topLeft.y - Constants.MARGIN - Constants.MARGIN_TOP - distanceMod*2);
                float offset = Constants.MARGIN + distanceMod;
            r.position = new Vector2(r.position.x + offset, r.position.y + offset);
            */

            Rect r = new Rect(
                -gameWidth/2 + distanceMod,
                -gameHeight/2 + distanceMod*2,
                gameWidth - distanceMod*2,
                gameHeight - distanceMod*3);
            r.position = new Vector2(r.position.x, r.position.y - distanceMod);

            return r;
        }

        public void Flash()
        {
            flash.Run();
        }
    }

    
    
}
