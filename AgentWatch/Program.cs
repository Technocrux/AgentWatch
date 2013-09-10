using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using System.Threading;
namespace Calculator
{
    public class Program
    {
        const float PI = 3.141592654F;

        static DateTime dateTime;

        static float fRadius, fCenterX, fCenterY, fCenterCircleRadius, fHourLength;
        static float fMinLength, fSecLength, fHourThickness, fMinThickness, fSecThickness;
        static bool bDraw5MinuteTicks = true;
        static bool bDraw1MinuteTicks = true;
        static float fTicksThickness = 2;

        static Color hrColor = Colors.Red;
        static Color minColor = Colors.Red;
        static Color secColor = Colors.Red;
        static Color circleColor = Colors.Green;
        static Color ticksColor = Colors.Yellow;
        static Timer updateClockTimer;

        static Bitmap _display;
        
        public static void Main()
        {
            // initialize display buffer and properties
            _display = new Bitmap(Bitmap.MaxWidth, Bitmap.MaxHeight);
            fRadius = _display.Height / 2;
            fCenterX = _display.Width / 2;
            fCenterY = _display.Height / 2;
            fHourLength = (float)_display.Height / 3 / 1.85F;
            fMinLength = (float)_display.Height / 3 / 1.20F;
            fSecLength = (float)_display.Height / 3 / 1.15F;
            fHourThickness = (float)_display.Height / 100;
            fMinThickness = (float)_display.Height / 150;
            fSecThickness = (float)_display.Height / 200;
            fCenterCircleRadius = _display.Height / 50;

            // obtain the current time
            var currentTime = DateTime.Now;
            // set up timer to refresh time every minute
            // start timer at beginning of next second
            var dueTime = new TimeSpan(0, 0, 0, 0, 1000 - currentTime.Millisecond);
            // update time every second
            var period = new TimeSpan(0, 0, 0, 1, 0);
            // start our update timer
            updateClockTimer = new Timer(UpdateTime, null, dueTime, period); 

            Thread.Sleep(Timeout.Infinite);
        }

        static void UpdateTime(object state)
        {
            _display.Clear();
            dateTime = DateTime.Now;
            var fRadHr = (dateTime.Hour % 12 + dateTime.Minute / 60F) * 30 * PI / 180;
            var fRadMin = (dateTime.Minute) * 6 * PI / 180;
            var fRadSec = (dateTime.Second) * 6 * PI / 180;

            DrawLine(fHourThickness, fHourLength, hrColor, fRadHr);
            DrawLine(fMinThickness, fMinLength, minColor, fRadMin);
            DrawLine(fSecThickness, fSecLength, secColor, fRadSec);

            for (var i = 0; i < 60; i++)
            {
                if (i%5 == 0)
                {
                    var x1 = (int)(fCenterX + (float)(fRadius / 1.50F * System.Math.Sin(i * 6 * PI / 180)));
                    var y1 = (int)(fCenterY - (float)(fRadius / 1.50F * System.Math.Cos(i * 6 * PI / 180)));
                    var x2 = (int)(fCenterX + (float)(fRadius / 1.65F * System.Math.Sin(i * 6 * PI / 180)));
                    var y2 = (int)(fCenterY - (float)(fRadius / 1.65F * System.Math.Cos(i * 6 * PI / 180)));
                    _display.DrawLine(ticksColor, 1, x1, y1, x2, y2); 
                }
                else
                {
                    var x1 = (int)(fCenterX + (float)(fRadius / 1.50F * System.Math.Sin(i * 6 * PI / 180)));
                    var y1 = (int)(fCenterY - (float)(fRadius / 1.50F * System.Math.Cos(i * 6 * PI / 180)));
                    var x2 = (int)(fCenterX + (float)(fRadius / 1.25F * System.Math.Sin(i * 6 * PI / 180)));
                    var y2 = (int)(fCenterY - (float)(fRadius / 1.25F * System.Math.Cos(i * 6 * PI / 180)));
                    _display.DrawLine(Color.White, 1, x1, y1, x2, y2); 
                }
            }
            _display.DrawEllipse(circleColor,
                   (int)(fCenterX - fCenterCircleRadius / 2),
                   (int)(fCenterY - fCenterCircleRadius / 2),
                   (int)(fCenterCircleRadius), (int)(fCenterCircleRadius));


            _display.Flush();
        }
        

        private static void DrawLine(float fThickness, float fLength, Color color, float fRadians)
        {
            var x1 = (int)(fCenterX - (float)(fLength / 9 * System.Math.Sin(fRadians)));
            var y1 = (int)(fCenterY + (float)(fLength / 9 * System.Math.Cos(fRadians)));
            var x2 = (int)(fCenterX + (float)(fLength * System.Math.Sin(fRadians)));
            var y2 = (int)(fCenterY - (float)(fLength * System.Math.Cos(fRadians)));
            _display.DrawLine(color, (int)(fThickness), x1, y1, x2, y2);
            _display.Flush();
        }
        
    }
}
