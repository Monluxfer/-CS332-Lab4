using System;
using System.Collections.Generic;

namespace CG4
{
	public class Figure
	{
		List<CustomPoint> points;

		public Figure ()
		{
			points = new List<CustomPoint> ();
		}

        public void Add(CustomPoint t)
        {
            points.Add(t);
        }

        public List<CustomPoint> Points()
        {
            return points;
        }

        //Параллельное смещение фигуры
        public static Figure Displacement(Figure f, int x, int y)
        {
            Figure res = new Figure();
            for (int i = 0; i < f.points.Count; i++)
            {
                res.Add(CustomPoint.Displacement(f.points[i], x, y));
            }
            return res;
        }

        private static double ToRadian(int degrees)
        {
            return degrees * (Math.PI / 180);
        }

        //Поворот фигуры на x РАДИАН
        public static Figure Rotation(Figure f, int angle)
        {
            double x = Figure.ToRadian(angle);
            Figure res = new Figure();
            for (int i = 0; i < f.points.Count; i++)
            {
                res.Add(CustomPoint.Rotation(f.points[i], x));
            }
            return res;
        }

        //Поворот фигуры вокруг точки
        public static Figure RotationPoint(Figure f, CustomPoint p, int angle)
        {
            double x = Figure.ToRadian(angle);
            if (f.points.Count != 2)
                throw new Exception("This is not line");
            CustomPoint p1 = f.points[0];
            CustomPoint p2 = f.points[1];
            Figure res = new Figure();
            CustomPoint tmp = p;
            p1 = CustomPoint.Displacement(p1, (int)((-1) * tmp.x), (int)((-1) * tmp.y));
            p2 = CustomPoint.Displacement(p2, (int)((-1) * tmp.x), (int)((-1) * tmp.y));

            p1 = CustomPoint.Rotation(p1, x);
            p2 = CustomPoint.Rotation(p2, x);

            p1 = CustomPoint.Displacement(p1, (int)tmp.x, (int)tmp.y);
            p2 = CustomPoint.Displacement(p2, (int)tmp.x, (int)tmp.y);

            res.Add(p1);
            res.Add(p2);
            return res;
        }

        //Это поворот линии вокруг своего центра.
        //Если угол равен -3.14/2, то это поворот на 90 градусов
        public static Figure RotationLine(Figure f, int angle)
        {
            double x = Figure.ToRadian(angle);
            if (f.points.Count != 2)
                throw new Exception("This is not line");
            CustomPoint p1 = f.points[0];
            CustomPoint p2 = f.points[1];

            CustomPoint tmp = new CustomPoint();
            tmp.x = (p1.x + p2.x) / 2;
            tmp.y = (p1.y + p2.y) / 2;
            return Figure.RotationPoint(f, tmp, angle);
        }

        //положение точки относительно отрезка
        public enum PointOverEdge { СЛЕВА, СПРАВА, НАОТРЕЗКЕ, НАПРЯМОЙ }

        //положение точки p относительно отрезка vw
        public static PointOverEdge Classification(CustomPoint p, CustomPoint v, CustomPoint w)
        {
            //коэффициенты уравнения прямой
            double a = v.y - w.y;
            double b = w.x - v.x;
            double c = v.x * w.y - w.x * v.y;

            //подставим точку в уравнение прямой
            double f = a * p.x + b * p.y + c;
            if (f > 0)
                return PointOverEdge.СПРАВА; //точка лежит справа от отрезка
            if (f < 0)
                return PointOverEdge.СЛЕВА; //слева от отрезка

            double minX = Math.Min(v.x, w.x);
            double maxX = Math.Max(v.x, w.x);
            double minY = Math.Min(v.y, w.y);
            double maxY = Math.Max(v.y, w.y);

            if (minX <= p.x && p.x <= maxX && minY <= p.y && p.y <= maxY)
                return PointOverEdge.НАОТРЕЗКЕ; //точка лежит на отрезке
            return PointOverEdge.НАПРЯМОЙ; //точка лежит на прямой, но не на отрезке
        }

		private enum Position { ПРИКАСАЕТСЯ, ПЕРЕСЕКАЕТСЯ, НЕОБХОДИМО } //положение ребра

        //тип ребра vw для точки a
        private static Position EdgeType(CustomPoint a, CustomPoint v, CustomPoint w)
        {
            switch (Classification(a, v, w))
            {
                case PointOverEdge.СЛЕВА:
                    return ((v.y < a.y) && (a.y <= w.y)) ? Position.ПЕРЕСЕКАЕТСЯ : Position.НЕОБХОДИМО;
                case PointOverEdge.СПРАВА:
                    return ((w.y < a.y) && (a.y <= v.y)) ? Position.ПЕРЕСЕКАЕТСЯ : Position.НЕОБХОДИМО;
                case PointOverEdge.НАОТРЕЗКЕ:
                    return Position.ПРИКАСАЕТСЯ;
                default:
                    return Position.НЕОБХОДИМО;
            }
        }

        //положение точки в многоугольнике
        public enum PointInPolygon { ВНУТРИ, СНАРУЖИ, ГРАНИЦА }

        //положение точки в многоугольнике
        public PointInPolygon PointInFigure(CustomPoint a)
		{
			bool parity = true;
            for (int i = 0; i < points.Count; i++)
            {
                CustomPoint v = points[i];
                CustomPoint w = points[(i + 1) % points.Count];

                switch (EdgeType(a, v, w))
                {
                    case Position.ПРИКАСАЕТСЯ:
                        return PointInPolygon.ГРАНИЦА;
                    case Position.ПЕРЕСЕКАЕТСЯ:
                        parity = !parity;
                        break;
                }
            }

			return parity ? PointInPolygon.СНАРУЖИ : PointInPolygon.ВНУТРИ;
		}
	}
}
