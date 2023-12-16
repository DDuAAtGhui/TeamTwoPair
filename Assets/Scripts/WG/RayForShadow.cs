using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RayForShadow : MonoBehaviour
{
    [SerializeField]
    int howMuchRays = 360;

    float angle = 0;
    float angleIncrement;
    float radius = 2000f;

    [SerializeField] Transform rayTFTop;
    [SerializeField] Transform rayTFBottom;

    [SerializeField] LayerMask whatIsInteractable;
    // Start is called before the first frame update
    void Start()
    {
    }

    RaycastHit2D[] hits = null;
    bool isHit = false;
    void Update()
    {
        //angleIncrement = 360f / howMuchRays;


        //for (int i = 0; i < howMuchRays; i++)
        //{
        //    float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
        //    float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

        //    Vector3 rayDirection = new Vector3(x, y, 0);

        //    angle += angleIncrement;

        //    Debug.DrawRay(transform.position, rayDirection);
        //}
        //for (int i = 0; i < howMuchRays; i += 2)
        //{
        //    Debug.DrawRay(rayTFTop.position + new Vector3(0, -i, 0), new Vector3(1, -1, 0).normalized * 20000f, Color.red);

        //    hits = Physics2D.RaycastAll(rayTFTop.position + new Vector3(0, -i, 0),
        //        new Vector3(1, -1, 0).normalized, 20000f, whatIsInteractable);

        //    isHit = Physics.Raycast(rayTFTop.position + new Vector3(0, -i, 0), new Vector3(1, -1, 0).normalized,
        //        whatIsInteractable);


        //}

        //if (hits != null)
        //{
        //    Debug.Log(hits.Length);
        //}

        //Debug.Log(isHit);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }
}
