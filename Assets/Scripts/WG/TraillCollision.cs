using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

[RequireComponent(typeof(TrailRenderer))]
public class TraillCollision : MonoBehaviour
{
    TrailRenderer myTrail;
    [SerializeField] GameObject trailShadow;
    //Ʈ���� ������ Ư���� �ݶ��̴��� �ٸ�������Ʈ�� ���̰� �װ� ��������
    GameObject coliderGameObject;

    EdgeCollider2D myCollider;

    void Awake()
    {
        myTrail = GetComponent<TrailRenderer>();
        coliderGameObject = GameObject.Find("TrailStandard");
        coliderGameObject.AddComponent<EdgeCollider2D>();
        myCollider = coliderGameObject.GetComponent<EdgeCollider2D>();

        myCollider.edgeRadius = myTrail.startWidth / 2;
    }

    void Update()
    {
        SetColliderPointsFromTrail(myTrail, myCollider);

        if (PlayerController.moveDir != Vector2.zero)
        {
            GameObject trailShadow = ObjectPool.instance.objectsQueue.Dequeue();

            trailShadow.transform.position = transform.position;
            trailShadow.SetActive(true);


        }
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
