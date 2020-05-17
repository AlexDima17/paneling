using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using System.Drawing;

namespace MorphoProject
{
    class HCluster
    {
        public quadPanel initQuad;                 //first panel of cluster
        public List<quadPanel> assignedInputs;     //input panels that will be clustered
        public List<Mesh> assignedMeshes;          //meshes of panels for display
        public Vector3d meanVector;                //centroid vector of cluster
        public double[] clusterWeights;            //array of 3 weights

        public HCluster(quadPanel qP)
        {            
            initQuad = qP;         
            assignedInputs = new List<quadPanel>() { initQuad };
            assignedMeshes = new List<Mesh>() { initQuad.mesh };
            clusterWeights = initQuad.weights;
            meanVector = new Vector3d(0, 0, 0);
        }

        //method used when adding inputs of one cluster to another, to merge them
        public void AddInputs(HCluster clusterToMerge)            
        {
            assignedInputs.AddRange(clusterToMerge.assignedInputs);
            assignedMeshes.AddRange(clusterToMerge.assignedMeshes);
        }

        public void GetMean()
        {
            //find mean vector from the inputs values
            meanVector = new Vector3d(0, 0, 0);

            for (int i = 0; i < assignedInputs.Count; i++)
            {
                for (int j = 0; j < assignedInputs[i].weights.Length; j++)
                {
                    meanVector.X += assignedInputs[i].weights[0];
                    meanVector.Y += assignedInputs[i].weights[1];
                    meanVector.Z += assignedInputs[i].weights[2];
                }
               
            }

            meanVector.X /= assignedInputs.Count;
            meanVector.Y /= assignedInputs.Count;
            meanVector.Z /= assignedInputs.Count;
        }

        //when clusters are merged, find mean vector of the two before "destoying" the previous cluster
        //no need to run through all inputs again
        internal void UpdateMean(HCluster hCluster)
        {
            meanVector = new Vector3d((meanVector.X + hCluster.meanVector.X) / 2,
                (meanVector.Y + hCluster.meanVector.Y) / 2,
                (meanVector.Z + hCluster.meanVector.Z) / 2);
        }
    }
}
