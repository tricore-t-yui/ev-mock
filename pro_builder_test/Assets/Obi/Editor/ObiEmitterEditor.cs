using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Obi{
	
	/**
	 * Custom inspector for ObiEmitter components.
	 * Allows particle emission and constraint edition. 
	 * 
	 * Selection:
	 * 
	 * - To select a particle, left-click on it. 
	 * - You can select multiple particles by holding shift while clicking.
	 * - To deselect all particles, click anywhere on the object except a particle.
	 * 
	 * Constraints:
	 * 
	 * - To edit particle constraints, select the particles you wish to edit.
	 * - Constraints affecting any of the selected particles will appear in the inspector.
	 * - To add a new pin constraint to the selected particle(s), click on "Add Pin Constraint".
	 * 
	 */
	[CustomEditor(typeof(ObiEmitter)), CanEditMultipleObjects] 
	public class ObiEmitterEditor : ObiParticleActorEditor
	{

		public class EmitterParticleProperty : ParticleProperty
		{
		  public const int RotationalMass = 3;

		  public EmitterParticleProperty (int value) : base (value){}
		}

		[MenuItem("GameObject/3D Object/Obi/Obi Emitter",false,4)]
		static void CreateObiCloth()
		{
			GameObject c = new GameObject("Obi Emitter");
			Undo.RegisterCreatedObjectUndo(c,"Create Obi Emitter");
			c.AddComponent<ObiEmitter>();
			c.AddComponent<ObiEmitterShapeDisk>();
			c.AddComponent<ObiParticleRenderer>();
		}

		[MenuItem("GameObject/3D Object/Obi/Obi Emitter (with solver)",false,5)]
		static void CreateObiClothWithSolver()
		{

			GameObject c = new GameObject("Obi Emitter");
			Undo.RegisterCreatedObjectUndo(c,"Create Obi Emitter");
			ObiEmitter em = c.AddComponent<ObiEmitter>();
			c.AddComponent<ObiEmitterShapeDisk>();
			c.AddComponent<ObiParticleRenderer>();

			ObiSolver solver = c.AddComponent<ObiSolver>();
			em.Solver = solver;

		}
		
		ObiEmitter emitter;
		
		public override void OnEnable(){
			base.OnEnable();
			emitter = (ObiEmitter)target;

			particlePropertyNames.AddRange(new string[]{"Rotational Mass"});
		}
		
		public override void OnDisable(){
			base.OnDisable();
			EditorUtility.ClearProgressBar();
		}

		public override void UpdateParticleEditorInformation(){
			
			for(int i = 0; i < emitter.positions.Length; i++)
			{
				wsPositions[i] = emitter.GetParticlePosition(i);
				wsOrientations[i] = emitter.GetParticleOrientation(i);	
				facingCamera[i] = true;		
			}

		}
		
		protected override void SetPropertyValue(ParticleProperty property,int index, float value){
			if (index >= 0 && index < emitter.invMasses.Length){
				switch(property){
					case ParticleProperty.Mass: 
						emitter.invMasses[index] = 1.0f / Mathf.Max(value,0.00001f);
					break; 
					case ParticleProperty.Radius: 
						emitter.principalRadii[index] = Vector3.one * value;
					break; 
					case EmitterParticleProperty.RotationalMass: 
						emitter.invRotationalMasses[index] = 1.0f / Mathf.Max(value,0.00001f);
					break;
				}
			}
		}
		
		protected override float GetPropertyValue(ParticleProperty property, int index){
			if (index >= 0 && index < emitter.invMasses.Length){
				switch(property){
					case ParticleProperty.Mass:
						return 1.0f/emitter.invMasses[index];
					case ParticleProperty.Radius:
						return emitter.principalRadii[index][0];
					case EmitterParticleProperty.RotationalMass:
						return 1.0f/emitter.invRotationalMasses[index];
				}
			}
			return 0;
		}

		protected override void FixSelectedParticles(){
			base.FixSelectedParticles();
			for(int i = 0; i < selectionStatus.Length; i++){
				if (selectionStatus[i]){
					if (emitter.invRotationalMasses[i] != 0){	
						SetPropertyValue(EmitterParticleProperty.RotationalMass,i,Mathf.Infinity);
						newProperty = GetPropertyValue(currentProperty,i);
						emitter.angularVelocities[i] = Vector3.zero;
					}
				}
			}
			emitter.PushDataToSolver(ParticleData.INV_ROTATIONAL_MASSES | ParticleData.ANGULAR_VELOCITIES);
		}

		protected override void FixSelectedParticlesTranslation(){
			base.FixSelectedParticlesTranslation();
			for(int i = 0; i < selectionStatus.Length; i++){
				if (selectionStatus[i]){
					if (emitter.invRotationalMasses[i] == 0){	
						SetPropertyValue(EmitterParticleProperty.RotationalMass,i,1);
						newProperty = GetPropertyValue(currentProperty,i);
					}
				}
			}
			emitter.PushDataToSolver(ParticleData.INV_ROTATIONAL_MASSES | ParticleData.ANGULAR_VELOCITIES);
		}

		protected override void UnfixSelectedParticles(){
			base.UnfixSelectedParticles();
			for(int i = 0; i < selectionStatus.Length; i++){
				if (selectionStatus[i]){
					if (emitter.invRotationalMasses[i] == 0){	
						SetPropertyValue(EmitterParticleProperty.RotationalMass,i,1);
						newProperty = GetPropertyValue(currentProperty,i);
					}
				}
			}
			emitter.PushDataToSolver(ParticleData.INV_ROTATIONAL_MASSES);
		}	

		public override void OnInspectorGUI() {
			
			serializedObject.Update();

			GUI.enabled = emitter.Initialized;
			EditorGUI.BeginChangeCheck();
			editMode = GUILayout.Toggle(editMode,new GUIContent("Edit particles",EditorGUIUtility.Load("Obi/EditParticles.psd") as Texture2D),"LargeButton");
			if (EditorGUI.EndChangeCheck()){
				SceneView.RepaintAll();
			}
			GUI.enabled = true;

			EditorGUILayout.HelpBox("Active particles:"+ emitter.ActiveParticles,MessageType.Info);

			Editor.DrawPropertiesExcluding(serializedObject,"m_Script");
			
			// Apply changes to the serializedProperty
			if (GUI.changed){
				serializedObject.ApplyModifiedProperties();
			}
			
		}
		
	}
}




