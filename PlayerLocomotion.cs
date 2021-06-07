using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    public CharacterController controller; 
    public float speed = 12f; 
    public float gravity = -9.841f;
    public float jumpHeight = 3f; 

    public Transform groundCheck;
    public float groundDistance = 0.4f; 
    public LayerMask groundMask; 

    Vector3 velocity; 
    public bool isGrounded; 

    private float walkSpeed = 3f; 
    private float sprintSpeed = 3.5f; 

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); 

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }
        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical"); 

        Vector3 move = transform.right * x + transform.forward * z; 

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); 
        }

        velocity.y += gravity * Time.deltaTime; 
        controller.Move(velocity * Time.deltaTime);
        
        Sprint();
    }

    void Sprint()
    {
        if(Input.GetButtonDown("Sprint"))
        {
            speed = sprintSpeed; 
        }
        if(Input.GetButtonUp("Sprint"))
        {
            speed = walkSpeed; 
        }
    }

    public void OnCollisionEnter(Collision col)
    {
       if(col.gameObject.name == "ZombieHeadshot")
       {
           if(groundCheck.position.y >= col.gameObject.transform.localPosition.y)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, col.gameObject.transform.localPosition.y, gameObject.transform.position.z); 
            }
       }
    }
}
