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
	public static class MathUtils
	{
		public static void RotateAroundClamped(this Transform transform, Vector3 pivot, Vector3 axis, float angle)
		{
			if(!transform)
				return;

			var position = transform.position;
			var rotation = transform.rotation;

			var targetRotation = Quaternion.AngleAxis(angle, axis);
			var dirToPivot = position - pivot;
			dirToPivot = rotation * dirToPivot;
			var targetPosition = pivot + dirToPivot;
			
			targetRotation = Quaternion.Inverse(rotation) * targetRotation * rotation;			
			transform.SetPositionAndRotation(targetPosition, targetRotation);
		}
		public static float Percentage(float start, float end, float current)
		{
			return Mathf.InverseLerp(start, end, current);
		}
		
		public static float MapValueToRange(float fromMin, float fromMax, float toMin, float toMax, float value)
		{
			float percentage = Percentage(fromMin, fromMax, value);
			return Mathf.Lerp(toMin, toMax, percentage);
		}
	}
}