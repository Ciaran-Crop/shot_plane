using System.Collections;
using UnityEngine;
class BezierCurve : Singleton<BezierCurve>
{
    void Start()
    {

    }

    public Vector3 QuadraticBezierCurve(Vector3 startPoint, Vector3 middlePoint, Vector3 endPoint, float t)
    {
        return Vector3.Lerp(
            Vector3.Lerp(startPoint, middlePoint, t),
            Vector3.Lerp(middlePoint, endPoint, t),
            t
        );
    }

    public Vector3 CubicBezierCurve(Vector3 startPoint, Vector3 middlePoint1, Vector3 middlePoint2, Vector3 endPoint, float t)
    {
        return QuadraticBezierCurve(
            Vector3.Lerp(startPoint, middlePoint1, t),
            Vector3.Lerp(middlePoint1, middlePoint2, t),
            Vector3.Lerp(middlePoint2, endPoint, t),
            t
        );
    }
}