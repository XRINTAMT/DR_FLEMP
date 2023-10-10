using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AnimationMovement : MonoBehaviour
{
    [SerializeField] private Transform moveObj;
    [SerializeField] private float speed = 1;
    [SerializeField] List<Transform> moveListPonints = new List<Transform>();
    Animator animator;
    Transform nextPoint;
    int indexPoint;

    public UnityEvent OnStartMove;
    public UnityEvent OnFinishMove;
    bool startMoving;
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


        Transform wheelchair = GameObject.Find("Wheelchair").transform;
        GameObject point = new GameObject();
        point.transform.parent = wheelchair;
        point.transform.localPosition = new Vector3(0, 0.324f, 1.02f);
        point.transform.localEulerAngles= new Vector3(0, 180, 0);
        moveListPonints.Add(point.transform);

        foreach (Animator anim in GetComponentsInChildren<Animator>())
        {
            animator=anim;
        }
    }

    private void Move()
    {

        nextPoint = moveListPonints[indexPoint];

        Vector3 newDirection = Vector3.RotateTowards(moveObj.forward, nextPoint.position - moveObj.position, speed *3* Time.deltaTime, 1f);
        moveObj.rotation = Quaternion.LookRotation(newDirection);
        moveObj.position = Vector3.MoveTowards(moveObj.position, nextPoint.position, speed * Time.deltaTime);

        if (moveObj.position == nextPoint.position)
        {
            if (indexPoint < moveListPonints.Count - 1)
            {
                indexPoint++;
            }
        }

        if (moveObj.position == moveListPonints[moveListPonints.Count - 1].position)
        {
            OnFinishMove.Invoke();
            onPlace = true;

            animator.SetTrigger("Turn");
            moveObj.transform.parent = moveListPonints[moveListPonints.Count - 1].parent;
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
            //if (GetComponent<NavMeshAgent>())
            //{
            //    GetComponent<NavMeshAgent>().destination = MoveListPonints[0].position;
            //}
            animator.applyRootMotion = false;
            startMoving = true;
        }

        //if (GetComponent<NavMeshAgent>().remainingDistance<= GetComponent<NavMeshAgent>().stoppingDistance)
        //{
        //    GetComponent<NavMeshAgent>().enabled = false;
        //    rotate = true;
        //}

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
