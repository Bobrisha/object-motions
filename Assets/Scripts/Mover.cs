using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


public class Mover : MonoBehaviour
{
    #region Fields

    Transform objectToMove;
    Transform targetPointer;

    Coroutine moveCoroutine;
    Config config;
   
    Vector2 startPosition;

    float distance;

    #endregion



    #region Unity lifecycle

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            startPosition = objectToMove.transform.position;
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            distance = Vector2.Distance(startPosition, clickPosition);

            targetPointer.position = clickPosition;
            
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            switch (config.Trajectory)
            {
                case Trajectories.Linear:
                    moveCoroutine = StartCoroutine(LinearMove(clickPosition, config.Time));
                    break;

                case Trajectories.Spikes:
                    moveCoroutine = StartCoroutine(SpikesMove(clickPosition, config.Time, config.SpikesCount, config.SpikesHeight));
                    break;

                case Trajectories.Spiral:
                    moveCoroutine = StartCoroutine(SpiralMove(clickPosition, config.Time, config.SpiralTurns));
                    break;
            }
        }
    }

    #endregion



    #region Publick methods

    public void Init(Config config, Transform objectToMove, Transform targetPointer)
    {
        this.config = config;
        this.objectToMove = objectToMove;
        this.targetPointer = targetPointer;
    }

    #endregion



    #region Private methods

    IEnumerator LinearMove(Vector2 finishPosition, float time)
    {
        float startTime = Time.time;

        while ((Vector2)objectToMove.transform.position != finishPosition)
        {
            yield return null;

            float fractionDistance = (Time.time - startTime) / time;
            objectToMove.position = Vector2.Lerp(startPosition, finishPosition, fractionDistance);
        }
    }


    IEnumerator SpikesMove(Vector2 finishPosition, float time, int spikesCount, float spikesHeight)
    {
        Vector2 direction = (finishPosition - startPosition);
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

        transitPoints[transitPointsCount - 1] = finishPosition;
        
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
    

    IEnumerator SpiralMove(Vector2 finishPosition, float time, int spralTurns)
    {
        /*
        p = a * f / 2pi  - Archimedean spiral (Polar coordinates)
        f = turn angle
        a - spiral step

        x = p cos (f);
        y = p sin (f);

        distance(radius) / (n + 0.5) = a 
        n - spiral turns
        */       

        Vector2 spiralCenterOffset = startPosition - finishPosition;
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
            objectToMove.position = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * spiralPolarCoordinates + finishPosition;
        }
    }

    #endregion
}
