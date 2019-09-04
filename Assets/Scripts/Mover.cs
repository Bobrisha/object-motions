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


    [SerializeField] LineRenderer renderer = default;

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

                case Trajectories.Spikes:
                    MoveCoroutine = StartCoroutine(SpikesMove());
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

        while ((Vector2)objectToMove.transform.position != clickPosition)
        {
            yield return null;

            float fractionDistance = (Time.time - startTime) / time;
            objectToMove.position = Vector2.Lerp(startPosition, clickPosition, fractionDistance);
        }
    }


    IEnumerator SpikesMove()
    {
        Vector2 direction = (clickPosition - startPosition);
        direction.Normalize();

        Vector2 spikePosition  = new Vector2(-direction.y, direction.x) * spikesHeight;

        int transitPointsCount = spikesCount * 2 + 1;
        Vector2[] transitPoints = new Vector2[transitPointsCount];


        transitPoints[0] = startPosition;

        for (int i = 1; i < transitPointsCount - 1; i++)
        {
            float x = distance / 2 / spikesCount;

            if (i % 2 > 0)
            {
                transitPoints[i] = i * x * direction + spikePosition + startPosition;
            }
            else
            {
                transitPoints[i] = i * x * direction + startPosition;
            }
        }

        transitPoints[transitPointsCount - 1] = clickPosition;
        

        for (int i = 1; i < transitPointsCount; i++)
        {
            float startTime = Time.time;

            while ((Vector2)objectToMove.position != transitPoints[i])
            {
                yield return null;

                float fractionDistance = (Time.time - startTime) /  time * (spikesCount * 2);
                objectToMove.position = Vector2.Lerp(transitPoints[i - 1], transitPoints[i], fractionDistance); 
            }
        }
    }
    

    IEnumerator SpiralMove()
    {
         //p = a * f / 2pi  - Archimedean spiral (Polar coordinates)
         //f = turn angle
         //a - spiral step

         //x = p cos (f);
         //y = p sin (f);

         //distance(radius) / (n + 0.5) = a 
         //n - spiral turns
        
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
