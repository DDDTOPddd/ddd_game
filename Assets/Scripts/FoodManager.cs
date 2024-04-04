
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip eat;
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
        audioSource = gameObject.GetComponent<AudioSource>();
        GameObject[] foods = GameObject.FindGameObjectsWithTag("food");
        totalFoodCount = foods.Length;
    }
    public void AddFood()
    {
        totalFoodCount++; 
    }

    public void RemoveFood()
    {
        audioSource.clip = eat;
        audioSource.Play();
        totalFoodCount--; 
        if (totalFoodCount <= 0)
        {
            hole.OpenHole();
        }
    }

    
}
