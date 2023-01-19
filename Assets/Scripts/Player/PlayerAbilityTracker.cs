using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityTracker : MonoBehaviour
{
  public static PlayerAbilityTracker instance;

  private void Awake() {
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
  PlayerController player;
  private float initialGravity;

  [SerializeField] GameObject[] availableClasses;
  [SerializeField] GameObject[] activeClasses;
  [SerializeField] float classSwapCDTimer= 5f;
  private int selectedClassIndex;
  private bool canSwapClass;

  [Header("Fighter Section")]
  public bool FighterUnlocked, canBash;
  [SerializeField] GameObject shieldObject;
  [SerializeField] float bashActiveTimer = 0.3f;
  [SerializeField] float bashCooldownTimer = 3f;

  private Coroutine shieldBashInstance, shieldBashCooldown;

  // Start is called before the first frame update
  void Start()
  {
    player = PlayerController.instance;
    initialGravity = player.myRigidBody.gravityScale;
    selectedClassIndex = 0;
    canSwapClass = true;
  }

  //TODO: change this in the future in accordance to multiple skills used
  private void OnSkillUse(InputValue value)
  {
    //maybe object mapping, not sure yet
    if (value.isPressed)
    {
      if(activeClasses[selectedClassIndex].name == "Fighter")
      {
        activeClasses[selectedClassIndex].GetComponent<FighterController>().UseSkillOne();
      }
      else if (activeClasses[selectedClassIndex].name == "Dwight")
      {
        activeClasses[selectedClassIndex].GetComponent<DwightController>().UseSkillOne();
      }
    }
  }

  public void StartRecovery(float recoveryTime)
  {
    StartCoroutine(RecoveryPeriod(recoveryTime));
  }

  //Recovery period if needed for any skill
  IEnumerator RecoveryPeriod(float recoveryTime)
  {
    player.isPlayerRecovering = true;
    yield return new WaitForSeconds(recoveryTime);

    player.isUsingMovementSkill = false;
    player.isPlayerRecovering = false;
  }

  private void OnSwitchClass(InputValue value)
  {
    if (activeClasses.Length == 0)
    {
      return;
    }

    if (value.isPressed && canSwapClass)
    {
      player.isUsingMovementSkill = false;
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
      StartCoroutine(ClassSwapCooldown());
    }
  }

  IEnumerator ClassSwapCooldown()
  {
    yield return new WaitForSeconds(classSwapCDTimer);

    canSwapClass = true;
  }

  //using bash from Fighter Controller
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
    shieldObject.SetActive(false);
    player.myRigidBody.gravityScale = initialGravity;
  }

  IEnumerator ShieldBash(float bashActiveTimer)
  {
    canBash = false;

    yield return new WaitForSeconds(bashActiveTimer);
    player.isUsingMovementSkill = false;
    shieldObject.SetActive(false);
    player.myRigidBody.gravityScale = initialGravity;
  }

  IEnumerator ShieldBashCooldown(float bashCooldownTimer)
  {
    yield return new WaitForSeconds(bashCooldownTimer);

    Debug.Log("Off cooldown");
    canBash = true;
  }


}
