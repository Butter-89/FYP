using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour {

    public float _switchWeatherTimer = 0f;
    public float _resetWeatherTimer = 10f;

    public ParticleSystem _sunCloudsParticleSystem;
    public ParticleSystem _mistParticleSystem;
    public ParticleSystem _eruptionParticleSystem;

    private ParticleSystem.EmissionModule _sunClouds;
    private ParticleSystem.EmissionModule _mist;
    private ParticleSystem.EmissionModule _eruption;


    public Material[] skyboxList;
    private int _skyboxNumber;
    public WeatherStates _weatherState;
    private int _switchWeather;

    public enum WeatherStates
    {
        PickWeather,
        SunnyWeather,
        MistWeather,
        EruptionWeather,
    }
	// Use this for initialization
	void Start ()
    {
        _skyboxNumber = Random.Range(0, skyboxList.Length);
        RenderSettings.skybox = skyboxList[_skyboxNumber];
        StartCoroutine(weatherFSM());

        _sunClouds = _sunCloudsParticleSystem.emission;
        _mist = _mistParticleSystem.emission;
        _eruption = _eruptionParticleSystem.emission;
	}
	
	// Update is called once per frame
	void Update () {
        SwitchWeatherTimer();

	}

    void SwitchWeatherTimer()
    {
        //Debug.Log("SwitchWeatherTimer");

        _switchWeatherTimer -= Time.deltaTime;

        if (_switchWeatherTimer < 0)
            _switchWeatherTimer = 0;

        if (_switchWeatherTimer > 0)
            return;

        if (_switchWeatherTimer == 0)
            _weatherState = WeatherManager.WeatherStates.PickWeather;

        _switchWeatherTimer = _resetWeatherTimer;
    }

    IEnumerator weatherFSM()
    {
        while(true)
        {
            switch(_weatherState)
            {
                case WeatherStates.PickWeather:
                    PickWeather();
                    break;
                case WeatherStates.SunnyWeather:
                    SunnyWeather();
                    break;
                case WeatherStates.MistWeather:
                    MistWeather();
                    break;
                case WeatherStates.EruptionWeather:
                    EruptionWeather();
                    break;
            }
            yield return null;
        }
    }

    void PickWeather()
    {
        _switchWeather = Random.Range(0, 3);

        _sunCloudsParticleSystem.enableEmission = false;
        _mistParticleSystem.enableEmission = false;
        _eruptionParticleSystem.enableEmission = false;
        //_mist.enabled = false;



        switch (_switchWeather)
        {
            case 0:
                _weatherState = WeatherManager.WeatherStates.SunnyWeather;
                break;
            case 1:
                _weatherState = WeatherManager.WeatherStates.MistWeather;
                break;
            case 2:
                _weatherState = WeatherManager.WeatherStates.EruptionWeather;
                break;
        }
     

    }

    void SunnyWeather()
    {
        _sunClouds.enabled = true;

    }

    void MistWeather()
    {

        _mist.enabled = true;
    }

    void EruptionWeather()
    {

        _eruption.enabled = true;

    }
}





