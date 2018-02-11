using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTest : MonoBehaviour{
    private ParticleSystem ps;

    public Transform target;
    public float force;

    void Start(){
        ps = GetComponent<ParticleSystem>();
    }

    void LateUpdate(){
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        ps.GetParticles(particles);

        for(int i = 0; i < particles.Length; i++){
            ParticleSystem.Particle p = particles[i];

            Vector3 particleWorldPos;
            if(ps.main.simulationSpace == ParticleSystemSimulationSpace.Local){
                particleWorldPos = transform.TransformPoint(p.position);
            }
            else{
                particleWorldPos = p.position;
            }

            Vector3 targetDirection = (target.position - particleWorldPos).normalized;
            Vector3 seekForce = (targetDirection * force) * Time.deltaTime;

            p.velocity += seekForce;

            particles[i] = p;
        }

        ps.SetParticles(particles, particles.Length);
    }
}