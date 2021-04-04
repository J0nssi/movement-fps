using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    public void PracticeRoom()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void CreateMatch()
    {
        //TO BE DONE
    }
    
    public void JoinMatch()
    {
        //TO BE DONE
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
