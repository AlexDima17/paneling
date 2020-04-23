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
        public quadPanel centroid;
        public List<quadPanel> assignedInputs;
        public List<Point3d> assignedPts;
        public Vector3d centroidVec;

        public Cluster(quadPanel centroid)
        {
            this.centroid = centroid;
            this.centroidVec = new Vector3d(centroid.weight,0,0);
            this.assignedInputs = new List<quadPanel>();
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
            centroidVec = new Vector3d(value,0,0);
        }

    }
}
