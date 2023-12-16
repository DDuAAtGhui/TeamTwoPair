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
    //트레일 렌더러 특성상 콜라이더를 다른오브젝트에 붙이고 그걸 가져오기
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
        //포인트 담을 리스트들
        List<Vector2> points = new List<Vector2>();

        for (int position = 0; position < trail.positionCount; position++)
        {
            points.Add(trail.GetPosition(position));
        }
        edge.SetPoints(points);
    }
}
