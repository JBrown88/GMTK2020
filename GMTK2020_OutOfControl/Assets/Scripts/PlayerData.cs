//=============================================================================================================================//
//
//	Project: GMTK2020_OutOfControl
//	Copyright: Indie Pirates
//  Created on: 7/11/2020 6:16:14 PM
//
//=============================================================================================================================//


#region Usings

using UnityEngine;

#endregion

namespace GMTK2020_OutOfControl
{
	[CreateAssetMenu (menuName = "GMTK2020_OutOfControl/PlayerData")]
	public class PlayerData : ScriptableObject
	{
		//=====================================================================================================================//
		//================================================= Public Properties ==================================================//
		//=====================================================================================================================//

		#region Public Properties

		public float _mass;
		public float _gravityScale;
		public float _maxSpeed;
		public float _acceleration;
		public float _linearDrag;
		public float _angularDrag;
		public PhysicsMaterial2D _physicsMaterial;

		public float _hitAnimationThreshold = 10f;
		public float _groundCheckDistance = 0.2f;
		public float _groundCheckMultiplier = 1.35f;
		public LayerMask _groundMask;
		
		#endregion

	}
}