using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace SFGraphics.GLObjects
{
    internal static class ReferenceCounting
    {
        public static void AddReference(ConcurrentDictionary<Tuple<GLObject.GLObjectType, int>, int> referenceCountByGLObject, Tuple<GLObject.GLObjectType, int> glObject)
        {
            if (referenceCountByGLObject.ContainsKey(glObject))
                referenceCountByGLObject[glObject] += 1;
            else
                referenceCountByGLObject.TryAdd(glObject, 1);
        }

        public static void RemoveReference(ConcurrentDictionary<Tuple<GLObject.GLObjectType, int>, int> referenceCountByGLObject, Tuple<GLObject.GLObjectType, int> glObject)
        {
            // Don't allow negative references just in case.
            if (referenceCountByGLObject.ContainsKey(glObject))
            {
                if (referenceCountByGLObject[glObject] > 0)
                    referenceCountByGLObject[glObject] -= 1;
            }
        }

        public static HashSet<Tuple<GLObject.GLObjectType, int>> FindIdsWithNoReferences(ConcurrentDictionary<Tuple<GLObject.GLObjectType, int>, int> referenceCountByGLObject)
        {
            HashSet<Tuple<GLObject.GLObjectType, int>> idsWithoutReferences = new HashSet<Tuple<GLObject.GLObjectType, int>>();

            foreach (var glObject in referenceCountByGLObject)
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
