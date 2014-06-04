using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

public class Ship : System.Windows.Forms.Panel
{
    Point DownPoint;
    bool IsDragMode;
    bool isLeftMouseButtonDown = false;

    protected override void OnMouseDown(MouseEventArgs mevent)
    {
        if (mevent.Button == MouseButtons.Left)
            isLeftMouseButtonDown = true;

        if (mevent.Button == MouseButtons.Right && isLeftMouseButtonDown)// переворачивание корабля правой кнопкой маши, при этом левая кнопка мыши должна быть нажата
        {
            this.Size = new Size(Height, Width);
        }

        DownPoint = mevent.Location;
        IsDragMode = true;
        base.OnMouseDown(mevent);
    }

    protected override void OnMouseUp(MouseEventArgs mevent)// метод для того, что корабль становился на поле игры
    {
        if (mevent.Button == MouseButtons.Left)
        isLeftMouseButtonDown = false;
        IsDragMode = false;
        base.OnMouseUp(mevent);
    }

    protected override void OnMouseMove(MouseEventArgs mevent)// перетаскивание корабля
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

