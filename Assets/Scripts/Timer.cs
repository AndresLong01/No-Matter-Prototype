using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
  // fill fractions calculated for the Cooldowns
  public float fillFractionClassSwap;
  public float fillFractionAbilityOneA, fillFractionAbilityOneB;
  public float fillFractionAbilityTwoA, fillFractionAbilityTwoB;

  // class Swap cooldown Timer
  private float maxTimerValueClassSwap, currentTimerValueClassSwap;

  //Ability one, dependent on current selected class
  private float abilityOneAMaxTimer, abilityOneACurrentTimer;
  private float abilityOneBMaxTimer, abilityOneBCurrentTimer;

  //Ability two, dependent on current selected class
  private float abilityTwoAMaxTimer, abilityTwoACurrentTimer;
  private float abilityTwoBMaxTimer, abilityTwoBCurrentTimer;

  private void Update()
  {
    UpdateTimer();
  }

  public void SetClassTimer(float timeToSet)
  {
    maxTimerValueClassSwap = timeToSet;
    currentTimerValueClassSwap = timeToSet;
  }

  public void SetAbilityOneTimer(int selectedClassIndex, float timeToSet)
  {
    //class index 0
    if (selectedClassIndex == 0)
    {
      abilityOneAMaxTimer = timeToSet;
      abilityOneACurrentTimer = timeToSet;
    }

    //class index 1
    if (selectedClassIndex == 1)
    {
      abilityOneBMaxTimer = timeToSet;
      abilityOneBCurrentTimer = timeToSet;
    }
  }

  public void SetAbilityTwoTimer(int selectedClassIndex, float timeToSet)
  {
    //class index 0
    if (selectedClassIndex == 0)
    {
      abilityTwoAMaxTimer = timeToSet;
      abilityTwoACurrentTimer = timeToSet;
    }

    //class index 1
    if (selectedClassIndex == 1)
    {
      abilityTwoBMaxTimer = timeToSet;
      abilityTwoBCurrentTimer = timeToSet;
    }
  }

  private void UpdateTimer()
  {
    if (currentTimerValueClassSwap > 0)
    {
      currentTimerValueClassSwap -= Time.deltaTime;
      fillFractionClassSwap = currentTimerValueClassSwap / maxTimerValueClassSwap;
    }

    //might need revision
    if (abilityOneACurrentTimer > 0 || abilityTwoACurrentTimer > 0)
    {
      abilityOneACurrentTimer -= Time.deltaTime;
      fillFractionAbilityOneA = abilityOneACurrentTimer / abilityOneAMaxTimer;

      abilityTwoACurrentTimer -= Time.deltaTime;
      fillFractionAbilityTwoA = abilityTwoACurrentTimer / abilityTwoAMaxTimer;
    }

    if (abilityOneBCurrentTimer > 0 || abilityTwoBCurrentTimer > 0)
    {
      abilityOneBCurrentTimer -= Time.deltaTime;
      fillFractionAbilityOneB= abilityOneBCurrentTimer / abilityOneBMaxTimer;

      abilityTwoBCurrentTimer -= Time.deltaTime;
      fillFractionAbilityTwoB= abilityTwoBCurrentTimer / abilityTwoBMaxTimer;
    }
  }

  public void CancelTimers()
  {
    // resets all timers
    currentTimerValueClassSwap = 0;
    abilityOneACurrentTimer = 0;
    abilityOneBCurrentTimer = 0;
    abilityTwoACurrentTimer = 0;
    abilityTwoBCurrentTimer = 0;
  }
}
