//=============================================================================================================================//
//
//	Project: GMTK2020_OutOfControl
//	Copyright: Indie Pirates
//  Created on: 7/11/2020 4:28:57 PM
//
//=============================================================================================================================//


#region Usings

using UnityEngine;

#endregion

namespace GMTK2020_OutOfControl
{
	public static class ExtensionMethods
	{
		public static float Percentage(float start, float end, float current)
		{
			return Mathf.InverseLerp(start, end, current);
		}
		
		public static float MapValueToRange(float fromMin, float fromMax, float toMin, float toMax, float value)
		{
			float percentage = Percentage(fromMin, fromMax, value);
			return Mathf.Lerp(toMin, toMax, percentage);
		}

		public static float Abs(this float inValue)
		{
			return Mathf.Abs(inValue);
		}
		
		public static float Lerp(this Vector2 range, float value)
		{
			return Mathf.Lerp(range.x, range.y, Mathf.Clamp01(value));
		}
		
		public static bool IsNullOrEmpty(this string original)
		{
			return string.IsNullOrEmpty(original);
		}

		public static void Clamp2D(this Transform transform)
		{
			var position = transform.position;
			position.z = 0;
			transform.position = position;
		}

		public static void ClampVelocity(this Rigidbody2D rigidbody, float maxSpeed)
		{
			var velocity = rigidbody.velocity;
			velocity = velocity.normalized * Mathf.Min(velocity.magnitude, maxSpeed);
			rigidbody.velocity = velocity;
		}
	}
}