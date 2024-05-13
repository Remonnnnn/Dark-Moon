using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Game Scene/GameSceneSO")]
public class GameSceneSo : ScriptableObject
{
    public SceneType sceneType;

    public AssetReference sceneReference;

    public Vector2 startPos;

    public int bgmIndex;


}
