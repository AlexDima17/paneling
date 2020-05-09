using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using System.Drawing;

namespace MorphoProject
{
    class HClusterGroup
    {
        public int N;
        //quadPanel[] inputs;
        public List<HCluster> hClusters;

        public HClusterGroup(quadPanel[] inputs)
        {
            //initially set as many clusters as the inputs
            N = inputs.Length;
            //this.inputs = inputs;
            this.hClusters = new List<HCluster>();

            for (int i = 0; i < inputs.Length; i++)
            {
                hClusters.Add(new HCluster(inputs[i]));
            }
        }


        public void UpdateClusters()
        {
            List<double> minDistances = new List<double>();
            Dictionary<double, HCluster[]> dissimilarities = new Dictionary<double, HCluster[]>();

            for (int i = 0; i < hClusters.Count; i++)
            {
                hClusters[i].GetMean();
            }

            int winnerA = -1;
            int winnerB = -1;
            double minDistance = double.MaxValue;
            for (int i = 0; i < hClusters.Count-1; i++)
            {
                for (int j = i+1; j < hClusters.Count; j++)
                {
                    double d = distance(hClusters[i], hClusters[j]);
                    if (d < minDistance)
                    {
                        minDistance = d;
                        winnerA = i;
                        winnerB = j;
                        
                    }
                }

            }

            hClusters[winnerA].GetNewMean(hClusters[winnerB]);
            hClusters[winnerA].AddInputs(hClusters[winnerB]);
            hClusters.Remove(hClusters[winnerB]);     
            N--;
        }



        public static double distance(HCluster clusterA, HCluster clusterB)
        {

            double d = Math.Abs(clusterA.mean - clusterB.mean);

            //for (int i = 0; i < clusterA.clusterWeights.Length; i++)
            //{
            //    d += Math.Pow(clusterA.mean - clusterB.mean, 2);
            //}

            //return Math.Sqrt(d);

            double dist= Math.Pow(clusterA.meanVector.X - clusterB.meanVector.X, 2)
            + Math.Pow(clusterA.meanVector.Y - clusterB.meanVector.Y, 2)
            + Math.Pow(clusterA.meanVector.Z - clusterB.meanVector.Z, 2);

            return (Math.Sqrt(dist));

            return d;
        }
    }
}
