using System;
using System.Diagnostics;
using System.Threading;
using Прямые_методы_решения_СЛАУ;

namespace Com_Methods
{
    class CONST
    {
        //точность решения
        public static double EPS = 1e-15;
    }
    class Tools
    {
        //относительная погрешность
        public static double Relative_Error(Vector X, Vector x)
        {
            double s = 0.0;
            for (int i = 0; i < X.N; i++)
            {
                s += Math.Pow(X.Elem[i] - x.Elem[i], 2);
            }
            return Math.Sqrt(s) / x.Norm();
        }
    }
    class Program
    {
        static void Main()
        {
            try
            {
                //прямые методы: метод Гаусса, LU-разложение, QR-разложение

                int N = 10;
                var A = new Matrix(N, N);
                var X_true = new Vector(N);

                //заполнение СЛАУ
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                            A.Elem[i][j] = 1.0/(2.0+i+j);
                    }
                    X_true.Elem[i] = 1;
                }
                

                //создаем объект для определения времени
                Stopwatch stopwatch1 = new Stopwatch();
                Stopwatch stopwatch2 = new Stopwatch();

                //правая часть
                var F = A * X_true;
                var C = F;
                //решатель
                //var Solver1 = new Gauss_Method();

                stopwatch1.Start();
                var Solver1 = new LU_Decomposition(A);
                var Y = Solver1.Start_Solver(F);
                stopwatch1.Stop();

                stopwatch2.Start();
                var Solver2 = new QR_Decomposition(A, QR_Decomposition.QR_Algorithm.Householder);
                var X = Solver2.Start_Solver(C);
                stopwatch2.Stop();

                //вычисление числа обусловленности
                Console.WriteLine("\nCond(A) = " + A.Cond_InfinityNorm() + "\n");

                Console.Write("Result for QR_Decomposition: ");
                //X.Console_Write_Vector();
                Console.WriteLine("\nError: {0}\n", Tools.Relative_Error(X, X_true));

                Console.Write("Result for LU_Decomposition: ");
                //Y.Console_Write_Vector();
                Console.WriteLine("\nError: {0}\n", Tools.Relative_Error(Y, X_true));

                //for (int i = 0; i < Y.N; i++) Console.WriteLine("X[{0}] = {1}", i + 1, Y.Elem[i]);
                //Console.WriteLine("\n");
                //for (int i = 0; i < X.N; i++) Console.WriteLine("X[{0}] = {1}", i + 1, X.Elem[i]);

                //Console.Write("LU_Decomposition time: "); Console.WriteLine(stopwatch1.ElapsedMilliseconds);
                //Console.Write("Householder time: "); Console.WriteLine(stopwatch2.ElapsedMilliseconds);
            }
            catch (Exception E)
            {
                Console.WriteLine("\n*** Error! ***");
                Console.WriteLine("Method:  {0}",   E.TargetSite);
                Console.WriteLine("Message: {0}\n", E.Message);
            }


        }
    }
}