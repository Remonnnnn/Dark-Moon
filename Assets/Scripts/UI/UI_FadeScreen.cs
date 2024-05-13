using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FadeScreen : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim= GetComponent<Animator>();
    }

    public void FadeOut() => anim.SetTrigger("fadeOut");//½¥³ö Öð½¥±äÁÁ
    public void FadeIn() => anim.SetTrigger("fadeIn");//½¥Èë Öð½¥ºÚÆÁ
}
