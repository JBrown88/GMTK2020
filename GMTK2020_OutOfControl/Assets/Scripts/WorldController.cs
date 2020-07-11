//=============================================================================================================================//
//
//	Project: GMTK2020_OutOfControl
//	Copyright: Indie Pirates
//  Created on: 7/11/2020 11:19:09 AM
//
//=============================================================================================================================//


#region Usings

using System.Collections;
using Cinemachine;
using UnityEngine;

#endregion

namespace GMTK2020_OutOfControl
{
	public class WorldController : Singleton<WorldController>
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

		[SerializeField] private float _rotationSpeed = 10f;
		[SerializeField] private float _rotationDampTime = 0.15f;
		[SerializeField] private float _resetRotationTime = 0.15f;
		[SerializeField] private Vector2 _rotationLimits = new Vector2(-45, 45);
		[SerializeField] private float _bumpForce = 500f;
		[SerializeField] private float _bumpCooldownTime = 0.5f;
		[SerializeField] private float _bumpDuration = 0.3f;
		[SerializeField] private AnimationCurve _bumpCurve;
		[SerializeField] private bool _resetRotation;
		[SerializeField] private Transform _worldPivot;
		//[SerializeField] private CinemachinePathBase _pivotPath;
		
		#endregion

		//=====================================================================================================================//
		//=================================================== Private Fields ==================================================//
		//=====================================================================================================================//

		#region Private Fields

		private float _rotationDamp;
		private float _targetRotationDamp;
		private Transform _transform;
		
		private float _turnAngle;

		private CooldownTimer _bumpCooldown;
		private bool _bCanBump;

		private float _curRotation;
		
		#endregion

		//=====================================================================================================================//
		//================================================= Public Properties ==================================================//
		//=====================================================================================================================//

		#region Public Properties

		public static Vector3 Center => Instance._transform.position;

		#endregion

		//=====================================================================================================================//
		//============================================= Unity Callback Methods ================================================//
		//=====================================================================================================================//

		#region Unity Callback Methods

		private void Start()
		{
			_transform = transform;
			_turnAngle = 0f;
			_bumpCooldown = new CooldownTimer(_bumpCooldownTime, false);
			_bCanBump = true;
		}
		
		private void Update()
		{
			var rotateInput =  -Input.GetAxis("Horizontal");
			//var pointOnPath = _pivotPath.FindClosestPoint(PlayerCharacter.Position, 0, 3, 5);
			_worldPivot.position = PlayerCharacter.Position;//_pivotPath.EvaluatePosition(pointOnPath);
			
			float targetRotation;
			if (!_resetRotation)
			{
				_curRotation += rotateInput * _rotationSpeed * Mathf.Deg2Rad;
				targetRotation = _curRotation;
			}
			else
			{
				var lerpValue = MathUtils.MapValueToRange(-1, 1, 0, 1, rotateInput);
				_curRotation = Mathf.SmoothDamp(_curRotation, _rotationLimits.Lerp(lerpValue), ref _targetRotationDamp, _resetRotationTime);
				targetRotation = _curRotation;
			}
			
			targetRotation = targetRotation.Clamp(_rotationLimits);

			_turnAngle = Mathf.SmoothDampAngle(_turnAngle, targetRotation, ref _rotationDamp, _rotationDampTime);
			var angleDelta = Mathf.DeltaAngle(_transform.eulerAngles.z, _turnAngle);
			_transform.RotateAround(_worldPivot.position, _transform.forward, angleDelta);

			if (_bCanBump && _bumpCooldown.IsValid)
			{
				if (Input.GetButtonDown("A") || Input.GetKeyDown(KeyCode.Space))
				{
					_bCanBump = false;

					if (PlayerCharacter.IsGrounded)
					{
						PlayerCharacter.ApplyImpulse(_bumpForce * 10 * _transform.up);
					}

					StartCoroutine(Bump());
				}
			}
		}

		#endregion

		//=====================================================================================================================//
		//================================================== Private Methods ==================================================//
		//=====================================================================================================================//

		#region Private Methods

		#endregion

		//=====================================================================================================================//
		//=================================================== Public Methods ==================================================//
		//=====================================================================================================================//

		#region Public Methods

		#endregion

		//=====================================================================================================================//
		//===================================================== Coroutines ====================================================//
		//=====================================================================================================================//

		#region Coroutines

		private IEnumerator Bump()
		{
			var baseLocation = _transform.position;
			var animTime = 0f;
			while (animTime < _bumpDuration)
			{
				var yOffset = _bumpCurve.Evaluate(animTime / _bumpDuration);
				_transform.position = baseLocation + (yOffset * _bumpForce * transform.up);
				yield return null;
				animTime += Time.deltaTime;
			}
			
			_bCanBump = true;
			_bumpCooldown.Trigger();
		}
		
		#endregion

		//=====================================================================================================================//
		//================================================ Debugging & Testing ================================================//
		//=====================================================================================================================//

		#region Debugging & Testing

		#endregion
	}
}