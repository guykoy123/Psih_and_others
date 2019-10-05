using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollector : MonoBehaviour {

    List<GameObject> ActiveParticleSystems = new List<GameObject>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        HandleActiveParticleSystems();
	}

    private void HandleActiveParticleSystems()
    {
        //checks for inactive particle systems in the scene and removes them

        if (ActiveParticleSystems.Count > 0)
        {
            for (int i = 0; i < ActiveParticleSystems.Count; i++)
            {
                if (!ActiveParticleSystems[i].GetComponent<ParticleSystem>().IsAlive())
                {
                    Destroy(ActiveParticleSystems[i]);//destroy inactive particle system
                    ActiveParticleSystems.RemoveAt(i);//remove destroyed object from list
                }

            }
        }
    }

    public void AddParticleSystem(GameObject NewParticleSystem)
    {
        /*
         * check that object is of type particle system
         * if yes: add to list
         * if not: throw argument exception
         */
        ParticleSystem temp = NewParticleSystem.GetComponent<ParticleSystem>();
        if (temp != null)
            ActiveParticleSystems.Add(NewParticleSystem);
        else
            throw new System.ArgumentException("Object is not a particle system", "NewParticleSystem");
    }
}
