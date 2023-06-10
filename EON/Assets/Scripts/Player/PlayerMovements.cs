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


    [Header("Graund islemleri iste ya pff")]
    public float playerHeight;
    public LayerMask GraundLayer;
    bool graunded;

    public Transform annen;

    [SerializeField] float horizontalInput, verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public Animator _handAnim;


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

        AnimatorStateInfo currentAnimationState = _handAnim.GetCurrentAnimatorStateInfo(0);
        if (Input.GetMouseButtonDown(0) && currentAnimationState.IsName("shotgun"))
        {
            RaycastHit fireHit;
            Ray fireRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(fireRay, out fireHit))
            {
                if (fireHit.transform.gameObject.tag == "Enemy")
                {
                    fireHit.transform.gameObject.SetActive(false);
                }
            }

        }


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

}
