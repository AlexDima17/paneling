using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace MorphoProject
{
    class Cluster
    {
        public Point3d centroid;
        public List<hexPanel> assignedInputs;
        public List<Point3d> assignedPts;
        public Vector3d curvatureVec;

        public Cluster(Point3d pt)
        {
            this.centroid = pt;
            this.curvatureVec = new Vector3d(new Random().NextDouble(),0,0);
            this.assignedInputs = new List<hexPanel>();
            this.assignedPts = new List<Point3d>();
        }

        public void MeanVector()
        {
            double value = 0.0;            

            for (int i = 0; i < assignedInputs.Count; i++)
            {
                value += assignedInputs[i].weight;
              
            }

            value /= assignedInputs.Count;
            curvatureVec = new Vector3d(value,0,0);
        }

    }
}
