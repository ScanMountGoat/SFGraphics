using System;
using OpenTK;
using SFGraphics.Tools;

namespace SFGraphics.Cameras
{
    /// <summary>
    /// A container for 4x4 camera matrices. The matrices can not be set directly.
    /// To edit the translation matrix, the camera position should be changed, for example.
    /// <para>Keyboard/mouse controls can be added by inheriting from this class and using the Pan(), Rotate(), Zoom() methods.</para>
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// The position of the camera in scene units. 
        /// Updates all matrices when set.
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
            // Only store radians internally.
            get { return (float)VectorTools.GetDegrees(fovRadians); }
            set
            {
                if (value > 0 && value < 180)
                {
                    fovRadians = (float)VectorTools.GetRadians(value);
                    UpdateMatrices();
                }
            }
        }

        /// <summary>
        /// The rotation around the x-axis in radians.
        /// Updates <see cref="RotationXDegrees"/> and all matrices when set.
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
        /// Updates <see cref="rotationXRadians"/> and all matrices when set.
        /// </summary>
        public float RotationXDegrees
        {
            get { return (float)VectorTools.GetDegrees(rotationXRadians); }
            set
            {
                // Only store radians internally.
                rotationXRadians = (float)VectorTools.GetRadians(value);
                UpdateMatrices();
            }
        }

        /// <summary>
        /// The rotation around the y-axis in radians.
        /// Updates all matrices when set.
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
        /// Updates all matrices when set.
        /// </summary>
        public float RotationYDegrees
        {
            get { return (float)VectorTools.GetDegrees(rotationYRadians); }
            set
            {
                // Only store radians internally.
                rotationYRadians = (float)VectorTools.GetRadians(value);
                UpdateMatrices();
            }
        }

        /// <summary>
        /// The far clip plane of the perspective matrix.
        /// Updates all matrices when set.
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
        /// Updates all matrices when set.
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
        /// 
        /// </summary>
        public Camera()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">The initial position of the camera.</param>
        /// <param name="rotX">The rotation around the x-axis in radians</param>
        /// <param name="rotY">The rotation around the y-axis in radians</param>
        /// <param name="renderWidth">The width of the viewport in pixels</param>
        /// <param name="renderHeight">The height of the viewport in pixels</param>
        public Camera(Vector3 position, float rotX, float rotY, int renderWidth = 1, int renderHeight = 1)
        {
            Position = position;
            this.renderHeight = renderHeight;
            this.renderWidth = renderWidth;
            RotationXRadians = rotX;
            RotationYRadians = rotY;
        }

        /// <summary>
        /// Rotates the camera around the x and y axes by the specified amounts.
        /// </summary>
        /// <param name="xAmount">Amount to rotate around the x-axis in radians</param>
        /// <param name="yAmount">Amount to rotate around the y-axis in radians</param>
        public void Rotate(float xAmount, float yAmount)
        {
            rotationXRadians += xAmount;
            rotationYRadians += yAmount;
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
        }

        /// <summary>
        /// Updates the <see cref="TranslationMatrix"/>, <see cref="RotationMatrix"/>, 
        /// <see cref="PerspectiveMatrix"/>, <see cref="ModelViewMatrix"/>, 
        /// and <see cref="MvpMatrix"/>.
        /// </summary>
        public void UpdateMatrices()
        {
            UpdateTranslationMatrix();
            UpdateRotationMatrix();
            UpdatePerspectiveMatrix();

            UpdateModelViewMatrix();
            UpdateMvpMatrix();
        }

        private void UpdateTranslationMatrix()
        {
            translationMatrix = Matrix4.CreateTranslation(position.X, -position.Y, position.Z);
        }

        private void UpdateRotationMatrix()
        {
            rotationMatrix = Matrix4.CreateRotationY(rotationYRadians) * Matrix4.CreateRotationX(rotationXRadians);
        }

        private void UpdatePerspectiveMatrix()
        {
            perspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(fovRadians, renderWidth / (float)renderHeight, nearClipPlane, farClipPlane);
        }

        private void UpdateModelViewMatrix()
        {
            modelViewMatrix = rotationMatrix * translationMatrix;
        }

        private void UpdateMvpMatrix()
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="center">The position of the center of the bounding sphere.</param>
        /// <param name="radius">The radius of the bounding sphere.</param>
        public void FrameBoundingSphere(Vector3 center, float radius)
        {
            // Calculate a right triangle using the bounding sphere radius as the height and the fov as the angle.
            // The distance is the base of the triangle. 
            float distance = radius / (float)Math.Tan(fovRadians / 2.0f);

            float offset = 10 / fovRadians;
            rotationXRadians = 0;
            rotationYRadians = 0;
            position.X = -center.X;
            position.Y = center.Y;
            position.Z = -1 * (distance + offset);
        }
    }
}
