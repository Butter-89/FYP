using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public Material skybox1;
    private void Awake()
    {
        RenderSettings.skybox = skybox1;
    }
}
