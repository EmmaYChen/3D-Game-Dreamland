/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
/// 
/// Methods for evaluating curves.
/// 
/// </summary>

using UnityEngine;

namespace Pixelplacement
{
	public static class BezierCurves
	{
		#region Quadratic Bezier
		public static Vector3 GetPoint (Vector3 startPosition, Vector3 controlPoint, Vector3 endPosition, float percentage)
		{
			percentage = Mathf.Clamp01 (percentage);
			float oneMinusT = 1f - percentage;
			return oneMinusT * oneMinusT * startPosition + 2f * oneMinusT * percentage * controlPoint + percentage * percentage * endPosition;
		}

		public static Vector3 GetFirstDerivative (Vector3 startPoint, Vector3 controlPoint, Vector3 endPosition, float percentage)
		{
			percentage = Mathf.Clamp01 (percentage);
			return 2f * (1f - percentage) * (controlPoint - startPoint) + 2f * percentage * (endPosition - controlPoint);
		}
		#endregion

		#region Cubic Bezier
		public static Vector3 GetPoint (Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, float percentage)
		{
			percentage = Mathf.Clamp01 (percentage);
			float OneMinusT = 1f - percentage;
			return OneMinusT * OneMinusT * OneMinusT * startPosition + 3f * OneMinusT * OneMinusT * percentage * startTangent + 3f * OneMinusT * percentage * percentage * endTangent + percentage * percentage * percentage * endPosition;
		}

		public static Vector3 GetFirstDerivative (Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, float percentage)
		{
			percentage = Mathf.Clamp01 (percentage);
			float oneMinusT = 1f - percentage;
			return 3f * oneMinusT * oneMinusT * (startTangent - startPosition) + 6f * oneMinusT * percentage * (endTangent - startTangent) + 3f * percentage * percentage * (endPosition - endTangent);
		}
		#endregion
	}
}