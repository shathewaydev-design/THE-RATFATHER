using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class SoldatoScript : MonoBehaviour
{

    // core stages (4):
    // 1. Chase
    // 2. Stomp
    // 3. Charge wind up
    // 4. Charge attack

    public Transform player;
    public NavMeshAgent agent;

    public int health = 16;

    public float stompRange = 2f;
    public float stompCooldown = 2f;

    public float chargeCooldown = 15f;
    public float chargeSpeed = 15f;
    public float chargeDuration = 1.5f;

    private float stompTimer;
    private float chargeTimer;

    float windupTime = 3f;
    float windupTimer = 0f;

    float dashTimer = 0f;

    float stunDuration = 10f;
    float stunTimer = 0f;

    public enum State { Chase, Stomp, ChargeWindup, ChargeDash, Stunned }
    public State currentState;

    private Vector3 chargeDirection;

    public LineRenderer chargeLine;
    public float maxChargeDistance = 10f;

    public static SoldatoScript Instance;

    public CraneScript currCrane;


    int currBossHealthImg = 0;
    public Texture[] bossHealth;
    public RawImage bossHealthImg;

    float damageCooldown = 1.5f;   
    float lastDamageTime = -999f;

    public Animator animator;

    void Awake()
    {
        Instance = this;
        GameManager.Instance.inBossScene = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        NavMeshHit hit;

        //GameManager.Instance.inBossScene = true;

        if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            agent.Warp(hit.position);
        }
        else
        {
            Debug.LogError("No NavMesh found near boss spawn point!");
        }



        currentState = State.Chase;





    }

    // Update is called once per frame
    void Update()
    {
        stompTimer += Time.deltaTime;
        //chargeTimer += Time.deltaTime;
        //Debug.Log("Charge Timer: " + chargeTimer);
        CheckHealth();

        // movement test
        // agent.SetDestination(player.position);


        switch (currentState)
        {
            case State.Chase:
                Chase();
                break;

            case State.Stomp:
                Stomp();
                break;

            case State.ChargeWindup:
                ChargeWindup();
                break;

            case State.ChargeDash:
                ChargeDash();
                break;
            case State.Stunned:
                Stunned();
                break;
        }



    }

    public void TakeDamage()
    {
        if (Time.time < lastDamageTime + damageCooldown)
        {
            return;
        }

        lastDamageTime = Time.time;

        health -= 4;


        Debug.Log("Boss Takes Damage! " + health);

        UpdateBossHealth();

        EnterStun();

    }

    void EnterStun()
    {
        currentState = State.Stunned;

        //animator.SetBool("IsStunned", true);
        animator.SetTrigger("StunTrigger");

        stunTimer = 0f;

        // stop everything immediately
        agent.isStopped = true;
        agent.ResetPath();

        //rb.linearVelocity = Vector3.zero;

        chargeTimer = 0f;
        windupTimer = 0f;
        dashTimer = 0f;

    }

    void Stunned()
    {


        animator.SetBool("IsStunned", true);

        stunTimer += Time.deltaTime;

        agent.isStopped = true; 

        //rb.linearVelocity = Vector3.zero;

        if (stunTimer >= stunDuration)
        {

            animator.SetBool("IsStunned", false);

            animator.SetBool("IsCharging", false);

            stunTimer = 0f;

            currentState = State.Chase;

            
        }
    }


    void CheckHealth()
    {
        if (health <= 0)
        {
            // trigger ending (for now comic ending + demo ending)
            GameManager.Instance.blackScreenPanel.SetActive(true);
            GameManager.Instance.cutsceneText.text = "Thank you for playing the vertical slice of THE RAT FATHER. ALT+F4 to quit.";
            // triggered in game manager
            Debug.Log("Boss is dead!!!");
            agent.isStopped = true;

        }
    }

    void Chase()
    {
        //animator.SetBool("StunnedOver", false);
        animator.SetFloat("Speed", agent.velocity.magnitude);

        chargeLine.enabled = false; // to be safe, line is disabled in other states

        agent.isStopped = false;
        agent.SetDestination(player.position);

        chargeTimer += Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= stompRange)
        {
            currentState = State.Stomp;
            //Debug.Log("Stomping!"); // move to stomp method?
            return;
        }

        if (CanCharge() && chargeTimer >= chargeCooldown)
        {
            chargeTimer = 0f; 
            chargeTimer = 0f; 
            windupTimer = 0f;


            currentState = State.ChargeWindup;
            agent.isStopped = true;
        }

    }

    bool CanCharge()
    {
        if (chargeTimer >= 10)
        {
            //animator.SetBool("TimerIsUp", true);
            return true;
        }

        return false;
    }

    void Stomp()
    {

        animator.SetTrigger("Stomp");

        chargeLine.enabled = false;

        agent.isStopped = true;

        transform.LookAt(player);

        if (stompTimer >= stompCooldown)
        {
            stompTimer = 0f;
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= stompRange)
            {
                //Debug.Log("Player takes stomp damage");
                // need to apply damage here

                GameManager.Instance.TakeDamage();

            }
            else
            {
                //animator.ResetTrigger("Stomp");
                //animator.ResetTrigger("Walking");
                chargeTimer = 0f;
                currentState = State.Chase;
            }


        }
    }

    void ChargeWindup()
    {

        animator.SetBool("IsCharging", true);

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        //rb.linearVelocity = Vector3.zero;
        agent.ResetPath(); // ensures stops boss

        windupTimer += Time.deltaTime;

        // rotating towards player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {

            transform.rotation = Quaternion.LookRotation(direction);

        }


        // enable line
        chargeLine.enabled = true;
        chargeLine.positionCount = 2;
        chargeLine.startWidth = 0.6f;
        chargeLine.endWidth = 0.6f;

        // START 
        Vector3 start = transform.position;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5f))
        {
            start = hit.point + Vector3.up * 0.05f;
        }

        // END
        Vector3 end = player.position;
        if (Physics.Raycast(player.position, Vector3.down, out RaycastHit hit2, 5f))
        {
            end = hit2.point + Vector3.up * 0.05f;
        }

        chargeLine.SetPosition(0, start);
        chargeLine.SetPosition(1, end);

        // check timer
        if (windupTimer >= windupTime)
        {

            windupTimer = 0f;
            chargeTimer = 0f;

            chargeDirection = transform.forward;

            currentState = State.ChargeDash;

        }


    }


    private bool chargeAnimPlayed;

    void ChargeDash()
    {

        animator.SetTrigger("Charge");

        chargeLine.enabled = false;

        agent.isStopped = false;


        // ONLY trigger animation once
        if (!chargeAnimPlayed)
        {
            animator.SetTrigger("Charge");
            chargeAnimPlayed = true;
        }


        //agent.enabled = false;

        dashTimer += Time.deltaTime;

        //agent.isStopped = true;

        transform.position += chargeDirection * chargeSpeed * Time.deltaTime;
        //rb.linearVelocity = chargeDirection * chargeSpeed;

        if (dashTimer >= chargeDuration) 
        {
            chargeAnimPlayed = false;

            animator.SetBool("IsCharging", false);
            //animator.SetBool("TimerIsUp", false);

            dashTimer = 0f;
            chargeTimer = 0f;
            windupTimer = 0f;

            //rb.linearVelocity = Vector3.zero; // check this

            //agent.enabled = true;
            agent.Warp(transform.position);

            currentState = State.Chase;
        }

        //dashTimer = 0f;


    }


    private float lastCraneHitTime = -999f;
    private float currentWindowDuration = 0f;

    public void StartCraneWindow(CraneScript sourceCrane, float duration)
    {
        currCrane = sourceCrane;
        lastCraneHitTime = Time.time;
        currentWindowDuration = duration;

        //Debug.Log("Crane window started by: " + sourceCrane.name);
       
    }

    public bool IsCraneWindowActive()
    {
        return Time.time <= lastCraneHitTime + currentWindowDuration;
    }


    void OnTriggerEnter(Collider other)
    {

        //Debug.Log("Something entered: " + other.name);

        if (other.gameObject.CompareTag("MetalBeam"))
        {
            if (IsCraneWindowActive())
            {
                Debug.Log("VALID crane hit!");
                // disable crane
                currCrane.hitBoss = true;
                TakeDamage();
            }
            else
            {
                Debug.Log("Hit ignored");
                // need to reset
                if (currCrane == null)
                {
                    return;
                }
                currCrane.hitBoss = false;

            }
        }
    }


    public void UpdateBossHealth()
    {

        if (currBossHealthImg >= bossHealth.Length)
        {
            return;
        }

        bossHealthImg.texture = bossHealth[currBossHealthImg];
        currBossHealthImg += 1;

    }

}


