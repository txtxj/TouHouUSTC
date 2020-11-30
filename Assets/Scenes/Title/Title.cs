using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
