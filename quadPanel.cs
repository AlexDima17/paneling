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
        public double[] weights; //weights will be curvature, the dimensions and the diagonal of the panel
   
        public quadPanel(Point3d[] pts)
        {
            this.pts = pts;
            mesh = CreatePanel();
            ComputeWeights();
            //  curvature = 2;
        }

        public quadPanel(Mesh mesh)
        {
            this.mesh = mesh;
            weights = new double[4];
            ComputeWeights();
            //   curvature = 2;
        }

        private Mesh CreatePanel()
        {
            mesh = new Mesh();

            for (int i = 0; i < pts.Length; i++)
            {
                mesh.Vertices.Add(pts[i]);
            }

            //  1_____2
            //  | \ / |
            //  |  4  |
            //  | / \ |
            //  0_____3

            //mesh.Faces.AddFace(0, 1, 4);
            //mesh.Faces.AddFace(1, 2, 4);
            //mesh.Faces.AddFace(2, 3, 4);
            //mesh.Faces.AddFace(3, 0, 4);

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
            weights[0] = Math.Abs((angle - 2 * Math.PI) * 360 / (2 * Math.PI)); //gaussian curvature
            weights[1] = vec1.Length; //dimension 1
            weights[2] = vec2.Length; //dimension 2
            weights[3] = Vector3d.Subtract(vec1, vec2).Length; //one of the diagonals
            
        }


        //private Mesh CreatePanel() FOR HEX
        //{
        //    Mesh mesh = new Mesh();

        //    for (int i = 0; i < pts.Length; i++)
        //    {
        //        mesh.Vertices.Add(pts[i]);
        //    }

        //    mesh.Faces.AddFace(0, 1, 6);
        //    mesh.Faces.AddFace(1, 2, 6);
        //    mesh.Faces.AddFace(2, 3, 6);
        //    mesh.Faces.AddFace(3, 4, 6);
        //    mesh.Faces.AddFace(4, 5, 6);
        //    mesh.Faces.AddFace(5, 0, 6);

        //    return mesh;
        //}

        ///////////////////GAUSIAN CURVATURE WITH VERTEX IN THE MIDDLE OF PANEL////////////////////
        //double angle = 0.0;
        //Vector3d centerVec = new Vector3d(mesh.Vertices[4].X, mesh.Vertices[4].Y, mesh.Vertices[4].Z);

        //for (int i = 0; i < mesh.Vertices.Count - 1; i++)
        //{
        //    int nxt = (mesh.Vertices.Count + i + 1) % mesh.Vertices.Count;
        //    Vector3d vec1 = Vector3d.Subtract(new Vector3d(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z), centerVec);
        //    Vector3d vec2 = Vector3d.Subtract(new Vector3d(mesh.Vertices[nxt].X, mesh.Vertices[nxt].Y, mesh.Vertices[nxt].Z), centerVec);
        //    angle += Math.Abs(Vector3d.VectorAngle(vec1, vec2));
        //}
        //curvature = Math.Abs((angle - 2 * Math.PI) * 360 / (2 * Math.PI));
        //return curvature;
    }
}
