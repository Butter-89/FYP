using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

// © Glitch Squirrel 2017;

[System.Serializable]
public class TurretParameters {

	[Header("Status")]
	[Tooltip("Activate or deactivate the Turret")]
	public bool active;
	public bool canFire;

	[Header("Shooting")]
	[Tooltip("Burst the force when hit")]
	public int power;
	[Tooltip("Pause between shooting")]
	[Range(0.5f,2)]
	public float ShootingDelay;
	[Tooltip("Radius of the turret view")]
	public float radius;
}

[System.Serializable]
public class TurretFX {

	[Tooltip("Muzzle transform position")]
	public Transform muzzle;
	[Tooltip("Spawn this GameObject when shooting")]
	public GameObject shotFX;
}
	
[System.Serializable]
public class TurretAudio {
	
	public AudioClip shotClip;
}

[System.Serializable]
public class TurretTargeting {

	[Tooltip("Speed of aiming at the target")]
	public float aimingSpeed;
	[Tooltip("Pause before the aiming")]
	public float aimingDelay;
	[Tooltip("GameObject with folowing tags will be identify as enemy")]
	public string[] tagsToFire;
	public List<Collider> targets = new List<Collider>();
	public Collider target;

}

	
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Actor))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Animator))]


public class Turret : MonoBehaviour { //Change here!!!

	public TurretParameters parameters;
	public TurretTargeting targeting;
	public TurretFX VFX;
	public TurretAudio SFX;
	
	private bool playerDead = false;
	public int teamNo;

	private void Awake() {
		
		GetComponent<SphereCollider> ().isTrigger = true;
		GetComponent<SphereCollider> ().radius = parameters.radius;
		GetComponent<BoxCollider> ().size =  new Vector3(2,2,2);
		GetComponent<BoxCollider> ().center =  new Vector3(0,1,0);
		/*
		if (teamNo == 1)
			targeting.tagsToFire[0] = "Team2";
		else
		{
			targeting.tagsToFire[0] = "Team1";
		}*/

	}

	private void FixedUpdate () {
		
		if (parameters.active == false) {
			return;
		}

		if (targeting.target == null) {
			ClearTargets ();
		}

		if (targeting.target != null)
		{
			ClearTargets ();
		}
			
		if (targeting.target != null) {
			Aiming();
			Invoke ("Shooting", parameters.ShootingDelay);
		}
	}

	public void UpdateTargetTag()
	{
		//Debug.Log("Updating target tag...");
		if (teamNo == 1)
			targeting.tagsToFire[0] = "Team2";
		else if (teamNo == 2)
		{
			targeting.tagsToFire[0] = "Team1";
		}
		Debug.Log("Team"+teamNo);
	}

	#region Aiming and Shooting

	private void Shot() {

		GetComponent<AudioSource> ().PlayOneShot (SFX.shotClip, Random.Range(0.75f,1));
		GetComponent<Animator> ().SetTrigger ("Shot");
		GameObject newShotFX = Instantiate (VFX.shotFX, VFX.muzzle);
		Destroy (newShotFX, 2);
	}
		
	private void Shooting() {

		if (targeting.target == null) {
			return;
		}

		if (parameters.canFire == false) {
			return;
		}
			
		RaycastHit hit; 
		if (Physics.Raycast (VFX.muzzle.position, VFX.muzzle.transform.forward, out hit, parameters.radius)) {
			if (CheckTags (hit.collider) == true) {
				Shot ();
				//hit.collider.GetComponent<Actor> ().ReceiveDamage (parameters.power, hit.point);
				string playerID = hit.collider.name;
				//Debug.Log("Turret hit "+playerID);
				hit.collider.GetComponent<Player>().CmdDamage(parameters.power,playerID);
			}

			ClearTargets ();	
			CancelInvoke ();
		}
	}

	
	
	public void Aiming() {

		if (targeting.target == null) {
			return;
		}

		Vector3 delta = targeting.target.transform.position - transform.position;
		float angle = Vector3.Angle (transform.forward, delta);
		Vector3 cross = Vector3.Cross (transform.forward, delta);
		GetComponent<Rigidbody> ().AddTorque (cross * angle * targeting.aimingSpeed);
	}

	#endregion

	#region Targeting
		
	private void OnTriggerEnter(Collider other) {

		if (parameters.active == false) {
			return;
		}

		ClearTargets ();

		if (CheckTags (other) == true) {
			if (targeting.targets.Count == 0) {
				targeting.target = other.GetComponent<Collider> ();
			}

			targeting.targets.Add (other.GetComponent<Collider> ());
		}
	}

	private void OnTriggerExit(Collider other) {

		if (parameters.active == false) {
			return;
		}

		ClearTargets ();

		if (CheckTags (other) == true) {
			targeting.targets.Remove (other.GetComponent<Collider>());
			if (targeting.targets.Count != 0) {
				targeting.target = targeting.targets.First ();
			} else {
				targeting.target = null;
			}
		}
	}

	private bool CheckTags (Collider toMatch) {

		bool Match = false;

		for (int i = 0; i < targeting.tagsToFire.Length; i++) {
			if (toMatch.tag == targeting.tagsToFire [i]) {
				Match = true;
			}
		}

		return(Match);
	}

	private void ClearTargets ()
	{
		if (targeting.target != null) {
			//Debug.Log(targeting.target.GetComponent<Collider> ().enabled);
			if (targeting.target.GetComponent<Collider> ().enabled == false) {
				
				targeting.targets.Remove (targeting.target);
				targeting.target = null;
				//Debug.Log("Target removed");
			}
		}
		
		foreach (Collider target in targeting.targets.ToList()) {
			
			if (target == null) {
				targeting.targets.Remove (target);
			}

			if (targeting.targets.Count != 0) {
				targeting.target = targeting.targets.First ();
			} else {
				targeting.target = null;
			}
		}
	}

	#endregion
}