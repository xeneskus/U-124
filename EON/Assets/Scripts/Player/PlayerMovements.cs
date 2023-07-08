using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    private PlayerVar _playerVar;


    [Header("hareketEtKahpe")]
    public float moveSpeed;

    public float graundDrag;

    public float jumpForce, jumpCooldown, airMultiplier;
    bool canJump = true;

    //KnockBack
    public float knockBackStrength;


    [Header("Graund islemleri iste ya pff")]
    public float playerHeight;
    public LayerMask GraundLayer;
    bool graunded;

    public Transform annen;

    [SerializeField] float horizontalInput, verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public Animator _handAnim;

    
    private int SmgBullet = 30;
    private float _smgTime = 0;
    private float _smgShootDelay = 0.15f;


    //fire effect
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;
    public Transform tracerStartPoint;

    #region fire

    #endregion

    public float geriTepmeSpeed;


    //graplinggun var.
    public bool freeze;
    public bool activeGrapple;

    private float ShockTime = 2;

    //hand and weapon Uý
    public GameObject weaponImage;
    public GameObject handImage;
    public ParticleSystem shockW;

    //Ses
    public AudioSource shockSound;
    public AudioSource machineGunSounds;
    public AudioSource translateHandSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        _playerVar = GetComponent<PlayerVar>();
    }

    void Update()
    {
        

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && canJump && graunded)
        {
            canJump = false;
            Jump();
            Invoke("ResetJump", jumpCooldown);
        }

        if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y == 0 && graunded == false)
        {
            canJump = false;
            Jump();
            Invoke("ResetJump", jumpCooldown);
        }

        
        
        #region Gun
        

        AnimatorStateInfo currentAnimationState = _handAnim.GetCurrentAnimatorStateInfo(0);

        ShockTime -= Time.deltaTime;
        if (Input.GetMouseButtonDown(1) && ShockTime < 0 && _playerVar.curPil >= 25)
        {
            if (!currentAnimationState.IsName("IdlHand")) return;
            _handAnim.SetTrigger("ShockW");
            Invoke("FireHandShockWawe", 0.5f);
            shockSound.Play();
            ShockTime = 2;
            _playerVar.UsePil(25);
        }
        //if (_playerVar._bulletVar <= 0) { _handAnim.SetTrigger("SMGrelo"); _playerVar._bulletVar = 30; } RELOAD
        //print(_smgTime);

        if (Input.GetMouseButton(0) && currentAnimationState.IsName("MachineGunIdl") && _playerVar._bulletVar > 0 && Time.time >= _smgTime)
        {
            print(_playerVar._bulletVar);
            _smgTime = Time.time + _smgShootDelay;
            _playerVar.UseBullet(1);
            muzzleFlash.Emit(10);
            //_handAnim.SetTrigger("SMGfire");
            machineGunSounds.Play();
            RaycastHit fireHit;
            Ray fireRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            var tracer = Instantiate(tracerEffect, tracerStartPoint.position, Quaternion.identity);
            tracer.AddPosition(tracerStartPoint.position);
            
            if (Physics.Raycast(fireRay, out fireHit))
            {
                hitEffect.transform.position = fireHit.point;
                hitEffect.transform.forward = fireHit.normal;
                hitEffect.Emit(15);

                tracer.transform.position = fireHit.point;
                //Debug.DrawLine(muzzleFlash.transform.position, fireHit.transform.position, Color.red, 1f);
                if (fireHit.transform.gameObject.tag == "Enemy")
                {
                    if (fireHit.transform.gameObject.GetComponent<Rigidbody>() == null) { Destroy(fireHit.transform.gameObject); }
                    else
                    {
                        Vector3 knockDirection = fireHit.transform.position - transform.position;
                        knockDirection.y = 0;

                        Rigidbody enemyRb = fireHit.transform.gameObject.GetComponent<Rigidbody>();
                        enemyRb.AddForce(knockDirection.normalized * knockBackStrength, ForceMode.Impulse);
                    }
                }
            }
            
        }
        #endregion

        if (freeze)
        {
            rb.velocity = Vector3.zero;
        }


        SpeedController();

        //hoplayabilcenmi ona bakim
        graunded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, GraundLayer);
        

        //surtunmeeeee
        if (graunded && !activeGrapple)
        {
            rb.drag = graundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        //change Weapon
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeWeaponAS();
        }
        

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Collect/heart") { Destroy(collision.gameObject); _playerVar.RegenHealth(50); }
        if (collision.transform.tag == "Collect/battery") { Destroy(collision.gameObject); _playerVar.TakePil(50); }
        if (collision.transform.tag == "Collect/bullet") { Destroy(collision.gameObject); _playerVar.TakeBullet(20); }

    }

    private bool changeWeaponBool = false;
    private void ChangeWeaponAS()
    {
        changeWeaponBool = !changeWeaponBool;
        _handAnim.SetBool("chageWeapon", changeWeaponBool);
        translateHandSound.Play();
        if (handImage.activeSelf == true) { weaponImage.SetActive(true); handImage.SetActive(false); }else { weaponImage.SetActive(false); handImage.SetActive(true); }

    }

    private void MovePlayer()
    {
        if (activeGrapple) return;

        moveDirection = annen.forward * verticalInput + annen.right * horizontalInput;

        //yerdeyse
        if (graunded) { rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); }
        else if (!graunded) { rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force); }

    }

    private void SpeedController()
    {
        if (activeGrapple) return;

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        //y vel sifirla
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }

    public void ShotgunReload()
    {
        _handAnim.SetTrigger("Srelo");
        //ShotgunBullet = 2;
    }

    private bool enableMovementOnNextTouch;
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestrictions), 3f);
    }

    public void ResetRestrictions()
    {
        activeGrapple = false;
        
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;

            ResetRestrictions();

            GetComponent<Grappling>().StopGrapple();
        }
    }

    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;

        
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2.5f * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }


    public void FireHandShockWawe()
    {
        shockW.Play();
        RaycastHit fireHit;
        Ray fireRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                
        if (Physics.Raycast(fireRay, out fireHit))
        {

            Vector3 GeriMal = Camera.main.transform.forward * -1;

            rb.AddForce(GeriMal.normalized * geriTepmeSpeed, ForceMode.Impulse);


            if (fireHit.transform.gameObject.tag == "Enemy")
            {
                if (fireHit.transform.gameObject.GetComponent<Rigidbody>() == null) { Destroy(fireHit.transform.gameObject); }
                else
                {
                    Vector3 knockDirection = fireHit.transform.position - transform.position;
                    knockDirection.y = 0;

                    Rigidbody enemyRb = fireHit.transform.gameObject.GetComponent<Rigidbody>();
                    enemyRb.AddForce(knockDirection.normalized * 30, ForceMode.Impulse);
                }
            }
        }
    }


}
