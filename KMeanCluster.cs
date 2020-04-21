using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using MorphoProject.Properties;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;


namespace MorphoProject
{
    public class KMeanCluster : GH_Component
    {
       
        public KMeanCluster()
          : base("RationalizeMesh", "MRational",
              "Construct an Archimedean, or arithmetic, spiral given its radii and number of turns.",
              "PanelizationTools", "PanelizationTools")
        {
        }

     
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("meshPanels", "mP", "list of panels", GH_ParamAccess.list);
            pManager.AddIntegerParameter("clusterNum", "k", "number of panel families", GH_ParamAccess.item);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Line", "ln", "intersection curve", GH_ParamAccess.item);
            pManager.AddCurveParameter("intersections", "inters", "intersection curves", GH_ParamAccess.list);
            pManager.AddCurveParameter("intersectionsNext", "interNext", "intersection curves", GH_ParamAccess.list);
            pManager.AddPointParameter("intersectionspt", "intPt", "intersection curves", GH_ParamAccess.list);
            pManager.AddPointParameter("intersectionspt", "intPt", "intersection curves", GH_ParamAccess.list);
            pManager.AddPointParameter("intersectionspt", "intPt", "intersection curves", GH_ParamAccess.list);

        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Mesh> meshPanels= new List<Mesh>();
            int k=1;
          
            if (!DA.GetData(0, ref meshPanels)) return;
            if (!DA.GetData(1, ref k)) return;
    
            //DA.SetDataList(3, pts[0]);
            //DA.SetDataList(4, pts[1]);
            //DA.SetDataList(5, pts[2]);


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
