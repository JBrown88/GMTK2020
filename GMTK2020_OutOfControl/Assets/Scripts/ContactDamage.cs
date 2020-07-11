//=============================================================================================================================//
//
//	Project: GMTK2020_OutOfControl
//	Copyright: Indie Pirates
//  Created on: 7/11/2020 7:04:43 PM
//
//=============================================================================================================================//


#region Usings

using UnityEngine;

#endregion

namespace GMTK2020_OutOfControl
{
	public class ContactDamage : MonoBehaviour
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
		
		[SerializeField] private float _contactDamage = 25f;
		[SerializeField] private float _pushbackForce = 5f;

		#endregion

		//=====================================================================================================================//
		//=================================================== Private Fields ==================================================//
		//=====================================================================================================================//

		#region Private Fields

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
		
		private void OnCollisionEnter2D(Collision2D collision)
		{
			var pushback = (collision.transform.position - transform.position).normalized * _pushbackForce;
			Debug.DrawRay(collision.contacts[0].point, pushback, Color.cyan, 5f);

			if (collision.otherRigidbody == PlayerCharacter.Rigidbody)
			{
				PlayerCharacter.ApplyImpulse(pushback);
				PlayerCharacter.DealDamage(_contactDamage);
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

		#endregion

		//=====================================================================================================================//
		//================================================ Debugging & Testing ================================================//
		//=====================================================================================================================//

		#region Debugging & Testing

		#endregion
	}
}