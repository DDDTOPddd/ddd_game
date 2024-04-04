using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [SerializeField]
    private int totalFoodCount = 0;
    public Hole hole;
    public static FoodManager Instance { get; private set; }

    private void Awake()
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
        GameObject[] foods = GameObject.FindGameObjectsWithTag("food");
        totalFoodCount = foods.Length;
        // Store the number of food items as needed
    }
    public void AddFood()
    {
        totalFoodCount++; 
    }

    public void RemoveFood()
    {
        totalFoodCount--; 
        if (totalFoodCount <= 0)
        {
            hole.OpenHole();
        }
    }

    
}
