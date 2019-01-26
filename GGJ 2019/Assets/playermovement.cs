using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{

    public CharacterController2D controller;

    public float runspeed = 40f;
    float horizontalmove = 0f;
    bool jp = false;

   

    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        horizontalmove = Input.GetAxisRaw("Horizontal") * runspeed;

        if (Input.GetKeyDown("space"))
        {
            jp = true;
        }

    }

    void FixedUpdate()
    {
        controller.Move(horizontalmove * Time.fixedDeltaTime, jp);
        jp = false;
    }
}
