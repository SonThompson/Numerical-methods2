using System;
using System.Diagnostics;
using System.Threading;


namespace Com_Methods
{
    class CONST
    {
        //точность решения
        public static double EPS = 1e-15;
    }

    class Program
    {
        static void Main()
        {
            try
            {
                //прямые методы: метод Гаусса, LU-разложение, QR-разложение

                int N = 400;
                var A = new Matrix(N, N);
                var X_true = new Vector(N);

                //заполнение СЛАУ
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        if (i != j)
                        {
                            A.Elem[i][j] = (1.0 + 0.1 * i + 0.2 * j);
                        }
                        else
                        {
                            A.Elem[i][j] = 100.0;
                        }
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

                double delta1 = (Y - X_true).Norm() / X_true.Norm();
                double delta2 = (X - X_true).Norm() / X_true.Norm();

                for (int i = 0; i < Y.N; i++) Console.WriteLine("X[{0}] = {1}", i + 1, Y.Elem[i]);
                Console.WriteLine("\n");
                for (int i = 0; i < X.N; i++) Console.WriteLine("X[{0}] = {1}", i + 1, X.Elem[i]);

                Console.Write("LU_Decomposition time: "); Console.WriteLine(stopwatch1.ElapsedMilliseconds);
                Console.Write("Householder time: "); Console.WriteLine(stopwatch2.ElapsedMilliseconds);
                Console.Write("delta for LU_Decomposition: "); Console.WriteLine(delta1);
                Console.Write("delta for Householder: "); Console.WriteLine(delta2);
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