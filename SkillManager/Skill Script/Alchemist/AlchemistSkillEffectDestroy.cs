using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class AlchemistSkillEffectDestroy : MonoBehaviour
{
	// If true, deactivate the object instead of destroying it
	public bool OnlyDeactivate;
	
	void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}
	
	IEnumerator CheckIfAlive ()
	{
		ParticleSystem ps = this.GetComponent<ParticleSystem>();
		while(true && ps != null)
		{
			yield return new WaitForSeconds(0.3f);
			if(!ps.IsAlive(true))
			{
				if (transform.parent.GetComponent<LifeBodyComponent>().IsDead())
				{
					Destroy(this);
				}
			}
		}
	}
}

