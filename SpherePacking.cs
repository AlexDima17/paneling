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
        public Polyline path;
        public List<PolylineCurve> sphIntersCurrent;
        public List<PolylineCurve> sphIntersNext;
        public List<List<PolylineCurve>> intersectionsList;
        public List<List<Point3d>> centers;
        public SpherePacking(double radius, Mesh mesh, double startHeight, int rep)
        {
            this.radius = radius;
            this.mesh = mesh;
            this.centers = new List<List<Point3d>>();
            this.intersectionsList = new List<List<PolylineCurve>>();
            path = InitialPath(startHeight);
            sphIntersCurrent = SphIntersectionsFirstRow();
            intersectionsList.Add(sphIntersCurrent);
            

            for (int i = 0; i < rep; i++)
            {
                sphIntersNext = SphIntersections(intersectionsList[intersectionsList.Count-1]);
                intersectionsList.Add(sphIntersNext);               
            }
        }

        private Polyline InitialPath(double startHeight)
        {
            Plane plane = new Plane(new Point3d(0, 0, startHeight), Vector3d.ZAxis); //I will use this plane to intersect it with the input mesh
            Polyline[] firstLine = Intersection.MeshPlane(mesh, plane);
            return firstLine[0];
        }

        private List<PolylineCurve> SphIntersections(List<PolylineCurve> previousRow)
        {
            //this method receives the previews row of sphere inteersections
            //finds intersection points between the curves and sets a next row of spheres
            List<PolylineCurve> sphIntersections = new List<PolylineCurve>();
            List<Point3d> subCenters = new List<Point3d>();

            for (int i = 0; i < previousRow.Count; i++)
            {
                int next = (i + previousRow.Count+1) % previousRow.Count;

                Curve pl1 = previousRow[i];
                Curve pl2 = previousRow[next];
                var curveIntersection = Intersection.CurveCurve(pl1, pl2, 0.0, 0.0);

                //THIS IS THE REASON IT CRASHES \/
                Point3d pA = curveIntersection[0].PointA;
                Point3d pB = curveIntersection[curveIntersection.Count-1].PointA;

                if(pA.Z>pB.Z)
                {
                    subCenters.Add(pA);
                }
                else
                {
                    subCenters.Add(pB);
                }

                Sphere sph = new Sphere(subCenters[subCenters.Count-1], radius);
                var sphM = Mesh.CreateFromSphere(sph, 10, 10);
                Polyline[] intersectionLines = Intersection.MeshMeshAccurate(mesh, sphM, 0.0);

                sphIntersections.Add(intersectionLines[0].ToPolylineCurve());
            }

            centers.Add(subCenters);
            return sphIntersections;

        }


        private List<PolylineCurve> SphIntersectionsFirstRow()
        {
            List<PolylineCurve> meshSphereInterPlns = new List<PolylineCurve>();
            List<Point3d> subCenters = new List<Point3d>();//intersections between spheres on a single row
            int numOfSpheres =(int) (path.Length/radius)+2; //how many spheres along the path
            Point3d center = path.PointAt(0);
            Point3d prevCenter = path.PointAt(0);
            int iter = 0;
          
            for (int i = 0; i < numOfSpheres; i++)
            {
                subCenters.Add(center);
                Sphere sph = new Sphere(center, radius);
                var sphM = Mesh.CreateFromSphere(sph, 15, 15); //sphere mesh I will use for intersections
               
                //intersection points of sphere with path that will be be the next center
                Point3d[] sphOnPathPts = Intersection.MeshPolyline(sphM, path.ToPolylineCurve(), out int[] fIDs);
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
                meshSphereInterPlns.Add(intersectionLines[0].ToPolylineCurve());

            }

            centers.Add(subCenters);
            return meshSphereInterPlns;
        }

    }
}
