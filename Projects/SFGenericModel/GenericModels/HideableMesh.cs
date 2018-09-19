namespace SFGenericModel.GenericModels
{
    /// <summary>
    /// A container for a drawable mesh that supports toggling visibility.
    /// </summary>
    public class HideableMesh
    {
        /// <summary>
        /// The mesh to render.
        /// </summary>
        public IDrawableMesh Mesh { get; }

        /// <summary>
        /// Indicates if the mesh should be rendered.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Creates a new instance with the specified mesh and visibility.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="visible"></param>
        public HideableMesh(IDrawableMesh mesh, bool visible)
        {
            Mesh = mesh;
            Visible = visible;
        }
    }
}
