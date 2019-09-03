using System.Collections;
using UnityEngine;


public class Mover : MonoBehaviour
{
    enum Trajectories
    {
        Linear,
        Spikes,
        Spiral
    }


    [SerializeField] Trajectories trajectory = default;
    [SerializeField] Transform objectToMove = default;
    [SerializeField] float time = default;
    [SerializeField] int spikesCount = default;
    [SerializeField] float spikesHeight = default;


    float startTime;
    float distance;

    Vector2 clickPosition;
    Vector2 startPosition;

    Coroutine MoveCoroutine;

    // x = p cos (f);
    // y = p sin (f);

    // p = a * f / 2pi 
    // f = (0, 2pi)
    // a = spiral steps

  
    void Update()
    {


        if (Input.GetMouseButtonDown(0))
        {
            clickPosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            startTime = Time.time;
            startPosition = objectToMove.transform.position;

            //switch (trajectory)
            //{
                 distance = Vector2.Distance(startPosition, clickPosition);
                //case Trajectories.Linear:
                   
                //    break;

                //case Trajectories.Spikes:
                    //distance = Mathf.Sqrt(Mathf.Pow((Vector2.Distance(startPosition, clickPosition) / spikesCount), 2) + spikesHeight * spikesHeight) * spikesCount * 2;
                    //break;
            //}

            if (MoveCoroutine != null)
            {
                StopCoroutine(MoveCoroutine);
            }

            switch (trajectory)
            {
                case Trajectories.Linear:
                    MoveCoroutine = StartCoroutine(Move());
                    break;

                case Trajectories.Spiral:
                    MoveCoroutine = StartCoroutine(SpiralMove());
                    break;
            }

        }
    }


    IEnumerator Move()
    {
        while (clickPosition != (Vector2)objectToMove.transform.position)
        {
            yield return null;

            switch (trajectory)
            {
                case Trajectories.Linear:
                    float fractionDitance = (Time.time - startTime) / time;
                    objectToMove.position = Vector3.Lerp(startPosition, clickPosition, fractionDitance);
                    break;

            }
        }
    }


    float angleStep;
    float startAngle;

    float angle;

    float a = 1;


    IEnumerator SpiralMove()
    {
        Vector2 offset = (Vector2)objectToMove.position - clickPosition;
        startAngle = Mathf.Atan2(offset.y , offset.x);

        //if (startAngle < 0f)
        //{
        //    startAngle = 2 * Mathf.PI + startAngle;
        //}

        print(startAngle);
        angle = startAngle + Mathf.Floor(distance / a) * 2 * Mathf.PI;

        while (angle > 0.0f)
        {
            yield return null;

            //angleStep = Time.deltaTime * startAngle / time;
            //angle -= angleStep;
            angle -= 0.1f;
            //print(angle);

            float p = a * angle / 2.0f / Mathf.PI;
            objectToMove.position = new Vector2(p * Mathf.Cos(angle), p * Mathf.Sin(angle)) + clickPosition;

          
        }
    }
}
