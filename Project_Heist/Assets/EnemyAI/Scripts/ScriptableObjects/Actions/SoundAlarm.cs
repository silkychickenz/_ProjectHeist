using UnityEngine;
using EnemyAI;

[CreateAssetMenu(menuName = "Enemy AI/Actions/Sound Alarm")]
public class SoundAlarm : Action
{ 
	// The act function, called on Update() (State controller - current state - action).
	public override void Act(StateController controller)
	{
		return;
	}

	public override void OnEnableAction(StateController controller)
    {
		if (controller.m_squad && !controller.m_squad.alarmOn)
		{
			controller.m_squad.SoundAlarm(controller.personalTarget);
		}
	}
}
