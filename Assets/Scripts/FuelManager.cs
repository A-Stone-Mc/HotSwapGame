using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class FuelManager : MonoBehaviour
{
    public static FuelManager Instance; 

    public Image fuelBar; // UI will be here
    public float maxFuel = 100f; //amount needed to filll bar
    public float currentFuel; 

    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        ResetFuel();
        currentFuel = 0f;
        UpdateFuelBar();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetFuel();
    }

    public void UpdateFuelBar()
    {
        if (fuelBar != null)
        {
            fuelBar.fillAmount = currentFuel / maxFuel; 
        }
    }
    void AssignFuelBar()
    {
        if (fuelBar == null)
        {
            fuelBar = GameObject.Find("Fill").GetComponent<Image>(); // Find the fuel bar in the scene by name
        }
    }

    
    public void AddFuel(float amount)
    {
        currentFuel += amount;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);//limits to max amount
        UpdateFuelBar();
    }

    public bool FuelIsEnough()
    {
        return currentFuel >= maxFuel;
    }

    public void ResetFuel()
    {
        currentFuel = 0f; // Reset currentFuel to zero
        UpdateFuelBar();  // Update the fuel bar UI
    }


    private void OnDestroy()
    {
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
