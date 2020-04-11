
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

    

    //tracks the states of the gun
    private bool Aiming = false; //tracks if player is aiming
    private bool Reloading = false; //tracks if player is reloading
    private bool SustainedFire = false;//tracks if player is firing continuously
    private int SustainedFireCount = 0;//tracks the amount of bullets fired without releasing the trigger

    //multipliers
    private float MovingMultiplier = 0.95f;//reduces accuracy when player is moving
    private float StandingMultiplier = 0.98f;//reduces accuracy when player is standing up (not Crouching)
    private float HipFireMultiplier = 0.97f;//reduces accuracy when player is firing from the hip

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
    private GameObject FirePoint; //holds the empty game object that is located at the end of the barrel (where the muzzle flash is spawned)
    private Camera PlayerCamera; //stores the player camera
    private PlayerController Player; //stores the player controller
    public Text AmmoTextbox; //stores the ammo display textbox
    public Text ReloadTextBox; //stores the reload message

    //Animtors
    private Animator GunAnimator; //stores the gun animator (used to control gun animations)
    private Animator HitMarkerAnimation; //stores the hit marker animator (used tp control hit marker animations)
    public Animator CameraAnimator; //stores the camera animator

    private GarbageCollector Garbage;//collects garbage to be destroyed to prevent resource hogging

    // Use this for initialization
    void Start () {
        //PlayerCamera = GetComponentInParent<Transform>();

        //load components
        GunAnimator = GetComponent<Animator>(); //load the gun animator
        HitMarkerAnimation = GameObject.Find("HitMarker").GetComponent<Animator>(); //load the hit marker animator
        PlayerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();//load player camera
        Player = GameObject.Find("Player").GetComponent<PlayerController>(); //load player
        Garbage = GameObject.Find("GarbageCollector").GetComponent<GarbageCollector>();

        PlayerFOV = PlayerCamera.fieldOfView; //get player FOV

        //if gun is equipped
        if(EquippedGun != null)
        {
            FirePoint = GameObject.Find("FirePoint");//load fire point from the weapon currently equipped
            UpdateAmmo(); //display ammo
            GunAnimator.SetInteger("GunType", EquippedGun.GetTypeCode());
        }

        ReloadTextBox.gameObject.SetActive(false); //disable reload message
            
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
                //GunAnimator.SetTrigger("Reload");//trigger reload animation
                GunAnimator.SetTrigger("Reload");
                Reloading = true; //set reloading to true
                ReloadTextBox.gameObject.SetActive(false); //disable reload message
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
    }


    void Fire()
    {
        if (EquippedGun.Shoot())
        {
            UpdateAmmo();
            RaycastHit hit;
            if (Physics.Raycast(PlayerCamera.transform.position, CalculateShotVector(), out hit))
            {
                if (hit.transform.tag == "Enemy") //check if enemy is hit
                {
                    Enemy enemy = hit.transform.GetComponent<Enemy>(); //get enemy controller component
                    Debug.Log("Hit enemy: " + enemy.GetName());
                    enemy.Hit(EquippedGun.GetDamage()); //call hit on enemy
                    HitMarkerAnimation.SetTrigger("Hit"); //trigger hit marker animation (placeholder animation)
                    Garbage.AddParticleSystem(((GameObject)Instantiate(EnemyHitVFX, hit.point, Quaternion.LookRotation(hit.normal))));//instatiate hit effect for enemy

                }
                else //if missed enemy
                {
                    Debug.Log("hit:" + hit.transform.name); //print object hit
                    Garbage.AddParticleSystem((GameObject)Instantiate(EnviromentHitVFX, hit.point, Quaternion.LookRotation(hit.normal))); //instatiate hit effect for enviroment
                    Instantiate(BulletHole, hit.point + Vector3.Scale(new Vector3(0.01f, 0.01f, 0.01f),hit.normal), Quaternion.LookRotation(hit.normal)); //instatiate bullet hole
                }
            }

            /* TODO: implement muzzle flash
            GameObject NewMuzzleFlash = (GameObject)Instantiate(MuzzleFlash, FirePoint.transform.position, Quaternion.identity); //instatiate muzzle flash
            NewMuzzleFlash.transform.parent = FirePoint.transform; //set as child of FirePoint (will follow its position)
            Garbage.AddParticleSystem(NewMuzzleFlash); //add to particle system list
            */

            CameraAnimator.SetTrigger("Recoil"); //trigger recoil camera animation

        }
        else //no ammo
            ReloadTextBox.gameObject.SetActive(true); //activate reload message
            
            

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

    public void EquipGun(Gun NewGun) 
    {
        /* equip new gun
         * displays the ammo
         * load the guns fire point
         * update gun type in the animator
         * trigger player animation for holding gun
         */
         //TODO: needs to load gun into scene

        EquippedGun = NewGun; //equip new gun
        UpdateAmmo(); //display ammo
        FirePoint = GameObject.Find("FirePoint");//load fire point
        GunAnimator.SetInteger("GunType", EquippedGun.GetTypeCode());
        Instantiate(EquippedGun.GetWeaponMesh(), gameObject.transform); //load the weapon into the scene as child of WeaponController
        GameObject.Find("Player").GetComponent<PlayerController>().TriggerGunEquipAnim(); //TODO: do this based on the gun
    }
}
