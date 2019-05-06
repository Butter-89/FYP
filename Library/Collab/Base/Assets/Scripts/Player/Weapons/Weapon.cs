using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class MyMessageTypes
{
    public static short MSG_LOGIN_RESPONSE = 1000;
    public static short MSG_SCORE = 1005;
};

public class MyScoreMessage : MessageBase
{
    public int score;
    //public Vector3 scorePos;
}
public enum WeaponType //to determine what the weapon type is.
{
    Projectile,
    Raycast
}
public enum Auto // determine the fire mode
{
    Full,
    Semi
}


public class Weapon : NetworkBehaviour {
    public WeaponType type;
    public Auto auto;
    public GameObject weaponModel;
    public Transform raycastStartSpot;
    public Camera cam;

    public bool isLocalPlayer = true;

    //projectile control
    //public GameObject projectile;
    //public Transform projectileSpawnSpot;

    public float range = 9999.0f; // raycast weapon range
    
    //Parameters that control the rate of fire
    public float rateOfFire = 10;
    private float _actualRoF;
    private float _fireTimer;

    //Ammo control
    public int ammoCapacity = 12;
    public int shotPerRound = 1;
    private int _currentAmmo;
    public float reloadTime = 2.0f;
    public bool showCurrentAmmo = true;

    //Accuracy control
    public float accuracy = 80.0f;
    private float _currentAccuracy;
    public float accuracyDropPerShot = 1.0f;
    public float accuracyRecoverRate = 0.1f;

    //Recoil control
    public bool recoil = true;
    public float recoilKickBackMin = 0.1f;	// The minimum distance the weapon will kick backward when fired
    public float recoilKickBackMax = 0.3f;   // The maximum distance the weapon will kick backward when fired
    public float recoilRotationMin = 0.1f;    // The minimum rotation the weapon will kick when fired
    public float recoilRotationMax = 0.25f;   // The maximum rotation the weapon will kick when fired
    public float recoilRecoveryRate = 0.01f;    // The rate at which the weapon recovers from the recoil displacement

    //Power (damage)
    public int power = 80;
    public float forceMultiplier = 10.0f;

    //Audio control
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public AudioClip dryFireSound;
    public ParticleSystem muzzleFlash;

    //Semi-auto control
    private bool _canFire = true;

    private BulletHole _pool;
    [SerializeField] private GameObject[] bulletHolePrefabs;

    //Projectile prefab
    [SerializeField] private GameObject projectile;

    [SerializeField] private Player _player;
    private NetworkIdentity _ni;
    void Start ()
    {
        _pool = GameObject.FindObjectOfType<BulletHole>();
        
        if (rateOfFire != 0)
            _actualRoF = 1.0f / rateOfFire;
        else
            _actualRoF = 0.01f;

        _fireTimer = 0.0f;
        _currentAmmo = ammoCapacity;  // change it to accommodate with the element system
        
        if (GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent(typeof(AudioSource));
        }
        if (raycastStartSpot == null)
            raycastStartSpot = gameObject.transform;
        //if (projectileSpawnSpot == null)
        //    projectileSpawnSpot = gameObject.transform;
        if (weaponModel == null)
            weaponModel = gameObject;


        _player = GetComponentInParent<Player>();
        _ni = _player.GetComponent<NetworkIdentity>();
    }

    
	
	void Update () {
        _currentAccuracy = Mathf.Lerp(_currentAccuracy, accuracy, accuracyRecoverRate * Time.deltaTime);
        _fireTimer += Time.deltaTime;
        if(isLocalPlayer&&!PauseMenu.IsOn)
            CheckForUserInput();
        //control the recoil "animation".
        if (recoil)
        {
            weaponModel.transform.position = Vector3.Lerp(weaponModel.transform.position, transform.position, recoilRecoveryRate * Time.deltaTime);
            weaponModel.transform.rotation = Quaternion.Lerp(weaponModel.transform.rotation, transform.rotation, recoilRecoveryRate * Time.deltaTime);
        }

    }

    private void CheckForUserInput()
    {
        if (type == WeaponType.Raycast)  //raycast weapon input manager
        {
            if (_fireTimer >= _actualRoF && _canFire)
            {
                if (Input.GetButton("Fire1"))
                {
                    Fire();
                }
            }
        }
        if (type == WeaponType.Projectile) //projectile weapon input manager
        {
            if (_fireTimer >= _actualRoF && _canFire)
            {
                if (Input.GetButton("Fire1"))
                {
                    Launch();
                }
            }

        }
        //if (Input.GetButtonDown("Reload"))
        //    Reload();
        // If the weapon is semi-auto
        if (Input.GetButtonUp("Fire1"))
            _canFire = true;
    }

