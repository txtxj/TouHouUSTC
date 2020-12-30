using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Settings : MonoBehaviour
{
	public Slider slider;
	public AudioSource source;
	public Text percentage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitSettings()
    {
    	
    }

    public void ChangeVolume()
    {
    	source.volume = slider.value / 100f;
    	percentage.text = slider.value.ToString("F0") + "%";
    }
}
