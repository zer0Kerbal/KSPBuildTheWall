using System;
using UnityEngine;

namespace BuildTheWall
{
    public class WallMesh
    {
        private GameObject gameObject;
        private MeshFilter meshFilter;
        private Mesh mesh;
        private MeshRenderer meshRenderer;
        private static Texture2D meshTexture;
        private Vector3d startPos;
        private Vector3d endPos;
        private bool collidable;

        public WallMesh(Vector3d startPos, Vector3d endPos, bool collidable)
        {
            this.startPos = startPos;
            this.endPos = endPos;
            this.collidable = collidable;
            CreateGameObject();
        }

        private void CreateGameObject()
        {
            if (meshTexture == null)
            {
                if (GameDatabase.Instance.ExistsTexture("BuildTheWall/wall"))
                {
                    Debug.Log("Using wall texture");
                    meshTexture = GameDatabase.Instance.GetTexture("BuildTheWall/wall", false);
                }
                else
                {
                    Debug.Log("Using red texture - wall texture missing!");
                    meshTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                    meshTexture.SetPixel(0, 0, Color.red);
                    meshTexture.Apply();
                }
            }
            if (gameObject == null)
            {
                gameObject = new GameObject();
                meshFilter = gameObject.AddComponent<MeshFilter>();
                mesh = BuildWallMesh();
                meshFilter.mesh = mesh;
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
                meshRenderer.material.color = Color.white;
                meshRenderer.material.mainTexture = meshTexture;
                if (collidable)
                {
                    gameObject.AddComponent<BoxCollider>();
                }
            }
        }

        //This is the part that kexico pays for
        private Mesh BuildWallMesh()
        {
            Mesh retMesh = new Mesh();
            //[Front/Back][Bottom/Top][Left/Right]
            Vector3 FBL = new Vector3(0f, 0f, 0f);
            Vector3 FTL = new Vector3(0f, 1f, 0f);
            Vector3 FTR = new Vector3(0f, 1f, 1f);
            Vector3 FBR = new Vector3(0f, 0f, 1f);
            Vector3 BBL = new Vector3(1f, 0f, 0f);
            Vector3 BTL = new Vector3(1f, 1f, 0f);
            Vector3 BTR = new Vector3(1f, 1f, 1f);
            Vector3 BBR = new Vector3(1f, 0f, 1f);

            Vector3[] vertices = new Vector3[24];
            //Front
            vertices[0] = FBL;
            vertices[1] = FTL;
            vertices[2] = FTR;
            vertices[3] = FBR;
            //Top
            vertices[4] = FTL;
            vertices[5] = BTL;
            vertices[6] = BTR;
            vertices[7] = FTR;
            //LHS
            vertices[8] = BBL;
            vertices[9] = BTL;
            vertices[10] = FTL;
            vertices[11] = FBL;
            //RHS
            vertices[12] = FBR;
            vertices[13] = FTR;
            vertices[14] = BTR;
            vertices[15] = BBR;
            //Back
            vertices[16] = BBR;
            vertices[17] = BTR;
            vertices[18] = BTL;
            vertices[19] = BBL;
            //Bottom
            vertices[20] = BBL;
            vertices[21] = FBL;
            vertices[22] = FBR;
            vertices[23] = BBR;

            int[] triangles = new int[36];
            //Front
            triangles[0] = 0;
            triangles[1] = 2;
            triangles[2] = 1;
            triangles[3] = 0;
            triangles[4] = 3;
            triangles[5] = 2;
            //Top
            triangles[6] = 4;
            triangles[7] = 6;
            triangles[8] = 5;
            triangles[9] = 4;
            triangles[10] = 7;
            triangles[11] = 6;
            //LHS
            triangles[12] = 8;
            triangles[13] = 10;
            triangles[14] = 9;
            triangles[15] = 8;
            triangles[16] = 11;
            triangles[17] = 10;
            //RHS
            triangles[18] = 12;
            triangles[19] = 14;
            triangles[20] = 13;
            triangles[21] = 12;
            triangles[22] = 15;
            triangles[23] = 14;
            //Back
            triangles[24] = 16;
            triangles[25] = 18;
            triangles[26] = 17;
            triangles[27] = 16;
            triangles[28] = 19;
            triangles[29] = 18;
            //Bottom
            triangles[30] = 20;
            triangles[31] = 22;
            triangles[32] = 21;
            triangles[33] = 20;
            triangles[34] = 23;
            triangles[35] = 22;

            Vector2[] uv = new Vector2[24];
            //Front
            uv[0] = new Vector2(0f, 0f);
            uv[1] = new Vector2(0f, 1f);
            uv[2] = new Vector2(1f, 1f);
            uv[3] = new Vector2(1f, 0f);
            //Top
            uv[4] = new Vector2(0f, 0f);
            uv[5] = new Vector2(0f, 0.2f);
            uv[6] = new Vector2(1f, 0.2f);
            uv[7] = new Vector2(1f, 0f);
            //LHS
            uv[8] = new Vector2(0f, 0f);
            uv[9] = new Vector2(0f, 1f);
            uv[10] = new Vector2(0.2f, 1f);
            uv[11] = new Vector2(0.2f, 0f);
            //RHS
            uv[12] = new Vector2(0f, 0f);
            uv[13] = new Vector2(0f, 1f);
            uv[14] = new Vector2(0.2f, 1f);
            uv[15] = new Vector2(0.2f, 0f);
            //Back
            uv[16] = new Vector2(0f, 0f);
            uv[17] = new Vector2(0f, 1f);
            uv[18] = new Vector2(1f, 1f);
            uv[19] = new Vector2(1f, 0f);
            //Bottom
            uv[20] = new Vector2(0f, 0f);
            uv[21] = new Vector2(0f, 0.2f);
            uv[22] = new Vector2(1f, 0.2f);
            uv[23] = new Vector2(1f, 0f);

            retMesh.vertices = vertices;
            retMesh.triangles = triangles;
            retMesh.uv = uv;
            retMesh.Optimize();
            retMesh.RecalculateNormals();
            return retMesh;
        }

        private void UpdatePosition()
        {
            Vector3d realStartPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition(startPos.x, startPos.y, startPos.z);
            Vector3d realEndPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition(endPos.x, endPos.y, endPos.z);
            Vector3d upVector = FlightGlobals.ActiveVessel.mainBody.GetSurfaceNVector(startPos.x, startPos.y);
            Quaternion rotation = Quaternion.LookRotation((realEndPos - realStartPos).normalized, upVector);
            gameObject.transform.position = realStartPos;
            gameObject.transform.rotation = rotation;
            float wallLength = (float)(realEndPos - realStartPos).magnitude;
            gameObject.transform.localScale = new Vector3(2f, 10f, wallLength);
        }

        public void Update()
        {
            if (gameObject != null)
            {
                UpdatePosition();
            }
        }

        public void FixedUpdate()
        {
            if (gameObject != null)
            {
                UpdatePosition();
            }
        }

        public void MakeCollidable()
        {
            collidable = true;
            if (gameObject != null)
            {
                if (gameObject.GetComponent<BoxCollider>() == null)
                {
                    gameObject.AddComponent<BoxCollider>();
                }
            }
        }

        public void Destroy()
        {
            if (gameObject != null)
            {
                GameObject.Destroy(gameObject);
            }
            gameObject = null;
        }
    }
}

