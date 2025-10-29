using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;

namespace GISDev
{
    public partial class Form1 : Form
    {
        string m_BasicOperationTool = "";
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Title = "open map document";
            openFileDialog.Filter = "map document (*.mxd)|*.mxd";
            openFileDialog.ShowDialog();
            string strFilePath = openFileDialog.FileName;
            if (axMapControl1.CheckMxFile(strFilePath))
            {
                axMapControl1.MousePointer = ESRI.ArcGIS.Controls.esriControlsMousePointer.esriPointerHourglass;
                axMapControl1.LoadMxFile(strFilePath, 0, Type.Missing);
                axMapControl1.MousePointer = ESRI.ArcGIS.Controls.esriControlsMousePointer.esriPointerDefault;
            }
            else
            {
                MessageBox.Show(strFilePath + " is not a valid map document");
            }
        }

        // 全屏
        private void tlbGloble_Click(object sender, EventArgs e)
        {
            m_BasicOperationTool = "isZoomFull";
            axMapControl1.Extent = axMapControl1.FullExtent; // 直接生效
        }
        // 放大
        private void tlbZoomIn_Click(object sender, EventArgs e)
        {
            m_BasicOperationTool = "ZoomIn";
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerZoomIn;
        }
        // 缩小
        private void tlbZoomOut_Click(object sender, EventArgs e)
        {
            m_BasicOperationTool = "ZoomOut";
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerZoomOut;
        }
        // 漫游
        private void tlbPan_Click(object sender, EventArgs e)
        {
            m_BasicOperationTool = "ZoomPan";
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerPan;
        }

        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            // 本方法用于在axMapControl上进行点击后才生效的功能,如放大缩小和漫游
            switch (m_BasicOperationTool)
            {
                case "ZoomIn":
                    axMapControl1.Extent = axMapControl1.TrackRectangle(); // 拉框放大
                    break;
                case "ZoomOut":
                    IEnvelope objEnvelope = axMapControl1.TrackRectangle();
                    // 扩大包络线范围以缩小显示
                    objEnvelope.Expand(2, 2, true);
                    axMapControl1.Extent = objEnvelope;
                    break;
                case "ZoomPan":
                    axMapControl1.Pan(); // 鼠标拖动平移
                    break;
            }
        }
    }
}
