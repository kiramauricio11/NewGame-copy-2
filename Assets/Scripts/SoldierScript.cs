using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class SoldierScript : MonoBehaviour
{
    /*
    private Animator anim;
    private Rigidbody rb;
    private float yrot = 0;
    private float speed = 0.02f;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            RaycastHit hit;
            // check if moving forward in the distance of speed*5
            // will hit anything, if will not hit anything, animate running and move
            // otherwise stop animating running and not move
            if (rb.SweepTest(transform.forward, out hit, speed * 5) == false)
            {
                anim.SetBool("running", true);
                transform.position = transform.position + transform.forward * speed;
            }
            else
            {
                anim.SetBool("running", false);
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            RaycastHit hit;
            // check if moving backward in the distance of speed*5 will hit anything
            // if will not hit anything, animate running and move
            // otherwise stop animating running and not move
            if (rb.SweepTest(-transform.forward, out hit, speed * 5) == false)
            {
                anim.SetBool("running", true);
                transform.position = transform.position - transform.forward * speed;
            }
            else
                anim.SetBool("running", false);
        }
        else  // no W key or S key pressed
        {
            anim.SetBool("running", false);
        }

        
        if (Input.GetKey(KeyCode.A))
        {
            yrot -= 1.0f; // in degree
            Quaternion targetRotation = Quaternion.Euler(0, yrot, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation,
        targetRotation, Time.deltaTime * 10);
        }
        if (Input.GetKey(KeyCode.D))
        {
            yrot += 1.0f; // in degree
            Quaternion targetRotation = Quaternion.Euler(0, yrot, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation,
        targetRotation, Time.deltaTime * 10);
        }
        


    }
    */

    public float speed = 6f;

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;

    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        bool running = h != 0f || v != 0f;
        anim.SetBool("running", running);
    }
}
