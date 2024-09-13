using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelManager : MonoBehaviour
{
    public static FuelManager Instance; 

    public Image fuelBar; // UI will be here
    public float maxFuel = 100f; //amount needed to filll bar
    private float currentFuel; 

    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //keeps the fuel manager in every scene
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        currentFuel = 0f;
        UpdateFuelBar();
    }


    void UpdateFuelBar()
    {
        if (fuelBar != null)
        {
            fuelBar.fillAmount = currentFuel / maxFuel; 
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
}
