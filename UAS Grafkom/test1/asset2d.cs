using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace test1
{
    internal class asset2d
    {
        float[]vertices =
        {
            
        };
        uint[] _indices =
        {
        };
        int _vertexbufferobject;
        int _vertexarrayobject;
        int _elementBufferObject;
        Shader _shader;
        int _index;
        int[] _pascal;

        public asset2d (float[]vertices,uint[]indices)
        {
            this.vertices = vertices;
            _indices = indices;
            _index = 0;

        }
        public void Load(String color1,String coli)
        {
            
            _vertexbufferobject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexbufferobject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float),
            vertices, BufferUsageHint.StaticDraw);
            _vertexarrayobject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexarrayobject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            if(_indices.Length != 0)
            {
                _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices,BufferUsageHint.StaticDraw);

            }
            _shader = new Shader(color1,coli);
            _shader.Use();
        }
        public void Render(int pilihan)
        {
            _shader.Use();
            //int vertexColorLocation = GL.GetUniformLocation(_shader.Handle,"ourColor");
            //Gl.Uniform4(vertexColorLocation, 0.3f,0.2f,0.0f,1.0f);
            GL.BindVertexArray(_vertexarrayobject);
            if(_indices.Length != 0)
            {
                GL.DrawElements(PrimitiveType.Triangles, _indices.Length,DrawElementsType.UnsignedInt,0);
            }
            else
            {
                if (pilihan == 0)
                {
                    GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
                }
                else if (pilihan == 1)
                {
                    GL.DrawArrays(PrimitiveType.TriangleFan, 0, (vertices.Length + 1) / 3);
                }
                else if (pilihan ==2)
                {
                    GL.DrawArrays(PrimitiveType.LineStrip, 0, _index);
                }
                else if (pilihan == 3)
                {
                    GL.DrawArrays(PrimitiveType.LineStrip, 0, vertices.Length);
                }

            }
        }
        public void createCircle(float center_x,float center_y,float radius)
        {
            vertices = new float[1080];
            for (int i = 0; i < 360; i++)
            {
                double degInRad = i * Math.PI / 180;
                //x
                vertices[i * 3] = radius * (float)Math.Cos(degInRad) + center_x;
                //y
                vertices[i * 3 + 1] = radius * (float)Math.Sin(degInRad) + center_y;
                //z
                vertices[i * 3 + 2] = 0;
            }
        }
        public void createElipse(float center_x, float center_y, float radiusX ,float radiusY)
        {
            vertices = new float[1080];
            for (int i = 0; i < 360; i++)
            {
                double degInRad = i * Math.PI / 180;
                //x
                vertices[i * 3] = radiusX * (float)Math.Cos(degInRad) + center_x;
                //y
                vertices[i * 3 + 1] = radiusY * (float)Math.Sin(degInRad) + center_y;
                //z
                vertices[i * 3 + 2] = 0;
            }
        }
        public void updatemouseposition(float x, float y)
        {
          vertices[_index * 3] = x;

          vertices[_index * 3 + 1] = y;

          vertices[_index * 3 + 2] = 0;
            
          _index++;

          GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

          GL.VertexAttribPointer(0,       //tempat penyimpanan shader
                                  3,       //jumlah vertices
                                  VertexAttribPointerType.Float,
                                  false,                      //tidak pakai normalisasi
                                  3 * sizeof(float),          //ukuran byte dalam 1 vertex
                                  0                           //baca mulai dari index 0
                                  );
          GL.EnableVertexAttribArray(0); //refrence dari yang atas
        }
        public List<int> getRow(int rowIndex)
        {
            List<int> currow = new List<int>();
            //------
            currow.Add(1);
            if (rowIndex == 0)
            {
                return currow;
            }
            //-----
            List<int> prev = getRow(rowIndex - 1);
            for (int i = 1; i < prev.Count; i++)
            {
                int curr = prev[i - 1] + prev[i];
                currow.Add(curr);
            }
            currow.Add(1);
            return currow;
        }

        public List<float> createCurveBezier()
        {
            List<float> _vertices_bezier = new List<float>();
            List<int> pascal = getRow(_index - 1);
            _pascal = pascal.ToArray();
            for (float t = 0.0f; t <= 1.0f; t += 0.001f)
            {
                Vector2 p = getP(_index,t);
                _vertices_bezier.Add(p.X);
                _vertices_bezier.Add(p.Y);
                _vertices_bezier.Add(0);
            }
            return _vertices_bezier;
        }

        public Vector2 getP(int n, float t)
        {
            Vector2 p = new Vector2(0, 0);
            float k;
            for (int i = 0; i < n; i++)
            {
                k = (float)Math.Pow((1 - t), n - 1 - i) * (float)Math.Pow(t, i) * _pascal[i];
                p.X += k * vertices[i * 3];
                p.Y += k * vertices[i * 3 + 1];
            }
            return p;
        }
        public bool getVerticesLength()
        {
            if (vertices[0] == 0)
            {
                return false;
            }
            if((vertices.Length+1)/3 > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void setVertices(float[] _temp)
        {
            vertices = _temp;
        }
    }
}