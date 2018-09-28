﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3DTransform
{
    public partial class Form1 : Form
    {
        int a;
        Triangle3D t;
        Matrix4X4 m_scale;
        Matrix4X4 m_rotationX;
        Matrix4X4 m_rotationY;
        Matrix4X4 m_rotationZ;
        Matrix4X4 m_view;
        Matrix4X4 m_projection;
        public Form1()
        {
            InitializeComponent();
            m_scale = new Matrix4X4();
            m_scale[1, 1] = 250;
            m_scale[2, 2] = 250;
            m_scale[3, 3] = 250;
            m_scale[4, 4] = 1;

            m_view = new Matrix4X4();
            m_view[1, 1] = 1;
            m_view[2, 2] = 1;
            m_view[3, 3] = 1;
            m_view[4, 3] = 250;
            m_view[4, 4] = 1;


            m_projection = new Matrix4X4();
            m_projection[1, 1] = 1;
            m_projection[2, 2] = 1;
            m_projection[3, 3] = 1;
            m_projection[3, 4] = 1.0 / 250;


            m_rotationX = new Matrix4X4();
            m_rotationY = new Matrix4X4();
            m_rotationZ = new Matrix4X4();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Vector4 a = new Vector4(0, 0.5, 0, 1);
            Vector4 b = new Vector4(0.5, -0.5, 0, 1);
            Vector4 c = new Vector4(-0.5, -0.5, 0, 1);
            t = new Triangle3D(a, b, c);
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            t.Draw(e.Graphics);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            a += 2;
            double angle = a / 360.0 * Math.PI;

            //==========X
            m_rotationX[1, 1] = 1;
            m_rotationX[2, 2] = Math.Cos(angle);
            m_rotationX[2, 3] = Math.Sin(angle);
            m_rotationX[3, 2] = -Math.Sin(angle);
            m_rotationX[3, 3] = Math.Cos(angle);
            m_rotationX[4, 4] = 1;

            //==========Y
            m_rotationY[1, 1] = Math.Cos(angle);
            m_rotationY[1, 3] = Math.Sin(angle);
            m_rotationY[2, 2] = 1;
            m_rotationY[3, 1] = -Math.Sin(angle);
            m_rotationY[3, 3] = Math.Cos(angle);
            m_rotationY[4, 4] = 1;

            //===========Z
            m_rotationZ[1, 1] = Math.Cos(angle);
            m_rotationZ[1, 2] = Math.Sin(angle);
            m_rotationZ[2, 1] = -Math.Sin(angle);
            m_rotationZ[2, 2] = Math.Cos(angle);
            m_rotationZ[3, 3] = 1;
            m_rotationZ[4, 4] = 1;

            //===========
            if (this.cbx.Checked)
            {
                Matrix4X4 tx = m_rotationX.Transpose();
                m_rotationX = m_rotationX.Mul(tx);
            }
            if (this.cby.Checked)
            {
                Matrix4X4 ty = m_rotationY.Transpose();
                m_rotationY = m_rotationY.Mul(ty);
            }
            if (this.cbz.Checked)
            {
                Matrix4X4 tz = m_rotationZ.Transpose();
                m_rotationZ = m_rotationZ.Mul(tz);
            }

            Matrix4X4 mall = m_rotationX.Mul(m_rotationY.Mul(m_rotationZ));

            Matrix4X4 m = m_scale.Mul(mall);
            m = m.Mul(m_view);
            m = m.Mul(m_projection);
          
            t.Transform(m);
            this.Invalidate();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            m_view[4, 3] = (sender as TrackBar).Value;
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
