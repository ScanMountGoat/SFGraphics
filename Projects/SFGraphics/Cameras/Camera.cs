using System;
using OpenTK;
using SFGraphics.Utils;

namespace SFGraphics.Cameras
{
    /// <summary>
    /// A container for 4x4 matrices. The matrices can not be set directly.
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// The camera's initial position or position after <see cref="ResetToDefaultPosition"/>.
        /// Defaults to (0, 10, -80).
        /// </summary>
        public Vector3 DefaultPosition { get; } = new Vector3(0, 10, -80);

        /// <summary>
        /// The position of the camera in scene units. 
        /// </summary>
        public Vector3 Position
        {
            get
            {
                // Account for rotation when calculating the camera's actual position.
                return (rotationMatrix * new Vector4(translation, 1)).Xyz;
            }
            set
            {
                translation = value;
                UpdateMatrices();
            }
        }
        private Vector3 translation = new Vector3(0, 10, -80);

        /// <summary>
        /// The scale for all objects. Defaults to 1.
        /// </summary>
        public float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                UpdateMatrices();
            }
        }
        private float scale = 1;

        /// <summary>
        /// The vertical field of view in radians. 
        /// Updates <see cref="FovDegrees"/> and all matrices when set.
        /// <para>Values less than or equal to 0 or greater than or equal to PI are ignored.</para>
        /// </summary>
        public float FovRadians
        {
            get { return fovRadians; }
            set
            {
                if (value > 0 && value < Math.PI)
                {
                    fovRadians = value;
                    UpdateMatrices();
                }
            }
        }
        private float fovRadians = (float)Math.PI / 6.0f; // 30 degrees

        /// <summary>
        /// The vertical field of view in degrees. 
        /// Updates <see cref="FovRadians"/> and all matrices when set.
        /// <para>Values less than or equal to 0 or greater than or equal to 180 are ignored.</para>
        /// </summary>
        public float FovDegrees
        {
            get { return (float)VectorUtils.GetDegrees(fovRadians); }
            set
            {
                if (value > 0 && value < 180)
                {
                    fovRadians = (float)VectorUtils.GetRadians(value);
                    UpdateMatrices();
                }
            }
        }

        /// <summary>
        /// The rotation around the x-axis in radians.
        /// </summary>
        public float RotationXRadians
        {
            get { return rotationXRadians; }
            set
            {
                rotationXRadians = value;

                UpdateRotationMatrix();
                UpdateModelViewMatrix();
                UpdateMvpMatrix();
            }
        }
        private float rotationXRadians = 0;

        /// <summary>
        /// The rotation around the x-axis in degrees.
        /// </summary>
        public float RotationXDegrees
        {
            get { return (float)VectorUtils.GetDegrees(rotationXRadians); }
            set
            {
                // Only store radians internally.
                rotationXRadians = (float)VectorUtils.GetRadians(value);

                UpdateRotationMatrix();
                UpdateModelViewMatrix();
                UpdateMvpMatrix();
            }
        }

        /// <summary>
        /// The rotation around the y-axis in radians.
        /// </summary>
        public float RotationYRadians
        {
            get { return rotationYRadians; }
            set
            {
                rotationYRadians = value;

                UpdateRotationMatrix();
                UpdateModelViewMatrix();
                UpdateMvpMatrix();
            }
        }
        private float rotationYRadians = 0;

        /// <summary>
        /// The rotation around the y-axis in degrees.
        /// </summary>
        public float RotationYDegrees
        {
            get { return (float)VectorUtils.GetDegrees(rotationYRadians); }
            set
            {
                rotationYRadians = (float)VectorUtils.GetRadians(value);

                UpdateRotationMatrix();
                UpdateModelViewMatrix();
                UpdateMvpMatrix();
            }
        }

        /// <summary>
        /// The far clip plane of the perspective matrix.
        /// </summary>
        public float FarClipPlane
        {
            get { return farClipPlane; }
            set
            {
                farClipPlane = value;
                UpdateMatrices();
            }
        }
        private float farClipPlane = 100000;

        /// <summary>
        /// The near clip plane of the perspective matrix.
        /// </summary>
        public float NearClipPlane
        {
            get { return nearClipPlane; }
            set
            {
                nearClipPlane = value;
                UpdateMatrices();
            }
        }
        private float nearClipPlane = 1;

        /// <summary>
        /// The width of the viewport or rendered region in pixels.
        /// Values less than 1 are set to 1.
        /// </summary>
        public int RenderWidth
        {
            get { return renderWidth; }
            set
            {
                renderWidth = Math.Max(value, 1);
                UpdateMatrices();
            }
        }
        private int renderWidth = 1;

        /// <summary>
        /// The height of the viewport or rendered region in pixels.
        /// Values less than 1 are set to 1.
        /// </summary>
        public int RenderHeight
        {
            get { return renderHeight; }
            set
            {
                renderHeight = Math.Max(value, 1);
                UpdateMatrices();
            }
        }
        private int renderHeight = 1;

        /// <summary>
        /// The ratio for <see cref="RenderWidth"/> / <see cref="RenderHeight"/>.
        /// </summary>
        public float AspectRatio => (float)RenderWidth / RenderHeight;

        /// <summary>
        /// See <see cref="ModelViewMatrix"/>
        /// </summary>
        protected Matrix4 modelViewMatrix = Matrix4.Identity;

        /// <summary>
        /// The result of <see cref="RotationMatrix"/> * <see cref="TranslationMatrix"/>
        /// </summary>
        public Matrix4 ModelViewMatrix { get { return modelViewMatrix; } }

        /// <summary>
        /// See <see cref="MvpMatrix"/>
        /// </summary>
        protected Matrix4 mvpMatrix = Matrix4.Identity;

        /// <summary>
        /// The result of <see cref="ModelViewMatrix"/> * <see cref="PerspectiveMatrix"/>
        /// </summary>
        public Matrix4 MvpMatrix { get { return mvpMatrix; } }

        /// <summary>
        /// See <see cref="RotationMatrix"/>
        /// </summary>
        protected Matrix4 rotationMatrix = Matrix4.Identity;

        /// <summary>
        /// The result of <see cref="Matrix4.CreateRotationY(float)"/> * <see cref="Matrix4.CreateRotationX(float)"/>
        /// </summary>
        public Matrix4 RotationMatrix { get { return rotationMatrix; } }

        /// <summary>
        /// See <see cref="TranslationMatrix"/>
        /// </summary>
        protected Matrix4 translationMatrix = Matrix4.Identity;

        /// <summary>
        /// The result of <see cref="Matrix4.CreateTranslation(float, float, float)"/> for X, -Y, Z of <see cref="Position"/>
        /// </summary>
        public Matrix4 TranslationMatrix { get { return translationMatrix; } }

        /// <summary>
        /// See <see cref="PerspectiveMatrix"/>
        /// </summary>
        protected Matrix4 perspectiveMatrix = Matrix4.Identity;

        /// <summary>
        /// The result of <see cref="Matrix4.CreatePerspectiveFieldOfView(float, float, float, float)"/> for 
        /// <see cref="FovRadians"/>, <see cref="RenderWidth"/> / <see cref="RenderHeight"/>, <see cref="NearClipPlane"/>,
        /// <see cref="FarClipPlane"/>
        /// </summary>
        public Matrix4 PerspectiveMatrix { get { return perspectiveMatrix; } }

        /// <summary>
        /// Creates a new <see cref="Camera"/> located at <see cref="DefaultPosition"/>.
        /// </summary>
        public Camera()
        {
            UpdateMatrices();
        }

        /// <summary>
        /// Translates the camera along the x and y axes by a specified amount.
        /// </summary>
        /// <param name="xAmount">The amount to add to the camera's x coordinate</param>
        /// <param name="yAmount">The amount to add to the camera's y coordinate</param>
        /// <param name="scaleByDistanceToOrigin">When <c>true</c>, panning is faster for more distance objects.</param>
        public void Pan(float xAmount, float yAmount, bool scaleByDistanceToOrigin = true)
        {
            // Find the change in normalized screen coordinates.
            float deltaX = xAmount / RenderWidth;
            float deltaY = yAmount / RenderHeight;

            if (scaleByDistanceToOrigin)
            {
                // Translate the camera based on the distance from the origin and field of view.
                // Objects will "follow" the mouse while panning.
                translation.Y += deltaY * ((float)Math.Sin(fovRadians) * translation.Length);
                translation.X += deltaX * ((float)Math.Sin(fovRadians) * translation.Length);
            }
            else
            {
                // Regular panning.
                translation.Y += deltaY;
                translation.X += deltaX;
            }

            UpdateMatrices();
        }

        /// <summary>
        /// Translates the camera along the z-axis by a specified amount.
        /// </summary>
        /// <param name="amount">The amount to zoom in scene units</param>
        /// <param name="scaleByDistanceToOrigin">When <c>true</c>, the <paramref name="amount"/> 
        /// is multiplied by the magnitude of <see cref="Position"/></param>
        public void Zoom(float amount, bool scaleByDistanceToOrigin = true)
        {
            // Increase zoom speed when zooming out. 
            float zoomScale = 1;
            if (scaleByDistanceToOrigin)
                zoomScale *= Math.Abs(translation.Z);

            translation.Z += amount * zoomScale;

            UpdateMatrices();
        }

        /// <summary>
        /// Updates all matrix properties using the respective update methods.
        /// </summary>
        protected void UpdateMatrices()
        {
            UpdateTranslationMatrix();
            UpdateRotationMatrix();
            UpdatePerspectiveMatrix();

            UpdateModelViewMatrix();

            UpdateMvpMatrix();
        }

        /// <summary>
        /// Calculates <see cref="translationMatrix"/>.
        /// </summary>
        protected virtual void UpdateTranslationMatrix()
        {
            translationMatrix = Matrix4.CreateTranslation(translation.X, -translation.Y, translation.Z);
        }

        /// <summary>
        /// Calculates <see cref="rotationMatrix"/>.
        /// </summary>
        protected virtual void UpdateRotationMatrix()
        {
            rotationMatrix = Matrix4.CreateRotationY(rotationYRadians) * Matrix4.CreateRotationX(rotationXRadians);
        }

        /// <summary>
        /// Calculates <see cref="perspectiveMatrix"/>.
        /// </summary>
        protected virtual void UpdatePerspectiveMatrix()
        {
            perspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(fovRadians, RenderWidth / (float)RenderHeight, nearClipPlane, farClipPlane);
        }

        /// <summary>
        /// Calculates <see cref="modelViewMatrix"/>.
        /// </summary>
        protected virtual void UpdateModelViewMatrix()
        {
            modelViewMatrix = Matrix4.CreateScale(scale)* rotationMatrix * translationMatrix;
        }

        /// <summary>
        /// Calculates <see cref="mvpMatrix"/>.
        /// </summary>
        protected virtual void UpdateMvpMatrix()
        {
            mvpMatrix = modelViewMatrix * perspectiveMatrix;
        }

        /// <summary>
        /// Sets <see cref="rotationXRadians"/> and <see cref="RotationYRadians"/> to 0.
        /// </summary>
        public void ResetToDefaultPosition()
        {
            translation = DefaultPosition;
            rotationXRadians = 0;
            rotationYRadians = 0;
            UpdateMatrices();
        }

        /// <summary>
        /// Transforms the camera to frame a sphere of the given dimensions in the viewport.
        /// </summary>
        /// <param name="center">The position of the center of the bounding sphere.</param>
        /// <param name="radius">The radius of the bounding sphere in scene units</param>
        /// <param name="offset">The offset in scene units</param>
        public virtual void FrameBoundingSphere(Vector3 center, float radius, float offset = 10)
        {
            // Find the min to avoid clipping for non square aspect ratios.
            float fovHorizontal = (float)(2 * Math.Atan(Math.Tan(fovRadians / 2) * AspectRatio));
            float minFov = Math.Min(fovRadians, fovHorizontal);

            // Calculate the height of a right triangle using field of view and the sphere radius.
            float distance = radius / (float)Math.Tan(minFov / 2.0f);

            // TODO: Don't reset rotation.
            rotationXRadians = 0;
            rotationYRadians = 0;

            translation.X = -center.X;
            translation.Y = center.Y;

            // TODO: Why divide by field of view?
            float distanceOffset = offset / minFov;
            translation.Z = -1 * (distance + distanceOffset);

            UpdateMatrices();
        }
    }
}
