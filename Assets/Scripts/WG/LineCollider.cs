using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 엣지 콜라이더 없으면 엣지 콜라이더 추가
[RequireComponent(typeof(EdgeCollider2D))]
public class LineCollider : MonoBehaviour
{
    EdgeCollider2D edgeCollider;

    LineRenderer myLine;

    void Awake()
    {
        myLine = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
    }
    void Start()
    {

    }

    void Update()
    {
        SetEdgeCollider(myLine);
        SetColliderRadius();
    }

    void SetEdgeCollider(LineRenderer lineRenderer)
    {
        List<Vector2> edges = new List<Vector2>();

        //라인 렌더러 컴포넌트의 전체 포인트 개수만큼만 point 카운트
        for (int point = 0; point < lineRenderer.positionCount; point++)
        {
            //각 point의 Vector3 포지션 받아오기
            Vector3 lineRendererPoint = lineRenderer.GetPosition(point);

            //각 point x,y 좌표 추가
            edges.Add(new Vector2(lineRendererPoint.x, lineRendererPoint.y));
        }

        //벡터2 리스트를 포인트들로 지정
        edgeCollider.SetPoints(edges);

    }

    void SetColliderRadius()
    {
        //선 두께 일정하게할거면 Radius 조정
        edgeCollider.edgeRadius = myLine.endWidth / 2;
    }
}
