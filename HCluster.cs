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
        public quadPanel initQuad;
        public List<HCluster> subClusters;
        public List<quadPanel> assignedInputs;
        public List<Mesh> assignedMeshes;

        public double mean;
        public double[] clusterWeights;

        public HCluster(quadPanel qP)
        {            
            initQuad = qP;         
            assignedInputs = new List<quadPanel>() { initQuad };
            assignedMeshes = new List<Mesh>() { initQuad.mesh };
            clusterWeights = initQuad.weights;
          //  GetMean();

            subClusters = new List<HCluster>
            {
                this
            };
        }

        public void GetMean()
        {
            double value = 0.0;

            for (int i = 0; i < assignedInputs.Count; i++)
            {
                for (int j = 0; j < assignedInputs[i].weights.Length; j++)
                {
                    value += assignedInputs[i].weights[j];
                }

                value /= assignedInputs[i].weights.Length;
            }

            mean = value / assignedInputs.Count;
        }

        //public void GetMean()
        //{
        //    double sum=0.0;

        //    for (int i = 0; i < subClusters.Count; i++)
        //    {
        //        for (int j = 0; j < subClusters[i].clusterWeights.Length; j++)
        //        {
        //            sum += subClusters[i].clusterWeights[j];
        //        }
        //        sum /= subClusters[i].clusterWeights.Length;
        //    }

        //    mean= sum / subClusters.Count;
        //}

    }
}
