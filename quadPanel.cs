using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;


namespace MorphoProject
{
    class quadPanel
    {
        Point3d[] pts;
        public Mesh mesh;
        public double[] weights; //weights will be curvature, the dimensions ratio and the diagonal of the panel


        //constructors can take points or meshes as input
        public quadPanel(Point3d[] pts)
        {
            this.pts = pts;
            mesh = CreatePanel();
            weights = new double[3];
            ComputeWeights();
        }

        public quadPanel(Mesh mesh)
        {
            this.mesh = mesh;
            weights = new double[3];
            ComputeWeights();
        }

        private Mesh CreatePanel()
        {
            mesh = new Mesh();

            for (int i = 0; i < pts.Length; i++)
            {
                mesh.Vertices.Add(pts[i]);
            }

            //  1_____2
            //  |     |
            //  |     |
            //  |     |
            //  0_____3

            mesh.Faces.AddFace(0, 1, 2, 3);
            return mesh;
        }

        public void ComputeWeights()
        {

            //represent the panel edges with vectors
            Vector3d vec1 = Vector3d.Subtract(new Vector3d(mesh.Vertices[0].X, mesh.Vertices[0].Y, mesh.Vertices[0].Z),
                new Vector3d(mesh.Vertices[1].X, mesh.Vertices[1].Y, mesh.Vertices[1].Z));

            Vector3d vec2 = Vector3d.Subtract(new Vector3d(mesh.Vertices[1].X, mesh.Vertices[1].Y, mesh.Vertices[1].Z),
                new Vector3d(mesh.Vertices[2].X, mesh.Vertices[2].Y, mesh.Vertices[2].Z));

            Vector3d vec3 = Vector3d.Subtract(new Vector3d(mesh.Vertices[2].X, mesh.Vertices[2].Y, mesh.Vertices[2].Z),
            new Vector3d(mesh.Vertices[3].X, mesh.Vertices[3].Y, mesh.Vertices[3].Z));

            Vector3d vec4 = Vector3d.Subtract(new Vector3d(mesh.Vertices[0].X, mesh.Vertices[0].Y, mesh.Vertices[0].Z),
            new Vector3d(mesh.Vertices[3].X, mesh.Vertices[3].Y, mesh.Vertices[3].Z));

            double angle = Math.Abs(Vector3d.VectorAngle(vec1, vec2)) + Math.Abs(Vector3d.VectorAngle(vec2, vec3))
                + Math.Abs(Vector3d.VectorAngle(vec3, vec4)) + Math.Abs(Vector3d.VectorAngle(vec4, vec1));

            //set weights
            weights[0] = Math.Abs((angle - 2 * Math.PI) * 360 / (2 * Math.PI));    //gaussian curvature          
            weights[1] = vec1.Length/ vec2.Length;                                 //dimension 1/dimension 2           
            weights[2] = Vector3d.Subtract(vec1, vec2).Length;                     //one of the diagonals

        }
    }
}
