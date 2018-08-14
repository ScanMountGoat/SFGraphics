using System.Collections.Generic;
using System.Collections.Concurrent;

namespace SFGraphics.GLObjects.GLObjectManagement
{
    internal static class ReferenceCounting
    {
        public static void IncrementReference<T>(ConcurrentDictionary<T, int> referenceCountByGLObject, T objToIncrement)
        {
            if (referenceCountByGLObject.ContainsKey(objToIncrement))
                referenceCountByGLObject[objToIncrement] += 1;
            else
                referenceCountByGLObject.TryAdd(objToIncrement, 1);
        }

        public static void DecrementReference<T>(ConcurrentDictionary<T, int> referenceCountByGLObject, T objToDecrement)
        {
            // Don't allow negative references just in case.
            if (referenceCountByGLObject.ContainsKey(objToDecrement))
            {
                if (referenceCountByGLObject[objToDecrement] > 0)
                    referenceCountByGLObject[objToDecrement] -= 1;
            }
        }

        public static HashSet<T> GetObjectsWithNoReferences<T>(ConcurrentDictionary<T, int> referenceCountByObject)
        {
            HashSet<T> objectsWithNoReferences = new HashSet<T>();

            foreach (var glObject in referenceCountByObject)
            {
                if (glObject.Value == 0)
                {
                    objectsWithNoReferences.Add(glObject.Key);
                }
            }

            return objectsWithNoReferences;
        }
    }
}
