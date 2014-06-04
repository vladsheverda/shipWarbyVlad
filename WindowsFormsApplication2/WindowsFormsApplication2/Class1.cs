using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ShipWar
{
    public class Ship : System.Windows.Forms.Panel
    {
        Point DownPoint;
        bool IsDragMode;
        bool isLeftMouseButtonDown = false;

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
                isLeftMouseButtonDown = true;

            if (mevent.Button == MouseButtons.Right && isLeftMouseButtonDown)
            {
                this.Size = new Size(Height, Width);
            }

            DownPoint = mevent.Location;
            IsDragMode = true;
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
                isLeftMouseButtonDown = false;
            IsDragMode = false;
            base.OnMouseUp(mevent);
        }

        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            if (IsDragMode)
            {
                Point p = mevent.Location;
                Point dp = new Point(p.X - DownPoint.X, p.Y - DownPoint.Y);
                Location = new Point(Location.X + dp.X, Location.Y + dp.Y);
            }

            base.OnMouseMove(mevent);
        }
    }
}
