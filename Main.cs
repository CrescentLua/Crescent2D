using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour {
    bool Success = false;
    bool Fail = false; 
   public InputField Answer;
    public Text ErrorMessage;

    void Start()
    {
       
    }

    void Update()
    {
        if (Success == true)
        {
            StartCoroutine(Complete());
            Success = false;
        }

        if (Fail == true)
        {
            StartCoroutine(Failed());
            Fail = false;
        }
    }

    IEnumerator Complete()
    {
        
        yield return new WaitForSeconds(1.0f);
        ErrorMessage.text = "Success, unlocking next level...";
        ErrorMessage.color = new Color(0.0f, 1.0f, 0.0f);

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        ErrorMessage.text = " ";
    }

    IEnumerator Failed()
    {
       
        yield return new WaitForSeconds(1.0f);
        ErrorMessage.text = "Failed, please try again...";
        ErrorMessage.color = new Color(1.0f, 0.0f, 0.0f);

        yield return new WaitForSeconds(1.0f);
        ErrorMessage.text = " ";
    }

    public void SubmitAnswer()
    {
        if (Answer.text == "871")
        {
            Debug.Log("Answer is correct! You may go through!");
            Success = true;
        }
        if (Answer.text != "871")
        {
            Debug.Log("Answer is wrong, you may not go through!");
            Fail = true;
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}




    