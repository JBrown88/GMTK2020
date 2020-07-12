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
		private static Action _onLoadingCallback;

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
		private bool _isReady = false;
		private bool _isFirstLoad = true;
		
		#endregion

		//=====================================================================================================================//
		//================================================= Public Properties ==================================================//
		//=====================================================================================================================//

		#region Public Properties

		public static bool IsReady => Instance._isReady;

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
			_endCreditsScreen.SetActive(false);
		}

		private void Update()
		{
			//TODO: pause button
			//TODO: quit game button
		}

		#endregion

		//=====================================================================================================================//
		//================================================== Private Methods ==================================================//
		//=====================================================================================================================//

		#region Private Methods
		
		private void Load(string inSceneName)
		{	
			if (!inSceneName.IsNullOrEmpty())
			{
				StartCoroutine( Instance.LoadAsync(inSceneName));
			}
		}

		private void UnloadCurrentLevel()
		{
			DisableCharater();
			SceneManager.UnloadSceneAsync(_currentSceneName);
		}

		private void EnableCharacter(Vector3 startPosition)
		{
			_playerCharacter.SetActive(true);
				
			PlayerCharacter.Position = startPosition;
			PlayerCharacter.SetRotation(Quaternion.identity);
				
			PlayerCharacter.Rigidbody.WakeUp();
			PlayerCharacter.Rigidbody.simulated = true;
		}

		private void DisableCharater()
		{
			_playerCharacter.SetActive(false);
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
			
			if(levelIdx == LastSceneIndex)
			{
				//TODO: show end screen
				Instance._endCreditsScreen.SetActive(true);
				Instance.UnloadCurrentLevel();
				Instance.DisableCharater();
				Instance._isReady = false;
				Instance._inGameMenu.SetActive(false);
			}
			else
			{	
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
			_endCreditsScreen.SetActive(false);
			_mainMenu.SetActive(false);
			Load(_startingLevelIdx);
		}

		public void GameOverReturn()
		{
			_isReady = false;
			_isFirstLoad = true;
			_mainMenu.SetActive(true);
			_gameOverScreen.SetActive(false);
			_endCreditsScreen.SetActive(false);
		}

		public static void NotifyPlayerDead()
		{
			Instance._isReady = false;
			Instance._gameOverScreen.SetActive(true);
			Instance._inGameMenu.SetActive(false);
			Instance.DisableCharater();
			Instance.Load(Instance._currentSceneName);
		}

		public void Quit()
		{
			Application.Quit();
		}

		#endregion

		//=====================================================================================================================//
		//===================================================== Coroutines ====================================================//
		//=====================================================================================================================//

		#region Coroutines

		private IEnumerator LoadAsync(string inSceneName)
		{
			_isReady = false;
			
			
			_gameOverScreen.SetActive(false);
			_mainMenu.SetActive(false);
			_inGameMenu.SetActive(false);
			
			yield return new WaitForSeconds(0.5f);
			
			_loadingScreen.SetActive(true);
			
			if(!_isFirstLoad)
				UnloadCurrentLevel();
			
			yield return new WaitForSeconds(0.5f);
			
			var asyncOperation = SceneManager.LoadSceneAsync(inSceneName, LoadSceneMode.Additive);

			while (!asyncOperation.isDone)
			{
				yield return null;
			}
			
			_currentSceneName = inSceneName;
			
			var playerStart = FindObjectOfType<PlayerStart>();
			if (playerStart)
			{
				_gameOverScreen.SetActive(false);
				_mainMenu.SetActive(false);
				
				EnableCharacter(playerStart.transform.position);
				
				yield return new WaitForSeconds(0.25f);
				
				_loadingScreen.SetActive(false);
				_inGameMenu.SetActive(true);
			}
			
			yield return new WaitForSeconds(0.25f);

			_isFirstLoad = false;
			_isReady = true;
		}
		
		

		#endregion

		//=====================================================================================================================//
		//================================================ Debugging & Testing ================================================//
		//=====================================================================================================================//

		#region Debugging & Testing

		#endregion
	}
}