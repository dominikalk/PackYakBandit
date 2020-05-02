using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    private Rigidbody2D myRigid;
    [SerializeField] private float speed = default;
    [SerializeField] private float jumpForce = default;
    private Rigidbody2D checkFloor;

    [SerializeField] private GameObject[] coins = default;

    public bool onGround = true;
    public bool onLadder = false;
    [HideInInspector] public Animator charAnim;

    private int coinsCollected = 0;
    [SerializeField] private Text coinText = default;

    [SerializeField] private Rope grapple = default;
    [SerializeField] private Transform handPosition = default;
    [SerializeField] private AnimationCurve grappleSpeedGraph = default;
    [SerializeField] private float grappleSpeed = default;
    [SerializeField] private float grappleMaxDistance = default;
    private bool grappleShot;
    private bool stillShooting;
    [SerializeField] private float pullForce;
    [SerializeField] private float fallOffDistance;
    [SerializeField] private AnimationCurve fallOffCurve;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
        charAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // --------------- WASD KEYS ----------------------------
        if(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {

        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                charAnim.SetBool("IsMoving", true);
                myRigid.velocity = new Vector2(-speed, myRigid.velocity.y);
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * -1f, transform.localScale.y);
            }
            if (Input.GetKey(KeyCode.D))
            {
                charAnim.SetBool("IsMoving", true);
                myRigid.velocity = new Vector2(speed, myRigid.velocity.y);
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            }
        }
        if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            charAnim.SetBool("IsMoving", false);
            myRigid.velocity = new Vector2(0, myRigid.velocity.y);
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (onLadder)
            {
                myRigid.velocity = new Vector2(myRigid.velocity.x, speed);
                charAnim.SetBool("Climbing", true);
            }
            else if (onGround)
            {
                myRigid.AddForce(new Vector2(0, jumpForce));
            }
        }
        else
        {
            charAnim.SetBool("Climbing", false);
        }
        // -------------------------------

        if (Input.GetMouseButtonDown(0) && !grappleShot)
        {
            startGrapple();
            //StopAllCoroutines();
            grappleShot = true;
            stillShooting = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            StopAllCoroutines();
            grapple.gameObject.SetActive(false);
            stillShooting = false;
            grappleShot = false;
            Debug.Log("Up");
        }

    }

    void startGrapple()
    {
        StartCoroutine(grappleMoving(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }

    IEnumerator grappleMoving(Vector2 mousePos)
    {
        grapple.transform.position = handPosition.position;
        grapple.gameObject.SetActive(true);
        Vector2 dirVector = mousePos - new Vector2(handPosition.position.x, handPosition.position.y);
        Vector2 targetPos = new Vector2(handPosition.position.x, handPosition.position.y) + dirVector;
        float distanceTravelled = 0;

        RaycastHit2D[] hits = Physics2D.RaycastAll(handPosition.position, dirVector);

        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Platform"))
            {
                targetPos = hit.point;
                break;
            }
        }

        while (new Vector2(grapple.transform.position.x, grapple.transform.position.y) != targetPos)
        {
            float step = grappleSpeed * Time.deltaTime * grappleSpeedGraph.Evaluate(distanceTravelled / grappleMaxDistance);
            distanceTravelled += step;
            grapple.transform.position = Vector2.MoveTowards(grapple.transform.position, targetPos, step);
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Here");
        if (stillShooting)
        {
            StartCoroutine(grapplePull(targetPos));
        }
        else
        {

        }
    }

    IEnumerator grapplePull(Vector2 targetPos)
    {
        while (stillShooting)
        {
            Vector2 pullDirection = targetPos - new Vector2(handPosition.position.x, handPosition.position.y);
            float xDist = Mathf.Abs(targetPos.x - handPosition.position.x);
            float yDist = Mathf.Abs(targetPos.y - handPosition.position.y);
            if (xDist < fallOffDistance)
            {
                pullDirection = new Vector2( pullDirection.x * fallOffCurve.Evaluate(xDist / fallOffDistance) , pullDirection.y);
            }
            if (yDist < fallOffDistance)
            {
                pullDirection = new Vector2(pullDirection.x, pullDirection.y * fallOffCurve.Evaluate(yDist / fallOffDistance));
            }
            myRigid.AddForce(pullDirection * pullForce);
            yield return new WaitForEndOfFrame();
        }
    }

    void resultFunc(List<RaycastHit2D> results)
    {

    }

    public void coinCollected(GameObject hitCoin)
    {
        List<GameObject> nonVisibleCoins = new List<GameObject>();
        foreach(GameObject coin in coins)
        {
            if(coin.activeSelf == false)
            {
                nonVisibleCoins.Add(coin);
            }
        }
        int rand = Random.Range(0, nonVisibleCoins.Count);
        nonVisibleCoins[rand].SetActive(true);
        hitCoin.SetActive(false);
        coinsCollected += 1;
        coinText.text = coinsCollected.ToString();
    }
}
