using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using MorphoProject.Properties;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using System.Windows.Forms;



namespace MorphoProject
{

    //pick k random panels from the input list
    //these panels will be the "centroids"
    public class KClusterComponent : GH_Component
    {
        Timer timer;
        int counter;
        int maxCounter;
        int interval;
        bool reset, run;
        List<Mesh> meshPanels = new List<Mesh>();
        int k = 1;

        quadPanel[] qPans; //the inputs
        ClusterGroup clusterGroup;
        int iter = 0;

        public KClusterComponent()
          : base("ClusterPanelsK", "KCluster", "Group input panels", "PanelizationTools", "PanelizationTools")
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
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!DA.GetData(0, ref run)) return;
            if (!DA.GetData(1, ref reset)) return;
            if (!DA.GetData(2, ref maxCounter)) return;

            if (timer == null)
            {
                timer = new Timer();
                timer.Interval = 1000;
                timer.Tick += UpdateSolution;
            }

            if (reset) Reset();

            if (run && !timer.Enabled)
            {
                Start();
            }
            else if (!run || timer.Enabled && maxCounter != 0 && counter >= maxCounter)
            {
                Stop();
            }


          
            if (!DA.GetDataList(3, meshPanels)) return;
            if (!DA.GetData(4, ref k)) return;
           
            // if (!DA.GetData(2, ref meshPoints)) return;


           // DA.SetDataList(0,clusterGroup.centroids[0].assignedPts);
            DA.SetData(0, counter);
        }


        public void Start()
        {
            timer.Start();
        }
        public void Stop()
        {
            timer.Stop();
        }
        public void Reset()
        {
            counter = 0;
        }
        public void Update()
        {
            // DoSomethingEpic 

            //qPans = new quadPanel[meshPanels.Count];

            //for (int i = 0; i < meshPanels.Count; i++)
            //{
            //    quadPanel qPan = new quadPanel(meshPanels[i]);
            //    qPans[i] = qPan;
            //}

            //clusterGroup = new ClusterGroup(k, qPans);

            //clusterGroup.UpdateClusters();

            counter++;
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
                return null;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("82785197-afcc-4cca-9cd7-3c9ac4bb1059"); }
        }
    }
}
