using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MathTools
{
    //TODO: remove unused methods
    
    public static float Sign(this float val)
    {
        return Mathf.Sign(val);
    }
    
    public static Hashtable Hash(params object[] args)
    {
        var hash = new Hashtable(args.Length / 2);
        if (args.Length % 2 != 0)
        {
            Debug.LogError("error: requires an -EVEN- number or arguments");
            return null;
        }
        else
        {
            var i = 0;
            while (i < args.Length - 1)
            {
                hash.Add(args[i], args[i + 1]);
                i += 2;
            }

            return hash;
        }
    }

    public static Vector3 ConformDeadzones(this Vector3 axis, Vector3 deadzones)
    {
        if (Mathf.Abs(axis.x) <= deadzones.x)
            axis.x = 0f;

        if (Mathf.Abs(axis.y) <= deadzones.y)
            axis.y = 0f;

        if (Mathf.Abs(axis.z) <= deadzones.z)
            axis.z = 0f;

        return axis;
    }

    public static Vector3 ClampToAxis(this Vector3 axis, Vector3 targetAxis)
    {
        axis.x *= targetAxis.x;
        axis.y *= targetAxis.y;
        axis.z *= targetAxis.z;

        return axis;
    }

    public static Vector3 ClampRange(this Vector3 curPosition, Vector3 startPosition, Vector3 range)
    {
        curPosition.x = Mathf.Clamp(curPosition.x, startPosition.x - range.x, startPosition.x + range.x);
        curPosition.y = Mathf.Clamp(curPosition.y, startPosition.y - range.y, startPosition.y + range.y);
        curPosition.z = Mathf.Clamp(curPosition.z, startPosition.z - range.z, startPosition.z + range.z);

        return curPosition;
    }

    public static Vector3 ClampBetween(this Vector3 curPosition, Vector3 minPosition, Vector3 maxPosition)
    {
        curPosition.x = Mathf.Clamp(curPosition.x, minPosition.x, maxPosition.x);
        curPosition.y = Mathf.Clamp(curPosition.y, minPosition.y, maxPosition.y);
        curPosition.z = Mathf.Clamp(curPosition.z, minPosition.z, maxPosition.z);

        return curPosition;
    }

    public static T AddMissingComponent<T>(this GameObject obj) where T : Component
    {
        if (obj == null)
            return null;

        var comp = obj.GetComponent<T>();
        if (comp == null)
            comp = obj.AddComponent<T>();

        return comp;
    }

    public static float RandomValue(this Vector2 interval)
    {
        return Random.Range(interval.x, interval.y);
    }

    /// <summary>
    /// Return random value in interval. Maximally inclusive
    /// </summary>
    /// <param @name="interval"></param>
    /// <returns></returns>
    public static int RandomValue(this Vector2Int interval)
    {
        return Random.Range(interval.x, interval.y + 1);
    }

    public static Matrix4x4 TRS(this Transform transform, bool useUnitScale = true)
    {
        var scale = useUnitScale ? Vector3.one : transform.localScale;
        var position = transform.position;
        var rotation = transform.rotation;

        return Matrix4x4.TRS(position, rotation, scale);
    }

    /// <summary>
    /// Extension method to determine if given layer is in LayerMask
    /// </summary>
    /// <param @name="mask"></param>
    /// <param @name="layer"></param>
    /// <returns></returns>
    public static bool Contains(this LayerMask mask, int layer)
    {
        return (mask == (mask | (1 << layer)));
    }

    public static Vector3 RandomPointAround(this Vector3 center, float radius, float minDistance)
    {
        radius -= minDistance;
        return center + Random.onUnitSphere.normalized * minDistance +
               Random.insideUnitSphere.normalized * Random.Range(-radius, +radius);
    }

    public static Vector2 RandomPointAround(this Vector2 center, float radius, float minDistance)
    {
        radius -= minDistance;
        return center + Random.insideUnitCircle.normalized * minDistance +
               Random.insideUnitCircle.normalized * Random.Range(-radius, +radius);
    }

    public static Vector3 DirectionFromAngle(Transform transform, float angle, bool isGlobal = true)
    {
        if (!isGlobal)
        {
            angle += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public static Vector3 RandomPointBetween(this Vector3 start, Vector3 end)
    {
        return start.RandomPointBetween(end, Vector3.zero);
    }

    public static Vector3 RandomPointBetween(this Vector3 start, Vector3 end, Vector2 angleRange)
    {
        //Random distance between the two points
        var dist = Random.Range(0.2f, Vector3.Distance(start, end));

        //Random angle in radians
        var angleRad = Random.value * Mathf.PI * 2;

        if (angleRange != Vector2.zero)
            angleRad = Random.Range(angleRange.x, angleRange.y) * Mathf.Deg2Rad;


        var x = dist * Mathf.Cos(angleRad);
        var y = dist * Mathf.Sin(angleRad);
        var z = dist;

        return start + new Vector3(x, y, z);
    }

    public static bool Contains(this Vector2 range, float angle, bool inclusive = true)
    {
        if (inclusive)
        {
            if (angle >= range.x && angle <= range.y)
                return true;
        }
        else
        {
            if (angle > range.x && angle < range.y)
                return true;
        }

        return false;
    }

    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static List<Vector3> GetBezierCurveTo(this Vector3 startPt, Vector3 endPt, Vector3 ctrlPt,
        float step = 0.02f)
    {
        var path = new List<Vector3>();

        for (var t = 0f; t < 1f; t += step)
        {
            var x = (1 - t) * (1 - t) * startPt.x + 2 * (1 - t) * t * ctrlPt.x + t * t * endPt.x;
            var y = (1 - t) * (1 - t) * startPt.y + 2 * (1 - t) * t * ctrlPt.y + t * t * endPt.y;
            var z = (1 - t) * (1 - t) * startPt.z + 2 * (1 - t) * t * ctrlPt.z + t * t * endPt.z;

            path.Add(new Vector3(x, y, z));
        }

        return path;
    }

    public static Vector3 GetVelocityToTarget(Vector3 origin, Vector3 target, float launchAngle)
    {
        //Calculate launch angle in radians
        var angle = launchAngle * Mathf.Deg2Rad;

        //Position of player and debris on the same plane
        var planarTarget = new Vector3(target.x, 0f, target.z);
        var planarOrigin = new Vector3(origin.x, 0f, origin.z);

        //distance between target and origin
        var distance = Vector3.Distance(planarTarget, planarOrigin);
        //distance along the y axis
        var yOffset = origin.y - target.y;

        //launch velocity
        var launchSpeed = (1 / Mathf.Cos(angle)) *
                          Mathf.Sqrt((0.5f * Physics.gravity.magnitude * distance * distance) /
                                     (distance * Mathf.Tan(angle) + yOffset));
        var velocity = new Vector3(0f, launchSpeed * Mathf.Sin(angle), launchSpeed * Mathf.Cos(angle));

        //Rotate velocity to match the direction to target
        var rotationAngle = Vector3.Angle(Vector3.forward, planarTarget - planarOrigin);
        var launchVelocity = Quaternion.AngleAxis(rotationAngle, Vector3.up) * velocity;

        return launchVelocity;
    }

    public static Vector3 GetLaunchVelocity(Vector3 launchPoint, Vector3 targetPoint, float timeToTarget,
        float launchAngle, out Vector3 gravity)
    {
        var dirToTarget = targetPoint - launchPoint;
        var dirToTargetXz = dirToTarget;
        dirToTargetXz.y = 0;

        var yOffset = dirToTarget.y;
        var distance = dirToTargetXz.magnitude;

        var initialYVel = yOffset / timeToTarget + .5f * Physics.gravity.magnitude * timeToTarget;
        var initialXzVel = distance / timeToTarget;

        var velocity = dirToTargetXz.normalized * initialXzVel;
        velocity.y = initialYVel;

        gravity = Physics.gravity;

        if (launchAngle != 0f)
        {
            gravity = Quaternion.AngleAxis(launchAngle, dirToTarget.normalized) * gravity;
            var axis = Vector3.Cross(Physics.gravity, gravity).normalized;
            var angle = Vector3.Angle(gravity, Physics.gravity);
            velocity = Quaternion.AngleAxis(angle, axis) * velocity;
        }

        return velocity;
    }

    public static Vector3 GetNormalVector(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        var a = p2 - p1;
        var b = p3 - p1;

        return Vector3.Cross(a, b).normalized;
    }

    /// <summary>
    /// Rotates Point around axis by given angle (in rads).
    /// </summary>
    /// <returns>Rodriguesed rotation.</returns>
    /// <param @name="point">Point.</param>
    /// <param @name="axis">Axis.</param>
    /// <param @name="theta">Theta.</param>
    public static Vector3 RotateAround(Vector3 point, Vector3 axis, float theta)
    {
        var cosTheta = Mathf.Cos(theta);
        var sinTheta = Mathf.Sin(theta);
        var cross = Vector3.Cross(axis, point);
        var dot = Vector3.Dot(axis, point);

        return point * cosTheta + cross * sinTheta + point * dot * (1 - cosTheta);
    }

    /// <summary>
    /// Smooths given path using Chaikin's Algorithm.
    /// </summary>
    /// <returns>The path.</returns>
    /// <param @name="waypoints">Waypoints.</param>
    /// <param @name="numRefinements">Number refinements.</param>
    /// <param @name="refineRange"></param>
    /// <param @name="isOpenPath">If set to <c>true</c> is open path.</param>
    public static List<Vector3> SmoothPath(Vector3[] waypoints, int numRefinements, Vector2 refineRange,
        bool isOpenPath)
    {
        if (waypoints == null || waypoints.Length < 3)
            return null;

        if (numRefinements < 1)
            numRefinements = 1;

        var path = new List<Vector3>();
        path.AddRange(waypoints);
        for (var iter = 0; iter < numRefinements; iter++)
        {
            for (var i = 0; i < path.Count - 1; i++)
            {
                var q = refineRange.x * path[i] + refineRange.y * path[i + 1];
                var r = refineRange.y * path[i] + refineRange.x * path[i + 1];
                path[i] = q;
                path[i + 1] = r;
            }
        }

        if (isOpenPath)
        {
            var newPath = new List<Vector3>();
            newPath.Add(waypoints[0]);
            newPath.AddRange(path);
            newPath.Add(waypoints[waypoints.Length - 1]);

            return newPath;
        }

        return path;
    }

    public static List<Vector3> GenerateDomeAround(this Transform source, float domeFov, float axisFov,
        float domeAngleOffset, float numAxes, float pointsPerAxis, float radius)
    {
        if (axisFov <= 0) throw new ArgumentOutOfRangeException("axisFov");
        domeFov = Mathf.Clamp(domeFov, 0f, 360f);

        axisFov = Mathf.Clamp(axisFov, 0f, 180f);

        domeAngleOffset = Mathf.Clamp(domeAngleOffset, 0f, 180f);

        if (radius < 0.5f)
            radius = 0.5f;

        if (numAxes < 2)
            numAxes = 2;

        if (pointsPerAxis < 2)
            pointsPerAxis = 2;

        var domePoints = new List<Vector3>();

        var stepAngle = axisFov / (pointsPerAxis - 1);
        var axisAngleStep = domeFov / (numAxes - 1);

        for (var i = 0; i < pointsPerAxis; i++)
        {
            for (var axis = 0; axis < numAxes; axis++)
            {
                Vector3 point;
                if (source != null)
                {
                    var angle = source.eulerAngles.y - axisFov / 2f + stepAngle * i;
                    var direction = Quaternion.AngleAxis(domeAngleOffset + axisAngleStep * axis, -source.right) *
                                    DirectionFromAngle(source, angle);
                    point = source.TransformPoint(direction * radius);
                }
                else
                {
                    var angle = -axisFov / 2f + stepAngle * i;
                    var direction = Quaternion.AngleAxis(domeAngleOffset + axisAngleStep * axis, Vector3.left) *
                                    DirectionFromAngle(null, angle);
                    point = direction * radius;
                }

                domePoints.Add(point);
            }
        }

        return domePoints;
    }
    // See: gist.github.com/aeroson/043001ca12fe29ee911e
    //		public static Quaternion LookRotation (ref Vector3 forward, ref Vector3 up)
    //		{
    //			forward.Normalize ();
    //
    //			var right = Vector3.Cross (up, forward).normalized;
    //			up = Vector3.Cross (forward, right);
    //
    //			var m00 = right.x;
    //			var
    //
    //		}

    public static bool Approximately(float valueA, float valueB, float maxError, bool inclusive = true)
    {
        valueA = Mathf.Abs(valueA);
        valueB = Mathf.Abs(valueB);
        maxError = Mathf.Abs(maxError);

        if (valueA < valueB)
        {
            if (inclusive)
                return Mathf.Abs(valueB - valueA) <= maxError;

            return Mathf.Abs(valueB - valueA) < maxError;
        }
        else if (valueB < valueA)
        {
            if (inclusive)
                return Mathf.Abs(valueA - valueB) <= maxError;

            return Mathf.Abs(valueA - valueB) < maxError;
        }

        return true;
    }

    public static Vector3 RandomPointInArea(this Vector3 area)
    {
        return new Vector3(Random.Range(-area.x, area.x), Random.Range(-area.y, area.y), Random.Range(-area.z, area.z));
    }

    public static Vector2 RandomPointInArea(this Vector2 area)
    {
        return new Vector3(Random.Range(-area.x, area.x), Random.Range(-area.y, area.y));
    }

    public static bool InArea(this Vector3 point, Vector3 center, Vector3 area)
    {
        return new Vector2(center.x - area.x, center.x + area.x).Contains(point.x) &&
               new Vector2(center.y - area.y, center.y + area.y).Contains(point.y) &&
               new Vector2(center.z - area.z, center.z + area.z).Contains(point.z);
    }

    public static bool InRadius(this Vector3 point, Vector3 center, float radius)
    {
        return Vector3.Distance(point, center) <= radius;
    }

    public static Vector3 GetRandomVisiblePoint(Camera camera, Vector2 xRange, Vector2 yRange, Vector2 zRange)
    {
        if (camera == null)
            return Vector3.zero;

        xRange.x = Mathf.Clamp(xRange.x, 0f, camera.pixelWidth);
        xRange.y = Mathf.Clamp(xRange.y, xRange.x, camera.pixelWidth);

        yRange.x = Mathf.Clamp(yRange.x, 0f, camera.pixelHeight);
        yRange.y = Mathf.Clamp(yRange.y, yRange.x, camera.pixelHeight);

        zRange.x = Mathf.Clamp(zRange.x, camera.nearClipPlane + 0.2f, camera.farClipPlane);
        zRange.y = Mathf.Clamp(zRange.y, zRange.x, camera.farClipPlane);

        var screenX = Random.Range(xRange.x, xRange.y);
        var screenY = Random.Range(yRange.x, yRange.y);
        var screenZ = Random.Range(zRange.x, zRange.y);

        return camera.ScreenToWorldPoint(new Vector3(screenX, screenY, screenZ));
    }

    public static void RandomizePitch(this AudioSource aSource, Vector2 pitchRange)
    {
        aSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
    }

    public static Vector3[] GetPointsOnPlane(Transform plane, Vector2 size, int numPoints, float avoidRadius = 0,
        int maxIterations = 50, float height = 0.1f)
    {
        var points = new Vector3[numPoints];
        var pointCount = 0;

        for (var i = 0; i < numPoints; i++)
        {
            Vector3 newPoint;
            var it = 0;
            var overlaps = false;
            do
            {
                var x = Random.Range(-size.x / 2, size.x / 2);
                var z = Random.Range(-size.y / 2, size.y / 2);

                if (plane == null)
                    newPoint = new Vector3(x, height, z);
                else
                    newPoint = plane.TransformPoint(new Vector3(x, height, z));

                for (var pt = 0; pt < pointCount; pt++)
                {
                    if (Vector3.Distance(newPoint, points[pt]) <= avoidRadius)
                    {
                        overlaps = true;
                        break;
                    }
                }

                it++;
            } while (overlaps && it < maxIterations);

            points[i] = newPoint;
            pointCount++;
        }

        return points;
    }

    public static float Difference(float a, float b)
    {
        a = Mathf.Abs(a);
        b = Mathf.Abs(b);

        if (a > b)
        {
            return Mathf.Abs(a - b);
        }

        return b - a;
    }

    public static void SetActive(this MonoBehaviour _ref, bool state, float delay)
    {
        if (_ref == null)
            return;

        delay = Mathf.Abs(delay);

        if (delay == 0f)
        {
            _ref.gameObject.SetActive(state);
        }
        else
        {
            if (_ref.isActiveAndEnabled && state == false)
                _ref.StartCoroutine(_SetActive(_ref.gameObject, false, delay));
        }
    }

    static IEnumerator _SetActive(GameObject go, bool state, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (go == null)
            yield break;

        go.SetActive(state);
    }

    public static bool StateTime(this AnimatorStateInfo stateInfo, float time, bool isNormalized = false)
    {
        if (isNormalized)
        {
            time = time.Clamp01();
            return Approximately((stateInfo.normalizedTime - (int) stateInfo.normalizedTime), time, 0.001f);
        }

        if (time < 0)
            time = Mathf.Abs(time);

        var curTime = stateInfo.length * (stateInfo.normalizedTime - (int) stateInfo.normalizedTime);

        return Approximately(curTime, time, 0.001f);
    }

    public static float Distance(this float start, float end)
    {
        float dist;

        if (XOR(start < 0, end < 0))
        {
            dist = Mathf.Abs(start) + Mathf.Abs(end);
        }
        else
        {
            dist = Difference(start, end);
        }

        return dist;
    }

    public static float Distance(this Vector2 interval)
    {
        float dist;

        if (XOR(interval.x < 0, interval.y < 0))
        {
            dist = Mathf.Abs(interval.x) + Mathf.Abs(interval.y);
        }
        else
        {
            dist = Difference(interval.x, interval.y);
        }

        return dist;
    }

    public static bool XOR(bool cond1, bool cond2)
    {
        return (cond1 && !cond2) || (cond2 && !cond1);
    }

    public static float Min(this Vector2 range)
    {
        return Mathf.Min(range.x, range.y);
    }

    // Integer Clamp
    public static int Clamp(this int value, int min, int max)
    {
        return Mathf.Clamp(value, min, max);
    }

    public static int Clamp(this int value, int minMaxValue)
    {
        return Mathf.Clamp(value, -minMaxValue, minMaxValue);
    }

    public static int Clamp(this int value, Vector2Int range)
    {
        return Mathf.Clamp(value, range.x, range.y);
    }

    // Float Clamp
    public static float Clamp(this float value, float minMaxValue)
    {
        return Mathf.Clamp(value, -minMaxValue, minMaxValue);
    }

    public static float Clamp(this float value, Vector2 range)
    {
        return Mathf.Clamp(value, range.x, range.y);
    }

    public static float Clamp(this float value, float min, float max)
    {
        return Mathf.Clamp(value, min, max);
    }

    public static float Clamp01(this float value)
    {
        return Mathf.Clamp01(value);
    }

    public static float Abs(this float value)
    {
        return Mathf.Abs(value);
    }

    public static float Max(this Vector2 range)
    {
        return Mathf.Max(range.x, range.y);
    }

   
    
    public static float Percentage(float start, float end, float current)
    {
        return Mathf.InverseLerp(start, end, current);
    }

    public static float PercentageOf(this float duration, float start, float current)
    {
        var end = start + duration;
        return Percentage(start, end, current);
    }

    public static float PercentageOf(this float value, Vector2 range)
    {
        value = value.Clamp(range);
        return Percentage(range.Min(), range.Max(), value);
    }

    public static float MapValueToRange(Vector2 inputRange, Vector2 outputRange, float value)
    {
        return MapValueToRange(inputRange.Min(), inputRange.Max(), outputRange.Min(), outputRange.Max(), value);            
    }

    public static float MapValueToRange(float fromMin, float fromMax, float toMin, float toMax, float value)
    {
        float percentage = Percentage(fromMin, fromMax, value);
        return Mathf.Lerp(toMin, toMax, percentage);
    }

    public static float GetPercentageValue(this Vector2 range, float percent)
    {
        return Mathf.InverseLerp(range.Min(), range.Max(), percent.Clamp01());
    }

    public static float Lerp(this Vector2 range, float value)
    {
        return Mathf.Lerp(range.x, range.y, value.Clamp01());
    }

    public static Vector3 ClampMagnitude(this Vector3 v, float min, float max)
    {
        return v.normalized * v.magnitude.Clamp(min, max);
    }

    public static Vector3 Vortex(float time)
    {
        return new Vector3(time, time * Mathf.Cos(time), time * Mathf.Sin(time));
    }

    /// <summary>
    /// Assumes that viewPoint's forward direction is the target axis
    /// </summary>
    /// <param name="viewPoint"></param>
    /// <param name="target"></param>
    /// <param name="coneHeight"></param>
    /// <param name="fov"></param>
    /// <returns></returns>
    public static bool InVisionCone(this Transform target, Transform viewPoint, float coneHeight, float fov)
    {
        if (viewPoint == null || target == null || coneHeight <= 0f || fov <= 0f)
            return false;

        var dirToTarget = (target.position - viewPoint.position);
        var distToTarget = dirToTarget.sqrMagnitude;
        var sqrDist = coneHeight * coneHeight;

        if (distToTarget > sqrDist)
            return false;

        return (Vector3.Angle(viewPoint.forward, dirToTarget.normalized) < fov / 2);
    }

    public static bool InVisionCone(this Vector3 targetPosition, Vector3 viewPoint, Vector3 direction, float coneHeight,
        float fov)
    {
        if (coneHeight <= 0f || fov <= 0f || direction == Vector3.zero)
            return false;

        var dirToTarget = (targetPosition - viewPoint);
        var distToTarget = dirToTarget.sqrMagnitude;
        var sqrDist = coneHeight * coneHeight;

        if (distToTarget > sqrDist)
            return false;

        return (Vector3.Angle(direction, dirToTarget.normalized) < fov / 2);
    }

    public static void Initialize<T>(this List<T> list)
    {
        if (list == null)
            list = new List<T>();
        else
            list.Clear();
    }

    public static bool IsValidIndex(this int index, ICollection collection)
    {
        return collection != null && index >= 0 && index < collection.Count;
    }

    public static bool IsValidIndex(this int index, Array array)
    {
        return array != null && index >= 0 && index < array.Length;
    }

    public static float AngleDelta180(float current, float target)
    {
        var result = Mathf.Repeat(target - current, 180);
        if (result > 180.0)
            result -= 360f;
        return result;
    }

    public static bool IsNaN(this float t)
    {
        return float.IsNaN(t);
    }

    public static bool CanSee(this Transform transform, Vector3 target, float maxDistance, float fieldOfView,
        bool checkLineOfSight = false, LayerMask? targetLayers = null)
    {
        if (transform == null || maxDistance <= 0f || fieldOfView <= 0f)
            return false;

        var dirToTarget = (target - transform.position);
        var distToTarget = dirToTarget.sqrMagnitude;
        var sqrDist = maxDistance * maxDistance;

        if (distToTarget > sqrDist)
            return false;

        var isVisible = (Vector3.Angle(transform.forward, dirToTarget.normalized) < fieldOfView / 2);
        if (checkLineOfSight)
        {
            RaycastHit hit;
            var collisionCheck = targetLayers == null
                ? Physics.Raycast(transform.position, dirToTarget, out hit, maxDistance)
                : Physics.Raycast(transform.position, dirToTarget, out hit, maxDistance, targetLayers.Value);

            return isVisible && (!collisionCheck || hit.transform.position == target);
        }

        return isVisible;
    }


    public static int SquareRoot(int iValue)
    {
        if (iValue < 0)
            return -1;

        if (iValue == 0 || iValue == 1)
            return iValue;

        var result = 0;
        var start = 0;
        var end = iValue / 2;

        while (start <= end)
        {
            var mid = (start + end) / 2;
            var midSqrd = mid * mid;
            if (midSqrd == iValue)
                return mid;

            if (midSqrd < iValue)
            {
                start = mid + 1;
                result = mid;
            }
            else
            {
                end = mid - 1;
            }
        }

        return result;
    }

    /// <summary>
    /// Converts an euler rotation in the 0 - 360 range to one in the -180 to 180 range.
    /// </summary>
    /// <param name="eulerAngle"></param>
    /// <returns></returns>
    public static Vector3 ClampEuler180(this Vector3 eulerAngle)
    {
        var x = eulerAngle.x;
        var y = eulerAngle.y;
        var z = eulerAngle.z;

        if (x > 180) x -= 360;
        if (y > 180) y -= 360;
        if (z > 180) z -= 360;

        return new Vector3(x, y, z);
    }

    public static void AimTowardsClamped180(this Transform transform, Vector3 target, float percent, Vector2 yawRange,
        Vector2 pitchRange, Vector2 rollRange, Space space)
    {
        if (transform == null)
            return;

        percent = percent.Abs().Clamp01();

        var dirToTarget = (target - (space == Space.Self ? transform.localPosition : transform.position)).normalized;

        var desiredRotation = Quaternion.FromToRotation(transform.forward, dirToTarget);
        var desiredEuler = desiredRotation.eulerAngles.ClampEuler180();
        desiredEuler.y = desiredEuler.y.Clamp(yawRange);
        desiredEuler.x = desiredEuler.x.Clamp(pitchRange);
        desiredEuler.z = desiredEuler.z.Clamp(rollRange);

        desiredRotation.eulerAngles = desiredEuler;

        if (space == Space.Self)
            transform.localRotation =
                (percent >= 1 ? desiredRotation : Quaternion.Lerp(Quaternion.identity, desiredRotation, percent)) *
                transform.localRotation;
        else
            transform.rotation =
                (percent >= 1 ? desiredRotation : Quaternion.Lerp(Quaternion.identity, desiredRotation, percent)) *
                transform.rotation;
    }
}