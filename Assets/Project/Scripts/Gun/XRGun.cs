using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace EpicXRCrossPlatformInput
{
    /// <summary>
    /// Probably don't need an XR gun... but we need a gun
    /// </summary>
    [RequireComponent(typeof(InteractableObject))]
    public class XRGun : MonoBehaviour
    {

        [Header("Input Settings")]
        public ButtonTypes ShootButton = ButtonTypes.Trigger;


        [Header("Clip and Shooting Settings")]
        public bool InfiniteAmmo = false;
        [DrawIf("InfiniteAmmo", false)]
        public float Clipsize = 12;
        public float FireRate = 30; // Shots per second if set to auto 
        public bool Automatic = true;
        public Transform ShootPosition; // Will shoot and or raycast from this position
        public float ShootDistance = 50; // raycast distance
        [DrawIf("ProjectileBullet", false)]
        public LayerMask ShootRaycastLayers = ~0;



        [Header("Projectile specific settings")]
        public bool ProjectileBullet = false; // If not hitscan will cast a projectile from shootPos
        [DrawIf("ProjectileBullet", true)]  // the user doesn't need to see these if no hands
        public float BulletPower = 1000;
        [DrawIf("ProjectileBullet", true)]  // the user doesn't need to see these if no hands
        public GameObject ProjectilePrefab;




        [Header("Effect Settings")]
        public AudioClip GunSound;
        public AudioClip EmptyClipSound;
        public ParticleSystem GunEffect; // Thing that plays when gun shoots




        private InteractableObject interactableObject;
        private bool leftShoot = false;
        private bool rightShoot = false;
        private AudioSource gunAudioSource;
        private bool shootRoutineRunning = false;
        private bool shooting = false;
        private float fireRate = 0;

        private void Start()
        {
            interactableObject = GetComponent<InteractableObject>();
            fireRate = 1 / FireRate; // Shots per second 
            InitAudio();
            CheckForInputConflicts();
        }

        private void Update()
        {
            // The word shoot is starting to not event sound like a word now
            GetShootInput();
            CheckShoot();
        }

        private void CheckShoot()
        {
            if (interactableObject.IsGrabbedLeft && leftShoot && !shootRoutineRunning) // dont shoot if already shooting :)
            {
                shooting = true;
                if (ProjectileBullet)
                {
                    StartCoroutine(ShootProjectile());
                }
                else
                {
                    StartCoroutine(ShootHitscan());
                }
            }
            else if (interactableObject.IsGrabbedRight && rightShoot && !shootRoutineRunning) // dont shoot if already shooting :)
            {
                shooting = true;

                if (ProjectileBullet)
                {
                    StartCoroutine(ShootProjectile());
                }
                else
                {
                    StartCoroutine(ShootHitscan());
                }
            }
            else
            {
                shooting = false;
            }

        }

        private void InitAudio()
        {
            if (GetComponent<AudioSource>() == null)
            {
                gunAudioSource = this.gameObject.AddComponent<AudioSource>();
            }
            else
            {
                gunAudioSource = GetComponent<AudioSource>();
            }
            gunAudioSource.clip = GunSound;
        }

        private IEnumerator ShootHitscan()
        {
            shootRoutineRunning = true;

            while (shooting)
            {
                PlayFX();
                PlaySound();
                RaycastHit hit;
                if (Physics.Raycast(ShootPosition.transform.position, ShootPosition.transform.forward, out hit, ShootDistance, ShootRaycastLayers))
                {
                    print("We hit the target boss man " + hit.transform.name);
                }
                yield return new WaitForSeconds(fireRate);
            }
            yield return null;
            StopFX();
            shootRoutineRunning = false;
        }

        private IEnumerator ShootProjectile()
        {
            shootRoutineRunning = true;

            while (shooting)
            {
                PlayFX();
                PlaySound();
                GameObject projectile = GameObject.Instantiate(ProjectilePrefab,ShootPosition.position,ShootPosition.rotation);
                projectile.GetComponent<Rigidbody>().AddForce(ShootPosition.TransformDirection(new Vector3(0,0, BulletPower)));
                yield return new WaitForSeconds(fireRate);
            }
            yield return null;
            StopFX();
            shootRoutineRunning = false;
        }

        private void PlayFX()
        {
            print("Wow sweet fx");
            if(!GunEffect.isPlaying)
            {
                GunEffect.Play();
            }
        }


        private void StopFX()
        {
            print("Wow sweet fx");
            if (GunEffect.isPlaying)
            {
                GunEffect.Stop();
            }
        }

        private void PlaySound()
        {
            print("Playing audio");
            gunAudioSource.PlayOneShot(GunSound);
        }

        /// <summary>
        /// Just looks to make sure the grab input isn't the same as the shoot becasue that'll break things
        /// </summary>
        private void CheckForInputConflicts()
        {
            if (interactableObject.GrabButton == ShootButton)
            {
                Debug.LogError("The Interact Grab script attatch to " + this.transform.name + " has the same input as the XRGun script on the same object");
            }
        }
        private void GetShootInput()
        {
            leftShoot = XRCrossPlatformInputManager.Instance.GetInputByButton(ShootButton, ControllerHand.Left, Automatic);
            rightShoot = XRCrossPlatformInputManager.Instance.GetInputByButton(ShootButton, ControllerHand.Right, Automatic);
        }
    }

}