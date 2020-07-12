//=============================================================================================================================//
//
//	Project: GMTK2020_OutOfControl
//	Copyright: Indie Pirates
//  Created on: 7/11/2020 1:14:28 PM
//
//=============================================================================================================================//


#region Usings

using System;
using UnityEngine;

#endregion

namespace GMTK2020_OutOfControl
{
	[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D), typeof(Animator))]
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

		#endregion

		//=====================================================================================================================//
		//================================================= Inspector Variables ===============================================//
		//=====================================================================================================================//

		#region Inspector Variables

		[SerializeField] private PlayerData _data;
		
		#endregion

		//=====================================================================================================================//
		//=================================================== Private Fields ==================================================//
		//=====================================================================================================================//

		#region Private Fields

		[SerializeField, HideInInspector] private Rigidbody2D _rigidbody;
		[SerializeField, HideInInspector] private CircleCollider2D _collider;
		[SerializeField, HideInInspector] private Animator _animator;
		
		private Transform _transform;
		private bool _bIsGrounded;
		private Vector3 _lastFrameVelocity;
		
		private static readonly int HitVerticalID = Animator.StringToHash("Hit_Vertical");
		private static readonly int HitHorizontalID = Animator.StringToHash("Hit_Horizontal");

		#endregion

		//=====================================================================================================================//
		//================================================= Public Properties ==================================================//
		//=====================================================================================================================//

		#region Public Properties

		public static bool IsGrounded => Instance._bIsGrounded;
		public static Vector3 Position => Instance._transform.position;
		public static Rigidbody2D Rigidbody => Instance._rigidbody;

		#endregion

		//=====================================================================================================================//
		//============================================= Unity Callback Methods ================================================//
		//=====================================================================================================================//

		#region Unity Callback Methods

		private void Awake()
		{
			Initialize();
		}

		private void FixedUpdate()
		{
			//TODO: detect ground in the correct direction based on the current world rotation
			var checkDir = Vector3.down;
			if (!Mathf.Approximately(_rigidbody.velocity.magnitude, 0f))
			{
				var velocity = _rigidbody.velocity.normalized;
				var angle = Vector3.SignedAngle(velocity, Vector3.down, Vector3.forward);
				checkDir = (Quaternion.AngleAxis(-angle, Vector3.back) * velocity).normalized;
				Debug.DrawRay(_transform.position, velocity, Color.blue, Time.deltaTime);
				Debug.DrawRay(_transform.position, checkDir * (_data._groundCheckDistance + _collider.radius), Color.red,
					Time.deltaTime);
			}

			_lastFrameVelocity = _rigidbody.velocity;
			_bIsGrounded = Physics2D.Raycast(_transform.position, checkDir, _data._groundCheckDistance + _collider.radius, _data._groundMask);
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			if (_lastFrameVelocity.magnitude >= _data._hitAnimationThreshold)
			{
				_animator.SetTrigger(HitVerticalID);
			}
		}

		#endregion

		//=====================================================================================================================//
		//================================================== Private Methods ==================================================//
		//=====================================================================================================================//

		#region Private Methods

		[ContextMenu("Initialize")]
		private void Initialize()
		{
			_rigidbody = GetComponent<Rigidbody2D>();
			_animator = GetComponent<Animator>();
			_collider = GetComponent<CircleCollider2D>();
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

		public static void DealDamage(float inDamage)
		{
			
		}

		#endregion

		//=====================================================================================================================//
		//===================================================== Coroutines ====================================================//
		//=====================================================================================================================//

		#region Coroutines

		#endregion

		//=====================================================================================================================//
		//================================================ Debugging & Testing ================================================//
		//=====================================================================================================================//

		#region Debugging & Testing


		#endregion
	}
}