using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry.Intersect;

namespace MorphoProject
{
    class SpherePacking
    {
        public double radius;
        public Mesh mesh;
        public Polyline intersectionLine;
        public List<Polyline> sphInters;
        public SpherePacking(double radius, Mesh mesh, double startHeight, int num)
        {
            this.radius = radius;
            this.mesh = mesh;

            intersectionLine = FirstIntersection(radius, startHeight, mesh);
            sphInters = SphIntersections(radius, mesh, intersectionLine, num);
        }

        private Polyline FirstIntersection(double radius, double startHeight, Mesh inputMesh)
        {
            Plane plane = new Plane(new Point3d(0, 0, startHeight), Vector3d.ZAxis); //I will use this plane to intersect it with the input mesh
            Polyline[] firstLine = Intersection.MeshPlane(inputMesh, plane);

            return firstLine[0];
        }

        private List<Polyline> SphIntersections(double radius, Mesh mesh, Polyline pathPoly, int num)
        {
            List<Polyline> plns = new List<Polyline>();
            int numOfSpheres =(int) (pathPoly.Length/radius)+2; //how many spheres along the path
            Point3d center = pathPoly.PointAt(0);
            Point3d prevCenter = pathPoly.PointAt(0);
            int iter = 0;

            for (int i = 0; i < numOfSpheres; i++)
            {
                Sphere sph = new Sphere(center, radius);
                var sphM = Mesh.CreateFromSphere(sph, 15, 15); //sphere mesh I will use for intersections
               
                //intersection points of sphere with path that will be be the next center
                Point3d[] sphOnPathPts = Intersection.MeshPolyline(sphM, pathPoly.ToPolylineCurve(), out int[] fIDs);
                var vec1 = Vector3d.Subtract((Vector3d)center, (Vector3d)prevCenter);
                var vec2 = Vector3d.Subtract((Vector3d)sphOnPathPts[0], (Vector3d)center);
                var vec3 = Vector3d.Subtract((Vector3d)sphOnPathPts[1], (Vector3d)center);

                prevCenter = center;
                if (Vector3d.Multiply(vec1, vec2) > Vector3d.Multiply(vec1, vec3))
                {
                    center = sphOnPathPts[0];
                }
                else
                {
                    center = sphOnPathPts[1];
                }

                Polyline[] intersectionLines = Intersection.MeshMeshAccurate(mesh, sphM, 0.0);
                iter++;
                plns.Add(intersectionLines[0]);

            }

            return plns;
        }

    }
}
