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
        public quadPanel centroid;                //chosen initial panel
        public List<quadPanel> assignedInputs;    //panels assigned to cluster
        public List<Mesh> assignedMeshes;         //meshes of the panels for display
        public double[] centroidWeights;          //array of cluster's weights
        public Vector3d weightVector;             //mean vector of cluster


        public KCluster(quadPanel centroid)
        {
            this.centroid = centroid;
            assignedInputs = new List<quadPanel>();
            assignedMeshes = new List<Mesh>();
            centroidWeights = centroid.weights;
            weightVector = new Vector3d(centroid.weights[0], centroid.weights[1], centroid.weights[2]);
        }

        public void MeanVector()
        {
            //set all the weights of cluster to 0
            for (int i = 0; i < centroidWeights.Length; i++)
            {
                centroidWeights[i] = 0.0;
            }

            //find the mean values of the assigned inputs weights for each weight
            //and assign them to the cluster centroid vector
            for (int i = 0; i < assignedInputs.Count; i++)
            {
                for (int j = 0; j < centroidWeights.Length; j++)
                {
                    centroidWeights[j] += assignedInputs[i].weights[j];
                }
            }
            for (int i = 0; i < centroidWeights.Length; i++)
            {
                centroidWeights[i] /= assignedInputs.Count;
            }

            weightVector = new Vector3d(centroidWeights[0], centroidWeights[1], centroidWeights[2]);
        }

    }
}
