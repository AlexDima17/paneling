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
        public Vector3d colorVec;
        public List<HCluster> subClusters;
        public double mean;
        public double[] clusterWeights;

        public HCluster(quadPanel qP)
        {
            initQuad = qP;
            subClusters = new List<HCluster>
            {
                this
            };
            clusterWeights = initQuad.weights;
            GetMean();
        }

        public void GetMean()
        {
            double sum=0.0;

            for (int i = 0; i < subClusters.Count; i++)
            {
                for (int j = 0; j < subClusters[i].clusterWeights.Length; j++)
                {
                    sum += subClusters[i].clusterWeights[j];
                }
                sum /= subClusters[i].clusterWeights.Length;
            }

            mean= sum / subClusters.Count;
        }

    }
}
