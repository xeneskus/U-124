using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovements pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;

    [Header("grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse0;

    private bool grappling;

    //el
    public GameObject _robotHand;
    Vector3 handPos;

    //ses
    public AudioSource graplingSound;



    private void Start()
    {
        pm = GetComponent<PlayerMovements>();
        handPos = _robotHand.transform.position;
    }

    private void Update()
    {
        
        AnimatorStateInfo currentAnimationState = pm._handAnim.GetCurrentAnimatorStateInfo(0);
        if (Input.GetKeyDown(grappleKey) && currentAnimationState.IsName("IdlHand")) { StartGrapple(); }

        if(grapplingCdTimer > 0)
        {
            grapplingCdTimer -= Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        if(grappling) 
        { 
            lr.SetPosition(0, gunTip.position);
        }
    }

    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;
        graplingSound.Play();
        grappling = true;

        pm.freeze = true;

        RaycastHit hit;
        if(Physics.Raycast(cam.position , cam.forward, out hit,maxGrappleDistance,whatIsGrappleable))
        {
            grapplePoint = hit.point;
            
            //var hand = Instantiate(_robotHand , new Vector3(hit.point.x , hit.point.y, hit.point.z) - new Vector3(0,100,0) , hit.transform.rotation);
            //hand.transform.position = hit.point;
            //Destroy(hand, 2);
            _robotHand.SetActive(false);

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {
        pm.freeze = false;

        

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        pm.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        pm.freeze = false;

        grappling = false;

        grapplingCdTimer = grapplingCd;

        lr.enabled = false;
        _robotHand.SetActive(true);
    }



}
