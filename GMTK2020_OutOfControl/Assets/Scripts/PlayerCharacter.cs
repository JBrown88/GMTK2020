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
	[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
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

		[SerializeField] private float _mass;
		[SerializeField] private float _gravityScale;
		[SerializeField] private float _maxSpeed;
		[SerializeField] private float _acceleration;
		[SerializeField] private float _linearDrag;
		[SerializeField] private float _angularDrag;
		[SerializeField] private PhysicsMaterial2D _physicsMaterial;

		[SerializeField] private float _groundCheckDistance = 0.1f;
		
		#endregion

		//=====================================================================================================================//
		//=================================================== Private Fields ==================================================//
		//=====================================================================================================================//

		#region Private Fields

		[SerializeField, HideInInspector] private Rigidbody2D _rigidbody;
		[SerializeField, HideInInspector] private CircleCollider2D _collider;

		private Transform _transform;
		private bool _bIsGrounded;
		
		#endregion

		//=====================================================================================================================//
		//================================================= Public Properties ==================================================//
		//=====================================================================================================================//

		#region Public Properties

		public static bool IsGrounded => Instance._bIsGrounded;
		public static Vector3 Position => Instance._transform.position;
		
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
			_bIsGrounded = Physics2D.Raycast(_transform.position, Vector2.down, _groundCheckDistance + _collider.radius);
			
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
			_collider = GetComponent<CircleCollider2D>();
			_transform = transform;

			_rigidbody.drag = _linearDrag;
			_rigidbody.mass = _mass;
			_rigidbody.gravityScale = _gravityScale;
			_rigidbody.angularDrag = _angularDrag;

			_collider.sharedMaterial = _physicsMaterial;
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