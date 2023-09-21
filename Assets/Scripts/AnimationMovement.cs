using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationMovement : MonoBehaviour
{
    [SerializeField] private Transform moveObj;
    [SerializeField] private float speed = 1;
    [SerializeField] List<Transform> MoveListPonints = new List<Transform>();
    Animator animator;
    Transform nextPoint;
    int indexPoint;

    public UnityEvent OnStartMove;
    public UnityEvent OnFinishMove;
    public bool startMoving;
    public Action walk;
    bool startEvent;
    bool onPlace;
    bool rotate;

    // Start is called before the first frame update
    void Start()
    {
        //foreach (Transform child in transform)
        //{
        //    MoveListPonints.Add(child);
        //}
        foreach (Animator anim in GetComponentsInChildren<Animator>())
        {
            animator=anim;
        }
    }

    private void Move()
    {

        nextPoint = MoveListPonints[indexPoint];

        Vector3 newDirection = Vector3.RotateTowards(moveObj.forward, nextPoint.position - moveObj.position, speed *3* Time.deltaTime, 1f);
        moveObj.rotation = Quaternion.LookRotation(newDirection);
        moveObj.position = Vector3.MoveTowards(moveObj.position, nextPoint.position, speed * Time.deltaTime);

        //if (moveObj.position == nextPoint.position)
        //{
        //    if (indexPoint < MoveListPonints.Count - 1)
        //    {
        //        indexPoint++;
        //    }
        //}

        if (moveObj.position == MoveListPonints[MoveListPonints.Count - 1].position)
        {
            OnFinishMove.Invoke();
            onPlace = true;

            animator.SetTrigger("Turn");
            moveObj.transform.parent = MoveListPonints[MoveListPonints.Count - 1].parent;
            rotate = true;
            startMoving = false;
            startEvent = false;
        }
    }

    public void StartMove(bool startMoving)
    {
        this.startMoving = startMoving;
        walk?.Invoke();
    }

    public void StartWheelChairANimation() 
    {
        animator.SetTrigger("StandUp");

    }
    // Update is called once per frame
    void Update()
    {

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Walk") && !startMoving && !onPlace)
        {
            animator.applyRootMotion = false;
            startMoving = true;
        }



        if (rotate)
        {

            if (moveObj.transform.localEulerAngles.y > 0)
            {
                moveObj.Rotate(0, 100 * Time.deltaTime, 0);
                if (moveObj.transform.localEulerAngles.y > 359 || moveObj.transform.localEulerAngles.y<5)
                {
                    moveObj.transform.localEulerAngles = new Vector3(0, 0, 0);
                    animator.SetTrigger("SitDown");
                    animator.applyRootMotion = true;
                    rotate = false;
                }
            }
            if (moveObj.transform.localEulerAngles.y < 0)
            {
                moveObj.Rotate(0, -100 * Time.deltaTime, 0);
            }
          
        }


        if (startMoving)
        {
            if (!startEvent)
            {
                OnStartMove.Invoke();
                startEvent = true;
            }
            Move();
        }

    }
}
