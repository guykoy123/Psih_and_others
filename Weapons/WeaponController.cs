
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour {
    /* Weapon controller
     * controlls the currently equiped weapon
     * actions: fire (hit), aim, reload
     */

    Gun EquippedGun; //currently equipped gun

    public GameObject Fire_Point; //holds the empty game object that is located at the end of the barrel (where the muzzle flash is spawned)

    //tracks the states of the gun
    private bool Aiming = false; //tracks if player is aiming
    private bool Reloading = false; //tracks if player is reloading
    private bool SustainedFire = false;//tracks if player is firing continuously
    private int SustainedFireCount = 0;//tracks the amount of bullets fired without releasing the trigger

    //multipliers
    private float MovingMultiplier = 0.9f;//reduces accuracy when player is moving
    private float StandingMultiplier = 0.95f;//reduces accuracy when player is standing up (not Crouching)
    private float HipFireMultiplier = 0.95f;//reduces accuracy when player is firing from the hip

    //constants
    private const float ZoomRate = 100f; //stores the zoom rate (higher number is faster zoom)

    //other variables
    private float PlayerFOV; //stores the base FOV of the player
    private float TimeToFire = 0f; //stores the time since last shot (used for controlling fire rate)

    //TODO: get from equipped gun (different for every weapon)
    public GameObject MuzzleFlash;

    public GameObject EnviromentHitVFX; //stores the effect of hitting enviroment (placeholder) need to add different types based on what was hit
    public GameObject EnemyHitVFX; //stores the effect of hitting enemy (placeholder) need to add different types based on what was hit
    public GameObject BulletHole; //stores the bullethole object (spawned where the bullet hits)

    //GameObjects
    private Animator GunAnimator; //stores the gun animator (used to control gun animations)
    private Animator HitMarkerAnimation; //stores the hit marker animator (used tp control hit marker animations)
    private Camera PlayerCamera; //stores the player camera
    public Animator CameraAnimator; //stores the camera animator
    private PlayerController Player; //stores the player controller
    public Text AmmoTextbox; //stores the ammo display textbox

    private List<GameObject> ParticleSystems = new List<GameObject>(); //stores all particle systems that are created (these are destroyed at the end of their animation)

    // Use this for initialization
    void Start () {
        //PlayerCamera = GetComponentInParent<Transform>();

        //load components
        GunAnimator = GetComponent<Animator>(); //load the gun animator
        HitMarkerAnimation = GameObject.Find("HitMarker").GetComponent<Animator>(); //load the hit marker animator
        PlayerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();//load player camera
        Player = GameObject.Find("Player").GetComponent<PlayerController>(); //load player

        PlayerFOV = PlayerCamera.fieldOfView; //get player FOV

        //display ammo if gun is equipped
        if(EquippedGun != null)
            UpdateAmmo();
    }

    // Update is called once per frame
    void Update ()
    {
        //check if gun equipped
        if (EquippedGun != null)
        {
            //primary fire
            if (EquippedGun.GetFiringMode() == 1 && Input.GetButtonDown("Fire1") && Time.time >= TimeToFire && !Player.isSprinting()) //check if firing mode is semi and the player pressed fire and it is time to fire and player isn't sprinting
            {
                Fire();
                TimeToFire = Time.time + (1 / (EquippedGun.GetFiringRate() * Time.deltaTime));
            }
            else if (Input.GetButton("Fire1") && Time.time >= TimeToFire && EquippedGun.GetFiringMode() != 1 && !Player.isSprinting()) //check if fire button is pressed and its time to fire and fire mode is auto and player isn't sprinting
            {
                if (SustainedFire)
                    SustainedFireCount++;
                else
                {
                    SustainedFire = true;
                    SustainedFireCount = 1;
                }
                Fire();
                TimeToFire = Time.time + (1 / (EquippedGun.GetFiringRate() * Time.deltaTime));
            }

            else if (Input.GetButtonUp("Fire1")) //check if fire button is released
                SustainedFire = false; //reset sustained fire tracker


            //aiming
            if (Input.GetButtonDown("Fire2")) //check if aim button down
            {
                GunAnimator.SetTrigger("Aim"); //trigger aim animation
                Aiming = true; //set aiming to true             
            }
            else if (Input.GetButtonUp("Fire2")) //check if not pressing aim button
            {
                GunAnimator.SetTrigger("ToHip"); //trigger to hip animation
                Aiming = false; //set aiming to false
            }

            //reloading
            if(Input.GetButtonDown("Reload") && !Reloading && !Aiming) //check if reload buttons is pressed while not reloading or aiming
            {
                GunAnimator.SetTrigger("Reload");//trigger reload animation
                Reloading = true; //set reloading to true
            }
            else if(!GunAnimator.GetCurrentAnimatorStateInfo(0).IsName("Reload") && Reloading) //check if reload animation is finished
            {
                Reloading = false;
                EquippedGun.Reload(); //reload current ammo
                UpdateAmmo(); //update ammo counter UI
            }
        }

        //animate zoom in/out of aiming
        if (Aiming) //check that player is aiming
            PlayerCamera.fieldOfView = Mathf.MoveTowards(PlayerCamera.fieldOfView, PlayerFOV - EquippedGun.GetZoomValue(), ZoomRate * Time.deltaTime); //zoom in to (base player FOV - the gun zoom), at spesific zoom rate
        else
            PlayerCamera.fieldOfView = Mathf.MoveTowards(PlayerCamera.fieldOfView, PlayerFOV, ZoomRate * Time.deltaTime); //zoom out to the base player FOV, at spesific zoom rate


        if (ParticleSystems.Count > 0) //removes inactive particle systems to reduce memory usage
            if (!ParticleSystems[0].GetComponent<ParticleSystem>().IsAlive())
            {
                Destroy(ParticleSystems[0]);
                ParticleSystems.RemoveAt(0);
            }

    }


    void Fire()
    {
        if (EquippedGun.Shoot())
        {
            RaycastHit hit;
            if (Physics.Raycast(PlayerCamera.transform.position, CalculateShotVector(), out hit))
            {
                if (hit.transform.tag == "Enemy") //check if enemy is hit
                {
                    Enemy enemy = hit.transform.GetComponent<Enemy>(); //get enemy controller component
                    Debug.Log("Hit enemy: " + enemy.GetName());
                    enemy.Hit(EquippedGun.GetDamage()); //call hit on enemy
                    HitMarkerAnimation.SetTrigger("Hit"); //trigger hit marker animation (placeholder animation)
                    ParticleSystems.Add((GameObject)Instantiate(EnemyHitVFX, hit.point, Quaternion.LookRotation(hit.normal))); //instatiate hit effect for enemy
                }
                else //if missed enemy
                {
                    Debug.Log("hit:" + hit.transform.name); //print object hit
                    ParticleSystems.Add((GameObject)Instantiate(EnviromentHitVFX, hit.point, Quaternion.LookRotation(hit.normal))); //instatiate hit effect for enviroment
                    Instantiate(BulletHole, hit.point + Vector3.Scale(new Vector3(0.01f, 0.01f, 0.01f),hit.normal), Quaternion.LookRotation(hit.normal)); //instatiate bullet hole
                }
            }

            GameObject NewMuzzleFlash = (GameObject)Instantiate(MuzzleFlash, Fire_Point.transform.position, Quaternion.identity); //instatiate muzzle flash
            NewMuzzleFlash.transform.parent = Fire_Point.transform; //set as child of Fire_Point (will follow its position)
            ParticleSystems.Add(NewMuzzleFlash); //add to particle system list

            //CameraAnimator.SetTrigger("Recoil"); //trigger recoil camera animation
            PlayerCamera.transform.localEulerAngles = Vector3.Lerp(PlayerCamera.transform.rotation.eulerAngles, new Vector3(0 ,0, 0), Time.deltaTime * PlayerCamera.transform.position.z);

        }
        else
            Debug.Log("Please Reload");

        UpdateAmmo();

    }

    private Vector3 CalculateShotVector()
    {
        /*
         * calculate the Vector3 of the next bullet based on sustained fire and weapon accuracy
         */
        return Vector3.Scale(GenerateOffsetVector(CalculateAccuracy()), PlayerCamera.transform.forward);
    }

    private Vector3 GenerateOffsetVector(float accuracy)
    {
        /*
         * generates a random vector3 where the values are offset around 1 by the current accuracy
         */
        return new Vector3(Random.Range(accuracy, 2f - accuracy), Random.Range(accuracy, 2f - accuracy), Random.Range(accuracy, 2f - accuracy));
    }

    private float CalculateAccuracy()
    {
        /*
         * calculates the accuracy for each shot (based on shooting position (aiming, hip fire) and sustained fire)
         * for the first few shots of sustained fire the accuracy is not reduced
         * after the first few rounds the accuracy starts to reduce based on the following function:
         * y=z+(1-z)/(1+x/2)
         * where:
         *      y - calculated accuracy
         *      z - minimum accuracy (which is the gun accuracy multiplied by 0.7 (0.85 when aiming))
         *      x - number of bullets fired in a row (divided by 2 to slow accuracy reduction rate)
         */

        float AccuracyMultiplier = GetAccuracyMultiplier();

        //accuracy is not reduced on the first 4 shots
        if (SustainedFireCount < 5)
            return EquippedGun.GetAccuracy() * AccuracyMultiplier;
        else
        {
            float MinimumAccuracy = EquippedGun.GetAccuracy() * AccuracyMultiplier;
            return MinimumAccuracy + (1 - MinimumAccuracy) / (1 + SustainedFireCount / 2);
        }
    }

    private float GetAccuracyMultiplier()
    {
        /*
         * calculates accuracy multiplier based on player state
         * moving/crouching/aiming
         */

        float Multiplier = 1f;
        if (!Aiming)
            Multiplier *= HipFireMultiplier;
        if (!Player.isCrouching())
            Multiplier *= StandingMultiplier;
        if (Player.isMoving())
            Multiplier *= MovingMultiplier;

        return Multiplier;
    }

    private void UpdateAmmo()
    {
        //update ammo counter UI
        AmmoTextbox.text = EquippedGun.GetCurrentAmmo() + " / " + EquippedGun.GetMagazineSize();
    }

    public float GetAccuracy()
    {
        float Accuracy = EquippedGun.GetAccuracy();
        return Accuracy;
    }
    public float GetCurrentAccuracy() {return EquippedGun.GetAccuracy() * GetAccuracyMultiplier();}
    
    public bool isGunEquipped()
    {
        if (EquippedGun != null)
            return true;
        return false;
    }

    public void EquipGun(Gun NewGun) //TODO: add weapon fire point
    {
        //equips new gun and displays the ammo
        EquippedGun = NewGun;
        UpdateAmmo();
    }
}
