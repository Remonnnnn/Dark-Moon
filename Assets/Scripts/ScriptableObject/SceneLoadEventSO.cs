using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSo,bool> LoadRequestEvent;

    public void RaiseLoadRequestEvent(GameSceneSo locationToLoad,bool canFadeScreen)
    {
        LoadRequestEvent?.Invoke(locationToLoad, canFadeScreen);
    }
}
