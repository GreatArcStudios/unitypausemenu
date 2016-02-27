using UnityEngine;
using System.Collections;
/// <summary>
/// Provided by http://rontavstudio.com/use-coroutines-independent-timescale-unity-3d/. Used for running coroutines when the time scale is 0.
/// </summary>

public static class CoroutineUtilities
{
    public static IEnumerator WaitForRealTime(float delay)
    {
        while (true)
        {
            float pauseEndTime = Time.realtimeSinceStartup + delay;
            while (Time.realtimeSinceStartup < pauseEndTime)
            {
                yield return 0;
            }
            break;
        }
    }

}
