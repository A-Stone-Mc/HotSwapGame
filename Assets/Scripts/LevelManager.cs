using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{

    public void EndLevel()
    {
        if (!FuelManager.Instance.FuelIsEnough())
        {
            //show fail screen
            RestartLevel();
        }
        else
        {
            ProceedToNextLevel();
        }
    }

    void RestartLevel()
    {
        // Reload the current level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ProceedToNextLevel()
    {
        // Move to the next level
        // SceneManager.LoadScene("NextLevel");
    }
}
