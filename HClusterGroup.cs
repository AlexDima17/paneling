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
        quadPanel[] inputs;
        public List<HCluster> hClusters;

        public HClusterGroup(quadPanel[] inputs)
        {
            //initially set as many clusters as the inputs
            N = inputs.Length;
            this.inputs = inputs;
            this.hClusters = new List<HCluster>();

            for (int i = 0; i < inputs.Length; i++)
            {
                hClusters.Add(new HCluster(inputs[i]));
            }
        }


        public void UpdateClusters()
        {
            List<HCluster> toPass = new List<HCluster>();
            List<double> minDistances = new List<double>();
            Dictionary<double, HCluster[]> dissimilarities = new Dictionary<double, HCluster[]>();

            for (int i = 0; i < hClusters.Count; i++)
            {
                hClusters[i].GetMean();
                toPass.Add(hClusters[i]);
            }

            for (int i = 0; i < hClusters.Count; i++)
            {
                int win = -1;
                double minD = double.MaxValue;

                for (int j = i + 1; j < hClusters.Count; j++)
                {
                    //find distance between 2 cluster means
                    //that I havent already checked
                    double d = distance(hClusters[i], hClusters[j]);
                    if (d < minD)
                    {
                        minD = d;
                        win = j;
                    }
                }

                var pair = new HCluster[2] { hClusters[i], hClusters[win] };
                dissimilarities.Add(minD, pair);
                minDistances.Add(minD);
                hClusters[i].subClusters.Add(toPass[win]);
                toPass.RemoveAt(win);
            }

            double minDist = double.MaxValue;
            var winPair = new HCluster[2];

           
                foreach (KeyValuePair<double, HCluster[]>  keyValuePair in dissimilarities)
                {
                    if(keyValuePair.Key<minDist)
                    {
                        winPair = keyValuePair.Value;
                    }
                }               
           

            winPair[0].subClusters.Add(winPair[1]);
            N--;
        }



        public static double distance(HCluster clusterA, HCluster clusterB)
        {

            double d = 0.0;

            for (int i = 0; i < clusterA.clusterWeights.Length; i++)
            {
                d += Math.Pow(clusterA.clusterWeights[i] - clusterB.clusterWeights[i], 2);
            }

            return Math.Sqrt(d);
        }
    }
}
