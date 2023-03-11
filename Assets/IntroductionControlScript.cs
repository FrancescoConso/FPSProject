using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroductionControlScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] canvasArray;

    /*void Start()
    {
        
    }*/

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    public void ShowPanel(int index)
    {
        for(int i=0; i<canvasArray.Length; i++)
        {
            if (i != index)
                canvasArray[i].SetActive(false);
            else
                canvasArray[i].SetActive(true);
        }
    }

    public void StartLevel()
    {
        SceneManager.LoadScene("TestLevel");
    }

}
