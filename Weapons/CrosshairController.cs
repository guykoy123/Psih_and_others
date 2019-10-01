using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour {

    private WeaponController Weapon;
    private float GunAccuracy;

    private GameObject Up;
    private GameObject Down;
    private GameObject Right;
    private GameObject Left;

    // Use this for initialization
    void Start () {
        Weapon = GameObject.Find("Weapon").GetComponent<WeaponController>();
        

        Up = GameObject.Find("Up");
        Down = GameObject.Find("Down");
        Right = GameObject.Find("Right");
        Left = GameObject.Find("Left");

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Weapon.isGunEquipped())
        {
            GunAccuracy = Weapon.GetAccuracy();
            float PositionMultiplier = 2 - (Weapon.GetCurrentAccuracy() / GunAccuracy) * 1.3f;
            float position = 20 * PositionMultiplier;
            UpdateCrosshairPosition(position);
        }     
	}

    private void UpdateCrosshairPosition(float position)
    {
        Up.transform.localPosition = new Vector3(0,position);
        Down.transform.localPosition = new Vector3(0,-position);
        Right.transform.localPosition = new Vector3(position, 0);
        Left.transform.localPosition = new Vector3(-position, 0);

    }
}
