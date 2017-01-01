using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

namespace ProjectLine
{
    public class LineManager : MonoBehaviour
    {
        private GameObject lineGameobject;
        public LineCollision lineCollision { get; private set; }
        private LineRenderer lineRenderer;
        private PolygonCollider2D polygonCollider = null;
        private float LineSize = (float)Screen.width / 10000;

        private int vertexCount = 0;
        private int colliderCount = 0;
        private Vector3 newPoint;
        private Vector3 previousPosition = new Vector3(0, 0, 0);
        private const float SIZECOLLIDER = 0.1f;

        void Awake()
        {
            lineGameobject = new GameObject("Line");
            lineCollision = lineGameobject.AddComponent<LineCollision>();
            lineRenderer = lineGameobject.AddComponent<LineRenderer>();
            lineRenderer.sortingLayerName = "Line";
            lineRenderer.SetColors(Color.white, Color.white);
            lineRenderer.SetWidth(LineSize, LineSize);
            lineRenderer.SetVertexCount(0);
            Rigidbody2D rigidBody = lineGameobject.AddComponent<Rigidbody2D>();
            rigidBody.isKinematic = true;
        }

        void Update()
        {
            if(GameManager.GetInstance.isGameRunning && !PauseManager.Instance.pause)
                //For Steam
                if (Input.GetMouseButton(0))
                {
                    newPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));
                    if (newPoint != previousPosition)
                    {
                        AddPoints();
                    }
                }
                else
                {
                    if (vertexCount != 0 || colliderCount != 0) lineCollision.FinisCollision();
                    RemoveLine();
                }

                //For Google Play
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Moved)
                    {
                        newPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));
                        if (newPoint != previousPosition)
                        {
                            AddPoints();
                        }
                    }

                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        if (vertexCount != 0 || colliderCount != 0)
                        {
                            lineCollision.FinisCollision();
                        }
                        RemoveLine();
                    }
                }
                //For i560
                /*
                if(Unity.DeviveModel == i560){
                    return;
                }
                */
        }

        private void AddPoints()
        {
            lineRenderer.SetVertexCount(vertexCount + 1);
            lineRenderer.SetPosition(vertexCount, newPoint);
            Vector3 colliderPoint;
            float diferenceWidth = newPoint.x - previousPosition.x;
            float differenceHeight = newPoint.y - previousPosition.y;
            float difference = Mathf.Max(Mathf.Abs(diferenceWidth), Mathf.Abs(differenceHeight));
            if (difference > SIZECOLLIDER)
            {
                if (colliderCount != 0)
                {
                    int iterator = (int)Mathf.Ceil(difference / (SIZECOLLIDER * 6));

                    for (int i = 1; i <= iterator; i++)
                    {
                        colliderPoint = new Vector3(previousPosition.x + ((diferenceWidth) * ((float)i / iterator)), previousPosition.y + ((differenceHeight) * ((float)i / iterator)), 5);
                        StartCoroutine(AddCollider(colliderPoint));
                    }
                }
                else
                {
                    colliderPoint = newPoint;
                    StartCoroutine(AddCollider(colliderPoint));
                }

                vertexCount++;
                previousPosition = newPoint;
            }
        }

        private IEnumerator AddCollider(Vector3 colliderPoint)
        {
            yield return 0;
            if (polygonCollider == null)
            {
                polygonCollider = lineGameobject.AddComponent<PolygonCollider2D>();
                polygonCollider.isTrigger = true;
            }
            polygonCollider.pathCount = colliderCount + 1;
            Vector2[] path = new Vector2[4];
            path[0] = new Vector2(colliderPoint.x + SIZECOLLIDER, colliderPoint.y - SIZECOLLIDER);
            path[1] = new Vector2(colliderPoint.x + SIZECOLLIDER, colliderPoint.y + SIZECOLLIDER);
            path[2] = new Vector2(colliderPoint.x - SIZECOLLIDER, colliderPoint.y + SIZECOLLIDER);
            path[3] = new Vector2(colliderPoint.x - SIZECOLLIDER, colliderPoint.y - SIZECOLLIDER);
            polygonCollider.SetPath(colliderCount, path);
            colliderCount++;
        }

        public void RemoveLine()
        {
            if (vertexCount !=0 || colliderCount!=0 )
            {
                vertexCount = 0;
                lineRenderer.SetVertexCount(vertexCount);
                colliderCount = 0;
                Destroy(polygonCollider);

                UnPauseGoods();
            }
        }

        private void UnPauseGoods()
        {
            foreach (Good good in lineCollision.listGood)
            {
                good.SetPause(false);
            }
        }
    }
}