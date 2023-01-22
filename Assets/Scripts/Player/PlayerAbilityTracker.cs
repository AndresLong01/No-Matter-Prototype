using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityTracker : MonoBehaviour
{
  public static PlayerAbilityTracker instance;

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

  //Instantiating the player controller
  private PlayerController player;
  private UIController UI;
  private float initialGravity;

  [Header("Class Swap Section")]
  [SerializeField] GameObject[] availableClasses;
  [SerializeField] GameObject[] activeClasses;
  [SerializeField] float classSwapCDTimer = 5f;
  public int selectedClassIndex;
  private bool canSwapClass;

  [Header("Ability One")]
  public bool canUseAbilityOneA, canUseAbilityOneB;
  private Coroutine AbilityOneACO, AbilityOneBCO;
  private Coroutine AbilityOneACooldownCO, AbilityOneBCooldownCO;

  [Header("Ability Two")]
  public bool canUseAbilityTwoA, canUseAbilityTwoB;
  // private Coroutine AbilityTwoACO, AbilityTwoBCO;
  // private Coroutine AbilityTwoACooldownCO, AbilityTwoBCooldownCO;

  //Can probably move some of these back to fighter
  [Header("Fighter Section")]
  public bool FighterUnlocked;

  [Header("Dwight Section")]
  public bool DwightUnlocked;
  

  // Start is called before the first frame update
  void Start()
  {
    player = PlayerController.instance;
    UI = UIController.instance;
    initialGravity = player.myRigidBody.gravityScale;
    selectedClassIndex = 0;
    canSwapClass = true;

    canUseAbilityOneA =true;
    canUseAbilityOneB = true;
    canUseAbilityTwoA = true; 
    canUseAbilityTwoB = true;
  }
  // ------------------------------------------------------------------- INPUT SYSTEM CALLS ---------------------------------------------------------------------------

  private void OnAbilityOneUse(InputValue value)
  {
    if((selectedClassIndex == 0 && !canUseAbilityOneA) || 
       (selectedClassIndex == 1 && !canUseAbilityOneB))
    {
      return;
    }
    //maybe object mapping, not sure yet
    if (value.isPressed)
    {
      if (GetCurrentClass().name == "Fighter")
      {
        GetCurrentClass().GetComponent<FighterController>().UseAbilityOne();
      }
      else if (GetCurrentClass().name == "Dwight")
      {
        GetCurrentClass().GetComponent<DwightController>().UseAbilityOne();
      }
    }
  }

  // ------------------------------------------------------------------- RECOVERY FRAMES ---------------------------------------------------------------------------

  public void StartRecovery(float recoveryTime)
  {
    StartCoroutine(RecoveryPeriod(recoveryTime));
  }

  //Recovery period if needed for any ability
  IEnumerator RecoveryPeriod(float recoveryTime)
  {
    player.isPlayerRecovering = true;
    yield return new WaitForSeconds(recoveryTime);

    player.isUsingAbility = false;
    player.isPlayerRecovering = false;
  }

  // ----------------------------------------------------------------- SWITCH CLASS METHODS ---------------------------------------------------------------------------
  public GameObject GetCurrentClass()
  {
    return activeClasses[selectedClassIndex];
  }

  private void OnSwitchClass(InputValue value)
  {
    if (activeClasses.Length == 0)
    {
      return;
    }

    if (value.isPressed && canSwapClass)
    {
      player.isUsingAbility = false;
      canSwapClass = false;
      activeClasses[selectedClassIndex].SetActive(!activeClasses[selectedClassIndex].activeSelf);

      if (selectedClassIndex < 1)
      {
        selectedClassIndex = 1;
      }
      else
      {
        selectedClassIndex = 0;
      }

      activeClasses[selectedClassIndex].SetActive(!activeClasses[selectedClassIndex].activeSelf);
      UI.timerController.SetClassTimer(classSwapCDTimer);
      UI.SwapActiveUI(selectedClassIndex);
      StartCoroutine(ClassSwapCooldown());
    }
  }

  IEnumerator ClassSwapCooldown()
  {
    yield return new WaitForSeconds(classSwapCDTimer);

    canSwapClass = true;
  }

  // ----------------------------------------------------------------- Ability One METHODS ---------------------------------------------------------------------------

  public void AbilityOneTrigger(float abilityActiveDuration, float abilityCooldownDuration, bool isInvincibleDuringDuration)
  {
    if (selectedClassIndex == 0 && canUseAbilityOneA)
    {
      canUseAbilityOneA = false;
      player.isUsingAbility = true;

      //Start coroutine for Slot 1
      AbilityOneACO = StartCoroutine(ActivateAbilityOne(abilityActiveDuration, isInvincibleDuringDuration));
      AbilityOneACooldownCO = StartCoroutine(AbilityOneCooldown(abilityCooldownDuration, 0));
    }
    else if (selectedClassIndex == 1 && canUseAbilityOneB)
    {
      canUseAbilityOneB = false;
      //Start coroutine for Slot 2
      AbilityOneBCO = StartCoroutine(ActivateAbilityOne(abilityActiveDuration, isInvincibleDuringDuration));
      AbilityOneBCooldownCO = StartCoroutine(AbilityOneCooldown(abilityCooldownDuration, 1));
    }
  }

  //for example if ability gets interrupted by damage or by collision early in the case of bash
  public void StopAbilityOneEarly()
  {
    if (selectedClassIndex == 0)
    {
      StopCoroutine(AbilityOneACO);
    }
    else if (selectedClassIndex == 1)
    {
      StopCoroutine(AbilityOneBCO);
    }
    player.isUsingAbility = false;
    player.myRigidBody.gravityScale = initialGravity;
  }

  IEnumerator ActivateAbilityOne(float abilityActiveDuration, bool isInvincibleDuringDuration)
  {
    if (isInvincibleDuringDuration)
    {
      FindObjectOfType<PlayerHealthController>().StartInvulnerability(abilityActiveDuration);
    }
    yield return new WaitForSeconds(abilityActiveDuration);

    player.isUsingAbility = false;
    player.myRigidBody.gravityScale = initialGravity;
  }

  IEnumerator AbilityOneCooldown(float abilityCooldownDuration, int storedSelectedClass)
  {
    UI.timerController.SetAbilityOneTimer(selectedClassIndex, abilityCooldownDuration);
    yield return new WaitForSeconds(abilityCooldownDuration);
    
    if(storedSelectedClass == 0)
    {
      canUseAbilityOneA = true;
    }
    else
    {
      canUseAbilityOneB = true;
    }
  }
}
