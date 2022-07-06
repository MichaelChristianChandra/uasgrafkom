using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;



namespace test1
{
    static class Constants
    {
        public const string path = "C:/Users/Loyal Core/source/repos/Project 2/Should be working/UAS Grafkom/test1/shader//";
    }
    internal class Window : GameWindow
    {
        private readonly List<Vector3> _pointLightPositions = new List<Vector3>()
    {
            //new Vector3(-2f, 0f, 0f),
            //new Vector3(2f, 0f, 0f),
            //new Vector3(0f, 0f, -2f),
            //new Vector3(0f, 0f, 2f)
        };
        private readonly List<Vector3> point_light_color_difuse = new List<Vector3>()
    {
            //new Vector3(1f, 0, 0f),
            //new Vector3(0f, 1f, 0f),
            //new Vector3(0f, 0f, 1f),
            //new Vector3(1f, 1f, 0f)
        };
        int Count=0;
        Asset3d[] _object3d = new Asset3d[15];
        double _time;
        float degr = 0;
        Camera _camera;
        bool _firstMove = true;
        Vector2 _lastPos;
        Vector3 _objecPost = new Vector3(0.0f, 4.0f, 2.0f);
        float _rotationSpeed = 0.4f;
        float _intime;
        Asset3d[] LightObject = new Asset3d[20];
        float speed = 0.05f;
        bool right = true;
        int count = 0;
        int walk = 0;
        bool moveplayer = true;
        //Asset3d[] cahaya = new Asset3d[2];
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }   
        public Matrix4 generateArbRotationMatrix(Vector3 axis, Vector3 center, float degree)
        {
            var rads = MathHelper.DegreesToRadians(degree);

            var secretFormula = new float[4, 4] {
                { (float)Math.Cos(rads) + (float)Math.Pow(axis.X, 2) * (1 - (float)Math.Cos(rads)), axis.X* axis.Y * (1 - (float)Math.Cos(rads)) - axis.Z * (float)Math.Sin(rads),    axis.X * axis.Z * (1 - (float)Math.Cos(rads)) + axis.Y * (float)Math.Sin(rads),   0 },
                { axis.Y * axis.X * (1 - (float)Math.Cos(rads)) + axis.Z * (float)Math.Sin(rads),   (float)Math.Cos(rads) + (float)Math.Pow(axis.Y, 2) * (1 - (float)Math.Cos(rads)), axis.Y * axis.Z * (1 - (float)Math.Cos(rads)) - axis.X * (float)Math.Sin(rads),   0 },
                { axis.Z * axis.X * (1 - (float)Math.Cos(rads)) - axis.Y * (float)Math.Sin(rads),   axis.Z * axis.Y * (1 - (float)Math.Cos(rads)) + axis.X * (float)Math.Sin(rads),   (float)Math.Cos(rads) + (float)Math.Pow(axis.Z, 2) * (1 - (float)Math.Cos(rads)), 0 },
                { 0, 0, 0, 1}
            };
            var secretFormulaMatix = new Matrix4
            (
                new Vector4(secretFormula[0, 0], secretFormula[0, 1], secretFormula[0, 2], secretFormula[0, 3]),
                new Vector4(secretFormula[1, 0], secretFormula[1, 1], secretFormula[1, 2], secretFormula[1, 3]),
                new Vector4(secretFormula[2, 0], secretFormula[2, 1], secretFormula[2, 2], secretFormula[2, 3]),
                new Vector4(secretFormula[3, 0], secretFormula[3, 1], secretFormula[3, 2], secretFormula[3, 3])
            );

            return secretFormulaMatix;
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            //ganti background
            GL.ClearColor(0.69f, 0.84f, 0.85f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            //Lampu Ruang 1
            LightObject[0] = new Asset3d();
            LightObject[0].createBoxVertices(0f, 4.34f, 2f, 0.5f, 0.1f, 0.5f);
            LightObject[0].load(Constants.path + "shader.vert", Constants.path + "shader1.frag", Size.X, Size.Y);

            _pointLightPositions.Add(LightObject[0]._centerPosition);
            point_light_color_difuse.Add(new Vector3(1f, 1f, 1f));

            LightObject[1] = new Asset3d();
            LightObject[1].createBoxVertices(-3f, 4.34f, 6.7f, 0.5f, 0.1f, 0.5f);
            LightObject[1].load(Constants.path + "shader.vert", Constants.path + "shader1.frag", Size.X, Size.Y);

            _pointLightPositions.Add(LightObject[1]._centerPosition);
            point_light_color_difuse.Add(new Vector3(1, 1, 1));

            LightObject[2] = new Asset3d();
            LightObject[2].createBoxVertices(3f , 4.34f, 6.7f, 0.5f, 0.1f, 0.5f);
            LightObject[2].load(Constants.path + "shader.vert", Constants.path + "shader1.frag", Size.X, Size.Y);

            _pointLightPositions.Add(LightObject[2]._centerPosition);
            point_light_color_difuse.Add(new Vector3(1, 1, 1));

            //Lampu Ruang 2
            LightObject[3] = new Asset3d();
            LightObject[3].createBoxVertices(-3f + 11f, 4.84f, 2f, 0.5f, 0.1f, 0.5f);
            LightObject[3].load(Constants.path + "shader.vert", Constants.path + "shader1.frag", Size.X, Size.Y);

            _pointLightPositions.Add(LightObject[3]._centerPosition);
            point_light_color_difuse.Add(new Vector3(1, 1, 1));

            LightObject[4] = new Asset3d();
            LightObject[4].createBoxVertices(3f + 11f, 4.84f, 2f, 0.5f, 0.1f, 0.5f);
            LightObject[4].load(Constants.path + "shader.vert", Constants.path + "shader1.frag", Size.X, Size.Y);

            _pointLightPositions.Add(LightObject[4]._centerPosition);
            point_light_color_difuse.Add(new Vector3(1, 1, 1));

            LightObject[5] = new Asset3d();
            LightObject[5].createBoxVertices(11f, 4.84f, 6.7f, 0.5f, 0.1f, 0.5f);
            LightObject[5].load(Constants.path + "shader.vert", Constants.path + "shader1.frag", Size.X, Size.Y);

            _pointLightPositions.Add(LightObject[5]._centerPosition);
            point_light_color_difuse.Add(new Vector3(1, 1, 1));

            //Lampu Hall
            LightObject[6] = new Asset3d();
            LightObject[6].createBoxVertices(12.6f + 1.5f, 4.84f, 15f, 0.5f, 0.1f, 0.5f);
            LightObject[6].load(Constants.path + "shader.vert", Constants.path + "shader1.frag", Size.X, Size.Y);

            _pointLightPositions.Add(LightObject[6]._centerPosition);
            point_light_color_difuse.Add(new Vector3(1, 1, 1));

            LightObject[7] = new Asset3d();
            LightObject[7].createBoxVertices(12.6f - 2.25f, 4.84f, 21.5f, 0.5f, 0.1f, 0.5f);
            LightObject[7].load(Constants.path + "shader.vert", Constants.path + "shader1.frag", Size.X, Size.Y);

            _pointLightPositions.Add(LightObject[7]._centerPosition);
            point_light_color_difuse.Add(new Vector3(1, 1, 1));

            //Ruang 3
            LightObject[8] = new Asset3d();
            LightObject[8].createBoxVertices(0f, 4.34f, 2f + 13.5f, 0.5f, 0.1f, 0.5f);
            LightObject[8].load(Constants.path + "shader.vert", Constants.path + "shader1.frag", Size.X, Size.Y);

            _pointLightPositions.Add(LightObject[8]._centerPosition);
            point_light_color_difuse.Add(new Vector3(1, 1, 1));

            LightObject[9] = new Asset3d();
            LightObject[9].createBoxVertices(-3f, 4.34f, 6.7f + 13.5f, 0.5f, 0.1f, 0.5f);
            LightObject[9].load(Constants.path + "shader.vert", Constants.path + "shader1.frag", Size.X, Size.Y);

            _pointLightPositions.Add(LightObject[9]._centerPosition);
            point_light_color_difuse.Add(new Vector3(1, 1, 1));

            LightObject[10] = new Asset3d();
            LightObject[10].createBoxVertices(3f, 4.34f, 6.7f + 13.5f, 0.5f, 0.1f, 0.5f);
            LightObject[10].load(Constants.path + "shader.vert", Constants.path + "shader1.frag", Size.X, Size.Y);

            _pointLightPositions.Add(LightObject[10]._centerPosition);
            point_light_color_difuse.Add(new Vector3(1, 1, 1));

            //LightObject[8] = new Asset3d();
            //LightObject[8].createBoxVertices(12.6f + 2f, 4.84f, 15f, 0.5f, 0.1f, 0.5f);
            //LightObject[8].load(Constants.path + "shader.vert", Constants.path + "shader1.frag", Size.X, Size.Y);

            //_pointLightPositions.Add(LightObject[8]._centerPosition);
            //point_light_color_difuse.Add(new Vector3(0.0f, 1, 1));
            _object3d[0] = new Asset3d();
            _object3d[0].createBoxVertices(0f, 2f, 0.0f, 10f, 5f, 0.1f);

            Asset3d tembokKiri1 = new Asset3d();
            tembokKiri1.createBoxVertices(-5f, 2f, 5f, 0.1f, 5f, 10f);
            Asset3d tembokKanan1 = new Asset3d();
            tembokKanan1.createBoxVertices(5f, 2f, 3.5f, 0.1f, 5f, 7f);
            Asset3d tembokKanan12 = new Asset3d();
            tembokKanan12.createBoxVertices(5f, 3.5f, 8f, 0.1f, 2f, 2f);
            Asset3d tembokKanan13 = new Asset3d();
            tembokKanan13.createBoxVertices(5f, 2f, 9.5f, 0.1f, 5f, 1f);
            Asset3d tembokBlkg1 = new Asset3d();
            tembokBlkg1.createBoxVertices(0f, 2f, 10f, 10f, 5f, 0.1f);
            Asset3d lantai1 = new Asset3d();
            lantai1.createBoxVertices(0f, -0.45f, 5f, 10f, 0.1f, 10f);
            Asset3d atap1 = new Asset3d();
            atap1.createBoxVertices(0f, 4.45f, 5f, 10f, 0.1f, 10f);

            _object3d[0].addChild(tembokKiri1);
            //_object3d[0].addChild(tembokKanan1);
            //_object3d[0].addChild(tembokKanan12);
            //_object3d[0].addChild(tembokKanan13);
            _object3d[0].addChild(tembokBlkg1);
            _object3d[0].addChild(lantai1);
            _object3d[0].addChild(atap1);

            _object3d[1] = new Asset3d();
            _object3d[1].createBoxVertices2(1f, 1f, 1f, 0.4f);

            Asset3d target1 = new Asset3d();
            Asset3d target2 = new Asset3d();
            Asset3d target3 = new Asset3d();
            Asset3d target4 = new Asset3d();
            Asset3d sTarget1 = new Asset3d();
            Asset3d sTarget2 = new Asset3d();
            Asset3d sTarget3 = new Asset3d();
            Asset3d sTarget4 = new Asset3d();
            Asset3d sTarget5 = new Asset3d();
            target1.createBoxVertices2(-3f, 2f, 3f, 0.7f);
            target2.createBoxVertices2(-1f, 3f, 2f, 0.6f);
            target3.createBoxVertices2(2f, 2.7f, 3.5f, 0.4f);
            target4.createBoxVertices2(-1.6f, 1.8f, 2.3f, 0.5f);
            sTarget1.createBoxVertices(0f, 0.5f, 4.5f, 1f, 2f, 0.1f);
            sTarget2.createBoxVertices(-3.5f, 0.5f, 4.5f, 1f, 2f, 0.1f);
            sTarget3.createBoxVertices(3.5f, 0.5f, 4.5f, 1f, 2f, 0.1f);
            sTarget4.createBoxVertices(-1.75f, 0.5f, 2.5f, 1f, 2f, 0.1f);
            sTarget5.createBoxVertices(1.75f, 0.5f, 2.5f, 1f, 2f, 0.1f);
            _object3d[1].addChild(target1);
            _object3d[1].addChild(target2);
            _object3d[1].addChild(target3);
            _object3d[1].addChild(target4);
            _object3d[1].addChild(sTarget1);
            _object3d[1].addChild(sTarget2);
            _object3d[1].addChild(sTarget3);
            _object3d[1].addChild(sTarget4);
            _object3d[1].addChild(sTarget5);

            _object3d[2] = new Asset3d();
            _object3d[2].createBoxVertices(0f, 0.5f, 6.7f, 10f, 0.3f, 0.3f);

            _object3d[3] = new Asset3d();
            _object3d[3].createBoxVertices(0f, 0.55f, 4.55f, 0.6f, 1.6f, 0.01f);

            Asset3d sTCoreB1 = new Asset3d();
            Asset3d sTCoreB2 = new Asset3d();
            Asset3d sTCoreB3 = new Asset3d();
            Asset3d sTCoreB4 = new Asset3d();
            Asset3d sTCoreB5 = new Asset3d();
            Asset3d sTCoreH1 = new Asset3d();
            Asset3d sTCoreH2 = new Asset3d();
            Asset3d sTCoreH3 = new Asset3d();
            Asset3d sTCoreH4 = new Asset3d();
            Asset3d sTCoreH5 = new Asset3d();
            sTCoreB1.createBoxVertices(0f, 0.55f, 4.58f, 0.1f, 0.1f, 0.01f);
            sTCoreB2.createBoxVertices(-3.5f, 0.55f, 4.58f, 0.1f, 0.1f, 0.01f);
            sTCoreB3.createBoxVertices(3.5f, 0.55f, 4.58f, 0.1f, 0.1f, 0.01f);
            sTCoreB4.createBoxVertices(-1.75f, 0.55f, 2.58f, 0.1f, 0.1f, 0.01f);
            sTCoreB5.createBoxVertices(1.75f, 0.55f, 2.58f, 0.1f, 0.1f, 0.01f);
            sTCoreH1.createBoxVertices(0f, 1.05f, 4.58f, 0.05f, 0.05f, 0.01f);
            sTCoreH2.createBoxVertices(-3.5f, 1.05f, 4.58f, 0.05f, 0.05f, 0.01f);
            sTCoreH3.createBoxVertices(3.5f, 1.05f, 4.58f, 0.05f, 0.05f, 0.01f);
            sTCoreH4.createBoxVertices(-1.75f, 1.05f, 2.58f, 0.05f, 0.05f, 0.01f);
            sTCoreH5.createBoxVertices(1.75f, 1.05f, 2.58f, 0.05f, 0.05f, 0.01f);
            _object3d[2].addChild(sTCoreB1);
            _object3d[2].addChild(sTCoreB2);
            _object3d[2].addChild(sTCoreB3);
            _object3d[2].addChild(sTCoreB4);
            _object3d[2].addChild(sTCoreB5);
            _object3d[2].addChild(sTCoreH1);
            _object3d[2].addChild(sTCoreH2);
            _object3d[2].addChild(sTCoreH3);
            _object3d[2].addChild(sTCoreH4);
            _object3d[2].addChild(sTCoreH5);

            Asset3d sTarget21 = new Asset3d();
            Asset3d sTarget31 = new Asset3d();
            Asset3d sTarget41 = new Asset3d();
            Asset3d sTarget51 = new Asset3d();
            sTarget21.createBoxVertices(-3.5f, 0.55f, 4.55f, 0.6f, 1.6f, 0.01f);
            sTarget31.createBoxVertices(3.5f, 0.55f, 4.55f, 0.6f, 1.6f, 0.01f);
            sTarget41.createBoxVertices(-1.75f, 0.55f, 2.55f, 0.6f, 1.6f, 0.01f);
            sTarget51.createBoxVertices(1.75f, 0.55f, 2.55f, 0.6f, 1.6f, 0.01f);
            _object3d[3].addChild(sTarget21);
            _object3d[3].addChild(sTarget31);
            _object3d[3].addChild(sTarget41);
            _object3d[3].addChild(sTarget51);
            Asset3d sTRadii1 = new Asset3d();
            Asset3d sTRadii2 = new Asset3d();
            Asset3d sTRadii3 = new Asset3d();
            Asset3d sTRadii4 = new Asset3d();
            Asset3d sTRadii5 = new Asset3d();
            sTRadii1.createBoxVertices(0f, 0.55f, 4.57f, 0.25f, 0.25f, 0.01f);
            sTRadii2.createBoxVertices(-3.5f, 0.55f, 4.57f, 0.25f, 0.25f, 0.01f);
            sTRadii3.createBoxVertices(3.5f, 0.55f, 4.57f, 0.25f, 0.25f, 0.01f);
            sTRadii4.createBoxVertices(-1.75f, 0.55f, 2.57f, 0.25f, 0.25f, 0.01f);
            sTRadii5.createBoxVertices(1.75f, 0.55f, 2.57f, 0.25f, 0.25f, 0.01f);
            _object3d[3].addChild(sTRadii1);
            _object3d[3].addChild(sTRadii2);
            _object3d[3].addChild(sTRadii3);
            _object3d[3].addChild(sTRadii4);
            _object3d[3].addChild(sTRadii5);

            Asset3d sTRHead1 = new Asset3d();
            Asset3d sTRHead2 = new Asset3d();
            Asset3d sTRHead3 = new Asset3d();
            Asset3d sTRHead4 = new Asset3d();
            Asset3d sTRHead5 = new Asset3d();
            sTRHead1.createBoxVertices(0f, 1.05f, 4.57f, 0.1f, 0.1f, 0.01f);
            sTRHead2.createBoxVertices(-3.5f, 1.05f, 4.57f, 0.1f, 0.1f, 0.01f);
            sTRHead3.createBoxVertices(3.5f, 1.05f, 4.57f, 0.1f, 0.1f, 0.01f);
            sTRHead4.createBoxVertices(-1.75f, 1.05f, 2.57f, 0.1f, 0.1f, 0.01f);
            sTRHead5.createBoxVertices(1.75f, 1.05f, 2.57f, 0.1f, 0.1f, 0.01f);
            _object3d[3].addChild(sTRHead1);
            _object3d[3].addChild(sTRHead2);
            _object3d[3].addChild(sTRHead3);
            _object3d[3].addChild(sTRHead4);
            _object3d[3].addChild(sTRHead5);

            _object3d[4] = new Asset3d();
            _object3d[4].createBoxVertices(0f, 0.55f, 4.56f, 0.5f, 0.5f, 0.01f);

            Asset3d sTBody2 = new Asset3d();
            Asset3d sTBody3 = new Asset3d();
            Asset3d sTBody4 = new Asset3d();
            Asset3d sTBody5 = new Asset3d();
            sTBody2.createBoxVertices(-3.5f, 0.55f, 4.56f, 0.5f, 0.5f, 0.01f);
            sTBody3.createBoxVertices(3.5f, 0.55f, 4.56f, 0.5f, 0.5f, 0.01f);
            sTBody4.createBoxVertices(-1.75f, 0.55f, 2.56f, 0.5f, 0.5f, 0.01f);
            sTBody5.createBoxVertices(1.75f, 0.55f, 2.56f, 0.5f, 0.5f, 0.01f);
            _object3d[4].addChild(sTBody2);
            _object3d[4].addChild(sTBody3);
            _object3d[4].addChild(sTBody4);
            _object3d[4].addChild(sTBody5);

            Asset3d sTHead1 = new Asset3d();
            Asset3d sTHead2 = new Asset3d();
            Asset3d sTHead3 = new Asset3d();
            Asset3d sTHead4 = new Asset3d();
            Asset3d sTHead5 = new Asset3d();
            sTHead1.createBoxVertices(0f, 1.05f, 4.56f, 0.25f, 0.25f, 0.01f);
            sTHead2.createBoxVertices(-3.5f, 1.05f, 4.56f, 0.25f, 0.25f, 0.01f);
            sTHead3.createBoxVertices(3.5f, 1.05f, 4.56f, 0.25f, 0.25f, 0.01f);
            sTHead4.createBoxVertices(-1.75f, 1.05f, 2.56f, 0.25f, 0.25f, 0.01f);
            sTHead5.createBoxVertices(1.75f, 1.05f, 2.56f, 0.25f, 0.25f, 0.01f);
            _object3d[4].addChild(sTHead1);
            _object3d[4].addChild(sTHead2);
            _object3d[4].addChild(sTHead3);
            _object3d[4].addChild(sTHead4);
            _object3d[4].addChild(sTHead5);

            float a = 12.6f;

            _object3d[5] = new Asset3d();
            
            _object3d[5].createBoxVertices(-1.8f + a, 2.29f, 0.0f, 11.2f, 5f, 0.5f);
            _object3d[6] = new Asset3d();
            _object3d[6].createEllipsoid2(-5.5f + a, 1.8f, 2.5f, 0.4f, 0.4f, 0.4f, 200, 1000);

            Asset3d tembokBelakang = new Asset3d();
            tembokBelakang.createBoxVertices(-3.2f + a, 2.29f, 10f, 7.7f, 5.5f, 0.5f);
            _object3d[5].addChild(tembokBelakang);//tembok belakang
            Asset3d tembokBelakangs = new Asset3d();
            tembokBelakangs.createBoxVertices(a + 3.4f, 2.29f, 10f, 2f, 5.5f, 0.5f);
            _object3d[5].addChild(tembokBelakangs);//tembok belakang
            Asset3d tembokBelakangss = new Asset3d();
            tembokBelakangss.createBoxVertices(a, 3.5f, 10f, 7.5f, 3f, 0.5f);
            _object3d[5].addChild(tembokBelakangss);//tembok belakang

            Asset3d pembatas = new Asset3d();
            pembatas.createBoxVertices(-1.8f + a, 0f, 3.5f, 11.2f, 1f, 0.2f);
            _object3d[5].addChild(pembatas);//pembatas
            Asset3d kotak = new Asset3d();
            kotak.createBoxVertices(2.8f + a, 0.3f, 1f, 1.5f, 1.5f, 1.5f);
            _object3d[5].addChild(kotak);//kotak



            Asset3d tembokKiri = new Asset3d();
            tembokKiri.createBoxVertices(-7.5f + a, 2.29f, 3.5f, 1f, 5.5f, 7f);
            Asset3d temboKiris = new Asset3d();
            temboKiris.createBoxVertices(-7.5f + a, 3.5f, 7.5f, 1f, 3f, 3f);
            Asset3d tembokKiriss = new Asset3d();
            tembokKiriss.createBoxVertices(-7.5f + a, 2f, 9.5f, 1f, 6f, 1f);
            _object3d[5].addChild(tembokKiri);//tembok kiri
            _object3d[5].addChild(temboKiris);//tembok kiri
            _object3d[5].addChild(tembokKiriss);//tembok kiri


            Asset3d tembokKanan = new Asset3d();
            tembokKanan.createBoxVertices(3.8f + a, 2.29f, 5f, 0.5f, 5.5f, 10.2f);
            _object3d[5].addChild(tembokKanan);//tembok kanan
            Asset3d atap = new Asset3d();
            atap.createBoxVertices(-1.85f + a, 5.1f, 5f, 11.5f, 0.5f, 10.2f);
            _object3d[5].addChild(atap);//atap
            Asset3d jalan = new Asset3d();
            jalan.createBoxVertices(-1.85f + a, -0.45f, 5f, 11.5f, 0.1f, 10.2f);
            _object3d[5].addChild(jalan);//jalan

            Asset3d badan = new Asset3d();
            badan.createCone(-5.5f + a, 0.2f, 2.5f, 0.5f, 1f, 0.4f, 200, 400);
            _object3d[6].addChild(badan);
            badan.createCone(-.5f + a, 0.2f, 2.5f, 0.5f, 1f, 0.4f, 200, 400);
            _object3d[6].addChild(badan);

            Asset3d kepala = new Asset3d();
            kepala.createEllipsoid2(-0.5f + a, 1.8f, 2.5f, 0.4f, 0.4f, 0.4f, 200, 1000);
            _object3d[6].addChild(kepala);


            //Hall
            _object3d[7] = new Asset3d();
            _object3d[7].createBoxVertices(0, 0, 0, 0, 0, 0);
            Asset3d hallKiri = new Asset3d();
            hallKiri.createBoxVertices(0f + a, 2.29f, 15f, 1f, 5.5f, 10f);
            _object3d[7].addChild(hallKiri);//tembok belakang
            Asset3d hallKanan = new Asset3d();
            hallKanan.createBoxVertices(a + 3f, 2.29f, 15f, 1f, 5.5f, 10f);
            _object3d[7].addChild(hallKanan);//tembok belakang
            Asset3d hallFloor = new Asset3d();
            hallFloor.createBoxVertices(a + 2f, -0.65f, 15f, 4f, 0.5f, 10f);
            _object3d[7].addChild(hallFloor);//tembok belakang

            Asset3d hallRoof = new Asset3d();
            hallRoof.createBoxVertices(a + 2f, 5.1f, 15f, 4f, 0.5f, 10f);
            _object3d[7].addChild(hallRoof);//tembok belakang

            Asset3d hallKiriPlus = new Asset3d();
            hallKiriPlus.createBoxVertices(a + 3f, 2.29f, 21.5f, 1f, 5.5f, 3f);
            _object3d[7].addChild(hallKiriPlus);//tembok belakang


            Asset3d hallKiri2 = new Asset3d();
            hallKiri2.createBoxVertices(a - 2.25f, 2.29f, 23f, 10.74f, 5.5f, 1f);
            _object3d[7].addChild(hallKiri2);//tembok belakang

            Asset3d hallFloor2 = new Asset3d();
            hallFloor2.createBoxVertices(a - 2.25f, -0.65f, 21.5f, 10.74f, 0.5f, 4f);
            _object3d[7].addChild(hallFloor2);//tembok belakang

            Asset3d hallRoof2 = new Asset3d();
            hallRoof2.createBoxVertices(a - 2.25f, 5.1f, 21.5f, 10.74f, 0.5f, 4f);
            _object3d[7].addChild(hallRoof2);//

            Asset3d hallKanan2 = new Asset3d();
            hallKanan2.createBoxVertices(-3.75f + a, 2.29f, 20f, 7.74f, 5.5f, 1f);
            _object3d[7].addChild(hallKanan2);//tembok belakang

            float b = 13.5f;


            //Ruang 3
            _object3d[8] = new Asset3d();
            _object3d[8].createBoxVertices(0f, 2f, 0.0f + b, 10f, 5f, 0.1f);

            tembokKiri1 = new Asset3d();
            tembokKiri1.createBoxVertices(-5f, 2f, 5f + b, 0.1f, 5f, 10f);
            tembokKanan1 = new Asset3d();
            tembokKanan1.createBoxVertices(5f, 2f, 3.5f + b, 0.1f, 5f, 7f);
            tembokKanan12 = new Asset3d();
            tembokKanan12.createBoxVertices(5f, 4f, 8f + b, 0.1f, 3f, 2f);
            tembokKanan13 = new Asset3d();
            tembokKanan13.createBoxVertices(5f, 2f, 9.5f + b, 0.1f, 5f, 1f);
            tembokBlkg1 = new Asset3d();
            tembokBlkg1.createBoxVertices(0f, 2f, 10f + b, 10f, 5f, 0.1f);
            lantai1 = new Asset3d();
            lantai1.createBoxVertices(0f, -0.45f, 5f + b, 10f, 0.1f, 10f);
            atap1 = new Asset3d();
            atap1.createBoxVertices(0f, 4.45f, 5f + b, 10f, 0.1f, 10f);
            Asset3d bantal1 = new Asset3d();
            bantal1.createBoxVertices(-0.55f, 0.75f, 6.3f + b, 1f, 0.3f, 0.5f);
            Asset3d bantal2 = new Asset3d();
            bantal2.createBoxVertices(0.55f, 0.75f, 6.3f + b, 1f, 0.3f, 0.5f);

            _object3d[8].addChild(tembokKiri1);
            _object3d[8].addChild(tembokKanan1);
            _object3d[8].addChild(tembokKanan12);
            _object3d[8].addChild(tembokKanan13);
            _object3d[8].addChild(tembokBlkg1);
            _object3d[8].addChild(lantai1);
            _object3d[8].addChild(atap1);
            _object3d[8].addChild(bantal1);
            _object3d[8].addChild(bantal2);

            _object3d[9] = new Asset3d();
            _object3d[9].createBoxVertices(0f, 0.5f, 6.7f + b, 3f, 2.3f, 0.3f);
            Asset3d mejaP1 = new Asset3d();
            mejaP1.createBoxVertices(0.0f, 0.1f, 0.5f + b, 2.5f, 1f, 1f);
            _object3d[9].addChild(mejaP1);
            Asset3d tvTombol = new Asset3d();
            tvTombol.createBoxVertices(1.1f, 1.05f, 0.2f + b, 0.1f, 0.1f, 0.05f);
            _object3d[9].addChild(tvTombol);

            _object3d[10] = new Asset3d();
            _object3d[10].createBoxVertices(0f, 0.35f, 5.3f + b, 2.6f, 0.5f, 2.5f);
            Asset3d tvScreen = new Asset3d();
            tvScreen.createBoxVertices(0f, 2f, 0.2f + b, 2.6f, 1.7f, 0.1f);
            _object3d[10].addChild(tvScreen);

            _object3d[12] = new Asset3d();
            _object3d[12].createBoxVertices(0f, -0.3f, 5.3f + b, 2.6f, 0.8f, 2.5f);
            Asset3d tv1 = new Asset3d();
            tv1.createBoxVertices(0f, 2f, 0.1f + b, 3f, 2f, 0.2f);
            _object3d[12].addChild(tv1);


            Asset3d pFrame = new Asset3d();
            pFrame.createBoxVertices(-4.9f, 2f, 5f + b, 0.1f, 3f, 4.5f);
            _object3d[9].addChild(pFrame);
            Asset3d pCanvas = new Asset3d();
            pCanvas.createBoxVertices(-4.89f, 2f, 5f + b, 0.1f, 2.7f, 4.2f);
            _object3d[10].addChild(pCanvas);

            Asset3d pP1 = new Asset3d();
            Asset3d pP2 = new Asset3d();
            Asset3d pP3 = new Asset3d();
            Asset3d pP4 = new Asset3d();
            Asset3d pP5 = new Asset3d();
            Asset3d pP21 = new Asset3d();
            Asset3d pP22 = new Asset3d();
            Asset3d pP23 = new Asset3d();
            Asset3d pP24 = new Asset3d();
            Asset3d pP31 = new Asset3d();
            Asset3d pP32 = new Asset3d();
            Asset3d pP33 = new Asset3d();
            Asset3d pP34 = new Asset3d();
            Asset3d pP41 = new Asset3d();
            Asset3d pP42 = new Asset3d();
            Asset3d pP43 = new Asset3d();
            Asset3d pP44 = new Asset3d();
            Asset3d pP45 = new Asset3d();
            Asset3d pP51 = new Asset3d();
            Asset3d pP52 = new Asset3d();
            Asset3d pP53 = new Asset3d();
            Asset3d pP54 = new Asset3d();
            Asset3d pP55 = new Asset3d();
            pP1.createBoxVertices(-4.88f, 2f, 5f + b, 0.1f, 0.1f, 0.1f);
            pP2.createBoxVertices(-4.88f, 3f, 6f + b, 0.1f, 0.1f, 0.1f);
            pP3.createBoxVertices(-4.88f, 1f, 4f + b, 0.1f, 0.1f, 0.1f);
            pP4.createBoxVertices(-4.88f, 2f, 7f + b, 0.1f, 0.1f, 0.1f);
            pP5.createBoxVertices(-4.88f, 2f, 3f + b, 0.1f, 0.1f, 0.1f);
            pP21.createBoxVertices(-4.88f, 2.8f, 5.8f + b, 0.1f, 0.1f, 0.1f);
            pP22.createBoxVertices(-4.88f, 2.6f, 5.6f + b, 0.1f, 0.1f, 0.1f);
            pP23.createBoxVertices(-4.88f, 2.4f, 5.4f + b, 0.1f, 0.1f, 0.1f);
            pP24.createBoxVertices(-4.88f, 2.2f, 5.2f + b, 0.1f, 0.1f, 0.1f);
            pP31.createBoxVertices(-4.88f, 1.2f, 4.2f + b, 0.1f, 0.1f, 0.1f);
            pP32.createBoxVertices(-4.88f, 1.4f, 4.4f + b, 0.1f, 0.1f, 0.1f);
            pP33.createBoxVertices(-4.88f, 1.6f, 4.6f + b, 0.1f, 0.1f, 0.1f);
            pP34.createBoxVertices(-4.88f, 1.8f, 4.8f + b, 0.1f, 0.1f, 0.1f);
            pP41.createBoxVertices(-4.88f, 2f, 6.67f + b, 0.1f, 0.1f, 0.1f);
            pP42.createBoxVertices(-4.88f, 2f, 6.33f + b, 0.1f, 0.1f, 0.1f);
            pP43.createBoxVertices(-4.88f, 2f, 6f + b, 0.1f, 0.1f, 0.1f);
            pP44.createBoxVertices(-4.88f, 2f, 5.67f + b, 0.1f, 0.1f, 0.1f);
            pP45.createBoxVertices(-4.88f, 2f, 5.33f + b, 0.1f, 0.1f, 0.1f);
            pP51.createBoxVertices(-4.88f, 2f, 3.33f + b, 0.1f, 0.1f, 0.1f);
            pP52.createBoxVertices(-4.88f, 2f, 3.67f + b, 0.1f, 0.1f, 0.1f);
            pP53.createBoxVertices(-4.88f, 2f, 4f + b, 0.1f, 0.1f, 0.1f);
            pP54.createBoxVertices(-4.88f, 2f, 4.33f + b, 0.1f, 0.1f, 0.1f);
            pP55.createBoxVertices(-4.88f, 2f, 4.67f + b, 0.1f, 0.1f, 0.1f);
            _object3d[12].addChild(pP1);
            _object3d[12].addChild(pP2);
            _object3d[12].addChild(pP3);
            _object3d[12].addChild(pP4);
            _object3d[12].addChild(pP5);
            _object3d[12].addChild(pP21);
            _object3d[12].addChild(pP22);
            _object3d[12].addChild(pP23);
            _object3d[12].addChild(pP24);
            _object3d[12].addChild(pP31);
            _object3d[12].addChild(pP32);
            _object3d[12].addChild(pP33);
            _object3d[12].addChild(pP34);
            _object3d[12].addChild(pP41);
            _object3d[12].addChild(pP42);
            _object3d[12].addChild(pP43);
            _object3d[12].addChild(pP44);
            _object3d[12].addChild(pP45);
            _object3d[12].addChild(pP51);
            _object3d[12].addChild(pP52);
            _object3d[12].addChild(pP53);
            _object3d[12].addChild(pP54);
            _object3d[12].addChild(pP55);






            //People
            //Head
            _object3d[11] = new Asset3d();
            _object3d[11].createBoxVertices(0, 1, 7.5f, 0.5f, 0.5f, 0.5f);
            //Body
            Asset3d body = new Asset3d();
            body.createBoxVertices(0,0, 7.5f, 0.5f, 1.25f, 0.5f);
            _object3d[11].addChild(body);





            for (int i = 0; i < _object3d.Length; i++)
            {
                if (_object3d[i] != null)
                {
                    _object3d[i].load(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);
                }
            }
            //0.8567858f, 1.2050177f, 8.05813f
            _camera = new Camera(new Vector3(-0.8567858f, 1.2050177f, 8.05813f), Size.X / Size.Y);
            _objecPost = _object3d[11]._centerPosition;
            _camera._front = -Vector3.Normalize(_camera.Position - _objecPost);
            _camera.Yaw += 85;
            CursorGrabbed = true;

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            Count += 1;
            base.OnRenderFrame(args);
            _time = (float)args.Time;
            _intime += (float)args.Time * 10;
            _objecPost = _object3d[11]._centerPosition;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 temp = Matrix4.Identity;
            for (int i = 0; i < LightObject.Length; i++) {
                if (LightObject[i] != null)
                {
                    LightObject[i].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
                    _pointLightPositions[i] = LightObject[i]._centerPosition;
                }
                    
            }

            for (int i = 0; i < _object3d.Length; i++)
            {
                if (_object3d[i] != null)
                {
                    _object3d[i].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
                    _object3d[i].setDirectionalLight(new Vector3(0.0f, -1, 0.0f), new Vector3(0.1f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.5f));
                    _object3d[i].setSpotLight(_camera.Position, _camera.Front, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f),
                    1.0f, 0.09f, 0.032f, MathF.Cos(MathHelper.DegreesToRadians(12.5f)), MathF.Cos(MathHelper.DegreesToRadians(17.5f)));
                    _object3d[i].setPointLights(_pointLightPositions, new Vector3(0.05f, 0.05f, 0.05f), point_light_color_difuse, new Vector3(1.0f, 1.0f, 1.0f), 1.0f, 0.09f, 0.032f);
                }
            }


            //Warna
            _object3d[0].setFragVariable(new Vector3(0.7f, 0.7f, 0.7f), new Vector3(0.7f, 0.7f, 0.7f),  _camera.Position);
            _object3d[1].setFragVariable(new Vector3(0f, 0f, 0.7f), new Vector3(0f, 0f, 0.7f), _camera.Position);
            _object3d[2].setFragVariable(new Vector3(0.6f, 0.3f, 0f), new Vector3(0.6f, 0.3f, 0f), _camera.Position);
            _object3d[3].setFragVariable(new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), _camera.Position);
            _object3d[4].setFragVariable(new Vector3(0f, 0f, 0f), new Vector3(1f, 1f, 1f), _camera.Position);

