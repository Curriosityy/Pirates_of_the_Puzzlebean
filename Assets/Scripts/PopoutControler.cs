using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopoutControler : MonoBehaviour
{
    public Animator animator;
    private Text popoutText;

    private void Awake()
    {
        popoutText = animator.GetComponent<Text>();
    }

    // Use this for initialization
    private void Start()
    {
        Destroy(gameObject, animator.runtimeAnimatorController.animationClips[0].length - 0.1f);
    }

    public void SetText(string xtext)
    {
        popoutText.text = xtext;
    }

    public void SetColor(Color xcolor)
    {
        popoutText.color = xcolor;
    }
}