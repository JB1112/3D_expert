using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpGround : MonoBehaviour
{
    public float jumpForce;
    private PlayerController controller;

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller; //컨트롤러 입력
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            controller.GetComponent<Rigidbody>().AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            controller.GetComponent<Animator>().SetTrigger("IsJump");
        }
    }
}
