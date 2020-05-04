using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace MorphoProject
{
    class KCluster
    {
        public quadPanel centroid;
        public List<quadPanel> assignedInputs;
        public List<Point3d> assignedPts;
        public List<Mesh> assignedMeshes;
        public Vector3d centroidVec;
        public double[] centroidWeights;

        public KCluster(quadPanel centroid)
        {
            this.centroid = centroid;
            centroidVec = new Vector3d(centroid.weights[0], 0, 0);
            assignedInputs = new List<quadPanel>();
            assignedPts = new List<Point3d>();
            assignedMeshes = new List<Mesh>();
            centroidWeights = centroid.weights;
        }

        public void MeanVector()
        {
            double value = 0.0; //i am not using this value further on

            //set all the weights of cluster to 0
            for (int i = 0; i < centroidWeights.Length; i++)
            {
                centroidWeights[i] = 0.0;
            }

            //find the mean values of the assigned inputs weights for each weight
            //and assign them to the cluster centroid
            for (int i = 0; i < assignedInputs.Count; i++)
            {
                value += assignedInputs[i].weights[0];
                for (int j = 0; j < centroidWeights.Length; j++)
                {
                    centroidWeights[j] += assignedInputs[i].weights[j];
                }
            }
            for (int i = 0; i < centroidWeights.Length; i++)
            {
                centroidWeights[i] /= assignedInputs.Count;
            }

            value /= assignedInputs.Count;
            centroidVec = new Vector3d(value, 0, 0);
        }

    }
}
