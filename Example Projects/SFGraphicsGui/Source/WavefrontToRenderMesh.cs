using OpenTK;
using System.Collections.Generic;

namespace SFGraphicsGui.Source
{
    internal static class WavefrontToRenderMesh
    {
        public static RenderMesh CreateRenderMesh(string fileName)
        {
            var vertices = GetVertices(FileFormatWavefront.FileFormatObj.Load(fileName, false));
            return new RenderMesh(vertices);
        }

        private static List<RenderVertex> GetVertices(FileFormatWavefront.FileLoadResult<FileFormatWavefront.Model.Scene> result)
        {
            // TODO: Groups?
            var vertices = new List<RenderVertex>(result.Model.UngroupedFaces.Count * 3);

            foreach (var face in result.Model.UngroupedFaces)
            {
                foreach (var index in face.Indices)
                {
                    AddVertex(result, vertices, index);
                }
            }

            return vertices;
        }

        private static void AddVertex(FileFormatWavefront.FileLoadResult<FileFormatWavefront.Model.Scene> result, List<RenderVertex> vertices, FileFormatWavefront.Model.Index index)
        {
            var position = new Vector3(result.Model.Vertices[index.vertex].x, result.Model.Vertices[index.vertex].y, result.Model.Vertices[index.vertex].z);

            var normal = Vector3.Zero;
            if (index.normal != null)
                normal = new Vector3(result.Model.Normals[(int)index.normal].x, result.Model.Normals[(int)index.normal].y, result.Model.Normals[(int)index.normal].z);

            var texCoord = Vector2.Zero;
            if (index.uv != null)
                texCoord = new Vector2(result.Model.Uvs[(int)index.uv].u, result.Model.Uvs[(int)index.uv].v);

            vertices.Add(new RenderVertex(position, normal, texCoord));
        }
    }
}
