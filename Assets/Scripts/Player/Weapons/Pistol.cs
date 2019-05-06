using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour {
    private Camera cam;
    public AudioSource gunFire;
    public AudioSource empty;
    private float hitForce = 200f;
    private float fireRate = 0.1f;
    private int _damage = 20;
    private bool firing = false;
    private int ammo = 7;
    private int mag = 0;
	
	void Start () {
        cam = GetComponentInParent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	void Update () {

            if (Input.GetButtonDown("Fire1") && !firing)
            {
                Fire();
            }
            if (Input.GetButtonUp("Fire1"))
            {
                firing = false;
            }
       
        
    }

    private void Fire()
    {
        Debug.Log("fire");
        if (ammo > 0)
        {
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, 100))
            {
                //Debug.Log(ammo);
                gunFire.Play();
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
                }
                //Debug.Log(hit.collider);
                Castle castle = hit.collider.GetComponent<Castle>();
                if(castle!=null)
                {
                    castle.CmdDamage(_damage);
                }
            }
            Debug.Log(ammo);
            ammo -= 1;
        }
        else
            empty.Play();
        firing = true;
    }

    public void increaseAmmo(int i)
    {
        ammo = ammo + i;
    }

    public void WeaponUpgrade()
    {
        _damage = 30;
    }
   
}
