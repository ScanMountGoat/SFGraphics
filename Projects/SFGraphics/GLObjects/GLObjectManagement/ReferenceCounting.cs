using System.Collections.Generic;
using System.Collections.Concurrent;

namespace SFGraphics.GLObjects.GLObjectManagement
{
    /// <summary>
    /// Provides helpers for maintaining object reference counts.
    /// <see cref="ConcurrentDictionary{TKey, TValue}"/> allows references to be incremented and
    /// decremented from separate threads.
    /// </summary>
    public static class ReferenceCounting
    {
        /// <summary>
        /// Increments the reference for <paramref name="objToIncrement"/> or 
        /// initializes to 1 if not found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="referenceCountByObject"></param>
        /// <param name="objToIncrement"></param>
        public static void IncrementReference<T>(ConcurrentDictionary<T, int> referenceCountByObject, T objToIncrement)
        {
            if (referenceCountByObject.ContainsKey(objToIncrement))
                referenceCountByObject[objToIncrement] += 1;
            else
                referenceCountByObject.TryAdd(objToIncrement, 1);
        }

        /// <summary>
        /// Decrements the reference count for <paramref name="objToDecrement"/> if
        /// the reference count is greater than zero.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="referenceCountByObject"></param>
        /// <param name="objToDecrement"></param>
        public static void DecrementReference<T>(ConcurrentDictionary<T, int> referenceCountByObject, T objToDecrement)
        {
            // Don't allow negative references just in case.
            if (referenceCountByObject.ContainsKey(objToDecrement))
            {
                if (referenceCountByObject[objToDecrement] > 0)
                    referenceCountByObject[objToDecrement] -= 1;
            }
        }

        /// <summary>
        /// Finds objects with zero references that can have their unmanaged data safely deleted.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="referenceCountByObject"></param>
        /// <returns>A hashset of objects with zero references</returns>
        public static HashSet<T> GetObjectsWithNoReferences<T>(ConcurrentDictionary<T, int> referenceCountByObject)
        {
            HashSet<T> objectsWithNoReferences = new HashSet<T>();

            foreach (var obj in referenceCountByObject)
            {
                if (obj.Value == 0)
                {
                    objectsWithNoReferences.Add(obj.Key);
                }
            }

            return objectsWithNoReferences;
        }
    }
}
