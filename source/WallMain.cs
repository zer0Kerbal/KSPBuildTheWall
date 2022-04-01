using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BuildTheWall
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class WallMain : MonoBehaviour
    {
        private static bool buildMode = false;
        private bool wallBuilt = false;
        private List<WallSegment> wallSegments = new List<WallSegment>();
        private Rect windowPos = new Rect(0, 0, 200, 100);
        private Vector3d lastPos;
        private string wallFile;

        public void Awake()
        {
            Debug.Log("BuildTheWall Awake"); 
            string dllPath = Path.GetDirectoryName(new DirectoryInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName);
            wallFile = Path.Combine(dllPath, "wall.txt");
            if (!File.Exists(wallFile))
            {
                buildMode = true;
            }
        }

        public void Update()
        {
            if (!wallBuilt && !buildMode && wallFile != null)
            {
                bool buildTheWall = false;
                if (HighLogic.LoadedScene == GameScenes.FLIGHT)
                {
                    Vessel activeVessel = FlightGlobals.ActiveVessel;
                    buildTheWall = FlightGlobals.ready && activeVessel != null && activeVessel.state != Vessel.State.DEAD && activeVessel.loaded;
                }
                if (buildTheWall)
                {
                    wallBuilt = true;
                    Vector3d lastLoadPos = Vector3d.zero;
                    using (StreamReader sr = new StreamReader(wallFile))
                    {
                        Debug.Log("Reading wall file");
                        string currentLine = null;
                        string bodyName = null;
                        while ((currentLine = sr.ReadLine()) != null)
                        {
                            if (!currentLine.Contains("="))
                            {
                                string[] split = currentLine.Split(',');
                                double wallLat = double.Parse(split[0].Trim());
                                double wallLong = double.Parse(split[1].Trim());
                                double wallAlt = double.Parse(split[2].Trim());
                                Vector3d newLoadPos = new Vector3d(wallLat, wallLong, wallAlt);
                                if (lastLoadPos != Vector3d.zero)
                                {
                                    wallSegments.Add(new WallSegment(bodyName, lastLoadPos, newLoadPos, true));
                                    Debug.Log("Wall pos: " + newLoadPos);
                                }
                                lastLoadPos = newLoadPos;
                            }
                            else
                            {
                                bodyName = currentLine.Substring(1);
                                lastLoadPos = Vector3d.zero;
                            }
                        }
                    }
                }
            }
            foreach (WallSegment wallSegment in wallSegments)
            {
                wallSegment.Update();
            }
        }

        public void FixedUpdate()
        {
            foreach (WallSegment wallSegment in wallSegments)
            {
                wallSegment.FixedUpdate();
            }
        }

        public void OnGUI()
        {
            if (buildMode)
            {
                if (HighLogic.LoadedSceneIsFlight)
                {
                    windowPos = GUILayout.Window(123456789, windowPos, DrawFunction, "BuildTheWall");
                }
            }
        }

        private void DrawFunction(int windowID)
        {
            GUI.DragWindow(new Rect(0, 0, float.PositiveInfinity, 20));
            if (GUILayout.Button("Clear"))
            {
                Debug.Log("Clearing Walls");
                foreach (WallSegment wallSegment in wallSegments)
                {
                    wallSegment.Destroy();
                }
                wallSegments.Clear();
            }
            if (GUILayout.Button("Add to Wall"))
            {
                Vector3d newPos = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude);
                Debug.Log("Adding wall at: " + newPos);
                if (lastPos != Vector3d.zero)
                {
                    wallSegments.Add(new WallSegment(FlightGlobals.ActiveVessel.mainBody.name, lastPos, newPos, false));
                }
                lastPos = newPos;
            }
            if (GUILayout.Button("Remove last Wall"))
            {
                if (wallSegments.Count > 0)
                {
                    Debug.Log("Removing wall");
                    WallSegment lastSegment = wallSegments[wallSegments.Count - 1];
                    lastPos = lastSegment.GetStartPosition();
                    wallSegments.Remove(lastSegment);
                    lastSegment.Destroy();
                }

            }
            if (GUILayout.Button("Make break"))
            {
                Debug.Log("Making break");
                lastPos = Vector3d.zero;
            }
            if (GUILayout.Button("Finish"))
            {
                Debug.Log("Finish wall");
                using (StreamWriter sw = new StreamWriter(wallFile))
                {
                    foreach (WallSegment wallSegment in wallSegments)
                    {
                        Vector3d startVector = wallSegment.GetStartPosition();
                        Vector3d endVector = wallSegment.GetEndPosition();
                        sw.WriteLine("=" + wallSegment.GetBodyName());
                        sw.WriteLine(startVector.x + ", " + startVector.y + ", " + startVector.z);
                        sw.WriteLine(endVector.x + ", " + endVector.y + ", " + endVector.z);
                        wallSegment.MakeCollidable();
                    }

                    wallBuilt = true;
                    buildMode = false;
                }
            }
        }

        public void OnDestroy()
        {
            foreach (WallSegment wallSegment in wallSegments)
            {
                wallSegment.Destroy();
            }
            wallSegments.Clear();
        }
    }
}

