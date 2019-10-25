using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        Cursor.visible = false;
    }

    // Update is called once per frame

    void Update()
    {
        //selects the animation state depending on the keys pressed
        if (Input.GetKey(KeyCode.W))
        {
            m_Animator.SetInteger("MotionState", 2);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            m_Animator.SetInteger("MotionState", 3);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            m_Animator.SetInteger("MotionState", 4);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            m_Animator.SetInteger("MotionState", 5);
        }
        else
        {
            m_Animator.SetInteger("MotionState", 1);
        }
    }
}
