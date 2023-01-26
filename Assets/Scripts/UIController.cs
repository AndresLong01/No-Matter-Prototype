using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
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

  private PlayerAbilityTracker abilityTracker;
  private PlayerController player;

  [Header("Pause Screen")]
  [SerializeField] GameObject pauseScreen;
  [SerializeField] GameObject loadoutSelectionA;
  [SerializeField] Image currentClassA;
  [SerializeField] GameObject loadoutSelectionB;
  [SerializeField] Image currentClassB;

  [Header("Resource Bars")]
  public Slider healthSlider;

  [Header("Timers")]
  public Timer timerController;
  [SerializeField] Image classSwapTimer;

  [Header("Class LoadoutTimers")]
  [SerializeField] Image mobilityAbilityA;
  [SerializeField] Image abilityOneA;
  [SerializeField] Image abilityTwoA;
  [SerializeField] Image mobilityAbilityB;
  [SerializeField] Image abilityOneB;
  [SerializeField] Image abilityTwoB;

  [Header("Class One Game Objects")]
  [SerializeField] GameObject[] classOneGameObjects;

  [Header("Class Two Game Objects")]
  [SerializeField] GameObject[] classTwoGameObjects;

  void Start()
  {
    abilityTracker = PlayerAbilityTracker.instance;
    player = PlayerController.instance;
    timerController = FindObjectOfType<Timer>();
  }

  void Update()
  {
    if(Input.GetKeyDown(KeyCode.Escape))
    {
      PauseUnpause();
    }

    classSwapTimer.fillAmount = timerController.fillFractionClassSwap;

    //class index 0
    mobilityAbilityA.fillAmount = timerController.fillFractionMovementAbilityA;
    abilityOneA.fillAmount = timerController.fillFractionAbilityOneA;
    abilityTwoA.fillAmount = timerController.fillFractionAbilityTwoA;

    //class index 1
    mobilityAbilityB.fillAmount = timerController.fillFractionMovementAbilityB;
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

  public void PauseUnpause()
  {
    pauseScreen.SetActive(!pauseScreen.activeSelf);

    if(pauseScreen.activeSelf)
    {
      LoadCurrentClassLoadout();
      player.GetComponent<PlayerInput>().currentActionMap.Disable();
      loadoutSelectionA.SetActive(false);
      loadoutSelectionB.SetActive(false);
      Time.timeScale = 0f;
    }
    else
    {
      foreach(GameObject playerClass in abilityTracker.availableClasses)
      {
        playerClass.SetActive(false);
        timerController.CancelTimers();
        abilityTracker.ResetCooldowns();
      }
      abilityTracker.activeClasses[0].SetActive(true);
      abilityTracker.selectedClassIndex = 0;
      SwapActiveUI(0);
      player.GetComponent<PlayerInput>().currentActionMap.Enable();
      Time.timeScale = 1f;
    }
  }

  private void LoadCurrentClassLoadout()
  {
    Sprite classIconA = abilityTracker.activeClasses[0].GetComponent<ClassUIFeatures>().classIcon;
    Sprite classIconB = abilityTracker.activeClasses[1].GetComponent<ClassUIFeatures>().classIcon;

    currentClassA.sprite = classIconA;
    currentClassB.sprite = classIconB;
  }

  public void RevealLoadoutA()
  {
    loadoutSelectionB.SetActive(false);
    loadoutSelectionA.SetActive(!loadoutSelectionA.activeSelf);
  }

  public void RevealLoadoutB()
  {
    loadoutSelectionA.SetActive(false);
    loadoutSelectionB.SetActive(!loadoutSelectionA.activeSelf);
  }

  //TODO: have to run check to make sure you don't select the same class twice
  public void selectActiveClassA(int unlockedClassId)
  {
    if(abilityTracker.activeClasses[1].name == abilityTracker.availableClasses[unlockedClassId].name)
    {
      return;
    }

    abilityTracker.activeClasses[0] = abilityTracker.availableClasses[unlockedClassId];
    LoadCurrentClassLoadout();
  }

  public void selectActiveClassB(int unlockedClassId)
  {
    if(abilityTracker.activeClasses[0].name == abilityTracker.availableClasses[unlockedClassId].name)
    {
      return;
    }

    abilityTracker.activeClasses[1] = abilityTracker.availableClasses[unlockedClassId];
    LoadCurrentClassLoadout();
  }
}


