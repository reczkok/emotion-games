using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using Unity.Services.Analytics;
using System;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;
        public KeyCode leftKey;
        public KeyCode rightKey;
        public KeyCode jumpKey;
        List<KeyCode> keyPool;
        void Awake()
        {
            keyPool = new List<KeyCode>
            {
                KeyCode.W,
                KeyCode.A,
                KeyCode.S,
                KeyCode.D,
                KeyCode.LeftArrow,
                KeyCode.RightArrow,
                KeyCode.UpArrow,
                KeyCode.DownArrow,
                KeyCode.Space,
            };
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            RandomizeControls();
        }
        protected override void Update()
        {
            if (controlEnabled)
            {
                if (Input.GetKey(leftKey))
                {
                    move.x = -1;
                } else if (Input.GetKey(rightKey))
                {
                    move.x = 1;
                } else
                {
                    move.x = 0;
                }
                if (jumpState == JumpState.Grounded && Input.GetKeyDown(jumpKey))
                {
                    jumpState = JumpState.PrepareToJump;
                    Dictionary<string, object> parameters = new Dictionary<string, object>()
                    {
                    };

                    // The ‘myEvent’ event will get queued up and sent every minute
                    Events.CustomData("jumpPlayer", parameters);
                }
                else if (Input.GetKeyUp(jumpKey))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            base.Update();
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }

        internal void RandomizeControls()
        {
            Debug.Log("Randomize Controls");
            var pickedValues = TakeRandom3();
            leftKey = pickedValues[0];
            rightKey = pickedValues[1];
            jumpKey = pickedValues[2];
            FindObjectOfType<ControlsTextController>()?.SetControls();
        }

        internal List<KeyCode> TakeRandom3()
        {
            List<KeyCode> pickedValues = new List<KeyCode>();
            while(pickedValues.Count < 3)
            {
                var pickedFromPool = keyPool[UnityEngine.Random.Range(0, keyPool.Count)];
                if (!pickedValues.Contains(pickedFromPool))
                {
                    pickedValues.Add(pickedFromPool);
                }
            }

            return pickedValues;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}