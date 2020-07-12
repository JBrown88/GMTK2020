//=============================================================================================================================//
//
//	Project: GMTK2020_OutOfControl
//	Copyright: Indie Pirates
//  Created on: 7/12/2020 12:47:21 PM
//
//=============================================================================================================================//


#region Usings

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

namespace GMTK2020_OutOfControl
{
	
	public class SceneLoader : Singleton<SceneLoader>
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

		protected override void Initialize()
		{
			base.Initialize();
			
			SceneManager.sceneLoaded += (scene, mode) =>
			{
				OnLevelLoadedCallback();
			};
		}

		#endregion

		//=====================================================================================================================//
		//================================================== Private Methods ==================================================//
		//=====================================================================================================================//

		#region Private Methods
		
		private void OnLevelLoadedCallback()
		{
			var playerStart = FindObjectOfType<PlayerStart>();
			if (playerStart)
			{
				PlayerCharacter.Position = playerStart.transform.position;
				PlayerCharacter.SetRotation(Quaternion.identity);
			}
		}

		#endregion

		//=====================================================================================================================//
		//=================================================== Public Methods ==================================================//
		//=====================================================================================================================//

		#region Public Methods

		public static void Load(string inSceneName)
		{
			if(!inSceneName.IsNullOrEmpty())
				SceneManager.LoadScene(inSceneName);
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