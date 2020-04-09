using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpController : MonoBehaviour {
    /*
     * follows player until in attack range then attacks
     */

    Enemy Enemy;
    CharacterController Controller;
    Animator ImpAnimator;
    PlayerController Player;

    float AttackRange = 2f; //how close the imp needs to get to player before it can attack
    float DistanceToPLayer;
    float RotationSpeed = 2f;
    float MovingSpeed = 2f;

    float Gravity = -30f; //Gravity value
    float ySpeed = 0; //vertical Speed
    float MaxFallSpeed = 50f;

    bool Attacking = false;
    bool DeathAnimation = false;

    // Use this for initialization
    void Start () {
        Controller = GetComponent<CharacterController>();
        ImpAnimator = GetComponentInChildren<Animator>();
        Player = GameObject.Find("Player").GetComponent<PlayerController>();

        Enemy = GetComponent<Enemy>(); //get the enemy component

        Enemy.SetBaseHealth(150f);//set base health to 1000
        Enemy.SetCurrentHealth(150f);//update current health to base health
        Enemy.SetName("Imp");//set enemy name
        Enemy.SetDamage(10f);
    }
	
	// Update is called once per frame
	void Update () {
        /*
         * if enemy is not dead 
         * move to player to within attacking range
         * then attack
         * if enemy died play death animation
         * TODO: despwan the object
         */

        if (!Enemy.IsDead())//check that imp is not dead
        {
            DistanceToPLayer = Vector3.Distance(Player.transform.position, transform.position); //get distance to player
            if (DistanceToPLayer > AttackRange) //check if within attack range
            {
                RotateToTarget();
                MoveToTarget();
                ImpAnimator.SetBool("walking", true); //activate walking animation
            }
            else //within attack range
            {
                ImpAnimator.SetBool("walking", false); //stop walking animation
                if (!Attacking) //not attacking right now
                {
                    //check that all animations have stopped (to prevent playing to attack animations at once)
                    if (ImpAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !ImpAnimator.IsInTransition(0))
                    {
                        ImpAnimator.SetTrigger("attack"); //trigger attack aninmation
                        Player.Hit(Enemy.GetDamage()); //hit player based on set damage
                        Attacking = true;
                    }
                }
            }
            //check that all animations have stopped and were attacking
            if ((ImpAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !ImpAnimator.IsInTransition(0)) && Attacking)
                Attacking = false; //not attacking now


        }
        else
        {
            if (!DeathAnimation)
            {
                ImpAnimator.SetTrigger("dead"); //play death animation
                DeathAnimation = true;
            }
            else 
                //check that death animation has finished playing
                if (ImpAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !ImpAnimator.IsInTransition(0))
                    Enemy.PlayedDeadAnim();
                
        }

            
        
    }

    void RotateToTarget()
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = Player.transform.position - transform.position;
        targetDirection.y = 0; //set y rotation to zero, because imp only rotates on the ground (doesn't rotate upwards)

        // The step size is equal to speed times frame time.
        float singleStep = RotationSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    void MoveToTarget()
    {
        /*
         * move imp towards player
         * apply gravity
         */
        Vector3 movement = new Vector3(0f, 0f, MovingSpeed);
        movement = transform.TransformVector(movement); //rotate movement vector relative to the player rotation
        if (Controller.isGrounded)
            ySpeed = 0f; //set y movement to zero
        else
            ySpeed = Gravity * Time.deltaTime; //if not on the ground apply Gravity

        ySpeed = Mathf.Clamp(ySpeed, -MaxFallSpeed, MaxFallSpeed);//clamp falling speed
        movement.y = ySpeed;

        Controller.Move((movement) * Time.deltaTime);
    }
}
