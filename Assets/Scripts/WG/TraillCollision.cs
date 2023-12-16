using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TraillCollision : MonoBehaviour
{
    TrailRenderer myTrail;

    //Ʈ���� ������ Ư���� �ݶ��̴��� �ٸ�������Ʈ�� ���̰� �װ� ��������
    [SerializeField] GameObject coliderGameObject;

    EdgeCollider2D myCollider;

    void Awake()
    {
        myTrail = GetComponent<TrailRenderer>();
        coliderGameObject.AddComponent<EdgeCollider2D>();
        myCollider = coliderGameObject.GetComponent<EdgeCollider2D>();

        myCollider.edgeRadius = myTrail.startWidth / 2;
    }

    void Update()
    {
        SetColliderPointsFromTrail(myTrail, myCollider);
    }

    void SetColliderPointsFromTrail(TrailRenderer trail, EdgeCollider2D edge)
    {
        //����Ʈ ���� ����Ʈ��
        List<Vector2> points = new List<Vector2>();

        for (int position = 0; position < trail.positionCount; position++)
        {
            points.Add(trail.GetPosition(position));
        }
        edge.SetPoints(points);
    }
}
