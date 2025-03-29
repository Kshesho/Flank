using System.Collections.Generic;
using UnityEngine;

namespace Narzioth.Utilities
{
    /// <summary>
    /// A collection of Helper and Extention Methods.
    /// To make life a little easier. <3
    /// </summary>
    public static class HM
    {
        #region Helper Methods

        private static readonly Dictionary<float, WaitForSeconds> _waitDy = new Dictionary<float, WaitForSeconds>();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>A new or previously used WaitForSeconds</returns>
        public static WaitForSeconds WaitTime(float timeToWait)
        {
            if (_waitDy.TryGetValue(timeToWait, out WaitForSeconds wait))
                return wait;

            _waitDy[timeToWait] = new WaitForSeconds(timeToWait);
            return _waitDy[timeToWait];
        }

        private static readonly Dictionary<float, WaitForSecondsRealtime> _waitRealtimeDy = new Dictionary<float, WaitForSecondsRealtime>();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>A new or previously used WaitForSecondsRealtime</returns>
        public static WaitForSecondsRealtime WaitRealtime(float timeToWait)
        {
            if (_waitRealtimeDy.TryGetValue(timeToWait, out WaitForSecondsRealtime wait))
                return wait;

            _waitRealtimeDy[timeToWait] = new WaitForSecondsRealtime(timeToWait);
            return _waitRealtimeDy[timeToWait];
        }

        #endregion
        #region Extention Methods

        /// <summary>
        /// Rotates the z axis of the transform to face the <paramref name="target_"/>.
        /// </summary>
        /// <param name="transform_"></param>
        /// <param name="target_"></param>
        public static void LookAt2D(this Transform transform_, Transform target_)
        {
            Vector2 targetPos = target_.position;

            float angle = Mathf.Atan2(targetPos.y - transform_.position.y, targetPos.x - transform_.position.x) * Mathf.Rad2Deg;
            Quaternion lookAt = Quaternion.Euler(new Vector3(0, 0, angle - 90));

            transform_.rotation = lookAt;
        }
        /// <summary>
        /// Rotates the z axis of the transform over time by <paramref name="speed_"/> to face the <paramref name="target_"/>.
        /// </summary>
        /// <param name="transform_"></param>
        /// <param name="target_"></param>
        /// <returns>true when the rotation reaches the value that faces the <paramref name="target_"/>.</returns>
        public static bool LookAt2D(this Transform transform_, Transform target_, float speed_)
        {
            Vector2 targetPos = target_.position;

            float angle = Mathf.Atan2(targetPos.y - transform_.position.y, targetPos.x - transform_.position.x) * Mathf.Rad2Deg;
            Quaternion lookAt = Quaternion.Euler(new Vector3(0, 0, angle - 90));

            transform_.rotation = Quaternion.Lerp(transform_.rotation, lookAt, speed_ * Time.deltaTime);

            //return true if the rotation is within N degrees of the value that faces the target
            var rotationDifference = Mathf.Abs(transform_.rotation.eulerAngles.z - lookAt.eulerAngles.z);
            if (rotationDifference <= 3)
                return true;
            return false;
        }

        /// <summary>
        /// Randomly sets the bool to true or false.
        /// </summary>
        /// <param name="bool_"></param>
        public static void CoinFlip(this ref bool bool_)
        {
            int rand = UnityEngine.Random.Range(0, 2);
            if (rand == 0)
                bool_ = false;
            else bool_ = true;
        }

        #endregion
    }
}
