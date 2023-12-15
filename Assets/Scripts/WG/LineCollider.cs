using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �ݶ��̴� ������ ���� �ݶ��̴� �߰�
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

        //���� ������ ������Ʈ�� ��ü ����Ʈ ������ŭ�� point ī��Ʈ
        for (int point = 0; point < lineRenderer.positionCount; point++)
        {
            //�� point�� Vector3 ������ �޾ƿ���
            Vector3 lineRendererPoint = lineRenderer.GetPosition(point);

            //�� point x,y ��ǥ �߰�
            edges.Add(new Vector2(lineRendererPoint.x, lineRendererPoint.y));
        }

        //����2 ����Ʈ�� ����Ʈ��� ����
        edgeCollider.SetPoints(edges);

    }

    void SetColliderRadius()
    {
        //�� �β� �����ϰ��ҰŸ� Radius ����
        edgeCollider.edgeRadius = myLine.endWidth / 2;
    }
}
