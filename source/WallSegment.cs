using System;
using System.Collections.Generic;
using UnityEngine;

namespace BuildTheWall
{
    public class WallSegment
    {
        //Must be squared, as we check the square distance
        private static double loadDistance = Math.Pow(20000, 2);
        private static double unloadDistance = Math.Pow(25000, 2);
        private List<WallMesh> wallMeshes = new List<WallMesh>();
        private Vector3d startPos;
        private Vector3d endPos;
        private bool visible;
        private CelestialBody body;
        private bool collidable;


        public WallSegment(string bodyName, Vector3d startPos, Vector3d endPos, bool collidable)
        {
            this.startPos = startPos;
            this.endPos = endPos;
            this.collidable = collidable;
            visible = false;
            body = FlightGlobals.Bodies.Find(x => x.name == bodyName);
        }

        private void CreateSegments()
        {
            if (body != null)
            {
                Vector3d realStartPos = body.GetWorldSurfacePosition(startPos.x, startPos.y, startPos.z);
                Vector3d realEndPos = body.GetWorldSurfacePosition(endPos.x, endPos.y, endPos.z);
                long distance = (long)(realStartPos - realEndPos).magnitude;
                long divides = 1 + distance / 25;
                Vector3d segment = (realEndPos - realStartPos) / (double)divides;
                for (int i = 0; i < divides; i++)
                {
                    Vector3d segmentStart = realStartPos + segment * i;
                    Vector3d segmentEnd = realStartPos + segment * (i + 1);
                    double startLat = body.GetLatitude(segmentStart);
                    double startLong = body.GetLongitude(segmentStart);
                    double startAlt = body.GetAltitude(segmentStart);
                    double endLat = body.GetLatitude(segmentEnd);
                    double endLong = body.GetLongitude(segmentEnd);
                    double endAlt = body.GetAltitude(segmentEnd);
                    Vector3d startLatLongAlt = new Vector3d(startLat, startLong, startAlt);
                    Vector3d endLatLongAlt = new Vector3d(endLat, endLong, endAlt);
                    wallMeshes.Add(new WallMesh(startLatLongAlt, endLatLongAlt, collidable));
                }
                foreach (WallMesh wallMesh in wallMeshes)
                {
                    wallMesh.Update();
                }
            }
        }

        private void DestroySegments()
        {
            foreach (WallMesh wallMesh in wallMeshes)
            {
                wallMesh.Destroy();
            }
            wallMeshes.Clear();
        }

        public string GetBodyName()
        {
            return body.name;
        }

        public Vector3d GetStartPosition()
        {
            return startPos;
        }

        public Vector3d GetEndPosition()
        {
            return endPos;
        }

        private void SetVisibility()
        {
            if (body != FlightGlobals.ActiveVessel.mainBody)
            {
                if (visible)
                {
                    visible = false;
                    DestroySegments();
                }
                return;
            }
            double distanceStart = (FlightGlobals.ActiveVessel.GetWorldPos3D() - body.GetWorldSurfacePosition(startPos.x, startPos.y, startPos.z)).sqrMagnitude;
            double distanceEnd = (FlightGlobals.ActiveVessel.GetWorldPos3D() - body.GetWorldSurfacePosition(endPos.x, endPos.y, endPos.z)).sqrMagnitude;
            if (visible && distanceStart > unloadDistance && distanceEnd > unloadDistance)
            {
                visible = false;
                DestroySegments();
            }
            if (!visible && distanceStart < loadDistance && distanceEnd < loadDistance)
            {
                visible = true;
                CreateSegments();
            }
        }

        public void Update()
        {
            SetVisibility();
            foreach (WallMesh wallMesh in wallMeshes)
            {
                wallMesh.Update();
            }
        }

        public void FixedUpdate()
        {
            foreach (WallMesh wallMesh in wallMeshes)
            {
                wallMesh.FixedUpdate();
            }
        }

        public void MakeCollidable()
        {
            collidable = true;
            foreach (WallMesh wallMesh in wallMeshes)
            {
                wallMesh.MakeCollidable();
            }
        }

        public void Destroy()
        {
            DestroySegments();
        }
    }
}

