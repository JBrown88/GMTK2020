//=============================================================================================================================//
//
//	Project: GMTK2020_OutOfControl
//	Copyright: Indie Pirates
//  Created on: 7/12/2020 12:29:27 PM
//
//=============================================================================================================================//


#region Usings

using System;
using System.Security.AccessControl;
using UnityEngine;

#endregion

namespace GMTK2020_OutOfControl
{
	public class GoalFlag : MonoBehaviour
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

		[SerializeField] private uint _nextLevelIdx;

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

		private void OnTriggerEnter2D(Collider2D other)
		{
			if(!gameObject.activeSelf || other != PlayerCharacter.Collider)
				return;
			
			NextLevel();
			gameObject.SetActive(false);
		}

		private void OnTriggerStay2D(Collider2D other)
		{
			if(!gameObject.activeSelf || other != PlayerCharacter.Collider)
				return;
			
			NextLevel();
			gameObject.SetActive(false);
		}

		#endregion

		//=====================================================================================================================//
		//================================================== Private Methods ==================================================//
		//=====================================================================================================================//

		#region Private Methods

		private void NextLevel()
		{
			GameManager.TriggerLevelLoading(_nextLevelIdx);
		}

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

		private void OnDrawGizmos()
		{
			transform.Clamp2D();
		}

		#endregion
	}
}