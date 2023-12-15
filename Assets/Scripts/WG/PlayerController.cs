using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 moveDir;
    [SerializeField] LineRenderer myLine;
    [SerializeField] float growSpeed = 1f;

    [SerializeField] float timer = 0.3f;
    float tempTimer = 0f;
    void Start()
    {
        tempTimer = timer;
    }
    void Update()
    {
        timer -= Time.deltaTime;

        int lastLInePointIndex = myLine.positionCount - 1;

        if (moveDir != Vector2.zero)
        {
            if (timer > 0)
                return;

            timer = tempTimer;

            if (moveDir.x != 0 && moveDir.y != 0)
                return;
         
            else if (moveDir.y > 0 && GetPosition(lastLInePointIndex).y == GetPosition(lastLInePointIndex - 1).y)
            {
                myLine.positionCount++;

                myLine.SetPosition(lastLInePointIndex,
                    new Vector3(GetPosition(lastLInePointIndex).x
                    , GetPosition(lastLInePointIndex - 1).y));
            }

            else if (moveDir.x != 0 && GetPosition(lastLInePointIndex).y != GetPosition(lastLInePointIndex - 1).y)
            {
                myLine.positionCount++;

                myLine.SetPosition(lastLInePointIndex,
                      new Vector3(GetPosition(lastLInePointIndex - 1).x
                     , GetPosition(lastLInePointIndex).y));
            }

            myLine.SetPosition(lastLInePointIndex,
                myLine.GetPosition(lastLInePointIndex)
                + (Vector3)moveDir * Time.deltaTime * growSpeed);



        }
    }

    void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
        moveDir = new Vector2(dir.x, dir.y);
    }

    Vector3 GetPosition(int index)
    {
        return myLine.GetPosition(index);
    }
}
