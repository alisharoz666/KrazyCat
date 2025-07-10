using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public GameObject lockIcon;
    public bool isLocked;
    Animator anim;
    bool keyUsed = false;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (keyUsed == false && KeyManager.Instance.UseKey())
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
        keyUsed = true;
        isLocked = false;
        lockIcon.SetActive(false);
    }
}
