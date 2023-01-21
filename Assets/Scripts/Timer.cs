using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
  // fill fractions calculated for the Cooldowns
  public float fillFractionClassSwap;
  public float fillFractionSkillOneA, fillFractionSkillOneB;
  public float fillFractionSkillTwoA, fillFractionSkillTwoB;

  // class Swap cooldown Timer
  private float maxTimerValueClassSwap, currentTimerValueClassSwap;

  //Skill one, dependent on current selected class
  private float skillOneAMaxTimer, skillOneACurrentTimer;
  private float skillOneBMaxTimer, skillOneBCurrentTimer;

  //Skill two, dependent on current selected class
  private float skillTwoAMaxTimer, skillTwoACurrentTimer;
  private float skillTwoBMaxTimer, skillTwoBCurrentTimer;

  private void Update()
  {
    UpdateTimer();
  }

  public void SetClassTimer(float timeToSet)
  {
    maxTimerValueClassSwap = timeToSet;
    currentTimerValueClassSwap = timeToSet;
  }

  public void SetSkillOneTimer(int selectedClassIndex, float timeToSet)
  {
    //class index 0
    if (selectedClassIndex == 0)
    {
      skillOneAMaxTimer = timeToSet;
      skillOneACurrentTimer = timeToSet;
    }

    //class index 1
    if (selectedClassIndex == 1)
    {
      skillOneBMaxTimer = timeToSet;
      skillOneBCurrentTimer = timeToSet;
    }
  }

  public void SetSkillTwoTimer(int selectedClassIndex, float timeToSet)
  {
    //class index 0
    if (selectedClassIndex == 0)
    {
      skillTwoAMaxTimer = timeToSet;
      skillTwoACurrentTimer = timeToSet;
    }

    //class index 1
    if (selectedClassIndex == 1)
    {
      skillTwoBMaxTimer = timeToSet;
      skillTwoBCurrentTimer = timeToSet;
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
    if (skillOneACurrentTimer > 0 || skillTwoACurrentTimer > 0)
    {
      skillOneACurrentTimer -= Time.deltaTime;
      fillFractionSkillOneA = skillOneACurrentTimer / skillOneAMaxTimer;

      skillTwoACurrentTimer -= Time.deltaTime;
      fillFractionSkillTwoA = skillTwoACurrentTimer / skillTwoAMaxTimer;
    }

    if (skillOneBCurrentTimer > 0 || skillTwoBCurrentTimer > 0)
    {
      skillOneBCurrentTimer -= Time.deltaTime;
      fillFractionSkillOneB= skillOneBCurrentTimer / skillOneBMaxTimer;

      skillTwoBCurrentTimer -= Time.deltaTime;
      fillFractionSkillTwoB= skillTwoBCurrentTimer / skillTwoBMaxTimer;
    }
  }

  public void CancelTimers()
  {
    // resets all timers
    currentTimerValueClassSwap = 0;
    skillOneACurrentTimer = 0;
    skillOneBCurrentTimer = 0;
    skillTwoACurrentTimer = 0;
    skillTwoBCurrentTimer = 0;
  }
}
