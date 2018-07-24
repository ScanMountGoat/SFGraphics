using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace SFGraphics.GLObjects
{
    static class ReferenceCounting
    {
        public static void AddReference(ConcurrentDictionary<int, int> referenceCountById, int id)
        {
            if (referenceCountById.ContainsKey(id))
                referenceCountById[id] += 1;
            else
                referenceCountById.TryAdd(id, 1);
        }

        public static void RemoveReference(ConcurrentDictionary<int, int> referenceCountById, int id)
        {
            // Don't allow negative references just in case.
            if (referenceCountById.ContainsKey(id))
            {
                if (referenceCountById[id] > 0)
                    referenceCountById[id] -= 1;
            }
        }

        public static HashSet<int> FindIdsWithNoReferences(ConcurrentDictionary<int, int> referenceCountById)
        {
            HashSet<int> idsWithoutReferences = new HashSet<int>();
            foreach (var glObject in referenceCountById)
            {
                if (glObject.Value == 0)
                {
                    idsWithoutReferences.Add(glObject.Key);
                }
            }

            return idsWithoutReferences;
        }
    }
}
