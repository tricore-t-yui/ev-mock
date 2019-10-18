using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace Obi{

[ExecuteInEditMode]
[RequireComponent(typeof(ParticleSystem))]
public class ParticleAdvector : MonoBehaviour {

	public ObiSolver solver;
	public uint minNeighbors = 4;

	private ParticleSystem ps;
	private ParticleSystem.Particle[] particles;

	AlignedVector4Array positions;
	AlignedVector4Array velocities;
	AlignedIntArray neighbourCount;

	int alive;
	int solverOffset;

	public ParticleSystem Particles{
		get{return ps;}
	}

	void OnEnable(){

		if (solver != null){
			solver.OnStepEnd += Solver_OnStepEnd;
		}
	}

	void OnDisable(){
		if (solver != null){
			solver.OnStepEnd -= Solver_OnStepEnd;
		}
	}

	void ReallocateParticles(){

		if (ps == null){
			ps = GetComponent<ParticleSystem>();
			ParticleSystem.MainModule main = ps.main;
			main.simulationSpace = ParticleSystemSimulationSpace.World;
		}

		// Array to get/set particles:
		if (particles == null || particles.Length != ps.main.maxParticles){
			particles = new ParticleSystem.Particle[ps.main.maxParticles];
			positions = new AlignedVector4Array(ps.main.maxParticles);
			velocities = new AlignedVector4Array(ps.main.maxParticles);
			neighbourCount = new AlignedIntArray(ps.main.maxParticles);
		}

		alive = ps.GetParticles(particles);

	}

	void Solver_OnStepEnd (object sender, System.EventArgs e)
	{
		if (solver == null) return;

		ReallocateParticles();

		for (int i = 0; i < alive; ++i)
			positions[i] = particles[i].position;

		Oni.InterpolateDiffuseParticles(solver.OniSolver,solver.velocities.GetIntPtr(),positions.GetIntPtr(),velocities.GetIntPtr(),neighbourCount.GetIntPtr(),alive);

		for (int i = 0; i < alive; ++i){

			// kill the particle if it has very few neighbors:
			if (neighbourCount[i] < minNeighbors)
				particles[i].remainingLifetime = 0;

			particles[i].velocity = velocities[i];
		}

		ps.SetParticles(particles, alive);
	}
}
}