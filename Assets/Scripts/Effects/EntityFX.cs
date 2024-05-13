using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class EntityFX : MonoBehaviour
{
    protected Player player;
    protected SpriteRenderer sr;

    [Header("Pop Up text")]
    [SerializeField] private GameObject popUpTextPrefab;

    [Header("Flash FX")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originalMat;

    [Header("Ailment colors")]
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] shockColor;

    [Header("Ailment particles")]
    [SerializeField] private ParticleSystem igniteFx;
    [SerializeField]private ParticleSystem chillFx;
    [SerializeField]private ParticleSystem shockFx;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFx;
    [SerializeField] private GameObject criticalHitFx;

    [Header("Buff FX")]
    [SerializeField] private ParticleSystem recoverFx;

    private GameObject myHealthBar;


    protected virtual void Start()
    {
        sr= GetComponentInChildren<SpriteRenderer>();
        player = PlayerManager.instance.player;
        originalMat = sr.material;
    }

    public void CreatePopUpText(string _text,Color _color)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(-1, 1);

        Vector3 positionOffset = new Vector3(randomX, randomY, 0);

        GameObject myText = Instantiate(popUpTextPrefab, transform.position + positionOffset, Quaternion.identity);
        myText.GetComponent<TextMeshPro>().color = _color;
        myText.GetComponent<TextMeshPro>().text = _text;
    }

    public void MakeTransprent(bool _transprent)
    {
        if (_transprent)
        {
            myHealthBar?.SetActive(false);
            sr.color = Color.clear;
        }
        else
        {
            myHealthBar?.SetActive(true);
            sr.color = Color.white;
        }
    }
    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor=sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        sr.color = currentColor;
        sr.material = originalMat;

    }

    private void RedColorBlink()
    {
        sr.color = (sr.color == Color.white) ? Color.red : Color.white;
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color= Color.white;

        igniteFx.Stop();
        chillFx.Stop();
        shockFx.Stop();
    }

    public void IgniteFxFor(float _seconds)//点燃
    {
        igniteFx.Play();

        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);//_seconds后关闭颜色变化
    }

    public void ChillFxFor(float _seconds)//冰冻
    {
        chillFx.Play();

        InvokeRepeating("ChillColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }


    public void ShockFxFor(float _seconds)
    {
        shockFx.Play();

        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
        {
            sr.color = igniteColor[0];
        }
        else
        {
            sr.color = igniteColor[1];
        }
    }
    private void ChillColorFx()
    {
        if (sr.color != chillColor[0])
        {
            sr.color = chillColor[0];
        }
        else
        {
            sr.color = chillColor[1];
        }
    }
    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
        {
            sr.color = shockColor[0];
        }
        else
        {
            sr.color = shockColor[1];
        }
    }

    public void CreateHitFx(Transform _target,bool _critical)
    {
        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        Vector3 hitFxRotation = new Vector3(0, 0, zRotation);

        GameObject hitPrefab = hitFx;

        if(_critical)
        {
            hitPrefab = criticalHitFx;

            float yRotation = 0;
            zRotation = Random.Range(-45, 45);

            if(GetComponent<Entity>().facingDir==-1)//如果朝左
            {
                yRotation = 180;
            }

            hitFxRotation = new Vector3(0, yRotation, zRotation);
        }

        GameObject newHitFx = Instantiate(hitPrefab, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);

        newHitFx.transform.Rotate(hitFxRotation);

        Destroy(newHitFx, .5f);


    }

    public void PlayRecoverFx()
    {
        recoverFx.Play();
    }
}
