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

  [Header("Timers")]
  public Timer timerController;
  [SerializeField] Image classSwapTimer;

  [Header("Class LoadoutTimers")]
  [SerializeField] Image abilityOneA;
  [SerializeField] Image abilityTwoA;
  [SerializeField] Image abilityOneB;
  [SerializeField] Image abilityTwoB;

  [Header("Class One Game Objects")]
  [SerializeField] GameObject[] classOneGameObjects;

  [Header("Class Two Game Objects")]
  [SerializeField] GameObject[] classTwoGameObjects;

  void Start()
  {
    timerController = FindObjectOfType<Timer>();
  }

  void Update()
  {
    classSwapTimer.fillAmount = timerController.fillFractionClassSwap;

    //class index 0
    abilityOneA.fillAmount = timerController.fillFractionAbilityOneA;
    abilityTwoA.fillAmount = timerController.fillFractionAbilityTwoA;

    //class index 1
    abilityOneB.fillAmount = timerController.fillFractionAbilityOneB;
    abilityTwoB.fillAmount = timerController.fillFractionAbilityTwoB;
  }

  public void UpdateHealth(int currentHealth, int maxHealth)
  {
    healthSlider.maxValue = maxHealth;
    healthSlider.value = currentHealth;
  }

  public void SwapActiveUI(int selectedClassIndex)
  {
    if (selectedClassIndex == 0)
    {
      foreach (GameObject classUI in classOneGameObjects)
      {
        classUI.SetActive(true);
      }
      foreach (GameObject classUI in classTwoGameObjects)
      {
        classUI.SetActive(false);
      }
    }
    else if (selectedClassIndex == 1)
    {
      foreach (GameObject classUI in classOneGameObjects)
      {
        classUI.SetActive(false);
      }
      foreach (GameObject classUI in classTwoGameObjects)
      {
        classUI.SetActive(true);
      }

    }
  }


}


