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
        public int N;                           //target cluster number
        public List<HCluster> hClusters;        //clusters

        public HClusterGroup(quadPanel[] inputs)
        {
            //initially set as many clusters as the inputs
            N = inputs.Length;
            this.hClusters = new List<HCluster>();

            for (int i = 0; i < inputs.Length; i++)
            {
                hClusters.Add(new HCluster(inputs[i]));
            }

            for (int i = 0; i < hClusters.Count; i++)
            {
                hClusters[i].GetMean();
            }
        }


        public void UpdateClusters()
        {    
            //check all clusters in pairs, to find the pair with the minimum distance between them
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

            //move the inputs of one cluster to the other and update the cluster's mean vector
            hClusters[winnerA].UpdateMean(hClusters[winnerB]);
            hClusters[winnerA].AddInputs(hClusters[winnerB]);
            hClusters.Remove(hClusters[winnerB]);  
            
            //the number of clusters is reduced by 1
            N--;
        }



        public static double distance(HCluster clusterA, HCluster clusterB)
        {
            double dist = Math.Pow(clusterA.meanVector.X - clusterB.meanVector.X, 2)
            + Math.Pow(clusterA.meanVector.Y - clusterB.meanVector.Y, 2)
            + Math.Pow(clusterA.meanVector.Z - clusterB.meanVector.Z, 2);

            return (Math.Sqrt(dist));

        }
    }
}
