using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamCarving
{
    public static class BitmapExtensions
    {
        public static Color[,] GetBitMapColorMatrix(this Bitmap bitmap)
        {
            //
            int height = bitmap.Height;
            int width = bitmap.Width;

            Color[,] colorMatrix = new Color[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    colorMatrix[i, j] = bitmap.GetPixel(i, j);
                }
            }
            return colorMatrix;
        }

        public static Bitmap SetBitMapColorMatrix(this Color[,] matrix)
        {
            int width = (int)matrix.GetLongLength(0);
            int height = (int)matrix.GetLongLength(1);

            Bitmap bitmap = new Bitmap(width, height);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    bitmap.SetPixel(i, j, matrix[i, j]);
                }
            }
            return bitmap;
        }

        public static float[,] ToGrayScale(this Color[,] matrix)
        {
            int width = (int)matrix.GetLongLength(0);
            int height = (int)matrix.GetLongLength(1);
            var grayscale = new float[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grayscale[x, y] = matrix[x, y].GetBrightness();
                }
            }
            return grayscale;
        }

    }

    public class Seamcarving
    {
        public Bitmap bitmap;
        private Color[,] matrix;

        public Seamcarving(Bitmap bitmap)
        {
            this.bitmap = bitmap;
            this.matrix = bitmap.GetBitMapColorMatrix();
        }

        private void RemoveSeam(int[] coordinates)
        {
            long width = matrix.GetLongLength(0) - 1;
            long height = matrix.GetLongLength(1);
            var newmatrix = new Color[width, height];
            for (int y = 0; y < height; y++)
            {
                int pixeltoremove = coordinates[y];
                for (int x = 0; x < pixeltoremove; x++)
                    newmatrix[x, y] = matrix[x, y];
                for (int x = pixeltoremove; x < width; x++)
                    newmatrix[x, y] = matrix[x + 1, y];
            }
            matrix = newmatrix;
        }

        private int[] Getoptimalseam(float[,] grayscale)
        {
            int width;
            int height;

            var gradients = GetGradients(grayscale, out width, out height);
            var minimalPathsDPTable = GetMinimalPathsTable(width, height, gradients);
            var seam = BacktraceDPTable(width, height, minimalPathsDPTable);

            return seam;
        }

        private static int[] BacktraceDPTable(int width, int height, float[,] minimalPathsDPTable)
        {
            var seam = new int[height];
            var idx = 0;
            var bestpixel = minimalPathsDPTable[idx, height - 1];
            for (int x = 0; x < width; x++)
                if (minimalPathsDPTable[x, height - 1] < bestpixel)
                {
                    bestpixel = minimalPathsDPTable[x, height - 1];
                    idx = x;
                }
            seam[height - 1] = idx;
            for (int y = height - 2; y > 0; y--)
            {
                var originalidx = idx;
                var middle = minimalPathsDPTable[idx, y - 1];
                if (idx > 0)
                {
                    var left = minimalPathsDPTable[idx - 1, y - 1];
                    if (left < middle)
                    {
                        idx = originalidx - 1;
                        middle = left;
                    }
                }

                if (idx < width - 1)
                {
                    var right = minimalPathsDPTable[idx + 1, y - 1];
                    if (right < middle)
                    {
                        idx = originalidx + 1;
                        middle = right;
                    }
                }

                seam[y] = idx;
            }
            return seam;
        }

        private static float[,] GetMinimalPathsTable(int width, int height, float[,] gradients)
        {
            var minimalPathsDPTable = new float[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y == 0)
                        minimalPathsDPTable[x, y] = gradients[x, y];
                    else
                    {
                        var best = minimalPathsDPTable[x, y - 1];
                        if (x > 0)
                            best = Math.Min(best, minimalPathsDPTable[x - 1, y - 1]);
                        if (x < width - 1)
                            best = Math.Min(best, minimalPathsDPTable[x + 1, y - 1]);
                        minimalPathsDPTable[x, y] = best + gradients[x, y];
                    }
                }
            }
            return minimalPathsDPTable;
        }

        private static float[,] GetGradients(float[,] grayscale, out int width, out int height)
        {
            width = (int)grayscale.GetLongLength(0);
            height = (int)grayscale.GetLongLength(1);

            var gradients = new float[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    float gradient = 0;
                    if (x > 0)
                        gradient += Math.Abs(grayscale[x - 1, y] - grayscale[x, y]);
                    if (y > 0)
                        gradient += Math.Abs(grayscale[x, y - 1] - grayscale[x, y]);
                    gradients[x, y] = gradient;
                }
            return gradients;
        }

        public void Iterate()
        {
            var grayscale = matrix.ToGrayScale();

            int width, height;
            var gradients = GetGradients(grayscale, out width, out height);
            var minimalPathsDPTable = GetMinimalPathsTable(width, height, gradients);
            var optimalseam = BacktraceDPTable(width, height, minimalPathsDPTable);

            RemoveSeam(optimalseam);
            bitmap = matrix.SetBitMapColorMatrix();
        }
    }
}
