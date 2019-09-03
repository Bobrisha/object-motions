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

    [Header("Spikes")]
    [SerializeField] int spikesCount = default;
    [SerializeField] float spikesHeight = default;

    [Header("Spiral")]
    [SerializeField] int spralTurns = 5;


    Coroutine MoveCoroutine;

    Vector2 clickPosition;
    Vector2 startPosition;

    float distance;

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = objectToMove.transform.position;
            clickPosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            
            distance = Vector2.Distance(startPosition, clickPosition);

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
        float startTime = Time.time;

        while (clickPosition != (Vector2)objectToMove.transform.position)
        {
            yield return null;

            float fractionDitance = (Time.time - startTime) / time;
            objectToMove.position = Vector3.Lerp(startPosition, clickPosition, fractionDitance);
        }
    }
    

    IEnumerator SpiralMove()
    {
        // p = a * f / 2pi  - Archimedean spiral (Polar coordinates)
        // f = turn angle
        // a - spiral step

        // x = p cos (f);
        // y = p sin (f);

        // distance(radius) / (n + 0.5) = a 
        // n - spiral turns

        Vector2 spiralCenterOffset = startPosition - clickPosition;
        float startDirectionAngle = Mathf.Atan2(spiralCenterOffset.y , spiralCenterOffset.x);
        
        startDirectionAngle += (startDirectionAngle < 0f) ?  2 * Mathf.PI : 0;


        float currentAngle = startDirectionAngle + spralTurns * 2 * Mathf.PI;
        float fullStartAngle = currentAngle;

        float angleStep;

        while (currentAngle >= 0.0f)
        {
            yield return null;

            angleStep = Time.deltaTime * fullStartAngle / time;
            currentAngle -= angleStep;

            float spiralStep = distance / (spralTurns + 0.5f);
            float spiralPolarCoordinates = spiralStep * currentAngle / 2.0f / Mathf.PI;
            objectToMove.position = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * spiralPolarCoordinates + clickPosition;
        }
    }
}
