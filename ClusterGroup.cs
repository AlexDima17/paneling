using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace MorphoProject
{
    class ClusterGroup
    {
        public int K;
        public Cluster[] centroids;
        quadPanel[] inputs;

        public ClusterGroup(int K, quadPanel[] inputs)
        {
            this.K = K;
            var rnd = new Random();

            this.centroids = new Cluster[K];
            for (int i = 0; i < centroids.Length; i++)
            {
                Point3d pt = new Point3d(1.0 * rnd.NextDouble(), 1.0 * rnd.NextDouble(), 0);

                int randomIndex = rnd.Next(centroids.Length);
                centroids[i] = new Cluster(inputs[randomIndex]);
            }

            this.inputs = inputs;
        }

        public List<quadPanel> DrawClusters()
        {
            List<quadPanel> pts = new List<quadPanel>();
            for (int i = 0; i < centroids.Length; i++)
            {
                pts.Add(centroids[i].centroid);
            }
            return pts;
        }

        public void AssignClusters()
        {
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
               // centroids[winner].assignedPts.Add(inputs[i].pt);
            }
        }

        public void UpdateClusters()
        {
            for (int i = 0; i < centroids.Length; i++)
            {
                centroids[i].MeanVector();
                AssignClusters();
            }

        }

        public static double Distance(Cluster cluster, quadPanel input)
        {
            double d = Math.Pow(cluster.centroidVec.X - input.weight, 2);
              //+ Math.Pow(cluster.curvatureVec.Y - input.color.G, 2)
              //+ Math.Pow(cluster.curvatureVec.Z - input.color.B, 2);

            return Math.Sqrt(d);
        }

    }

}
  



