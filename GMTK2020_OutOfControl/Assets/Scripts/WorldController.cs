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
	public class WorldController : MonoBehaviour
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

		[SerializeField] private WorldControlData _data;
		[SerializeField] private Transform _worldPivot;
		
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

		#endregion

		//=====================================================================================================================//
		//============================================= Unity Callback Methods ================================================//
		//=====================================================================================================================//

		#region Unity Callback Methods

		private void Start()
		{
			_transform = transform;
			_turnAngle = 0f;
			_bumpCooldown = new CooldownTimer(_data._bumpCooldownTime, false);
			_bCanBump = true;
		}
		
		private void Update()
		{
			if(!GameManager.IsReady)
				return;
			
			var rotateInput =  -Input.GetAxis("Horizontal");
			_worldPivot.position = PlayerCharacter.Position;
			
			float targetRotation;
			if (!_data._resetRotation)
			{
				_curRotation += rotateInput * _data._rotationSpeed * Mathf.Deg2Rad;
				targetRotation = _curRotation;
			}
			else
			{
				var lerpValue = ExtensionMethods.MapValueToRange(-1, 1, 0, 1, rotateInput);
				_curRotation = Mathf.SmoothDamp(_curRotation, _data._rotationLimits.Lerp(lerpValue), ref _targetRotationDamp, _data._resetRotationTime);
				targetRotation = _curRotation;
			}

			if (!_data._useFreeRotation)
			{
				targetRotation = targetRotation.Clamp(_data._rotationLimits);
			}

			_turnAngle = Mathf.SmoothDampAngle(_turnAngle, targetRotation, ref _rotationDamp, _data._rotationDampTime);
			var angleDelta = Mathf.DeltaAngle(_transform.eulerAngles.z, _turnAngle);
			_transform.RotateAround(_worldPivot.position, _transform.forward, angleDelta);

			if (_bCanBump && _bumpCooldown.IsValid)
			{
				if (Input.GetButtonDown("A") || Input.GetKeyDown(KeyCode.Space))
				{
					_bCanBump = false;

					if (PlayerCharacter.IsGrounded)
					{
						PlayerCharacter.ApplyImpulse(_data._bumpForce * Vector3.up);
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
			while (animTime < _data._bumpDuration)
			{
				var yOffset = _data._bumpCurve.Evaluate(animTime / _data._bumpDuration);
				_transform.position = baseLocation + (yOffset * _data._bumpForce * Vector3.up);
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