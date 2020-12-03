using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    Loader loader = new Loader();
    // Start is called before the first frame update
    void Start()
    {
        loader.LoadSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Skip(int destination)
    {
        SceneManager.LoadScene(destination);
    }

    //To quit the game
    public void Quit()
    {
        Application.Quit();
    }
}
