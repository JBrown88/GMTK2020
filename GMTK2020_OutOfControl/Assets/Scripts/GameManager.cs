//=============================================================================================================================//
//
//	Project: GMTK2020_OutOfControl
//	Copyright: Indie Pirates
//  Created on: 7/12/2020 12:47:21 PM
//
//=============================================================================================================================//


#region Usings

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

namespace GMTK2020_OutOfControl
{
	
	public class GameManager : Singleton<GameManager>
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

		private const uint LastSceneIndex = 10;

		#endregion

		//=====================================================================================================================//
		//================================================= Inspector Variables ===============================================//
		//=====================================================================================================================//

		#region Inspector Variables

		[SerializeField] private GameObject _mainMenu;
		[SerializeField] private GameObject _inGameMenu;
		[SerializeField] private GameObject _loadingScreen;
		[SerializeField] private GameObject _gameOverScreen;
		[SerializeField] private GameObject _endCreditsScreen;
		[SerializeField] private GameObject _playerCharacter;
		
		[SerializeField] private uint _startingLevelIdx = 0;
		
		#endregion

		//=====================================================================================================================//
		//=================================================== Private Fields ==================================================//
		//=====================================================================================================================//

		#region Private Fields

		private string _currentSceneName = "Template0";

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
			
			_loadingScreen.SetActive(false);
			_mainMenu.SetActive(true);
			_playerCharacter.SetActive(false);
			_gameOverScreen.SetActive(false);
			_inGameMenu.SetActive(false);
			
			SceneManager.sceneLoaded += (scene, mode) =>
			{
				OnLevelLoadedCallback(scene);
			};
		}

		#endregion

		//=====================================================================================================================//
		//================================================== Private Methods ==================================================//
		//=====================================================================================================================//

		#region Private Methods
		
		private void OnLevelLoadedCallback(Scene scene)
		{
			var playerStart = FindObjectOfType<PlayerStart>();
			if (playerStart)
			{
				_loadingScreen.SetActive(false);
				_gameOverScreen.SetActive(false);
				_mainMenu.SetActive(false);
				
				_inGameMenu.SetActive(true);
				_playerCharacter.SetActive(true);
				PlayerCharacter.Rigidbody.WakeUp();
				PlayerCharacter.Rigidbody.simulated = true;
				
				_currentSceneName = scene.name;
				PlayerCharacter.Position = playerStart.transform.position;
				PlayerCharacter.SetRotation(Quaternion.identity);
			}
		}
		
		private void Load(string inSceneName)
		{
			if (!inSceneName.IsNullOrEmpty())
			{
				SceneManager.LoadScene(inSceneName, LoadSceneMode.Additive);
			}
		}

		private void UnloadCurrentLevel()
		{
			SceneManager.UnloadSceneAsync(_currentSceneName);
		}

		#endregion

		//=====================================================================================================================//
		//=================================================== Public Methods ==================================================//
		//=====================================================================================================================//

		#region Public Methods

		public static void TriggerLevelLoading(uint levelIdx)
		{
			PlayerCharacter.Rigidbody.simulated = false;
			PlayerCharacter.Rigidbody.Sleep();
			
			Instance._inGameMenu.SetActive(false);
			Instance._playerCharacter.SetActive(false);
			Instance._loadingScreen.SetActive(true);
			
			if(levelIdx == LastSceneIndex)
			{
				//TODO: show end screen
			}
			else
			{	
				Instance.UnloadCurrentLevel();
				Instance.Load(levelIdx);
			}
				
		}
		
		public void Load(uint inSceneIndex)
		{
			if (inSceneIndex < LastSceneIndex)
			{
				var sceneName = "Template" + inSceneIndex;
				Load(sceneName);
			}
		}

		public void StartGame()
		{
			_currentSceneName = "Template0";
			_loadingScreen.SetActive(true);
			_mainMenu.SetActive(false);
			Load(_startingLevelIdx);
		}

		public void OnGameOver()
		{
			
		}

		public void GameOverReturn()
		{
			_mainMenu.SetActive(true);
			_gameOverScreen.SetActive(false);
		}

		#endregion

		//=====================================================================================================================//
		//===================================================== Coroutines ====================================================//
		//=====================================================================================================================//

		#region Coroutines

		private IEnumerator LoadAsync(Scene inScene)
		{
			yield return null;
		}
		
		

		#endregion

		//=====================================================================================================================//
		//================================================ Debugging & Testing ================================================//
		//=====================================================================================================================//

		#region Debugging & Testing

		#endregion
	}
}