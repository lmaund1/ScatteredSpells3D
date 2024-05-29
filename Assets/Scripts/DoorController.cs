using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{ 
    public Animator animator;

    public string keyRequired = string.Empty;
    public GameObject doorBlock;
    public AudioClip creak;
    public AudioClip creak2;
    public AudioClip doorShut;
    
    private AudioSource audioSource;
    private BoxCollider doorBlockCollider;

    private void Awake()
    {
        doorBlockCollider = doorBlock.GetComponent<BoxCollider>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
 
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
                animator.SetTrigger("open");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("close");
        }
    }

    public void PlayDoorShut()
    {
        audioSource.clip = doorShut;
        audioSource.Play();
    }

    public void PlayDoorOpen()
    {
        audioSource.clip = creak;
        audioSource.Play();
    }

    private void RemoveDoorBlock()
    {
        doorBlockCollider.enabled = false;
    }

    private void AddDoorBlock()
    {
        doorBlockCollider.enabled = true;
    }
}
