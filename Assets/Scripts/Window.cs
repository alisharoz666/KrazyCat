using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public GameObject lockIcon;
    public bool isLocked;
    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (KeyManager.Instance.UseKey())
                Unlock();
        }
        if (other.gameObject.CompareTag("Player") && !isLocked)
        {
            anim.SetTrigger("Open");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isLocked)
        {
            anim.SetTrigger("Close");
        }
    }
    public void Unlock()
    {
        isLocked = false;
        lockIcon.SetActive(false);
    }
}
