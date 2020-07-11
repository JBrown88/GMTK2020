
#region Usings

using System;
using UnityEngine;

#endregion

	[Serializable]
	public class CooldownTimer
	{
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

		private float _cooldownTime;
		private float _endTime;
		private float _endRealTime;
		private float _startTime;
		private float _startRealTime;
		
		private readonly bool _useRealtime;

		#endregion

		//=====================================================================================================================//
		//================================================= Public Properties ==================================================//
		//=====================================================================================================================//

		#region Public Properties
		
		public bool IsValid => _useRealtime ? Time.realtimeSinceStartup >= _endRealTime : Time.time >= _endTime;

		#endregion

		//=====================================================================================================================//
		//=================================================== Public Methods ==================================================//
		//=====================================================================================================================//

		#region Public Methods

		public CooldownTimer(float cooldownTime = -1, bool useRealTime = true)
		{
			_cooldownTime = cooldownTime;
			_endRealTime = _startRealTime = Time.realtimeSinceStartup;
			_endTime =_startTime = Time.time;
			_useRealtime = useRealTime;
		}

		public void Trigger()
		{
			if(_cooldownTime < 0)
				return;

			_startRealTime = Time.realtimeSinceStartup;
			_endRealTime = _startRealTime + _cooldownTime;

			_startTime = Time.time;
			_endTime = _startTime + _cooldownTime;
		}

		public void Trigger(float cooldownTime)
		{
			if (!Mathf.Approximately(_cooldownTime, cooldownTime) && cooldownTime > 0)
				_cooldownTime = cooldownTime;
			
			if(cooldownTime < 0 && _cooldownTime < 0)
				return;

			_startRealTime = Time.realtimeSinceStartup;
			_endRealTime = _startRealTime + _cooldownTime;

			_startTime = Time.time;
			_endTime = _startTime + _cooldownTime;
		}

		public float CooldownRate()
		{
			if (_useRealtime)
				return Mathf.InverseLerp(_startRealTime, _endRealTime, Time.realtimeSinceStartup);

			return Mathf.InverseLerp(_startTime, _endTime, Time.time);
		}

		#endregion
	}
