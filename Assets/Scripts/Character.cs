using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class Character : MonoBehaviour
{
    private Rigidbody2D myRigid;
    [SerializeField] private float speed = default;
    [SerializeField] private float jumpForce = default;
    private Rigidbody2D checkFloor;

    [SerializeField] private GameObject[] coins = default;

    private bool canJump = true;
    public bool onGround = true;
    public bool onLadder = false;
    [HideInInspector] public Animator charAnim;

    public int coinsCollected = 0;
    [SerializeField] private Text coinText = default;

    [Space]
    [SerializeField] private Rope grapple = default;
    [SerializeField] private Transform handPosition = default;
    [SerializeField] private AnimationCurve grappleSpeedGraph = default;
    [SerializeField] private float grappleSpeed = default;
    [SerializeField] private float grappleMaxDistance = default;
    private bool grappleShot;
    private bool stillShooting;
    [SerializeField] private float pullForce = default;
    [SerializeField] private float fallOffDistance = default;
    [SerializeField] private AnimationCurve fallOffCurve = default;
    [SerializeField] private float grappleForgiveness = default;

    public bool changeSpeed;

    private bool WASD;

    [SerializeField] GameObject pauseMenu = default;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
        charAnim = GetComponent<Animator>();

        int coinsActive = 0;
        while(coinsActive < 3)
        {
            int rand = Random.Range(0, coins.Length);
            if (!coins[rand].gameObject.activeSelf)
            {
                coins[rand].SetActive(true);
                coinsActive += 1;
            }
        }

        AIPath[] enemies = FindObjectsOfType<AIPath>();
        foreach (AIPath enemy in enemies)
        {
            enemy.maxSpeed = 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            WASD = true;
        }
        else if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            WASD = false;
        }

        // --------------- WASD KEYS ----------------------------
        if (WASD)
        {
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
            if (Input.GetKey(KeyCode.W) && onLadder)
            {
                myRigid.velocity = new Vector2(myRigid.velocity.x, speed);
                charAnim.SetBool("Climbing", true);
            }
            else
            {
                charAnim.SetBool("Climbing", false);
            }

            if (Input.GetKeyDown(KeyCode.W) && onGround && canJump)
            {
                myRigid.AddForce(new Vector2(0, jumpForce));
                StartCoroutine("setCanJump");
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
            {

            }
            else
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    charAnim.SetBool("IsMoving", true);
                    myRigid.velocity = new Vector2(-speed, myRigid.velocity.y);
                    transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * -1f, transform.localScale.y);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    charAnim.SetBool("IsMoving", true);
                    myRigid.velocity = new Vector2(speed, myRigid.velocity.y);
                    transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
                }
            }
            if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                charAnim.SetBool("IsMoving", false);
                myRigid.velocity = new Vector2(0, myRigid.velocity.y);
            }
            if (Input.GetKey(KeyCode.UpArrow) && onLadder)
            {
                myRigid.velocity = new Vector2(myRigid.velocity.x, speed);
                charAnim.SetBool("Climbing", true);
            }
            else
            {
                charAnim.SetBool("Climbing", false);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && onGround && canJump)
            {
                myRigid.AddForce(new Vector2(0, jumpForce));
                StartCoroutine("setCanJump");
            }
        }
       

        // ---------- Grapple --------------

        if (Input.GetMouseButtonDown(0) && !grappleShot)
        {
            grappleShot = true;
            stillShooting = true;
            charAnim.SetBool("StillShooting", true);
            startGrapple();

        }
        if (!Input.GetMouseButton(0))
        {
            stillShooting = false;
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
        float angle = Mathf.Atan2(handPosition.position.x - mousePos.x, mousePos.y - handPosition.position.y) * 180 / Mathf.PI;

        grapple.transform.SetPositionAndRotation(grapple.transform.position, Quaternion.Euler(0, 0, angle));
        Vector2 targetPos = new Vector2(handPosition.position.x, handPosition.position.y) + Vector2.ClampMagnitude(dirVector, grappleMaxDistance);
        float distanceTravelled = 0;

        RaycastHit2D[] hits = Physics2D.RaycastAll(handPosition.position, dirVector, grappleMaxDistance);

        bool hitInteractable = false;

        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Platform"))
            {
                targetPos = hit.point;
                hitInteractable = true;
                break;
            }
        }

        while (Vector2.Distance(new Vector2(grapple.transform.position.x, grapple.transform.position.y), targetPos) > grappleForgiveness)
        {
            float step = grappleSpeed * Time.deltaTime * grappleSpeedGraph.Evaluate(distanceTravelled / grappleMaxDistance);
            distanceTravelled += step;
            grapple.transform.position = Vector2.MoveTowards(grapple.transform.position, targetPos, step);
            yield return new WaitForEndOfFrame();
        }
        if (!hitInteractable)
        {
            stillShooting = false;
        }
        StartCoroutine(grapplePull(targetPos));
    }

    IEnumerator grapplePull(Vector2 targetPos)
    {
        while (stillShooting)
        {
            Vector2 pullDirection = targetPos - new Vector2(handPosition.position.x, handPosition.position.y);
            float yDist = Mathf.Abs(targetPos.y - handPosition.position.y);
            if (yDist < fallOffDistance)
            {
                pullDirection = new Vector2(pullDirection.x, pullDirection.y * fallOffCurve.Evaluate(yDist / fallOffDistance));
            }

            pullDirection = new Vector2(pullDirection.x * 5f, pullDirection.y);
            myRigid.AddForce(pullDirection * pullForce);
            yield return new WaitForEndOfFrame();
        }

        // Pull Back
        float distanceTravelled = 0;
        while (Vector2.Distance(new Vector2(grapple.transform.position.x, grapple.transform.position.y), handPosition.position) > grappleForgiveness)
        {
            float step = grappleSpeed * Time.deltaTime * grappleSpeedGraph.Evaluate(-(distanceTravelled / grappleMaxDistance));
            distanceTravelled += step;
            grapple.transform.position = Vector2.MoveTowards(grapple.transform.position, handPosition.position, step);
            yield return new WaitForEndOfFrame();
        }
        charAnim.SetBool("StillShooting", false);
        grapple.gameObject.SetActive(false);
        grappleShot = false;
    }

    IEnumerator setCanJump()
    {
        canJump = false;
        yield return new WaitForSeconds(0.1f);
        canJump = true;
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
        hitCoin.GetComponent<Animator>().SetTrigger("CoinLeave");
        StartCoroutine(setCoinInactive(hitCoin));
        coinsCollected += 1;
        coinText.text = coinsCollected.ToString();
        if(coinsCollected % 3 == 0)
        {
            increaseSpeed();
        }
    }

    IEnumerator setCoinInactive(GameObject hitCoin)
    {
        yield return new WaitForSeconds(0.5f);
        hitCoin.SetActive(false);
    }

    void increaseSpeed()
    {
        AIPath[] enemies = FindObjectsOfType<AIPath>();
        foreach(AIPath enemy in enemies)
        {
            enemy.maxSpeed *= 1.5f;
        }
        changeSpeed = true;
    }
}
