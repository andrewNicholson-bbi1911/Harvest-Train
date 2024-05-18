using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaderBridge : MonoBehaviour
{
    public void LoadScene(int sceneIndex) => SceneLoader.LoadNewScene(sceneIndex);
}
