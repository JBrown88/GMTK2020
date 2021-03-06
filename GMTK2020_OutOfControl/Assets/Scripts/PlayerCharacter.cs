//=============================================================================================================================//
//
//	Project: GMTK2020_OutOfControl
//	Copyright: Indie Pirates
//  Created on: 7/11/2020 1:14:28 PM
//
//=============================================================================================================================//


#region Usings

using System;
using System.Collections;
using UnityEngine;

#endregion

namespace GMTK2020_OutOfControl
{
	[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D), typeof(Animator))]
	[RequireComponent(typeof(HitPoints))]
	public class PlayerCharacter : Singleton<PlayerCharacter>
	{
		//=====================================================================================================================//
		//=================================================== Pending Tasks ===================================================//
		//=====================================================================================================================//

		#region Pending Tasks

		#endregion

		//=====================================================================================================================//
		//================================================== Internal Classes =================================================//
		//=====================================================================================================================//

		#region Internal Classes
		
		private static readonly int HitVerticalID = Animator.StringToHash("Hit_Vertical");
		private static readonly int HitHorizontalID = Animator.StringToHash("Hit_Horizontal");
		private static readonly int DeadID = Animator.StringToHash("Dead");
		private static readonly int AnimSpeedID = Animator.StringToHash("AnimSpeed");

		#endregion

		//=====================================================================================================================//
		//================================================= Inspector Variables ===============================================//
		//=====================================================================================================================//

		#region Inspector Variables

		[SerializeField] private PlayerData _data;
		[SerializeField] private HitPoints _hp;
		#endregion

		//=====================================================================================================================//
		//=================================================== Private Fields ==================================================//
		//=====================================================================================================================//

		#region Private Fields

		[SerializeField, HideInInspector] private Rigidbody2D _rigidbody;
		[SerializeField, HideInInspector] private CircleCollider2D _collider;
		[SerializeField, HideInInspector] private Animator _animator;
		[SerializeField, HideInInspector] private Transform _transform;
		
		private bool _bIsGrounded;
		private Vector3 _lastFrameVelocity;
		private Vector3 _groundNormal;

		#endregion

		//=====================================================================================================================//
		//================================================= Public Properties ==================================================//
		//=====================================================================================================================//

		#region Public Properties

		public static bool IsGrounded => Instance._bIsGrounded;
		public static Vector3 Position
		{
			get => Instance.transform.position;
			set => Instance.transform.position = value;
		}

		public static Rigidbody2D Rigidbody => Instance._rigidbody;
		public static Vector3 GroundNormal => Instance._groundNormal;
		public static CircleCollider2D Collider => Instance._collider;

		#endregion

		//=====================================================================================================================//
		//============================================= Unity Callback Methods ================================================//
		//=====================================================================================================================//

		#region Unity Callback Methods

		private void Start()
		{
			InitializeSelf();
		}

		private void FixedUpdate()
		{
			var hit = Physics2D.OverlapCircle(Position, _collider.radius * _data._groundCheckMultiplier, _data._groundMask);
			_bIsGrounded = hit != null;
			if(_bIsGrounded)
			{
				_groundNormal = -(hit.transform.position - _transform.position);
			}

			_rigidbody.ClampVelocity(_data._maxSpeed);
			
			_lastFrameVelocity = _rigidbody.velocity;
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			if (_lastFrameVelocity.magnitude >= _data._hitAnimationThreshold)
			{
				var dot = Vector3.Dot(_lastFrameVelocity.normalized, _transform.up).Abs();
				_animator.SetTrigger(dot >= 0.5f ? HitVerticalID : HitHorizontalID);
			}
		}
		
		#endregion

		//=====================================================================================================================//
		//================================================== Private Methods ==================================================//
		//=====================================================================================================================//

		#region Private Methods

		[ContextMenu("Initialize")]
		public void InitializeSelf()
		{
			_rigidbody = GetComponent<Rigidbody2D>();
			_animator = GetComponent<Animator>();
			_collider = GetComponent<CircleCollider2D>();
			_hp = GetComponent<HitPoints>();

			_transform = transform;
			
			_rigidbody.sharedMaterial = _data._physicsMaterial;
			_rigidbody.drag = _data._linearDrag;
			_rigidbody.mass = _data._mass;
			_rigidbody.gravityScale = _data._gravityScale;
			_rigidbody.angularDrag = _data._angularDrag;

			_collider.sharedMaterial = _data._physicsMaterial;
		}
		
		#endregion

		//=====================================================================================================================//
		//=================================================== Public Methods ==================================================//
		//=====================================================================================================================//

		#region Public Methods

		public static void ApplyForce(Vector3 force)
		{
			Instance._rigidbody.AddForce(force, ForceMode2D.Force);
		}

		public static void ApplyImpulse(Vector3 force)
		{
			Instance._rigidbody.AddForce(force, ForceMode2D.Impulse);
			Instance._bIsGrounded = false;
		}

		public static void ApplyImpulse(float force)
		{
			Instance._rigidbody.AddForce(force * Instance._groundNormal, ForceMode2D.Impulse);
			Instance._bIsGrounded = false;
		}

		public static void DealDamage(float inDamage)
		{
			Instance.StartCoroutine(Instance.PlayDeathSequence());
			
		}

		public static void SetRotation(Quaternion inRot)
		{
			Instance.transform.rotation = inRot;
		}
		
		#endregion

		//=====================================================================================================================//
		//===================================================== Coroutines ====================================================//
		//=====================================================================================================================//

		#region Coroutines

		private IEnumerator PlayDeathSequence()
		{
			yield return null;
			_rigidbody.Sleep();
			_rigidbody.simulated = false;
			_animator.SetTrigger(DeadID);
			yield return new WaitForSeconds(0.1f);

			var frameCount = 30;
			
			while (frameCount > 15)
			{
				frameCount--;
				Time.timeScale = (frameCount / 30f);
				_animator.SetFloat(AnimSpeedID, 1/Time.deltaTime);
				yield return null;
			}

			Time.timeScale = 1;
			yield return  new WaitForSeconds(0.5f);
			GameManager.NotifyPlayerDead();
		}

		#endregion
	}
}