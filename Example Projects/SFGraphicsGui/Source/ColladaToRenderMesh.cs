using ColladaSharp;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFGraphicsGui.Source
{
    internal static class ColladaToRenderMesh
    {
        public static async Task<List<RenderVertex>> GetVerticesAsync(string filename)
        {
            var result = await Collada.ImportAsync(filename, new ColladaImportOptions(),
                new Progress<float>(), CancellationToken.None);

            var vertices = new List<RenderVertex>();
            foreach (var scene in result.Scenes)
            {
                foreach (var subMesh in scene.Model.Children)
                {
                    for (int i = 0; i < subMesh.Primitives.Triangles.Count; i++)
                    {
                        var face = subMesh.Primitives.GetFace(i);
                        vertices.Add(GetVertex(face.Vertex0));
                        vertices.Add(GetVertex(face.Vertex1));
                        vertices.Add(GetVertex(face.Vertex2));
                    }
                }
            }

            return vertices;
        }

        private static RenderVertex GetVertex(ColladaSharp.Models.Vertex vertex)
        {
            // TODO: Orientation?
            var position = new Vector4(vertex.Position.X, vertex.Position.Y, vertex.Position.Z, 1) * Matrix4.CreateRotationX(90);
            var normal = new Vector4(vertex.Normal.X, vertex.Normal.Y, vertex.Normal.Z, 1) * Matrix4.CreateRotationX(90);
            var texCoord = new Vector2(vertex.TexCoord.X, vertex.TexCoord.Y);
            return new RenderVertex(position.Xyz, normal.Xyz, texCoord);
        }

    }
}
