﻿using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace test1
{
    internal class Asset3d
    {
        List<Vector3> _vertices = new List<Vector3>();
        List<uint> _indices = new List<uint>();
        int _vertexBufferObject;
        int _vertexArrayObject;
        int _elementBufferObject;
        Shader _shader;
        Matrix4 _view;
        Matrix4 _projection;
        Matrix4 _model;
        public Vector3 _centerPosition;
        public List<Vector3> _euler;
        public List<Asset3d> Child;
        public float _sizex;
        public float _sizey;
        public float _sizez;
        float hitbox = 1.9f;

        public Asset3d(List<Vector3> vertices, List<uint> indices)
        {
            _vertices = vertices;
            _indices = indices;
            setdefault();
        }
        public Asset3d()
        {
            _vertices = new List<Vector3>();
            setdefault();
        }
        public void setdefault()
        {
            _euler = new List<Vector3>();
            //sumbu X
            _euler.Add(new Vector3(1, 0, 0));
            //sumbu y
            _euler.Add(new Vector3(0, 1, 0));
            //sumbu z
            _euler.Add(new Vector3(0, 0, 1));
            _model = Matrix4.Identity;
            _centerPosition = new Vector3(0, 0, 0);
            Child = new List<Asset3d>();

        }
        
        public void load(string shadervert, string shaderfrag, float Size_x, float Size_y)
        {
            //Buffer
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count
                * Vector3.SizeInBytes, _vertices.ToArray(), BufferUsageHint.StaticDraw);
            //VAO
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            //kalau mau bikin object settingannya beda dikasih if
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float,
                false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float,
                false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            //ada data yang disimpan di _indices
            if (_indices.Count != 0)
            {
                _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Count
                    * sizeof(uint), _indices.ToArray(), BufferUsageHint.StaticDraw);
            }
            _shader = new Shader(shadervert, shaderfrag);
            _shader.Use();

            _view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);

            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size_x / (float)Size_y, 0.1f, 100.0f);
            foreach (var item in Child)
            {
                item.load(shadervert, shaderfrag, Size_x, Size_y);
            }
        }
        public void render(int _lines, double time, Matrix4 temp, Matrix4 camera_view, Matrix4 camera_projection)
        {
            _shader.Use();
            GL.BindVertexArray(_vertexArrayObject);
            //_model = _model * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(time));
            //_model = temp;

            _shader.SetMatrix4("model", _model);
            _shader.SetMatrix4("view", camera_view);
            _shader.SetMatrix4("projection", camera_projection);


            if (_indices.Count != 0)
            {
                GL.DrawElements(PrimitiveType.Triangles, _indices.Count, DrawElementsType.UnsignedInt, 0);
                Console.WriteLine("tes");
            }
            else
            {

                if (_lines == 0)
                {
                    GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Count);
                }
                else if (_lines == 1)
                {
                    GL.DrawArrays(PrimitiveType.TriangleFan, 0, _vertices.Count);
                }
                else if (_lines == 2)
                {

                }
                else if (_lines == 3)
                {
                    GL.DrawArrays(PrimitiveType.LineStrip, 0, _vertices.Count);
                }
            }
            foreach (var item in Child)
            {
                item.render(_lines, time, temp, camera_view, camera_projection);
            }
        }

        public void createBoxVertices(float x, float y, float z, float lengthx, float lengthy, float lengthz)
        {
            _centerPosition.X = x;
            _centerPosition.Y = y;
            _centerPosition.Z = z;
            Vector3 temp_vector;

            _sizex = lengthx / hitbox;
            _sizey = lengthy / hitbox;
            _sizez = lengthz / hitbox;

            //FRONT FACE

            temp_vector.X = x - lengthx / 2.0f;
            temp_vector.Y = y + lengthy / 2.0f;
            temp_vector.Z = z - lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, -1.0f));


            temp_vector.X = x + lengthx / 2.0f;
            temp_vector.Y = y + lengthy / 2.0f;
            temp_vector.Z = z - lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, -1.0f));

            temp_vector.X = x - lengthx / 2.0f;
            temp_vector.Y = y - lengthy / 2.0f;
            temp_vector.Z = z - lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, -1.0f));

            temp_vector.X = x + lengthx / 2.0f;
            temp_vector.Y = y + lengthy / 2.0f;
            temp_vector.Z = z - lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, -1.0f));

            temp_vector.X = x - lengthx / 2.0f;
            temp_vector.Y = y - lengthy / 2.0f;
            temp_vector.Z = z - lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, -1.0f));

            temp_vector.X = x + lengthx / 2.0f;
            temp_vector.Y = y - lengthy / 2.0f;
            temp_vector.Z = z - lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, -1.0f));

            //BACK FACE
            //TITIK 5
            temp_vector.X = x - lengthx / 2.0f;
            temp_vector.Y = y + lengthy / 2.0f;
            temp_vector.Z = z + lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, 1.0f));

            //TITIK 6
            temp_vector.X = x + lengthx / 2.0f;
            temp_vector.Y = y + lengthy / 2.0f;
            temp_vector.Z = z + lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, 1.0f));
            //TITIK 7
            temp_vector.X = x - lengthx / 2.0f;
            temp_vector.Y = y - lengthy / 2.0f;
            temp_vector.Z = z + lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, 1.0f));

            //TITIK 6
            temp_vector.X = x + lengthx / 2.0f;
            temp_vector.Y = y + lengthy / 2.0f;
            temp_vector.Z = z + lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, 1.0f));
            //TITIK 7
            temp_vector.X = x - lengthx / 2.0f;
            temp_vector.Y = y - lengthy / 2.0f;
            temp_vector.Z = z + lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, 1.0f));

            //TITIK 8
            temp_vector.X = x + lengthx / 2.0f;
            temp_vector.Y = y - lengthy / 2.0f;
            temp_vector.Z = z + lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, 1.0f));

            //LEFT FACE
            //TITIK 1
            temp_vector.X = x - lengthx / 2.0f;
            temp_vector.Y = y + lengthy / 2.0f;
            temp_vector.Z = z - lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(-1.0f, 0.0f, 0.0f));
            //TITIK 3
            temp_vector.X = x - lengthx / 2.0f;
            temp_vector.Y = y - lengthy / 2.0f;
            temp_vector.Z = z - lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(-1.0f, 0.0f, 0.0f));
            //TITIK 5
            temp_vector.X = x - lengthx / 2.0f;
            temp_vector.Y = y + lengthy / 2.0f;
            temp_vector.Z = z + lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(-1.0f, 0.0f, 0.0f));
            //TITIK 3
            temp_vector.X = x - lengthx / 2.0f;
            temp_vector.Y = y - lengthy / 2.0f;
            temp_vector.Z = z - lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(-1.0f, 0.0f, 0.0f));
            //TITIK 5
            temp_vector.X = x - lengthx / 2.0f;
            temp_vector.Y = y + lengthy / 2.0f;
            temp_vector.Z = z + lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(-1.0f, 0.0f, 0.0f));
            //TITIK 7
            temp_vector.X = x - lengthx / 2.0f;
            temp_vector.Y = y - lengthy / 2.0f;
            temp_vector.Z = z + lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(-1.0f, 0.0f, 0.0f));

            //RIGHT FACE
            //TITIK 2
            temp_vector.X = x + lengthx / 2.0f;
            temp_vector.Y = y + lengthy / 2.0f;
            temp_vector.Z = z - lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(1.0f, 0.0f, 0.0f));
            //TITIK 4
            temp_vector.X = x + lengthx / 2.0f;
            temp_vector.Y = y - lengthy / 2.0f;
            temp_vector.Z = z - lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(1.0f, 0.0f, 0.0f));
            //TITIK 6
            temp_vector.X = x + lengthx / 2.0f;
            temp_vector.Y = y + lengthy / 2.0f;
            temp_vector.Z = z + lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(1.0f, 0.0f, 0.0f));
            //TITIK 4
            temp_vector.X = x + lengthx / 2.0f;
            temp_vector.Y = y - lengthy / 2.0f;
            temp_vector.Z = z - lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(1.0f, 0.0f, 0.0f));
            //TITIK 6
            temp_vector.X = x + lengthx / 2.0f;
            temp_vector.Y = y + lengthy / 2.0f;
            temp_vector.Z = z + lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(1.0f, 0.0f, 0.0f));
            //TITIK 8
            temp_vector.X = x + lengthx / 2.0f;
            temp_vector.Y = y - lengthy / 2.0f;
            temp_vector.Z = z + lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(1.0f, 0.0f, 0.0f));

            //BOTTOM FACES
            //TITIK 3
            temp_vector.X = x - lengthx / 2.0f;
            temp_vector.Y = y - lengthy / 2.0f;
            temp_vector.Z = z - lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, -1.0f, 0.0f));
            //TITIK 4
            temp_vector.X = x + lengthx / 2.0f;
            temp_vector.Y = y - lengthy / 2.0f;
            temp_vector.Z = z - lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, -1.0f, 0.0f));
            //TITIK 7
            temp_vector.X = x - lengthx / 2.0f;
            temp_vector.Y = y - lengthy / 2.0f;
            temp_vector.Z = z + lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, -1.0f, 0.0f));
            //TITIK 4
            temp_vector.X = x + lengthx / 2.0f;
            temp_vector.Y = y - lengthy / 2.0f;
            temp_vector.Z = z - lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, -1.0f, 0.0f));
            //TITIK 7
            temp_vector.X = x - lengthx / 2.0f;
            temp_vector.Y = y - lengthy / 2.0f;
            temp_vector.Z = z + lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, -1.0f, 0.0f));
            //TITIK 8
            temp_vector.X = x + lengthx / 2.0f;
            temp_vector.Y = y - lengthy / 2.0f;
            temp_vector.Z = z + lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, -1.0f, 0.0f));

            //TOP FACES
            //TITIK 1
            temp_vector.X = x - lengthx / 2.0f;
            temp_vector.Y = y + lengthy / 2.0f;
            temp_vector.Z = z - lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 1.0f, 0.0f));
            //TITIK 2
            temp_vector.X = x + lengthx / 2.0f;
            temp_vector.Y = y + lengthy / 2.0f;
            temp_vector.Z = z - lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 1.0f, 0.0f));
            //TITIK 5
            temp_vector.X = x - lengthx / 2.0f;
            temp_vector.Y = y + lengthy / 2.0f;
            temp_vector.Z = z + lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 1.0f, 0.0f));
            //TITIK 2
            temp_vector.X = x + lengthx / 2.0f;
            temp_vector.Y = y + lengthy / 2.0f;
            temp_vector.Z = z - lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 1.0f, 0.0f));
            //TITIK 5
            temp_vector.X = x - lengthx / 2.0f;
            temp_vector.Y = y + lengthy / 2.0f;
            temp_vector.Z = z + lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 1.0f, 0.0f));
            //TITIK 6
            temp_vector.X = x + lengthx / 2.0f;
            temp_vector.Y = y + lengthy / 2.0f;
            temp_vector.Z = z + lengthz / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 1.0f, 0.0f));
        }
        public void createBoxVertices2(float x, float y, float z, float length)
        {
            _centerPosition.X = x;
            _centerPosition.Y = y;
            _centerPosition.Z = z;
            Vector3 temp_vector;

            _sizex = length / hitbox;
            _sizey = length / hitbox;
            _sizez = length / hitbox;

            //FRONT FACE

            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, -1.0f));


            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, -1.0f));

            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, -1.0f));

            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, -1.0f));

            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, -1.0f));

            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, -1.0f));

            //BACK FACE
            //TITIK 5
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, 1.0f));

            //TITIK 6
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, 1.0f));
            //TITIK 7
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, 1.0f));

            //TITIK 6
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, 1.0f));
            //TITIK 7
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, 1.0f));

            //TITIK 8
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, 1.0f));

            //LEFT FACE
            //TITIK 1
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(-1.0f, 0.0f, 0.0f));
            //TITIK 3
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(-1.0f, 0.0f, 0.0f));
            //TITIK 5
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(-1.0f, 0.0f, 0.0f));
            //TITIK 3
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(-1.0f, 0.0f, 0.0f));
            //TITIK 5
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(-1.0f, 0.0f, 0.0f));
            //TITIK 7
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(-1.0f, 0.0f, 0.0f));

            //RIGHT FACE
            //TITIK 2
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(1.0f, 0.0f, 0.0f));
            //TITIK 4
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(1.0f, 0.0f, 0.0f));
            //TITIK 6
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(1.0f, 0.0f, 0.0f));
            //TITIK 4
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(1.0f, 0.0f, 0.0f));
            //TITIK 6
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(1.0f, 0.0f, 0.0f));
            //TITIK 8
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(1.0f, 0.0f, 0.0f));

            //BOTTOM FACES
            //TITIK 3
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, -1.0f, 0.0f));
            //TITIK 4
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, -1.0f, 0.0f));
            //TITIK 7
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, -1.0f, 0.0f));
            //TITIK 4
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, -1.0f, 0.0f));
            //TITIK 7
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, -1.0f, 0.0f));
            //TITIK 8
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, -1.0f, 0.0f));

            //TOP FACES
            //TITIK 1
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 1.0f, 0.0f));
            //TITIK 2
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 1.0f, 0.0f));
            //TITIK 5
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 1.0f, 0.0f));
            //TITIK 2
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 1.0f, 0.0f));
            //TITIK 5
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 1.0f, 0.0f));
            //TITIK 6
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 1.0f, 0.0f));
        }
        public void createEllipsoid(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z)
        {
            _centerPosition.X = _x;
            _centerPosition.Y = _y;
            _centerPosition.Z = _z;
            float pi = (float)Math.PI;
            Vector3 temp_vector;
            _sizex = radiusX / hitbox;
            _sizey = radiusY / hitbox;
            _sizez = radiusZ / hitbox;

            for (float u = -pi; u <= pi; u += pi / 300)
            {
                for (float v = -pi / 2; v <= pi / 2; v += pi / 300)
                {
                    temp_vector.X = _x + (float)Math.Cos(v) * (float)Math.Cos(u) * radiusX;
                    temp_vector.Y = _y + (float)Math.Cos(v) * (float)Math.Sin(u) * radiusY;
                    temp_vector.Z = _z + (float)Math.Sin(v) * radiusZ;
                    _vertices.Add(temp_vector);
                }
            }
        }
        public void createEllipsoid2(float _x, float _y, float _z, float radiusX, float radiusY, float radiusZ, int sectorCount, int stackCount)
        {
            float pi = (float)Math.PI;
            Vector3 temp_vector;
            _sizex = radiusX / hitbox;
            _sizey = radiusY / hitbox;
            _sizez = radiusZ / hitbox;

            float sectorStep = 2 * (float)Math.PI / sectorCount;
            float stackStep = (float)Math.PI / stackCount;
            float sectorAngle, StackAngle, x, y, z;

            for (int i = 0; i <= stackCount; ++i)
            {
                StackAngle = pi / 2 - i * stackStep;
                x = radiusX * (float)Math.Cos(StackAngle);
                y = radiusY * (float)Math.Cos(StackAngle);
                z = radiusZ * (float)Math.Sin(StackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x * (float)Math.Cos(sectorAngle) + _x;
                    temp_vector.Y = y * (float)Math.Sin(sectorAngle) + _y;
                    temp_vector.Z = z + _z;
                    _vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);
                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        _indices.Add(k1);
                        _indices.Add(k2);
                        _indices.Add(k1 + 1);
                    }
                    if (i != (stackCount - 1))
                    {
                        _indices.Add(k1 + 1);
                        _indices.Add(k2);
                        _indices.Add(k2 + 1);
                    }
                }
            }
        }

        public void createCone(float x, float y, float z, float radX, float radY, float radZ, float sectorCount, float stackCount)
        {
            _centerPosition = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radX * (float)Math.Cos(stackAngle);
                tempY = radY * (float)Math.Cos(stackAngle);
                tempZ = radZ * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tempY;
                    temp_vector.Z = z + tempZ * (float)Math.Sin(sectorAngle);

                    _vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        _indices.Add(k1);
                        _indices.Add(k2);
                        _indices.Add(k1 + 1);

                    }

                    if (i != stackCount - 1)
                    {
                        _indices.Add(k1 + 1);
                        _indices.Add(k2);
                        _indices.Add(k2 + 1);
                    }
                }
            }
        }
        
        //rotasi+translate
        public void rotatede(Vector3 pivot, Vector3 vector, float angle)
        {
            var radAngle = MathHelper.DegreesToRadians(angle);

            var arbRotationMatrix = new Matrix4
                (
                new Vector4((float)(Math.Cos(radAngle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(radAngle))), (float)(vector.X * vector.Y * (1.0f - Math.Cos(radAngle)) + vector.Z * Math.Sin(radAngle)), (float)(vector.X * vector.Z * (1.0f - Math.Cos(radAngle)) - vector.Y * Math.Sin(radAngle)), 0),
                new Vector4((float)(vector.X * vector.Y * (1.0f - Math.Cos(radAngle)) - vector.Z * Math.Sin(radAngle)), (float)(Math.Cos(radAngle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(radAngle))), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(radAngle)) + vector.X * Math.Sin(radAngle)), 0),
                new Vector4((float)(vector.X * vector.Z * (1.0f - Math.Cos(radAngle)) + vector.Y * Math.Sin(radAngle)), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(radAngle)) - vector.X * Math.Sin(radAngle)), (float)(Math.Cos(radAngle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(radAngle))), 0),
                Vector4.UnitW
                );

            _model *= Matrix4.CreateTranslation(-pivot);
            _model *= arbRotationMatrix;
            _model *= Matrix4.CreateTranslation(pivot);

            for (int i = 0; i < 3; i++)
            {
                _euler[i] = Vector3.Normalize(getRotationResult(pivot, vector, radAngle, _euler[i], true));
            }

            _centerPosition = getRotationResult(pivot, vector, radAngle, _centerPosition);


            //foreach (var i in Child)
            //{
            //    i.rotate(pivot, vector, angle);
            //}
        }
        public void scale(float x, Vector3 pivot)
        {
            _model = _model * Matrix4.CreateTranslation(-1 * (pivot)) * Matrix4.CreateScale(x) * Matrix4.CreateTranslation((pivot));
        }
        //rotate,float _x, float _y, float _z
        public void rotate(Vector3 pivot, Vector3 vector, float angle)
        {
            //_centerPosition.X = _x;
            //_centerPosition.Y = _y;
            //_centerPosition.Z = _z;
            //pivot -> mau rotate di titik mana
            //vector -> mau rotate di sumbu apa? (x,y,z)
            //angle -> rotatenya berapa derajat?
            var real_angle = angle;
            angle = MathHelper.DegreesToRadians(angle);

            //mulai ngerotasi
            for (int i = 0; i < _vertices.Count; i++)
            {
                _vertices[i] = getRotationResult(pivot, vector, angle, _vertices[i]);
            }
            //rotate the euler direction
            for (int i = 0; i < 3; i++)
            {
                _euler[i] = getRotationResult(pivot, vector, angle, _euler[i], true);

                //NORMALIZE
                //LANGKAH - LANGKAH
                //length = akar(x^2+y^2+z^2)
                float length = (float)Math.Pow(Math.Pow(_euler[i].X, 2.0f) + Math.Pow(_euler[i].Y, 2.0f) + Math.Pow(_euler[i].Z, 2.0f), 0.5f);
                Vector3 temporary = new Vector3(0, 0, 0);
                temporary.X = _euler[i].X / length;
                temporary.Y = _euler[i].Y / length;
                temporary.Z = _euler[i].Z / length;
                _euler[i] = temporary;
            }
            _centerPosition = getRotationResult(pivot, vector, angle, _centerPosition);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count * Vector3.SizeInBytes,
                _vertices.ToArray(), BufferUsageHint.StaticDraw);
            //foreach (var item in Child)
            //{
            //    item.rotate(pivot, vector, real_angle);
            //}
        }
        public Vector3 getRotationResult(Vector3 pivot, Vector3 vector, float angle, Vector3 point, bool isEuler = false)
        {
            Vector3 temp, newPosition;

            if (isEuler)
            {
                temp = point;
            }
            else
            {
                temp = point - pivot;
            }

            newPosition.X =
                temp.X * (float)(Math.Cos(angle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(angle))) +
                temp.Y * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) - vector.Z * Math.Sin(angle)) +
                temp.Z * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) + vector.Y * Math.Sin(angle));

            newPosition.Y =
                temp.X * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) + vector.Z * Math.Sin(angle)) +
                temp.Y * (float)(Math.Cos(angle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(angle))) +
                temp.Z * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) - vector.X * Math.Sin(angle));

            newPosition.Z =
                temp.X * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) - vector.Y * Math.Sin(angle)) +
                temp.Y * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) + vector.X * Math.Sin(angle)) +
                temp.Z * (float)(Math.Cos(angle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(angle)));

            if (isEuler)
            {
                temp = newPosition;
            }
            else
            {
                temp = newPosition + pivot;
            }
            return temp;
        }
        public void translate(float x, float y, float z)
        {
            _model *= Matrix4.CreateTranslation(x, y, z);
            _centerPosition.X += x;
            _centerPosition.Y += y;
            _centerPosition.Z += z;
        }
        public void resetEuler()
        {
            _euler[0] = new Vector3(1, 0, 0);
            _euler[1] = new Vector3(0, 1, 0);
            _euler[2] = new Vector3(0, 0, 1);
        }
        public void addChild(Asset3d child)
        {
            Child.Add(child);
        }
        public void setFragVariable(Vector3 difColor, Vector3 objColor, Vector3 viewPos)
        {
            _shader.SetVector3("objColor", objColor);
            _shader.SetVector3("objDifuseColor", difColor);
            //_shader.SetVector3("LightPos", LightPos);
            _shader.SetVector3("viewPos", viewPos);
            foreach (var item in Child)
            {
                item.setFragVariable(difColor, objColor, viewPos);
            }
        }

        public void setDirectionalLight(Vector3 direction, Vector3 ambient, Vector3 diffuse, Vector3 specular)
        {
            _shader.SetVector3("dirLight.direction", direction);
            _shader.SetVector3("dirLight.ambient", ambient);
            _shader.SetVector3("dirLight.diffuse", diffuse);
            _shader.SetVector3("dirLight.specular", specular);
            foreach (var item in Child)
            {
                item.setDirectionalLight(direction, ambient, diffuse, specular);
            }
        }
        public void setPointLight(Vector3 position, Vector3 ambient, Vector3 diffuse, Vector3 specular, float constant, float linear, float quadratic)
        {
            _shader.SetVector3("pointLight.position", position);
            _shader.SetVector3("pointLight.ambient", ambient);
            _shader.SetVector3("pointLight.diffuse", diffuse);
            _shader.SetVector3("pointLight.specular", specular);
            _shader.SetFloat("pointLight.constant", constant);
            _shader.SetFloat("pointLight.linear", linear);
            _shader.SetFloat("pointLight.quadratic", quadratic);
            foreach (var item in Child)
            {
                item.setPointLight(position, ambient, diffuse, specular, constant, linear, quadratic);
            }
        }

        public void setSpotLight(Vector3 position, Vector3 direction, Vector3 ambient, Vector3 diffuse, Vector3 specular, float constant, float linear, float quadratic, float cutOff, float outerCutOff)
        {
            _shader.SetVector3("spotLight.position", position);
            _shader.SetVector3("spotLight.direction", direction);
            _shader.SetVector3("spotLight.ambient", ambient);
            _shader.SetVector3("spotLight.diffuse", diffuse);
            _shader.SetVector3("spotLight.specular", specular);
            _shader.SetFloat("spotLight.constant", constant);
            _shader.SetFloat("spotLight.linear", linear);
            _shader.SetFloat("spotLight.quadratic", quadratic);
            _shader.SetFloat("spotLight.cutOff", cutOff);
            _shader.SetFloat("spotLight.outerCutOff", outerCutOff);
            foreach (var item in Child)
            {
                item.setSpotLight(position, direction, ambient, diffuse, specular, constant, linear, quadratic, cutOff, outerCutOff);
            }
        }

        public void setPointLights(List<Vector3> position, Vector3 ambient, List<Vector3> diffuse, Vector3 specular, float constant, float linear, float quadratic)
        {
            for (int i = 0; i < position.Count; i++)
            {
                _shader.SetVector3($"pointLight[{i}].position", position[i]);
                _shader.SetVector3($"pointLight[{i}].ambient", ambient);
                _shader.SetVector3($"pointLight[{i}].diffuse", diffuse[i]);
                _shader.SetVector3($"pointLight[{i}].specular", specular);
                _shader.SetFloat($"pointLight[{i}].constant", constant);
                _shader.SetFloat($"pointLight[{i}].linear", linear);
                _shader.SetFloat($"pointLight[{i}].quadratic", quadratic);
            }
            foreach (var item in Child)
            {
                item.setPointLights(position, ambient, diffuse, specular, constant, linear, quadratic);
            }
        }

    }
}
