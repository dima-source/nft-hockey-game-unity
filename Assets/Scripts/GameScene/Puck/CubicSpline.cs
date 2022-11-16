namespace GameScene.Puck
{
    public class CubicSpline
    {
        SplineTuple[] splines; 
    
        private struct SplineTuple
        {
            public double a, b, c, d, x;
        }
 
        public void BuildSpline(float[] x, float[] y, int n)
        {
            splines = new SplineTuple[n];
            for (int i = 0; i < n; ++i)
            {
                splines[i].x = x[i];
                splines[i].a = y[i];
            }
            splines[0].c = splines[n - 1].c = 0.0;
        
            double[] alpha = new double[n - 1];
            double[] beta  = new double[n - 1];
            alpha[0] = beta[0] = 0.0;
            for (int i = 1; i < n - 1; ++i)
            {
                double hi  = x[i] - x[i - 1];
                double hi1 = x[i + 1] - x[i];
                double a = hi;
                double c = 2.0 * (hi + hi1);
                double b = hi1;
                double f = 6.0 * ((y[i + 1] - y[i]) / hi1 - (y[i] - y[i - 1]) / hi);
                double z = (a * alpha[i - 1] + c);
                alpha[i] = -b / z;
                beta[i] = (f - a * beta[i - 1]) / z;
            }
 
            for (int i = n - 2; i > 0; --i)
            {
                splines[i].c = alpha[i] * splines[i + 1].c + beta[i];
            }
        
            for (int i = n - 1; i > 0; --i)
            {
                double hi = x[i] - x[i - 1];
                splines[i].d = (splines[i].c - splines[i - 1].c) / hi;
                splines[i].b = hi * (2.0 * splines[i].c + splines[i - 1].c) / 6.0 + (y[i] - y[i - 1]) / hi;
            }
        }   
 
        public double Interpolate(double x)
        {
            if (splines == null)
            {
                return double.NaN; 
            }
        
            int n = splines.Length;
            SplineTuple s;
        
            if (x <= splines[0].x) 
            {
                s = splines[0];
            }
            else if (x >= splines[n - 1].x) 
            {
                s = splines[n - 1];
            }
            else 
            {
                int i = 0;
                int j = n - 1;
                while (i + 1 < j)
                {
                    int k = i + (j - i) / 2;
                    if (x <= splines[k].x)
                    {
                        j = k;
                    }
                    else
                    {
                        i = k;
                    }
                }
                s = splines[j];
            }
        
            double dx = x - s.x;
            
            return s.a + (s.b + (s.c / 2.0 + s.d * dx / 6.0) * dx) * dx; 
        }
    }
}