using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectLine {
    public class LevelSpawner : MonoBehaviour
    {
        [SerializeField]
        private Portal portalRed = null;
        [SerializeField]
        private Portal portalYellow = null;
        [SerializeField]
        private Good good = null;
        [SerializeField]
        private Circle enemyCircle = null;
        [SerializeField]
        private Square enemySquare = null;
        [SerializeField]
        private Triangle enemyTriangle = null;

        [SerializeField]
        private GameManager gameManager;

        public enum EnemyType { Circle = 0, Square = 1, Triangle = 2 };

        private Queue<Good> listGood;
        private Queue<Circle> listCircle;
        private Queue<Square> listSquare;
        private Queue<Triangle> listTriangle;
        private Queue<Portal> listPortalRed;
        private Queue<Portal> listPortalYellow;

        private float portalRadius;

        void Awake()
        {
            listGood = new Queue<Good>();
            listCircle = new Queue<Circle>();
            listSquare = new Queue<Square>();
            listTriangle = new Queue<Triangle>();
            listPortalRed = new Queue<Portal>();
            listPortalYellow = new Queue<Portal>();

            portalRadius = portalRed.GetComponent<CircleCollider2D>().radius;
        }

        public void SpawnPortalEnemies(int enemyType, int number)
        {
            //Search a valid position
            Vector2 position = Vector2.zero;
            int maxIterations = 100;
            int it = 0;
            do
            {
                position = gameManager.GetRandonPosition();
                it++;
                if (it > maxIterations)
                    return;
            }
            while (!checkValidPortalPosition(position));

            Portal p = null;
            if(listPortalRed.Count == 0)
            {
                p = UnityEngine.Object.Instantiate<Portal>(portalRed);
            }
            else
            {
                p = listPortalRed.Dequeue();
            }
            Enemy[] enemies = new Enemy[number];
            for(int i = 0; i < number; i++)
            {
                enemies[i] = SpawnEnemy(enemyType);
                enemies[i].OnDestroyed += DestroyGameElement;
            }
            p.OnDestroyed += DestroyGameElement;

            p.Init(enemies, position, Portal.Type.Red);
            
        }

        public void SpawnPortalGoods(int number)
        {
            //Search a valid position
            Vector2 position = Vector2.zero;
            int maxIterations = 100;
            int it = 0;
            do
            {
                position = gameManager.GetRandonPosition();
                it++;
                if (it > maxIterations)
                    return;
            }
            while (!checkValidPortalPosition(position));

            Portal p = null;
            if (listPortalYellow.Count == 0)
            {
                p = UnityEngine.Object.Instantiate<Portal>(portalYellow);
            }
            else
            {
                p = listPortalYellow.Dequeue();
            }
            GameElement[] goods = new Good[number];
            for (int i = 0; i < number; i++)
            {
                goods[i] = SpawnGood();
                goods[i].OnDestroyed += DestroyGameElement;
            }
            p.OnDestroyed += DestroyGameElement;
            p.Init(goods, position, Portal.Type.Yellow);

        }

        private Good SpawnGood()
        {
            Good g;
            if (listGood.Count == 0)
            {
                g = UnityEngine.Object.Instantiate<Good>(good);
            }
            else
            {
                g = listGood.Dequeue();
            }
            return g;

        }

        private Enemy SpawnEnemy(int enemyType)
        {
            Enemy e = null;
            switch (enemyType)
            {
                case 0:
                    if(listCircle.Count == 0)
                    {
                        e = UnityEngine.Object.Instantiate<Circle>(enemyCircle);
                    }
                    else
                    {
                        e = listCircle.Dequeue();
                    }
                    break;
                case 1:
                    if (listSquare.Count == 0)
                    {
                        e = UnityEngine.Object.Instantiate<Square>(enemySquare);
                    }
                    else
                    {
                        e = listSquare.Dequeue();
                    }
                    break;
                case 2:
                    if (listTriangle.Count == 0)
                    {
                        e = UnityEngine.Object.Instantiate<Triangle>(enemyTriangle);
                    }
                    else
                    {
                        e = listTriangle.Dequeue();
                    }
                    break;
            }
            return e;
        }

        public void DestroyGameElement(GameElement element)
        {
            element.OnDestroyed -= DestroyGameElement;
            if (element.GetType() == typeof(Circle))
            {
                listCircle.Enqueue((Circle)element);
            }
            else if (element.GetType() == typeof(Square))
            {
                listSquare.Enqueue((Square)element);
            }
            else if (element.GetType() == typeof(Triangle))
            {
                listTriangle.Enqueue((Triangle)element);
            }
            else if (element.GetType() == typeof(Good))
            {
                listGood.Enqueue((Good)element);
            }
            else if (element.GetType() == typeof(Portal))
            {
                Portal p = (Portal)element;
                if(p.type == Portal.Type.Red)
                    listPortalRed.Enqueue(p);
                else
                    listPortalYellow.Enqueue(p);
            }
        }

        private bool checkValidPortalPosition(Vector2 position)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, portalRadius);
            int i = 0;
            while (i < hitColliders.Length)
            {
                return false;
            }
            return true;
        }
    }
}
