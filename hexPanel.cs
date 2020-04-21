using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace MorphoProject
{
    class hexPanel
    {
        Point3d[] pts;
        Mesh mesh;
        public double weight; //the weight will be the curvature(?)

        public hexPanel(Point3d[] pts)
        {
            this.pts = pts;
            this.mesh = CreatePanel();
        }

        private Mesh CreatePanel()
        {
            Mesh mesh = new Mesh();

            for (int i = 0; i < pts.Length; i++)
            {
                mesh.Vertices.Add(pts[i]);
            }

            mesh.Faces.AddFace(0, 1, 6);
            mesh.Faces.AddFace(1, 2, 6);
            mesh.Faces.AddFace(2, 3, 6);
            mesh.Faces.AddFace(3, 4, 6);
            mesh.Faces.AddFace(4, 5, 6);
            mesh.Faces.AddFace(5, 0, 6);

            return mesh;
        }
    }
}
