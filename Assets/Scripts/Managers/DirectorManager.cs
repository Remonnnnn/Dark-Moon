using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DirectorManager : MonoBehaviour
{ 
    public static DirectorManager instance;
    private PlayableDirector director;
    private Dictionary<string, PlayableBinding> bindingDict;

    [Header("Camera")]
    public GameObject MainCM;
    public GameObject BossCM;

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
    private void Start()
    {
        director = GetComponent<PlayableDirector>();
        bindingDict=new Dictionary<string, PlayableBinding>();
    }

    public void SetPlayable(PlayableAsset asset,Animator character)
    {
        director.playableAsset = asset;

        GetBinding();
        SetBinding(character.name, character.gameObject);

        director.Play();
    }

    public void GetBinding()
    {
        bindingDict.Clear();
        foreach (var bind in director.playableAsset.outputs)
        {
            if (!bindingDict.ContainsKey(bind.streamName))
            {
                bindingDict.Add(bind.streamName, bind);
            }
        }
    }

    public void SetBinding(string streamName, GameObject gameObject)
    {
        if(bindingDict.TryGetValue(streamName,out PlayableBinding track))
        {
            director.SetGenericBinding(track.sourceObject, gameObject);
        }
    }

}

