using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QPathFinder;

public class Enemy : MonoBehaviour
{
    Character theCharacter;
    [SerializeField] private float speed = default;

    IEnumerator currentFollowPath;

    // Start is called before the first frame update
    void Start()
    {
        theCharacter = FindObjectOfType<Character>();
        StartCoroutine("setNewPath");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator setNewPath()
    {
        while (true)
        {
            if(currentFollowPath != null)
            {
                StopCoroutine(currentFollowPath);
            }

            int enemyNode = PathFinder.instance.FindNearestNode(transform.position);

            int playerNode = PathFinder.instance.FindNearestNode(theCharacter.transform.position);

            PathFinder.instance.FindShortestPathOfPoints(enemyNode, playerNode, PathLineType.Straight, Execution.Asynchronously, onPathFound);
            yield return new WaitForSeconds(1f);
        }
    }

    void onPathFound(List<Vector3> pathPositions)
    {
        if(Vector3.Magnitude(pathPositions[1] - transform.position) < Vector3.Magnitude(pathPositions[1] - pathPositions[0]))
        {
            pathPositions.RemoveAt(0);
        }
        currentFollowPath = followPath(pathPositions);
        StartCoroutine(currentFollowPath);
    }

    IEnumerator followPath(List<Vector3> pathPositions)
    {
        int incrementor = 0;
        while (incrementor < pathPositions.Count)
        {
            Vector3 targetPos = pathPositions[incrementor];
            while(transform.position != targetPos)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
                yield return new WaitForEndOfFrame();
            }
            incrementor += 1;
        }
    }
}
