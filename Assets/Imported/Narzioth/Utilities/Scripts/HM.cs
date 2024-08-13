using System.Collections;
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



        #endregion
    }
}
