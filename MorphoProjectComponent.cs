using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
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
         //   pManager.AddIntegerParameter("num", "n", "height to start division from", GH_ParamAccess.item);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {

            pManager.AddCurveParameter("Line", "ln", "intersection curve", GH_ParamAccess.item);
            pManager.AddCurveParameter("intersections", "inters", "intersection curves", GH_ParamAccess.list);
            pManager.AddCurveParameter("intersectionsNext", "interNext", "intersection curves", GH_ParamAccess.list);
            pManager.AddPointParameter("intersectionspt", "intPt", "intersection curves", GH_ParamAccess.list);

        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double radius = 0.5;
            double startHeight = radius;
            Mesh inputMesh = new Mesh();
            Polyline intersectionLine;
            List<PolylineCurve> sphInters;
            int num=1;

            // Then we need to access the input parameters individually. 
            // When data cannot be extracted from a parameter, we should abort this method.
            if (!DA.GetData(0, ref radius)) return;
            if (!DA.GetData(1, ref startHeight)) return;
            if (!DA.GetData(2, ref inputMesh)) return;
           // if (!DA.GetData(3, ref num)) return;

            SpherePacking sphPacking = new SpherePacking(radius, inputMesh, startHeight,num);

            intersectionLine = sphPacking.path;
            sphInters = sphPacking.sphIntersFirst;
            var inter2 = sphPacking.sphIntersNext;
            var pts = sphPacking.centers;

            DA.SetData(0, intersectionLine);
            DA.SetDataList(1, sphInters);
            DA.SetDataList(2, inter2);
            DA.SetDataList(3, pts[1]);
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
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("82785197-afcc-4cca-9cd7-3c9ac4bb1058"); }
        }
    }
}
