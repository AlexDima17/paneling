using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace MorphoProject
{
    class KClusterGroup
    {
        public int K;                        //number of clusters
        public KCluster[] centroids;         //clusters
        quadPanel[] inputs;                  //input panels
      
        public KClusterGroup(int K, quadPanel[] inputs)
        {
            this.K = K;
            Random rnd = new Random();

            centroids = new KCluster[K];
           
            for (int i = 0; i < centroids.Length; i++)
            {
                int randomIndex = rnd.Next(centroids.Length);
                centroids[i] = new KCluster(inputs[randomIndex]);
            }
            this.inputs = inputs;

            //assign inputs to clusters for first time
            AssignClusters();
        }

      
        public void AssignClusters()
        {
            //clear the assigned inputs lists
            for (int i = 0; i < centroids.Length; i++)
            {
                centroids[i].assignedInputs = new List<quadPanel>();
                centroids[i].assignedMeshes = new List<Mesh>();
            }

            //find the best matching centroid for each input to be assigned to
            for (int i = 0; i < inputs.Length; i++)
            {
                double minDist = double.MaxValue;
                int winner = -1;

                for (int j = 0; j < centroids.Length; j++)
                {
                    double d = Distance(centroids[j], inputs[i]);

                    if (d < minDist)
                    {
                        minDist = d;
                        winner = j;
                    }
                }

                centroids[winner].assignedInputs.Add(inputs[i]);
                centroids[winner].assignedMeshes.Add(inputs[i].mesh);
            }
        }

        public void UpdateClusters()
        {
            for (int i = 0; i < centroids.Length; i++)
            {
                //update the weights of the centroid and re-assign
                centroids[i].MeanVector(); 
                AssignClusters();
            }

        }

        public double Distance(KCluster cluster, quadPanel input)
        {
            //euclidean distance between centroid weights and input weights
            double d = Math.Pow(cluster.weightVector.X- input.weights[0], 2)
            +Math.Pow(cluster.weightVector.Y - input.weights[1], 2)
            + Math.Pow(cluster.weightVector.Z - input.weights[2], 2);

            return Math.Sqrt(d);
        }

    }

}




