using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.BufferObjects;

namespace SFGraphics.Test.BufferObjectTests
{
    public abstract class BufferTest : Tests.ContextTest
    {
        protected BufferObject buffer;
        protected readonly float[] originalData = new float[] { 1.5f, 2.5f, 3.5f };

        [TestInitialize()]
        public override void Initialize()
        {
            base.Initialize();
            buffer = new BufferObject(BufferTarget.ArrayBuffer);
            buffer.SetData(originalData, BufferUsageHint.StaticDraw);
        }
    }
}
