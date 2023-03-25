using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Test.Models
{
    public class Cube : ModelVisual3D
    {

        private Vector3D _size;
        public Vector3D Size
        {
            get => _size;
            set
            {
                if (value.X > 0 && value.Y > 0 && value.Z > 0)
                    _size = value;
                else
                    throw new ArgumentNullException(nameof(Size), $"Size cannot be less then 0");
            }
        }
        public Point3D Position { get; set; }        


        public GeometryModel3D _model = new GeometryModel3D();
        private DiffuseMaterial Material { get; set; } = new DiffuseMaterial(Brushes.Gray);

        
        public Cube(Point3D position)
        {
            Position = position;
            SetModel();
        }
        public Cube(Point3D position, Vector3D size)
            : this(position)
        {
            Size = size;
            SetModel();

        }
        public Cube(Point3D position, Vector3D size, Brush brushes)
            : this(position, size)
        {
            Material = new DiffuseMaterial(brushes);
            SetModel();
        }
        

        private void SetModel()
        {
            var halfWidth = Size.X / 2;
            var halfHeight = Size.Y / 2;
            var halfDepth = Size.Z / 2;


            Point3D[] vertices = new Point3D[8];


            // Bottom face
            vertices[0] = new Point3D(Position.X - halfWidth, Position.Y - halfHeight, Position.Z - halfDepth);
            vertices[1] = new Point3D(Position.X + halfWidth, Position.Y - halfHeight, Position.Z - halfDepth);
            vertices[2] = new Point3D(Position.X + halfWidth, Position.Y - halfHeight, Position.Z + halfDepth);
            vertices[3] = new Point3D(Position.X - halfWidth, Position.Y - halfHeight, Position.Z + halfDepth);

            // Top face
            vertices[4] = new Point3D(Position.X - halfWidth, Position.Y + halfHeight, Position.Z - halfDepth);
            vertices[5] = new Point3D(Position.X + halfWidth, Position.Y + halfHeight, Position.Z - halfDepth);
            vertices[6] = new Point3D(Position.X + halfWidth, Position.Y + halfHeight, Position.Z + halfDepth);
            vertices[7] = new Point3D(Position.X - halfWidth, Position.Y + halfHeight, Position.Z + halfDepth);



            var indices = new int[]
            {
                // Bottom face
                0, 1, 2,
                2, 3, 0,

                // Top face
                4, 6, 5,
                6, 4, 7,

                // Left face
                0, 3, 4,
                4, 3, 7,

                // Right face
                1, 5, 2,
                2, 5, 6,

                // Front face
                0, 4, 1,
                1, 4, 5,

                // Back face
                2, 6, 3,
                3, 6, 7,
            };

            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions = new Point3DCollection(vertices);
            mesh.TriangleIndices = new Int32Collection(indices);

            _model = new GeometryModel3D(mesh, Material);
            Content = _model;
        }

 
    }
}
