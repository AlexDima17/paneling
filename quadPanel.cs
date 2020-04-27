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
        public double weight; //the weight will be the curvature(?)
        public double curvature;
        public quadPanel(Point3d[] pts)
        {
            this.pts = pts;
            mesh = CreatePanel();            
            weight = ComputeCurvature(); 
          //  curvature = 2;
        }

        public quadPanel(Mesh mesh)
        {
            this. mesh = mesh;
            weight = ComputeCurvature();
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

            mesh.Faces.AddFace(0, 1,2, 3);

            return mesh;
        }

        public double ComputeCurvature()
        {
            double angle = 0.0;
            Vector3d centerVec =new Vector3d(mesh.Vertices[4].X, mesh.Vertices[4].Y, mesh.Vertices[4].Z);
           
            for (int i = 0; i < mesh.Vertices.Count-1; i++)
            {
                int nxt = (mesh.Vertices.Count + i + 1) % mesh.Vertices.Count;
                Vector3d vec1 = Vector3d.Subtract(new Vector3d(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z), centerVec);
                Vector3d vec2 = Vector3d.Subtract(new Vector3d(mesh.Vertices[nxt].X, mesh.Vertices[nxt].Y, mesh.Vertices[nxt].Z), centerVec);
                angle += Math.Abs(Vector3d.VectorAngle(vec1, vec2));
            }
            curvature = Math.Abs((angle - 2 * Math.PI) * 360 / (2 * Math.PI));
            return curvature;
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
    }
}
