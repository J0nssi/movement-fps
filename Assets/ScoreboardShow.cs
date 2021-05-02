using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardShow : MonoBehaviour
{
    public GameObject scoreBoard;
    bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        scoreBoard.SetActive(isActive);
    }

    // Update is called once per frame
    void Update()
    {
        isActive = Input.GetKey(KeyCode.Tab);
        if (Cursor.lockState == CursorLockMode.Locked)
		{ 
            scoreBoard.SetActive(isActive);
        } else
		{
            scoreBoard.SetActive(false);
        }
    }
}
