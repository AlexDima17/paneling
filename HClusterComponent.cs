using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using MorphoProject.Properties;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using System.Windows.Forms;
using Grasshopper.Kernel.Data;

namespace MorphoProject
{

    //pick k random panels from the input list
    //these panels will be the "centroids"
    public class HClusterComponent : GH_Component
    {
        Timer timer;
        int counter;
        int maxCounter;
        bool reset, run;
        List<Mesh> meshPanels = new List<Mesh>();
        int k = 1;
        int played = 0;

        quadPanel[] qPans; //the inputs
        HClusterGroup clusterGroup;
        Grasshopper.DataTree<Mesh> meshTree = new Grasshopper.DataTree<Mesh>();
        Grasshopper.DataTree<int> myTree = new Grasshopper.DataTree<int>();
        List<double> curv=new List<double>();
        int iter = 0;

        public HClusterComponent()
          : base("ClusterPanelsHierachical", "HCluster", "Group input panels", "PanelizationTools", "PanelizationTools")
        {
        }


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Run", "Run", "", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Reset", "Rst", "", GH_ParamAccess.item, false);
            pManager.AddIntegerParameter("Max", "Max", "", GH_ParamAccess.item, 0);
            pManager.AddMeshParameter("meshPanels", "mP", "list of panels", GH_ParamAccess.list);
            pManager.AddIntegerParameter("clusterNum", "k", "number of panel families", GH_ParamAccess.item);

        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //  pManager.AddCurveParameter("Line", "ln", "intersection curve", GH_ParamAccess.item);
            pManager.AddIntegerParameter("count", "c", "intersection curves", GH_ParamAccess.item);
            pManager.AddMeshParameter("meshes", "mP", "mesh panels", GH_ParamAccess.tree);
          
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!DA.GetData(0, ref run)) return;
            if (!DA.GetData(1, ref reset)) return;
            if (!DA.GetData(2, ref maxCounter)) return;
            if (!DA.GetDataList(3, meshPanels)) return;
            if (!DA.GetData(4, ref k)) return;
            

            if (timer == null)
            {
                timer = new Timer();
                timer.Interval = 1000;
                timer.Tick += UpdateSolution;
            }

            if (reset)
            {
                meshPanels.Clear();
                if (!DA.GetDataList(3, meshPanels)) return;
                Reset();
            }
            if (run && !timer.Enabled)
            {  
                Start();
            }
            else if (!run || timer.Enabled && maxCounter != 0 && counter >= maxCounter)
            {
                Stop();
            }


            DA.SetData(0, played);
            DA.SetDataTree(1, meshTree);           
        }


        public void Start()
        {
            played = -100;
            qPans = new quadPanel[meshPanels.Count];
            curv.Clear();

            for (int i = 0; i < meshPanels.Count; i++)
            {
                quadPanel qPan = new quadPanel(meshPanels[i]);
                qPans[i] = qPan;
                curv.Add(qPan.weights[0]);
            }

            clusterGroup = new HClusterGroup(qPans);
            timer.Start();
        }
        public void Stop()
        {
            timer.Stop();
        }
        public void Reset()
        {            
            played = -100;
            counter = 0;

            qPans = new quadPanel[meshPanels.Count];
            curv = new List<double>();

            for (int i = 0; i < meshPanels.Count; i++)
            {
                quadPanel qPan = new quadPanel(meshPanels[i]);
                qPans[i] = qPan;

                curv.Add(qPan.weights[0]);

            }

            clusterGroup = new HClusterGroup(qPans);


        }
        public void Update()
        {   
            clusterGroup.UpdateClusters();
            played++;
            counter++;

            if (counter == maxCounter || clusterGroup.N==k)
            {
                //////////////////////
                myTree.Clear();
                for (int i = 0; i < k; i++)
                {
                    GH_Path pth = new GH_Path(i);

                    for (int j = 0; j < 10; j++)
                    {
                        myTree.Add(j, pth);
                    }
                }
                ///////////////////
                ///

                //\/\/\/\/\/\/
                meshTree = new Grasshopper.DataTree<Mesh>();
                for (int i = 0; i < clusterGroup.hClusters.Count; i++)
                {
                    GH_Path pth = new GH_Path(i);

                    meshTree.AddRange(clusterGroup.hClusters[i].assignedMeshes, pth);

                }

            }
        }

        public void UpdateSolution(object source, EventArgs e)
        {
            Update();
            ExpireSolution(true);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                
                return Resources.Image1;               
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("82785197-afcc-4cca-9cd7-3c9ac4bb1061"); }
        }
    }
}
