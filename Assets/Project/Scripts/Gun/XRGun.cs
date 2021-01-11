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


        [Header("Recoil Settings")]
        public float RecoilDuration = .2f;
        public float HorizontalRecoil = 1.5f;
        public float VerticalRecoil = 1.5f;


        [Header("Clip Settings")]
        public int ClipSize = 30;




        private InteractableObject interactableObject;
        private bool leftShoot = false;
        private bool rightShoot = false;
        private AudioSource gunAudioSource;
        private bool shootRoutineRunning = false;
        private bool shooting = false;
        private float fireRate = 0;
        private Quaternion startingLocalRotation;
        private bool wasGrabbed = false;
        private InteractableSecondaryGrab secondGrab;

        private int roundsInClip = 0;
        private bool clipHasRounds = true;
        private bool emptyClipAudioAloudToPlay = false;

        private void Start()
        {

            if(GetComponent<InteractableSecondaryGrab>())
            {
                secondGrab = GetComponent<InteractableSecondaryGrab>();
            }
            interactableObject = GetComponent<InteractableObject>();
            startingLocalRotation = transform.localRotation;
            fireRate = 1 / FireRate; // Shots per second 
            roundsInClip = ClipSize;
            InitAudio();
            CheckForInputConflicts();
        }

     
        private void SetRecoilPosition()
        {
            print(interactableObject.WasGrabbed);
            if(interactableObject.IsGrabbed && !wasGrabbed) // if we just grabbed
            {
                print("setting the pos ");
                wasGrabbed = true;
                startingLocalRotation = transform.localRotation;
            }
            else if(!interactableObject.IsGrabbed)
            {
                wasGrabbed = false;
            }
        }
        private void Update()
        {
            // The word shoot is starting to not event sound like a word now
            GetShootInput();
            CheckShoot();
            SetRecoilPosition();
        }

        private void CheckShoot()
        {
            if (interactableObject.IsGrabbedLeft && leftShoot && !shootRoutineRunning) // dont shoot if already shooting :)
            {
                if (!shooting)
                {
                    shooting = true;
                    if (ProjectileBullet && clipHasRounds)
                    {
                        StartCoroutine(ShootProjectile());
                    }
                    else
                    {
                        StartCoroutine(ShootHitscan());
                    }
                }
                else if (emptyClipAudioAloudToPlay)
                {
                    PlayEmptyClipAudio();
                }
            }
            else if (interactableObject.IsGrabbedRight && rightShoot && !shootRoutineRunning) // dont shoot if already shooting :)
            {
                if (!shooting && clipHasRounds)
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
                else if(emptyClipAudioAloudToPlay)
                {
                   
                    PlayEmptyClipAudio();
                }
            }

            if(interactableObject.IsGrabbedRight && !rightShoot)
            {
                emptyClipAudioAloudToPlay = true;
                shooting = false;
            }
            else if(interactableObject.IsGrabbedLeft && !leftShoot)
            {
                emptyClipAudioAloudToPlay = true;
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

        private void PlayEmptyClipAudio()
        {
            emptyClipAudioAloudToPlay = false;
            gunAudioSource.PlayOneShot(EmptyClipSound);
        }
        private IEnumerator ShootHitscan()
        {
            shootRoutineRunning = true;
            StopCoroutine(DoRecoil());
            StartCoroutine(DoRecoil());
            while (shooting)
            {
                PlayFX();
                PlaySound();
                RemoveBullet();
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
            StopCoroutine(DoRecoil());
            StartCoroutine(DoRecoil());
            while (shooting)
            {
                PlayFX();
                PlaySound();
                RemoveBullet();
                GameObject projectile = GameObject.Instantiate(ProjectilePrefab,ShootPosition.position,ShootPosition.rotation);
                projectile.GetComponent<Rigidbody>().AddForce(ShootPosition.TransformDirection(new Vector3(0,0, BulletPower)));

                yield return new WaitForSeconds(fireRate);
            }
            yield return null;
            StopFX();
            shootRoutineRunning = false;
        }

        private void RemoveBullet() // just subtracts 1 from the clip
        {
            if (roundsInClip > 0)
            {
                roundsInClip--;
            }
            else
            {
                emptyClipAudioAloudToPlay = true;
                clipHasRounds = false;
                shooting = false;
            }
        }
        private void PlayFX()
        {
            if(!GunEffect.isPlaying)
            {
                GunEffect.Play();
            }
        }


        private IEnumerator DoRecoil()
        {
            Quaternion postLocalRotation = startingLocalRotation;
            Vector3 recoil = new Vector3(HorizontalRecoil, VerticalRecoil, 0);
            while (shooting)
            {
                transform.localEulerAngles += recoil;
                postLocalRotation = transform.localRotation;
                yield return new WaitForSeconds(fireRate);
            }

            float rotationProgress = 0;
            float rotateAmount = Time.deltaTime / RecoilDuration;
            while (rotationProgress < (1.0f - rotateAmount))
            {
                rotationProgress += rotateAmount;
                if(!interactableObject.IsGrabbed || shooting ) // if we stop grabbing during recoil break or shooting resumes
                {
                    break; 
                }
               
                if (secondGrab == null || !secondGrab.IsGrabbed)
                {
                    transform.localRotation = Quaternion.Slerp(postLocalRotation, startingLocalRotation, rotationProgress);
                }


                postLocalRotation = transform.localRotation;
                yield return null;
            }
        }


        private void StopFX()
        {
            if (GunEffect.isPlaying)
            {
                GunEffect.Stop();
            }
        }

        private void PlaySound()
        {
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