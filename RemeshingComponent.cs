using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using MorphoProject.Properties;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace MorphoProject
{
    public class MeshRationalizer : GH_Component
    {
       
        public MeshRationalizer()
          : base("RationalizeMesh", "MRational",
              "Construct an Archimedean, or arithmetic, spiral given its radii and number of turns.",
              "PanelizationTools", "PanelizationTools")
        {
        }

     
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("radius", "r", "sphere radius", GH_ParamAccess.item);
            pManager.AddNumberParameter("startHeight", "h", "height to start division from", GH_ParamAccess.item);           
            pManager.AddMeshParameter("inputMesh", "m", "input a mesh", GH_ParamAccess.item);
            pManager.AddIntegerParameter("reps", "n", "repetitions", GH_ParamAccess.item);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Line", "ln", "intersection curve", GH_ParamAccess.item);
            pManager.AddCurveParameter("intersections", "inters", "intersection curves", GH_ParamAccess.list);
            pManager.AddCurveParameter("intersectionsNext", "interNext", "intersection curves", GH_ParamAccess.list);
            //pManager.AddPointParameter("intersectionspt", "intPt", "intersection curves", GH_ParamAccess.list);
            //pManager.AddPointParameter("intersectionspt", "intPt", "intersection curves", GH_ParamAccess.list);
            pManager.AddMeshParameter("mesh panels", "mPan", "mesh panels", GH_ParamAccess.list);
            pManager.AddNumberParameter("curvatures", "curv", "panel curvature", GH_ParamAccess.list);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double radius = 0.5;
            double startHeight = radius;
            Mesh inputMesh = new Mesh();
            Polyline intersectionLine;
            List<PolylineCurve> sphInters;
            List<PolylineCurve> allInter=new List<PolylineCurve>();
            int reps=1;
            List<Mesh> qPanels = new List<Mesh>();
            List<double> curvatures = new List<double>();
          
            if (!DA.GetData(0, ref radius)) radius=0.2;
            if (!DA.GetData(1, ref startHeight)) startHeight=0.2;
            if (!DA.GetData(2, ref inputMesh)) return;
            if (!DA.GetData(3, ref reps)) return;

            SpherePacking sphPacking = new SpherePacking(radius, inputMesh, startHeight,reps);

            intersectionLine = sphPacking.path;
            sphInters = sphPacking.sphIntersCurrent;
            var inter2 = sphPacking.intersectionsList;
            var pts = sphPacking.centers;

            //make the list of lists a single list
            for (int i = 0; i < inter2.Count; i++)
            {
                for (int j = 0; j < inter2[i].Count; j++)
                {
                    allInter.Add(inter2[i][j]);
                }

            }

            for (int i = 0; i < sphPacking.groupsOfFive.Count; i++)
            {
                quadPanel qP = new quadPanel(sphPacking.groupsOfFive[i]);
                qPanels.Add(qP.mesh);
                curvatures.Add(qP.weights[0]);
            }

            DA.SetData(0, intersectionLine);
            DA.SetDataList(1, sphInters);
            DA.SetDataList(2, allInter);        
            DA.SetDataList(3, qPanels);
            DA.SetDataList(4, curvatures);

            //DA.SetDataList(3, sphPacking.groupsOfFive[sphPacking.groupsOfFive.Count - 1]);
            //DA.SetDataList(4, sphPacking.groupsOfFive[1]);
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
                return Resources.icon1_01;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("82785197-afcc-4cca-9cd7-3c9ac4bb1058"); }
        }
    }
}
