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

    float f = 0;
    float a = 0.1f;

    void Update()
    {
        float p = a * f / 2 / Mathf.PI;
        objectToMove.position = new Vector2(p * Mathf.Cos(f), p * Mathf.Sin(f));
        f+= 0.1f;

        if (Input.GetMouseButtonDown(0))
        {
            clickPosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            startTime = Time.time;
            startPosition = objectToMove.transform.position;

            switch (trajectory)
            {
                case Trajectories.Linear:
                    distance = Vector2.Distance(startPosition, clickPosition);
                    break;

                case Trajectories.Spikes:
                    distance = Mathf.Sqrt(Mathf.Pow((Vector2.Distance(startPosition, clickPosition) / spikesCount), 2) + spikesHeight * spikesHeight) * spikesCount * 2;
                    break;
            }

            if (MoveCoroutine != null)
            {
                StopCoroutine(MoveCoroutine);
            }

            MoveCoroutine = StartCoroutine(Move());
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
}
