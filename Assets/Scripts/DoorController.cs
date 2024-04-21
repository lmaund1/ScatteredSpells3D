using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{ 
    public Animator animator;

    public string keyRequired = string.Empty;
    public GameObject doorBlock;
    private BoxCollider doorBlockCollider;
    private bool doorOpening = false;
    private bool isDoorOpen = false;
    private bool isDoorClosing = false;

    private void Awake()
    {
        doorBlockCollider = doorBlock.GetComponent<BoxCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (doorOpening)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 1f)
            {
                doorOpening = false;
                isDoorOpen = true;
                doorBlockCollider.enabled = false;
            }
        }
        else if (isDoorClosing)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 1f)
            {
                isDoorClosing = false;
                isDoorOpen = false;
                doorBlockCollider.enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bool canOpen = false;
            if(keyRequired == string.Empty)
            {
                canOpen = true;
            }
            else
            {
                canOpen = GameController.Instance.keysHeld.Contains(keyRequired);
            }
            if (canOpen)
            {
                doorOpening = true;
                isDoorOpen = false;
                animator.SetTrigger("open");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isDoorOpen || doorOpening)
            {
                isDoorOpen = false;
                doorOpening = false;
                isDoorClosing = true;
                animator.SetTrigger("close");
            }
            
        }
    }
}