            _object3d[5].setFragVariable(new Vector3(0.3f, 0.3f, 0.3f), new Vector3(0.3f, 0.3f, 0.3f), _camera.Position);
            _object3d[6].setFragVariable(new Vector3(0.55f, 0.46f, 0.46f), new Vector3(0.55f, 0.46f, 0.46f), _camera.Position);
            _object3d[7].setFragVariable(new Vector3(0.3f, 0.3f, 0.3f), new Vector3(0.3f, 0.3f, 0.3f), _camera.Position);
            _object3d[8].setFragVariable(new Vector3(0.7f, 0.7f, 0.7f), new Vector3(0.7f, 0.7f, 0.7f), _camera.Position);
            _object3d[9].setFragVariable(new Vector3(0f, 0f, 0.7f), new Vector3(0f, 0f, 0.7f), _camera.Position);
            _object3d[10].setFragVariable(new Vector3(0.6f, 0.3f, 0f), new Vector3(0.6f, 0.3f, 0f), _camera.Position);

            _object3d[8].setFragVariable(new Vector3(0.4f, 0.4f, 0.4f), new Vector3(0.4f, 0.4f, 0.4f), _camera.Position);
            //_object3d[9].setFragVariable(new Vector3(0f, 0f, 0.7f), _camera.Position);
            _object3d[9].setFragVariable(new Vector3(0.6f, 0.3f, 0f), new Vector3(0.6f, 0.3f, 0f), _camera.Position);
            _object3d[10].setFragVariable(new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f), _camera.Position);
            _object3d[12].setFragVariable(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), _camera.Position);

            _object3d[11].setFragVariable(new Vector3(0.5137f, 0.93f, 1f), new Vector3(0.5137f, 0.93f, 1f), _camera.Position);



            int childd = 0;
            if (count == 0)
            {
                speed = 0.005f;
                right = true;

            }
            else if (count == 100)
            {
                right = false;
                speed = -0.005f;
            }

            if (right)
            {
                count++;
            }
            else
            {
                count--;
            }

            _object3d[6].translate(speed * 2, 0, 0);
            foreach (var child in _object3d[6].Child)
            {
                if (childd == 0 || childd == 1)
                {
                    child.translate(speed, 0, 0);
                }
                else
                {
                    child.translate(speed * 2, 0, 0);
                }
                childd++;
            }
            SwapBuffers();
            walk += 1;
            //walk = 8375;
            if (walk < 200)
            {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objecPost;
                _camera.Yaw -= _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, -_rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;
                _object3d[11].rotatede(_object3d[11]._centerPosition, _object3d[11]._euler[1], 0.4f);
            }
            else if (walk < 400) { }
            else if (walk < 600)
            {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objecPost;
                _camera.Yaw += _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, _rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;
                _object3d[11].rotatede(_object3d[11]._centerPosition, new Vector3(0, 1, 0), -0.4f);
            }
            else if (walk < 1200) {
                _object3d[11].translate(0.01f, 0, 0);
                foreach (var obj in _object3d[11].Child)
                {
                    obj.translate(0.01f, 0, 0);
                }
                //-0.8567858f, 1.2050177f, 8.05813f
                _camera.Position = _camera.Position + new Vector3(0.01f, 0, 0);
            }
            else if (walk < 1350) {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objecPost;
                _camera.Yaw -= _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, -_rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;
                _object3d[11].rotatede(_object3d[11]._centerPosition, _object3d[11]._euler[1], 0.4f);
            }
            else if (walk < 1450) { }

            else if (walk < 1600) {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objecPost;
                _camera.Yaw += _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, _rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;
                _object3d[11].rotatede(_object3d[11]._centerPosition, new Vector3(0, 1, 0), -0.4f);
            }
            else if (walk < 2425) {
                _object3d[11].translate(0.01f, 0, 0);
                foreach (var obj in _object3d[11].Child)
                {
                    obj.translate(0.01f, 0, 0);
                }
                //-0.8567858f, 1.2050177f, 8.05813f
                _camera.Position = _camera.Position + new Vector3(0.01f, 0, 0);
            }
            else if (walk < 2650) {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objecPost;
                _camera.Yaw += _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, _rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;
                _object3d[11].rotatede(_object3d[11]._centerPosition, new Vector3(0, 1, 0), -0.4f);
            }
            else if (walk < 3025) {
                _object3d[11].translate(0, 0, 0.01f);
                foreach (var obj in _object3d[11].Child)
                {
                    obj.translate(0, 0, 0.01f);
                }
                //-0.8567858f, 1.2050177f, 8.05813f
                _camera.Position = _camera.Position + new Vector3(0, 0, 0.01f);
            }

            else if (walk < 3075) {
                var axis = new Vector3(1, 0, 0);
                _camera.Position -= _objecPost;
                _camera.Pitch += _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, -_rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;
                _object3d[11].rotatede(_object3d[11]._centerPosition, _object3d[11]._euler[2], 0.4f);
            }
            else if (walk < 3175) { }

            else if (walk < 3225)
            {
                var axis = new Vector3(1, 0, 0);
                _camera.Position -= _objecPost;
                _camera.Pitch -= _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, _rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;
                _object3d[11].rotatede(_object3d[11]._centerPosition, _object3d[11]._euler[2], -0.4f);
            }



            else if (walk < 4000) { }

            else if (walk <5025) {
                _object3d[11].translate(0, 0, 0.01f);
                foreach (var obj in _object3d[11].Child)
                {
                    obj.translate(0, 0, 0.01f);
                }
                //-0.8567858f, 1.2050177f, 8.05813f
                _camera.Position = _camera.Position + new Vector3(0, 0, 0.01f);
            }
            else if (walk < 5250) {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objecPost;
                _camera.Yaw += _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, _rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;
                _object3d[11].rotatede(_object3d[11]._centerPosition, new Vector3(0, 1, 0), -0.4f);
            }
            else if (walk < 6900) {
                _object3d[11].translate(-0.01f, 0, 0);
                foreach (var obj in _object3d[11].Child)
                {
                    obj.translate(-0.01f, 0, 0);
                }
                //-0.8567858f, 1.2050177f, 8.05813f
                _camera.Position = _camera.Position + new Vector3(-0.01f, 0, 0);
            }
            else if (walk < 7125) {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objecPost;
                _camera.Yaw += _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, _rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;
                _object3d[11].rotatede(_object3d[11]._centerPosition, new Vector3(0, 1, 0), -0.4f);
            }
            else if (walk < 7650) {
                _object3d[11].translate(0, 0, -0.01f);
                foreach (var obj in _object3d[11].Child)
                {
                    obj.translate(0, 0, -0.01f);
                }
                //-0.8567858f, 1.2050177f, 8.05813f
                _camera.Position = _camera.Position + new Vector3(0, 0, -0.01f);
            }
            else if (walk < 7875) {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objecPost;
                _camera.Yaw += _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, _rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;
                _object3d[11].rotatede(_object3d[11]._centerPosition, new Vector3(0, 1, 0), -0.4f);
            }
            else if (walk < 8150) {
                _object3d[11].translate(0.01f, 0, 0);
                foreach (var obj in _object3d[11].Child)
                {
                    obj.translate(0.01f, 0, 0);
                }
                //-0.8567858f, 1.2050177f, 8.05813f
                _camera.Position = _camera.Position + new Vector3(0.01f, 0, 0);
            }
            else if (walk < 8375) {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objecPost;
                _camera.Yaw -= _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, -_rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;
                _object3d[11].rotatede(_object3d[11]._centerPosition, _object3d[11]._euler[1], 0.4f);
            }
            else if (walk < 8475) {
                _camera.Position = _camera.Position + new Vector3(0, 0, 0.01f);

            }
            else if (walk < 8520)
            {
                _camera.Position = _camera.Position + new Vector3(-0.01f, 0, 0f);


            }
            else if (walk < 8585)
            {
                var axis = new Vector3(1, 0, 0);
                _camera.Position -= _objecPost;
                _camera.Pitch -= _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, _rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;

            }
            else if (walk < 10000) {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objecPost;
                _camera.Yaw += _rotationSpeed * 1.5f;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, _rotationSpeed * 1.5f).ExtractRotation());
                _camera.Position += _objecPost;
            }


        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
            _camera.AspectRatio = Size.X / (float)Size.Y;
        }

        public bool CheckCollision(Vector3 objPosition, float size, Vector3 objPosition2, float sizex, float sizey, float sizez)
        {
            bool collisionX = objPosition.X + sizex >= objPosition2.X && objPosition2.X >= objPosition.X - sizex;
            bool collisionY = objPosition.Y + sizey >= objPosition2.Y && objPosition2.Y >= objPosition.Y - sizey;
            bool collisionZ = objPosition.Z + sizez >= objPosition2.Z && objPosition2.Z >= objPosition.Z - sizez;

            return collisionX && collisionY && collisionZ;
        }

        public bool isThisCollision(Vector3 objPosition)
        {
            foreach (var object3d in _object3d)
            {
                if (object3d != null)
                {
                    bool a = CheckCollision(object3d._centerPosition, 0.5f, objPosition, object3d._sizex, object3d._sizey, object3d._sizez);
                    if (a)
                    {
                        return true;
                    }
                    foreach (var child in object3d.Child)
                    {
                        a = CheckCollision(child._centerPosition, 0.5f, objPosition, child._sizex, child._sizey, child._sizez);
                        if (a)
                        {

                            return true;
                        }

                    }
                }
            }
            return false;
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            float cameraSpeed = 3f;
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                var cameraafter = _camera.Position + _camera.Front * cameraSpeed * (float)args.Time;
                if (moveplayer == false)
                {
                    if (!isThisCollision(cameraafter))
                    {
                        _camera.Position += _camera.Front * cameraSpeed * (float)args.Time;
                    }
                    if (isThisCollision(_camera.Position) && !isThisCollision(cameraafter))
                    {
                        _camera.Position += _camera.Front * cameraSpeed * (float)args.Time;
                    }
                }
                else
                {
                    _object3d[11].translate(0.01f, 0, 0);
                    foreach (var obj in _object3d[11].Child)
                    {
                        obj.translate(0.01f, 0, 0);
                    }
                    //-0.8567858f, 1.2050177f, 8.05813f
                    _camera.Position = _camera.Position + new Vector3(0.01f, 0, 0);


                }


                Console.WriteLine("Pos" + _camera.Position);
                Console.WriteLine(_camera.Position);
                Console.WriteLine(_camera.Front);
                Console.WriteLine("Fro" + _camera.Front);
            }
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                var cameraafter = _camera.Position + _camera.Front * cameraSpeed * (float)args.Time;
                if (moveplayer == false)
                {
                    if (!isThisCollision(cameraafter))
                    {
                        _camera.Position -= _camera.Front * cameraSpeed * (float)args.Time;
                    }
                    if (isThisCollision(_camera.Position) && !isThisCollision(cameraafter))
                    {
                        _camera.Position -= _camera.Front * cameraSpeed * (float)args.Time;
                    }
                }
                else
                {
                    _object3d[11].translate(-0.01f, 0, 0);
                    foreach (var obj in _object3d[11].Child)
                    {
                        obj.translate(-0.01f, 0, 0);
                    }
                    //-0.8567858f, 1.2050177f, 8.05813f
                    _camera.Position = _camera.Position + new Vector3(-0.01f, 0, 0);


                }
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                var cameraafter = _camera.Position - _camera.Right * cameraSpeed * (float)args.Time;
                if (moveplayer == false)
                {
                    if (!isThisCollision(cameraafter))
                    {
                        _camera.Position -= _camera.Right * cameraSpeed * (float)args.Time;
                    }
                    if (isThisCollision(_camera.Position) && !isThisCollision(cameraafter))
                    {
                        _camera.Position -= _camera.Right * cameraSpeed * (float)args.Time;
                    }
                }

                else
                {
                    _object3d[11].translate(0, 0, -0.01f);
                    foreach (var obj in _object3d[11].Child)
                    {
                        obj.translate(0, 0, -0.01f);
                    }
                    //-0.8567858f, 1.2050177f, 8.05813f
                    _camera.Position = _camera.Position + new Vector3(0, 0, -0.01f);


                }
            }
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                var cameraafter = _camera.Position + _camera.Right * cameraSpeed * (float)args.Time;
                if (moveplayer == false)
                {
                    if (!isThisCollision(cameraafter))
                    {
                        _camera.Position += _camera.Right * cameraSpeed * (float)args.Time;
                    }
                    if (isThisCollision(_camera.Position) && !isThisCollision(cameraafter))
                    {
                        _camera.Position += _camera.Right * cameraSpeed * (float)args.Time;
                    }
                }
                else
                {
                    _object3d[11].translate(0, 0, 0.01f);
                    foreach (var obj in _object3d[11].Child)
                    {
                        obj.translate(0, 0, 0.01f);
                    }
                    //-0.8567858f, 1.2050177f, 8.05813f
                    _camera.Position = _camera.Position + new Vector3(0, 0, 0.01f);


                }
            }
            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                var cameraafter = _camera.Position + _camera.Up * cameraSpeed * (float)args.Time;
                if(moveplayer == false)
                {
                    if (!isThisCollision(cameraafter))
                    {
                        _camera.Position += _camera.Up * cameraSpeed * (float)args.Time;
                    }
                    if (isThisCollision(_camera.Position) && !isThisCollision(cameraafter))
                    {
                        _camera.Position += _camera.Up * cameraSpeed * (float)args.Time;
                    }
                }
            }
            if (KeyboardState.IsKeyDown(Keys.LeftShift))
            {
                var cameraafter = _camera.Position - _camera.Up * cameraSpeed * (float)args.Time;
                if (moveplayer == false)
                {
                    if (!isThisCollision(cameraafter))
                    {
                        _camera.Position -= _camera.Up * cameraSpeed * (float)args.Time;
                    }
                    if (isThisCollision(_camera.Position) && !isThisCollision(cameraafter))
                    {
                        _camera.Position -= _camera.Up * cameraSpeed * (float)args.Time;
                    }
                }
            }

            if (KeyboardState.IsKeyPressed(Keys.P))
            {
                moveplayer = !moveplayer;
            }

            var mouse = MouseState;
            var sensitivity = 0.2f;
            var headSens = 0.4f;
            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);
                if (moveplayer == false)
                {
                    _camera.Yaw += deltaX * sensitivity;
                    //_object3d[11].rotate(_object3d[11]._centerPosition, _object3d[11]._euler[1], deltaX * headSens);
                    _camera.Pitch -= deltaY * sensitivity;
                }

            }

            if (KeyboardState.IsKeyDown(Keys.Right))
            {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objecPost;
                _camera.Yaw += _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, _rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;
                _object3d[11].rotatede(_object3d[11]._centerPosition, new Vector3(0, 1, 0), -headSens);

                //_camera._front = -Vector3.Normalize(_camera.Position - _objecPost);
            }
            if (KeyboardState.IsKeyDown(Keys.Left))
            {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objecPost;
                _camera.Yaw -= _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, -_rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;
                _object3d[11].rotatede(_object3d[11]._centerPosition, _object3d[11]._euler[1], headSens);


                //_camera._front = -Vector3.Normalize(_camera.Position - _objecPost);
            }
            if (KeyboardState.IsKeyDown(Keys.Down))
            {
                var axis = new Vector3(1, 0, 0);
                _camera.Position -= _objecPost;
                _camera.Pitch -= _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, _rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;
                _object3d[11].rotatede(_object3d[11]._centerPosition, _object3d[11]._euler[2], -headSens);

                //_camera._front = -Vector3.Normalize(_camera.Position - _objecPost);
            }
            if (KeyboardState.IsKeyDown(Keys.Up))
            {
                var axis = new Vector3(1, 0, 0);
                _camera.Position -= _objecPost;
                _camera.Pitch += _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, -_rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;
                _object3d[11].rotatede(_object3d[11]._centerPosition, _object3d[11]._euler[2], headSens);

                //_camera._front = -Vector3.Normalize(_camera.Position - _objecPost);
            }
            if (KeyboardState.IsKeyDown(Keys.N))
            {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objecPost;
                _camera.Yaw += _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, _rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;

                //_camera._front = -Vector3.Normalize(_camera.Position - _objecPost);
            }
            if (KeyboardState.IsKeyDown(Keys.Comma))
            {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objecPost;
                _camera.Yaw -= _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, -_rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;

                //_camera._front = -Vector3.Normalize(_camera.Position - _objecPost);
            }
            if (KeyboardState.IsKeyDown(Keys.K))
            {
                var axis = new Vector3(1, 0, 0);
                _camera.Position -= _objecPost;
                _camera.Pitch -= _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, _rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;
                //_camera._front = -Vector3.Normalize(_camera.Position - _objecPost);
            }
            if (KeyboardState.IsKeyDown(Keys.M))
            {
                var axis = new Vector3(1, 0, 0);
                _camera.Position -= _objecPost;
                _camera.Pitch += _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objecPost, -_rotationSpeed).ExtractRotation());
                _camera.Position += _objecPost;
                //_camera._front = -Vector3.Normalize(_camera.Position - _objecPost);
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButton.Left)
            {
                float _x = (MousePosition.X - Size.X / 2) / (Size.X / 2);
                float _y = -(MousePosition.Y - Size.Y / 2) / (Size.Y / 2);

                Console.WriteLine("x = " + _x + "y = " + _y);
            }
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            Console.WriteLine("Offset Y: " + e.OffsetY);
            Console.WriteLine("Offset X: " + e.OffsetX);
            _camera.Fov = _camera.Fov - e.OffsetY;
        }


    }
}
