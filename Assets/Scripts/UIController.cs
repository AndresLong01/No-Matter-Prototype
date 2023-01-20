using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//TODO: for pause menu later
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
  public static UIController instance;

  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  [Header("Resource Bars")]
  public Slider healthSlider;

  void Start()
  {

  }

  void Update()
  {

  }

  public void UpdateHealth(int currentHealth, int maxHealth)
  {
    healthSlider.maxValue = maxHealth;
    healthSlider.value = currentHealth;
  }
}