    // Raycasting system
    //[Client]
    private void Fire()
    {
        
        // Reset the fireTimer to 0 (for ROF)
        _fireTimer = 0.0f;

        // If this is a semi-automatic weapon, set canFire to false (this means the weapon can't fire again until the player lets up on the fire button)
        if (auto == Auto.Semi)
            _canFire = false;

        // First make sure there is ammo
        if (_currentAmmo <= 0)
        {
            DryFire();
            return;
        }

        // Subtract 1 from the current ammo
        _currentAmmo--;

        // Fire once for each shotPerRound value
        for (int i = 0; i < shotPerRound; i++)
        {
            // Calculate accuracy for this shot
            float accuracyVary = (100 - _currentAccuracy) / 1000;
            Vector3 direction = cam.transform.forward;
            //direction.x += UnityEngine.Random.Range(-accuracyVary, accuracyVary);
            //direction.y += UnityEngine.Random.Range(-accuracyVary, accuracyVary);
            //direction.z += UnityEngine.Random.Range(-accuracyVary, accuracyVary);
            _currentAccuracy -= accuracyDropPerShot;
            if (_currentAccuracy <= 0.0f)
                _currentAccuracy = 0.0f;

            Vector3 origin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            // The ray that will be used for this shot
            Ray ray = new Ray(origin, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range))
            {
                int damage = power;
                Debug.Log(hit.collider.name);
                // Damage
                //send message might be an effective method.
                //hit.collider.gameObject.SendMessageUpwards("ChangeHealth", -damage, SendMessageOptions.DontRequireReceiver);
                Castle castle = hit.collider.GetComponent<Castle>();
                if (castle != null)
                {
                    castle.Damage(damage);
                }

                // Add force to the object that was hit
                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForce(ray.direction * power * forceMultiplier);
                }
                
                //show the bullet hole
                if (hit.collider.CompareTag("Wall"))
                {
                    GameObject bulletHoleToUse = bulletHolePrefabs[Random.Range(0, bulletHolePrefabs.Length-1)];
                    Instantiate(bulletHoleToUse, hit.point, Quaternion.LookRotation(hit.normal));
                }

                PlayerHealth ph = hit.collider.GetComponent<PlayerHealth>();
                if (ph)
                {
                    //ph.Damage(power);
                }

                if (hit.collider.CompareTag("Player"))
                {
                    //CmdPlayerShot(hit.collider.name);
                    //NetworkServer.
                    //MyScoreMessage msg = new MyScoreMessage();
                    //msg.score = 1002;
                    //msg.scorePos = scorePos;
                    //NetworkServer.SendToAll(MyMessageTypes.MSG_SCORE, msg);
                    //_player = hit.collider.GetComponent<Player>();
                    _player.CmdOnHit(hit.collider.name);
                    CmdPlayerShot(hit.collider.name);
                }
            }
        }

        // Recoil
        //if (recoil)
        //   Recoil();

        // Play the gunshot sound
        _player.CmdSyncShotEffect();
    }

    
    private void CmdPlayerShot(string playerID)
    {
        _player.CmdDamage(power,playerID);
    }

    
    public void DoShotEffect()
    {
        GetComponent<AudioSource>().PlayOneShot(fireSound);
        muzzleFlash.Play();
    }
    
    private void Launch()
    {
        // Reset the fireTimer to 0 (for ROF)
        _fireTimer = 0.0f;

        // If this is a semi-automatic weapon, set canFire to false (this means the weapon can't fire again until the player lets up on the fire button)
        if (auto == Auto.Semi)
            _canFire = false;

        // First make sure there is ammo
        if (_currentAmmo <= 0)
        {
            DryFire();
            return;
        }

        // Subtract 1 from the current ammo
        _currentAmmo--;
        
        //var newProjectile = Instantiate(projectile, raycastStartSpot.transform.position, Quaternion.LookRotation(cam.transform.forward),raycastStartSpot.transform);
        var newProjectile = Instantiate(projectile, raycastStartSpot.transform.position, raycastStartSpot.rotation);
        //Debug.Break();
        if (newProjectile.GetComponent<Rigidbody>())
        {
            //Debug.Log("Rigidbody attached");
            Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
            //Debug.Log(rb.ToString());
            rb.AddForce(newProjectile.transform.forward*forceMultiplier);
        }
        else
        {
            Debug.Log("Rigidbody is not attached to this projectile!");
        }
        
        GetComponent<AudioSource>().PlayOneShot(fireSound);
        muzzleFlash.Play();
    }

    public void Reload()
    {
        GetComponent<AudioSource>().PlayOneShot(reloadSound);
        _currentAmmo = ammoCapacity;
    }

    void DryFire()
    {
        GetComponent<AudioSource>().PlayOneShot(dryFireSound);
    }

    private void Recoil()
    {
        if (weaponModel == null)
        {
            Debug.Log("Weapon Model is null.  Make sure to set the Weapon Model field in the inspector.");
            return;
        }
        float kickBack = Random.Range(recoilKickBackMin, recoilKickBackMax);
        float kickRot = Random.Range(recoilRotationMin, recoilRotationMax);
        weaponModel.transform.Translate(new Vector3(0, 0, -kickBack), Space.Self);
        weaponModel.transform.Rotate(new Vector3(-kickRot, 0, 0), Space.Self);

    }
}
