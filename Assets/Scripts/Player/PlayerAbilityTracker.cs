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

  //Can probably move some of these back to fighter
  [Header("Fighter Section")]
  public bool FighterUnlocked, canBash;
  [SerializeField] GameObject shieldObject;
  [SerializeField] float bashActiveTimer = 0.3f;
  [SerializeField] float bashCooldownTimer = 3f;

  private Coroutine shieldBashInstance;

  [Header("Dwight Section")]
  public bool DwightUnlocked, canScreamDwight;
  [SerializeField] float dwightCooldownTimer = 25f;

  // Start is called before the first frame update
  void Start()
  {
    player = PlayerController.instance;
    UI = UIController.instance;
    initialGravity = player.myRigidBody.gravityScale;
    selectedClassIndex = 0;
    canSwapClass = true;

    //little funny easter egg
    canScreamDwight = true;
  }
  // ------------------------------------------------------------------- ABILITY USE ---------------------------------------------------------------------------

  //TODO: change this in the future in accordance to multiple abilitys used
  private void OnAbilityUse(InputValue value)
  {
    //maybe object mapping, not sure yet
    if (value.isPressed)
    {
      if (GetCurrentClass().name == "Fighter" && canBash)
      {
        GetCurrentClass().GetComponent<FighterController>().UseAbilityOne();
      }
      else if (GetCurrentClass().name == "Dwight" && canScreamDwight)
      {
        GetCurrentClass().GetComponent<DwightController>().UseAbilityOne();
        // for testing purposes
        StartCoroutine(TestDwight(dwightCooldownTimer));
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

    player.isUsingMovementAbility = false;
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
      player.isUsingMovementAbility = false;
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

  // ----------------------------------------------------------------- FIGHTER METHODS ---------------------------------------------------------------------------
  public void useShieldBash()
  {
    shieldBashInstance = StartCoroutine(ShieldBash(bashActiveTimer));
    StartCoroutine(ShieldBashCooldown(bashCooldownTimer));
  }

  //stopping the bash on collision
  public void stopShieldBashEarly()
  {
    StopCoroutine(shieldBashInstance);
    canBash = false;
    player.isUsingMovementAbility = false;
    shieldObject.SetActive(false);
    player.myRigidBody.gravityScale = initialGravity;
  }

  IEnumerator ShieldBash(float bashActiveTimer)
  {
    //Gives player invincibility for bash
    player.isPlayerRecovering = true;
    canBash = false;

    yield return new WaitForSeconds(bashActiveTimer);

    player.isUsingMovementAbility = false;
    player.isPlayerRecovering = false;
    shieldObject.SetActive(false);
    // player.myRigidBody.velocity = new Vector2(0f, 0f);
    player.myRigidBody.gravityScale = initialGravity;
  }

  IEnumerator ShieldBashCooldown(float bashCooldownTimer)
  {
    UI.timerController.SetAbilityOneTimer(selectedClassIndex, bashCooldownTimer);
    yield return new WaitForSeconds(bashCooldownTimer);

    canBash = true;
  }

  // ----------------------------------------------------------------- DWIGHT METHODS ---------------------------------------------------------------------------
  IEnumerator TestDwight(float dwightCooldownTimer)
  {
    UI.timerController.SetAbilityOneTimer(selectedClassIndex, dwightCooldownTimer);
    canScreamDwight = false;

    yield return new WaitForSeconds(dwightCooldownTimer);
    canScreamDwight = true;
  }
}
