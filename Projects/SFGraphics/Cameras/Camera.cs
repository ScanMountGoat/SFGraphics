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
        /// </summary>
        public Vector3 DefaultPosition { get; private set; }

        /// <summary>
        /// The position of the camera in scene units. 
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                UpdateMatrices();
            }
        }
        private Vector3 position = new Vector3(0, 10, -80);

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
                UpdateMatrices();
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
                UpdateMatrices();
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
                UpdateMatrices();
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
                UpdateMatrices();
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
        /// Only the ratio between <see cref="renderWidth"/> and <see cref="renderHeight"/> is important.
        /// </summary>
        public int renderWidth = 1;

        /// <summary>
        /// The height of the viewport or rendered region in pixels.
        /// Only the ratio between <see cref="renderWidth"/> and <see cref="renderHeight"/> is important.
        /// </summary>
        public int renderHeight = 1;

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
        /// <see cref="FovRadians"/>, <see cref="renderWidth"/> / <see cref="renderHeight"/>, <see cref="NearClipPlane"/>,
        /// <see cref="FarClipPlane"/>
        /// </summary>
        public Matrix4 PerspectiveMatrix { get { return perspectiveMatrix; } }

        /// <summary>
        /// Creates a new <see cref="Camera"/> located at <see cref="DefaultPosition"/>.
        /// </summary>
        public Camera()
        {
            DefaultPosition = new Vector3(new Vector3(0, 10, -80));
            ResetToDefaultPosition();
        }

        /// <summary>
        /// Translates the camera along the x and y axes by a specified amount.
        /// </summary>
        /// <param name="xAmount">The amount to add to the camera's x coordinate</param>
        /// <param name="yAmount">The amount to add to the camera's y coordinate</param>
        /// <param name="scaleByDistanceToOrigin">When <c>true</c>, the <paramref name="xAmount"/>
        /// and <paramref name="yAmount"/> are multiplied by the magnitude of <see cref="Position"/>
        /// and the sine of <see cref="FovRadians"/></param>
        public void Pan(float xAmount, float yAmount, bool scaleByDistanceToOrigin = true)
        {
            // Find the change in normalized screen coordinates.
            float deltaX = xAmount / renderWidth;
            float deltaY = yAmount / renderHeight;

            if (scaleByDistanceToOrigin)
            {
                // Translate the camera based on the distance from the origin and field of view.
                // Objects will "follow" the mouse while panning.
                position.Y += deltaY * ((float)Math.Sin(fovRadians) * position.Length);
                position.X += deltaX * ((float)Math.Sin(fovRadians) * position.Length);
            }
            else
            {
                // Regular panning.
                position.Y += deltaY;
                position.X += deltaX;
            }

            UpdateMatrices();
        }

        /// <summary>
        /// Translates the camera along the z-axis by a specified amount.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="scaleByDistanceToOrigin">When <c>true</c>, the <paramref name="amount"/> 
        /// is multiplied by the magnitude of <see cref="Position"/></param>
        public void Zoom(float amount, bool scaleByDistanceToOrigin = true)
        {
            // Increase zoom speed when zooming out. 
            float zoomScale = 1;
            if (scaleByDistanceToOrigin)
                zoomScale *= Math.Abs(position.Z);

            position.Z += amount * zoomScale;

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
        /// 
        /// </summary>
        protected virtual void UpdateTranslationMatrix()
        {
            translationMatrix = Matrix4.CreateTranslation(position.X, -position.Y, position.Z);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void UpdateRotationMatrix()
        {
            rotationMatrix = Matrix4.CreateRotationY(rotationYRadians) * Matrix4.CreateRotationX(rotationXRadians);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void UpdatePerspectiveMatrix()
        {
            perspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(fovRadians, renderWidth / (float)renderHeight, nearClipPlane, farClipPlane);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void UpdateModelViewMatrix()
        {
            modelViewMatrix = rotationMatrix * translationMatrix * Matrix4.CreateScale(scale);
        }

        /// <summary>
        /// 
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
            position = new Vector3(0, 10, -80);
            rotationXRadians = 0;
            rotationYRadians = 0;
            UpdateMatrices();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="center">The position of the center of the bounding sphere.</param>
        /// <param name="radius">The radius of the bounding sphere.</param>
        /// <param name="offset"></param>
        public void FrameBoundingSphere(Vector3 center, float radius, float offset = 10)
        {
            // Calculate a right triangle using the bounding sphere radius as the height and the fov as the angle.
            // The distance is the base of the triangle. 
            float distance = radius / (float)Math.Tan(fovRadians / 2.0f);

            float distanceOffset = offset / fovRadians;
            rotationXRadians = 0;
            rotationYRadians = 0;
            position.X = -center.X;
            position.Y = center.Y;
            position.Z = -1 * (distance + distanceOffset);

            UpdateMatrices();
        }
    }
}
