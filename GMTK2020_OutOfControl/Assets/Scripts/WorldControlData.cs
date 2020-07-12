//=============================================================================================================================//
//
//	Project: GMTK2020_OutOfControl
//	Copyright: Indie Pirates
//  Created on: 7/11/2020 6:05:09 PM
//
//=============================================================================================================================//


#region Usings

using UnityEngine;

#endregion

namespace GMTK2020_OutOfControl
{
	[CreateAssetMenu (menuName = "GMTK2020_OutOfControl/WorldControlData")]
	public class WorldControlData : ScriptableObject
	{
		//=====================================================================================================================//
		//================================================= Public Properties ==================================================//
		//=====================================================================================================================//

		#region Public Properties

		public float _rotationSpeed = 10f;
		public float _rotationDampTime = 0.15f;
		public float _resetRotationTime = 0.15f;
		public Vector2 _rotationLimits = new Vector2(-45, 45);
		public float _bumpForce = 500f;
		public float _bumpCooldownTime = 0.5f;
		public float _bumpDuration = 0.3f;
		public AnimationCurve _bumpCurve;
		public bool _resetRotation;
		public bool _useFreeRotation = false;

		#endregion
	}
}