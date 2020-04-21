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
        hexPanel[] inputs;

        public ClusterGroup(int K, hexPanel[] inputs)
        {
            this.K = K;
            var rnd = new Random();

            this.centroids = new Cluster[K];
            for (int i = 0; i < centroids.Length; i++)
            {
                Point3d pt = new Point3d(1.0 * rnd.NextDouble(), 1.0 * rnd.NextDouble(), 0);
                centroids[i] = new Cluster(pt);
            }

            this.inputs = inputs;
        }

        public List<Point3d> DrawClusterPts()
        {
            List<Point3d> pts = new List<Point3d>();
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

        public static double Distance(Cluster cluster, hexPanel input)
        {
            double d = Math.Pow(cluster.curvatureVec.X - input.weight, 2);
              //+ Math.Pow(cluster.curvatureVec.Y - input.color.G, 2)
              //+ Math.Pow(cluster.curvatureVec.Z - input.color.B, 2);

            return Math.Sqrt(d);
        }

    }

}
  



