using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
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

    private int ShotgunBullet = 2;

    //fire effect
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;
    public Transform tracerStartPoint;

    #region fire

    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
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
        #region Shotgun
        if (ShotgunBullet <= 0) { _handAnim.SetTrigger("Srelo"); ShotgunBullet = 2; }

        AnimatorStateInfo currentAnimationState = _handAnim.GetCurrentAnimatorStateInfo(0);
        if (Input.GetMouseButtonDown(0) && currentAnimationState.IsName("shotgun") && ShotgunBullet > 0)
        {
            ShotgunBullet--;
            muzzleFlash.Emit(1);
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



        SpeedController();

        //hoplayabilcenmi ona bakim
        graunded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, GraundLayer);
        

        //surtunmeeeee
        if (graunded)
        {
            rb.drag = graundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _handAnim.SetTrigger("deneme");
        }

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        moveDirection = annen.forward * verticalInput + annen.right * horizontalInput;

        //yerdeyse
        if (graunded) { rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); }
        else if (!graunded) { rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force); }

    }

    private void SpeedController()
    {
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
  

}
