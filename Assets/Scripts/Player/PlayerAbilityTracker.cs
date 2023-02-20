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
  public GameObject[] availableClasses;
  public GameObject[] activeClasses;
  public float classSwapCDTimer = 5f;
  public int selectedClassIndex;
  private bool canSwapClass;

  [Header("Movement Ability")]
  public bool canUseMovementA, canUseMovementB;
  private Coroutine MovementACO, MovementBCO;
  private Coroutine MovementACooldownCO, MovementBCooldownCO;

  [Header("Ability One")]
  public bool canUseAbilityOneA, canUseAbilityOneB;
  private Coroutine AbilityOneACO, AbilityOneBCO;
  private Coroutine AbilityOneACooldownCO, AbilityOneBCooldownCO;

  [Header("Ability Two")]
  public bool canUseAbilityTwoA, canUseAbilityTwoB;
  private Coroutine AbilityTwoACO, AbilityTwoBCO;
  private Coroutine AbilityTwoACooldownCO, AbilityTwoBCooldownCO;

  //Can probably move some of these back to fighter
  [Header("Fighter Section")]
  public bool fighterUnlocked;

  [Header("Dwight Section")]
  public bool dwightUnlocked;


  // Start is called before the first frame update
  void Start()
  {
    player = PlayerController.instance;
    UI = UIController.instance;
    initialGravity = player.myRigidBody.gravityScale;
    selectedClassIndex = 0;
    canSwapClass = true;

    canUseMovementA = true;
    canUseMovementB = true;
    canUseAbilityOneA = true;
    canUseAbilityOneB = true;
    canUseAbilityTwoA = true;
    canUseAbilityTwoB = true;
  }
  // ------------------------------------------------------------------- INPUT SYSTEM CALLS ---------------------------------------------------------------------------

  private void OnMobilityUse(InputValue value)
  {
    if ((selectedClassIndex == 0 && !canUseMovementA) ||
       (selectedClassIndex == 1 && !canUseMovementB))
    {
      return;
    }

    if (value.isPressed)
    {
      if (GetCurrentClass().name == "Fighter")
      {
        GetCurrentClass().GetComponent<FighterController>().UseMovementAbility();
      }
      else if (GetCurrentClass().name == "Dwight")
      {
        // GetCurrentClass().GetComponent<DwightController>().UseMovementAbility();
      }
    }
  }

  private void OnAbilityOneUse(InputValue value)
  {
    if ((selectedClassIndex == 0 && !canUseAbilityOneA) ||
       (selectedClassIndex == 1 && !canUseAbilityOneB))
    {
      return;
    }
    //maybe object mapping, not sure yet
    // REPLICATE CLASSUIFEATURES LOGIC WITH THIS ONE
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

  private void OnAbilityTwoUse(InputValue value)
  {
    if ((selectedClassIndex == 0 && !canUseAbilityTwoA) ||
       (selectedClassIndex == 1 && !canUseAbilityTwoB))
    {
      return;
    }
    //maybe object mapping, not sure yet
    // REPLICATE CLASSUIFEATURES LOGIC WITH THIS Two
    if (value.isPressed)
    {
      if (GetCurrentClass().name == "Fighter")
      {
        GetCurrentClass().GetComponent<FighterController>().UseAbilityTwo();
      }
      else if (GetCurrentClass().name == "Dwight")
      {
        // GetCurrentClass().GetComponent<DwightController>().UseAbilityTwo();
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

    if (FindObjectOfType<PlayerAttackController>().isPlayerAttacking)
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

  // ----------------------------------------------------------------- Movement Ability METHODS ---------------------------------------------------------------------------

  public void MovementAbilityTrigger(float abilityActiveDuration, float abilityCooldownDuration, bool isInvincibleDuringDuration)
  {
    if (selectedClassIndex == 0 && canUseMovementA)
    {
      canUseMovementA = false;
      player.isUsingAbility = true;

      //Start coroutine for Slot 1
      MovementACO = StartCoroutine(ActivateMovementAbility(abilityActiveDuration, isInvincibleDuringDuration));
      MovementACooldownCO = StartCoroutine(MovementAbilityCooldown(abilityCooldownDuration, 0));
    }
    else if (selectedClassIndex == 1 && canUseMovementB)
    {
      canUseMovementB = false;
      player.isUsingAbility = true;
      //Start coroutine for Slot 2
      MovementBCO = StartCoroutine(ActivateMovementAbility(abilityActiveDuration, isInvincibleDuringDuration));
      MovementBCooldownCO = StartCoroutine(MovementAbilityCooldown(abilityCooldownDuration, 1));
    }
  }

  //for example if ability gets interrupted by damage or by collision early in the case of bash
  public void StopMovementAbilityEarly()
  {
    if (selectedClassIndex == 0)
    {
      StopCoroutine(MovementACO);
    }
    else if (selectedClassIndex == 1)
    {
      StopCoroutine(MovementBCO);
    }
    player.isUsingAbility = false;
    player.myRigidBody.gravityScale = initialGravity;
  }

  IEnumerator ActivateMovementAbility(float abilityActiveDuration, bool isInvincibleDuringDuration)
  {
    var stackedChildren = player.transform.GetComponentsInChildren<Transform>(includeInactive: true);
    if (isInvincibleDuringDuration)
    {
      player.gameObject.layer = LayerMask.NameToLayer("No Contact");
      foreach (var child in stackedChildren)
      {
        child.gameObject.layer = LayerMask.NameToLayer("No Contact");
      }
    }

    yield return new WaitForSeconds(abilityActiveDuration);

    player.isUsingAbility = false;
    player.gameObject.layer = LayerMask.NameToLayer("Player");
    foreach (var child in stackedChildren)
    {
      child.gameObject.layer = LayerMask.NameToLayer("Player");
    }
    player.myRigidBody.gravityScale = initialGravity;
  }

  IEnumerator MovementAbilityCooldown(float abilityCooldownDuration, int storedSelectedClass)
  {
    UI.timerController.SetMovementAbilityTimer(selectedClassIndex, abilityCooldownDuration);
    yield return new WaitForSeconds(abilityCooldownDuration);

    if (storedSelectedClass == 0)
    {
      canUseMovementA = true;
    }
    else
    {
      canUseMovementB = true;
    }
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
      player.isUsingAbility = true;
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

    if (storedSelectedClass == 0)
    {
      canUseAbilityOneA = true;
    }
    else
    {
      canUseAbilityOneB = true;
    }
  }

  // ----------------------------------------------------------------- Ability Two METHODS ---------------------------------------------------------------------------

  public void AbilityTwoTrigger(float abilityActiveDuration, float abilityCooldownDuration, bool isInvincibleDuringDuration)
  {
    if (selectedClassIndex == 0 && canUseAbilityTwoA)
    {
      canUseAbilityTwoA = false;
      player.isUsingAbility = true;

      //Start coroutine for Slot 1
      AbilityTwoACO = StartCoroutine(ActivateAbilityTwo(abilityActiveDuration, isInvincibleDuringDuration));
      AbilityTwoACooldownCO = StartCoroutine(AbilityTwoCooldown(abilityCooldownDuration, 0));
    }
    else if (selectedClassIndex == 1 && canUseAbilityTwoB)
    {
      canUseAbilityTwoB = false;
      player.isUsingAbility = true;
      //Start coroutine for Slot 2
      AbilityTwoBCO = StartCoroutine(ActivateAbilityTwo(abilityActiveDuration, isInvincibleDuringDuration));
      AbilityTwoBCooldownCO = StartCoroutine(AbilityTwoCooldown(abilityCooldownDuration, 1));
    }
  }

  //for example if ability gets interrupted by damage or by collision early in the case of bash
  public void StopAbilityTwoEarly()
  {
    if (selectedClassIndex == 0)
    {
      StopCoroutine(AbilityTwoACO);
    }
    else if (selectedClassIndex == 1)
    {
      StopCoroutine(AbilityTwoBCO);
    }
    player.isUsingAbility = false;
    player.myRigidBody.gravityScale = initialGravity;
  }

  IEnumerator ActivateAbilityTwo(float abilityActiveDuration, bool isInvincibleDuringDuration)
  {
    if (isInvincibleDuringDuration)
    {
      FindObjectOfType<PlayerHealthController>().StartInvulnerability(abilityActiveDuration);
    }
    yield return new WaitForSeconds(abilityActiveDuration);

    player.isUsingAbility = false;
    player.myRigidBody.gravityScale = initialGravity;
  }

  IEnumerator AbilityTwoCooldown(float abilityCooldownDuration, int storedSelectedClass)
  {
    UI.timerController.SetAbilityTwoTimer(selectedClassIndex, abilityCooldownDuration);
    yield return new WaitForSeconds(abilityCooldownDuration);

    if (storedSelectedClass == 0)
    {
      canUseAbilityTwoA = true;
    }
    else
    {
      canUseAbilityTwoB = true;
    }
  }

  public void ResetCooldowns()
  {
    // Movement coroutines reset
    if (MovementACooldownCO != null)
    {
      StopCoroutine(MovementACooldownCO);
    }
    if (MovementBCooldownCO != null)
    {

      StopCoroutine(MovementBCooldownCO);
    }

    canUseMovementA = true;
    canUseMovementB = true;

    //Ability One coroutines reset
    if (AbilityOneACooldownCO != null)
    {
      StopCoroutine(AbilityOneACooldownCO);
    }
    if (AbilityOneBCooldownCO != null)
    {

      StopCoroutine(AbilityOneBCooldownCO);
    }

    canUseAbilityOneA = true;
    canUseAbilityOneB = true;

    //Ability Two coroutines reset
    if (AbilityTwoACooldownCO != null)
    {
      StopCoroutine(AbilityTwoACooldownCO);
    }
    if (AbilityTwoBCooldownCO != null)
    {

      StopCoroutine(AbilityTwoBCooldownCO);
    }

    canUseAbilityTwoA = true;
    canUseAbilityTwoB = true;
  }
}
