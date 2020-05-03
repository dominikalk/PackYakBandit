using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator spriteAnim;
    private AIDestinationSetter destSetter;
    private Character theChar;

    private Vector3 lastPosition;
    public bool charInside;

    [SerializeField] Transform[] randomPoints;
    private Vector2 randomTarget = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        destSetter = GetComponent<AIDestinationSetter>();
        theChar = FindObjectOfType<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        if(lastPosition != null)
        {
            if(Vector2.Distance(lastPosition, transform.position) <= 0.001f)
            {
                spriteAnim.SetTrigger("idle");
            }
            else
            {
                float angle = Mathf.Atan2(lastPosition.x - transform.position.x, transform.position.y - lastPosition.y) * 180 / Mathf.PI;
                angle = Mathf.Abs(angle);
                if (angle > 45 && angle < 135)
                {
                    spriteAnim.SetTrigger("walk");
                }
                else
                {
                    spriteAnim.SetTrigger("climb");
                }
            }
        }
        lastPosition = transform.position;
        if (charInside)
        {
            destSetter.target = theChar.transform;
        }
        else
        {
            if(destSetter.target == theChar.transform)
            {
                int random = Random.Range(0, randomPoints.Length);
                destSetter.target = randomPoints[random];
                randomTarget = randomPoints[random].position;
            }
        }

        if(Vector2.Distance(transform.position, randomTarget) < 0.05f)
        {
            int random = Random.Range(0, randomPoints.Length);
            destSetter.target = randomPoints[random];
            randomTarget = randomPoints[random].position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            FindObjectOfType<MenuController>().playerDied();
            FindObjectOfType<MusicController>().died();
            theChar.died();
        }
    }
}
