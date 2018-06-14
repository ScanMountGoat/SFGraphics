using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

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
        /// Updates all matrices when set.
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
        /// Updates all matrices when set.
        /// <para>Values less than or equal to 0 or greater than or equal to 180 are ignored.</para>
        /// </summary>
        public float FovDegrees
        {
            // Only store radians internally.
            get { return (float)(fovRadians * 180.0f / Math.PI); }
            set
            {
                if (value > 0 && value < 180)
                {
                    fovRadians = (float)(value / 180.0f * Math.PI);
                    UpdateMatrices();
                }
            }
        }

        /// <summary>
        /// The rotation around the x-axis in radians.
        /// Updates all matrices when set.
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
        /// Updates all matrices when set.
        /// </summary>
        public float RotationXDegrees
        {
            get { return (float)(rotationXRadians * 180.0f / Math.PI); }
            set
            {
                // Only store radians internally.
                rotationXRadians = (float)(value / 180.0f * Math.PI);
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
            get { return (float)(rotationYRadians * 180.0f / Math.PI); }
            set
            {
                // Only store radians internally.
                rotationYRadians = (float)(value / 180.0f * Math.PI);
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
        /// The far clip plane of the perspective matrix.
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

        public int renderWidth = 1;
        public int renderHeight = 1;

        // Matrices shouldn't be changed directly.
        // To change the rotation matrix, set the rotation values, for example.
        /// <summary>
        /// 
        /// </summary>
        protected Matrix4 modelViewMatrix = Matrix4.Identity;
        /// <summary>
        /// 
        /// </summary>
        public Matrix4 ModelViewMatrix { get { return modelViewMatrix; } }

        /// <summary>
        /// 
        /// </summary>
        protected Matrix4 mvpMatrix = Matrix4.Identity;
        /// <summary>
        /// 
        /// </summary>
        public Matrix4 MvpMatrix { get { return mvpMatrix; } }

        /// <summary>
        /// 
        /// </summary>
        protected Matrix4 rotationMatrix = Matrix4.Identity;
        /// <summary>
        /// 
        /// </summary>
        public Matrix4 RotationMatrix { get { return rotationMatrix; } }

        /// <summary>
        /// 
        /// </summary>
        protected Matrix4 translationMatrix = Matrix4.Identity;
        /// <summary>
        /// 
        /// </summary>
        public Matrix4 TranslationMatrix { get { return translationMatrix; } }

        /// <summary>
        /// 
        /// </summary>
        protected Matrix4 perspectiveMatrix = Matrix4.Identity;
        /// <summary>
        /// 
        /// </summary>
        public Matrix4 PerspectiveMatrix { get { return perspectiveMatrix; } }

        // Camera control settings. 
        protected float zoomDistanceScale = 1;
        protected float rotateYSpeed = 1;
        protected float rotateXSpeed = 1;
        protected float zoomSpeed = 1;

        /// <summary>
        /// 
        /// </summary>
        public Camera()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotX"></param>
        /// <param name="rotY"></param>
        /// <param name="renderWidth"></param>
        /// <param name="renderHeight"></param>
        public Camera(Vector3 position, float rotX, float rotY, int renderWidth = 1, int renderHeight = 1)
        {
            Position = position;
            this.renderHeight = renderHeight;
            this.renderWidth = renderWidth;
            RotationXRadians = rotX;
            RotationYRadians = rotY;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xAmount"></param>
        /// <param name="yAmount"></param>
        public void Rotate(float xAmount, float yAmount)
        {
            rotationYRadians += rotateYSpeed * xAmount;
            rotationXRadians += rotateXSpeed * yAmount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xAmount"></param>
        /// <param name="yAmount"></param>
        /// <param name="scaleByDistanceToOrigin"></param>
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
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="scaleByDistanceToOrigin"></param>
        public void Zoom(float amount, bool scaleByDistanceToOrigin = true)
        {
            // Increase zoom speed when zooming out. 
            float zoomscale = zoomSpeed;
            if (scaleByDistanceToOrigin)
                zoomscale *= Math.Abs(position.Z) * zoomDistanceScale;

            position.Z += amount * zoomscale;
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateMatrices()
        {
            translationMatrix = Matrix4.CreateTranslation(position.X, -position.Y, position.Z);
            rotationMatrix = Matrix4.CreateRotationY(rotationYRadians) * Matrix4.CreateRotationX(rotationXRadians);
            perspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(fovRadians, renderWidth / (float)renderHeight, nearClipPlane, farClipPlane);

            modelViewMatrix = rotationMatrix * translationMatrix;
            mvpMatrix = modelViewMatrix * perspectiveMatrix;
        }

        /// <summary>
        /// 
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
