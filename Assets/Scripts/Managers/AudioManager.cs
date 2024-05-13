using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool playBgm;
    public int bgmIndex;

    private bool canPlaySFX;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        if(!playBgm)
        {
            StopAllBGM();
        }
        else
        {
            if (!bgm[bgmIndex].isPlaying)
            {
                PlayBGM(bgmIndex);
            }
        }

        Invoke("AllowSFX", 2f);
    }
    public void PlaySFX(int _sfxIndex,Transform _source)
    {
        //if (sfx[_sfxIndex].isPlaying)
        //{
        //    return;
        //}

        if(!canPlaySFX)
        {
            return;
        }

        if(_source!=null && Vector2.Distance(PlayerManager.instance.player.transform.position,_source.position)>sfxMinimumDistance)//超过一定距离不播放
        {
            return;
        }

        if(_sfxIndex<sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(.85f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _index) => sfx[_index].Stop();

    public void StopSFXWithTime(int _index)
    {
        StartCoroutine(DecreaseVolume(sfx[_index]));
    }

    public void StopBGMWithTime()
    {
        StartCoroutine(DecreaseVolume(bgm[bgmIndex]));
    }

    public void TransformBGMTo(int _index)
    {
        StartCoroutine(DecreaseVolumeTransform(bgm[bgmIndex],_index));
    }

    private IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultVolume=_audio.volume;

        while(_audio.volume>.1f)
        {
            _audio.volume -= _audio.volume * .3f;
            yield return new WaitForSeconds(.6f);

            if(_audio.volume<=.1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;
                Debug.Log("finish bgm");
                break;
            }
        }

        playBgm = false;
    }

    private IEnumerator DecreaseVolumeTransform(AudioSource _audio,int _index)
    {
        yield return DecreaseVolume(_audio);
        playBgm=true;
        bgmIndex= _index;
    }
    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int _bgmIndex)
    {
        if(_bgmIndex==-1)
        {
            playBgm=false;
            return;
        }

        bgmIndex = _bgmIndex;

        StopAllBGM();
        bgm[bgmIndex].Play();
    }

    public void StopAllBGM()
    {
        Debug.Log("StopAllBGM");
        for(int i=0;i<bgm.Length;i++)
        {
            bgm[i].Stop();
        }
    }

    private void AllowSFX()=>canPlaySFX = true;

    public void CanPlayBGM() => playBgm = true;
    public void CannotPlayBGM()=>playBgm = false;

}
