using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    //Parameta in Unity veränderbar
    Rigidbody rb;
    private IEnumerator coroutine;

    [SerializeField] float movmentSpeedslow;
    [SerializeField] float movmentSpeedfast;
    [SerializeField] float jumpForce;
    [SerializeField] float djumpForce;
    [SerializeField] float dashForce;
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;
    [SerializeField] float airMultiplier;
    [SerializeField] float sphereCastRadiusWall;
    [SerializeField] float sphereCastRadiusSign;
    [SerializeField] float dedectionLengthWall;
    [SerializeField] float dedectionLengthSign;
    [SerializeField] float jumpTime;
    [SerializeField] LayerMask ground;
    [SerializeField] LayerMask destr;
    [SerializeField] LayerMask whatisWall;
    [SerializeField] LayerMask whatisSign;
    [SerializeField] Object coin;
    [SerializeField] GameObject Button;
    [SerializeField] Transform groundCheck;

    public Material Green;
    public bool candestroy = false;
    public bool signfront = false;
    
    bool canmove;
    bool doublejump = false;
    public bool canjump = false;
    public bool candash = true;
    bool wallFront;
    bool stoppedJumping;
    bool buttonon = false;
    float jumpTimeCounter;
    float movmentSpeed;
    float smoothTime = 0.1f;
    float smaoothVelocity;
    bool dashing;
    RaycastHit frontWallHit;
    RaycastHit frontSignHit;
    RaycastHit foodprintyes;
    Vector3 directionfacing;
    public ShakeCamera shakecamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        directionfacing = new Vector3(1.0f, 0.0f, 0.0f);
        jumpTimeCounter = jumpTime;
 
        
    }

    private void FixedUpdate()
    {
        Debug.Log( candestroy);
        //drag bei Boden und in der Luft
        if (canjump)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag;

        //Geschwindigkeits Stufen
        if (dashing)
        {
            movmentSpeed = movmentSpeedfast * 10;
        }
        else if (Input.GetButton("Sprint") && canjump == true)
        {
            movmentSpeed = movmentSpeedfast;
        }
        else
        {
            movmentSpeed = movmentSpeedslow;
        }

        // Geschwindigkeit limitieren
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > movmentSpeedfast)
        {
            Vector3 limitedVel = flatVel.normalized * movmentSpeedfast;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            Debug.Log("Zuschnell");
        }

        //Steuerung wasd
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        var camera = Camera.main;
        var jump = new Vector3(0.0f, 2.0f, 0.0f);
        

        Vector3 forward = camera.transform.forward;
        Vector3 right = camera.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        //Steuerung relativ zur Kamera
        Vector3 desiredMoveDirection = forward * verticalAxis + right * horizontalAxis;
        if (canmove)
        {
            if (canjump)
                rb.AddForce(desiredMoveDirection.normalized * movmentSpeed * 10, ForceMode.Force);

            if (!canjump)
                rb.AddForce(desiredMoveDirection.normalized * movmentSpeed * airMultiplier * 10, ForceMode.Force);

        }


        //rotieren des Spielers

        if (desiredMoveDirection.magnitude >= 0.1f)
        {
            float targetangle = Mathf.Atan2(desiredMoveDirection.x, desiredMoveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref smaoothVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            directionfacing = desiredMoveDirection;
        }


        Debug.Log(movmentSpeed);
        //


        if (Input.GetMouseButtonDown(0) && !canjump && candash)
        {

            candestroy = true;
            coroutine = Dash(forward, right, verticalAxis, horizontalAxis);
            StartCoroutine(coroutine);

            
        }


        /*
        if (Input.GetMouseButtonDown(0) && !canjump && candash)
        {
            Vector3 dashforward = forward * dashForce;
            Vector3 dashright = right * dashForce;
            float TimeInterval = 0;

            candash = false;
            candestroy = true;

            while (TimeInterval <= 3)
            {
                rb.AddForce(dashforward * verticalAxis + right * horizontalAxis, ForceMode.Impulse);
                TimeInterval = Time.deltaTime + TimeInterval;
                rb.useGravity = false;

            }
            rb.useGravity = true;

        }
        //dash resest
        if (canjump && !candash)
        {
            candash = true;
            candestroy = false;
        }
        */
        //GroundPound
        if (!canjump && Input.GetKeyDown("e"))
        {



            coroutine = GroundPound();
            StartCoroutine(coroutine);
        }

        //WallJump
        WallCheck(desiredMoveDirection);

        if (wallFront && Input.GetButtonDown("Jump") && !canjump)
        {
            rb.AddForce(-desiredMoveDirection.x * 3000f, jump.y * jumpForce * 10, -desiredMoveDirection.z * 3000f);



            Debug.Log(jump.y * jumpForce);
        }

        //Jump and doublejump
        IsGrounded();

        if (canjump)
        {

            jumpTimeCounter = jumpTime;
        }


        if (Input.GetButtonDown("Jump") && canjump)//bei betätigen der Taste
        {

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(jump * jumpForce, ForceMode.Impulse);

            FindObjectOfType<AudioManager>().Play("PlayerJumpSound");

            stoppedJumping = false;

        }

        if (Input.GetButton("Jump") && !canjump && !stoppedJumping)//beim halten der Taste 
        {

            if (jumpTimeCounter < 1.6 && jumpTimeCounter > 0.13)
            {

                rb.AddForce(jump, ForceMode.Impulse);

                jumpTimeCounter -= Time.deltaTime;

            }
            stoppedJumping = false;
        }

        if (Input.GetButtonUp("Jump") && !stoppedJumping)//beim loslassen der Taste
        {
            jumpTimeCounter = 0;
            stoppedJumping = true;
            doublejump = true;
        }



        else if (Input.GetButtonDown("Jump") && doublejump && stoppedJumping)//doubleJump
        {

            doublejump = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);

        }
        if (canjump)//Jumpreset
        {
            candestroy = false;
            canmove = true;
            
            candash = true;
        }



    }
    void Update()
    {




    }

    //wen player colides 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyHead"))
        {
            Destroy(collision.transform.parent.gameObject);
        }
        if (collision.gameObject.CompareTag("Barral") && candestroy)
        {
            Instantiate(coin, collision.transform.position, Quaternion.identity);
            Destroy(collision.transform.gameObject);
        }
        if (collision.gameObject.CompareTag("Button") && candestroy)
        {
            collision.gameObject.GetComponent<MeshRenderer>().material = Green;
            buttonon = true;
        }
    }

    //Schild UI
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "IsSign")
        {
            signfront = true;
        }
        else
        {
            signfront = false;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        signfront = false;
    }

    public bool ButtonIsGreen()
    {
        return buttonon;
    }

    private IEnumerator GroundPound()
    {

        rb.useGravity = false;
        rb.velocity = new Vector3(0f, 0f, 0f);
        canmove = false;


        yield return new WaitForSeconds(.5f);


        rb.useGravity = true;
        candestroy = true;
        rb.AddForce(new Vector3(0.0f, -15.0f, 0.0f), ForceMode.Impulse);


       
    }

    private IEnumerator Dash(Vector3 forward, Vector3 right,float verticalAxis, float horizontalAxis)
    {
        Vector3 dashforward = forward * dashForce;
        Vector3 dashright = right * dashForce;


        candash = false;
        dashing = true;
        rb.useGravity = false;
        rb.AddForce(dashforward * verticalAxis + dashright * horizontalAxis, ForceMode.Impulse);
        

        yield return new WaitForSeconds(0.3f);
        rb.useGravity = true;
        movmentSpeed = movmentSpeedslow;
        dashing = false;
        yield return new WaitForSeconds(1f);
        candestroy = false;

    }



    //checks if player is grounded
    void IsGrounded()
    {
        canjump = (Physics.CheckSphere(groundCheck.position, .1f, ground));
        
    }


    //foodprints
    /*
    void foodprints()
    {
        Physics.Raycast(transform.position, 0.0f, -1.0f, 0.0f,out foodprintyes)

    }
    */
    //infront of Wall
    void WallCheck(Vector3 desiredMoveDirection)
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastRadiusWall, desiredMoveDirection, out frontWallHit, dedectionLengthWall, whatisWall);

    }




}
