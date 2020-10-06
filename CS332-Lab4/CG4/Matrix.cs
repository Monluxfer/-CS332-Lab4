using System;

namespace CG4
{
	public class Matrix
	{
		private double [][] _matr;
		private int _m;
		private int _n;
        public Matrix(int n, int m)
        {
            if (n < 0 || m < 0)
                throw new Exception("size>0");
            _m = m;
            _n = n;
            _matr = new double[_m][];
            for (int i = 0; i < _m; i++)
                _matr[i] = new double[_n];
        }

        public void Print()
        {
            for (int i = 0; i < _m; i++)
            {
                for (int j = 0; j < _n; j++)
                {
                    Console.Write(_matr[i][j]);
                    Console.Write(" ");
                }
                Console.WriteLine();

            }
        }

        public void Set(double x, int i, int j)
        {
            if (i > _m || j > _n || i < 0 || j < 0)
                throw new Exception();
            _matr[i][j] = x;
        }

        public double Get(int i, int j)
        {
            if (i > _m || j > _n || i < 0 || j < 0)
                throw new Exception();
            return _matr[i][j];
        }

        public int M()
        {
            return _m;
        }

        public int N()
        {
            return _n;
        }

        public Matrix Mult(Matrix m2)
        {
            Matrix res = new Matrix(m2._n, _m);
            for (int i = 0; i < _m; i++)
            {
                for (int j = 0; j < m2._n; j++)
                {
                    for (int k = 0; k < m2._m; k++)
                    {
                        res._matr[i][j] += _matr[i][k] * m2._matr[k][j];
                    }
                }
            }
            return res;
        }

        public CustomPoint ToPoint()
        {
            if (_m >= 3)
            {
                return new CustomPoint(_matr[0][0] / _matr[2][0], _matr[1][0] / _matr[2][0]);
            }
            else
            {
                return new CustomPoint(_matr[0][0] / _matr[0][2], _matr[0][1] / _matr[0][2]);
            }
        }

        public static Matrix Displacement(int x, int y)
        {
            Matrix p = new Matrix(3, 3);
            p.Set(1, 0, 0);
            p.Set(1, 1, 1);
            p.Set(1, 2, 2);
            p.Set(x, 2, 0);
            p.Set(y, 2, 1);
            return p;
        }

        public static Matrix Rotation(double x)
        {
            Matrix p = new Matrix(3, 3);
            p.Set(Math.Cos(x), 0, 0);
            p.Set(Math.Sin(x), 0, 1);
            p.Set(Math.Sin(x) * (-1), 1, 0);
            p.Set(Math.Cos(x), 1, 1);
            p.Set(1, 2, 2);
            return p;
        }

        public static Matrix Scale(double k)
        {
            Matrix p = new Matrix(3, 3);
            p.Set(k, 0, 0);
            p.Set(k, 1, 1);
            p.Set(1, 2, 2);
            return p;
        }
	}
}
