using System;

class Program
{
    static float A, B, C;

    static float cubeWidth = 20;
    static int width = 160, height = 44;
    static float[] zBuffer = new float[160 * 44];
    static char[] buffer = new char[160 * 44];
    static int backgroundASCIICode = '.';
    static int distanceFromCam = 100;
    static float horizontalOffset;
    static float K1 = 40;
    static float incrementSpeed = 0.6f;

    static float x, y, z;
    static float ooz;
    static int xp, yp;
    static int idx;

    static void CalculateForSurface(float cubeX, float cubeY, float cubeZ, char ch)
    {
        x = CalculateX(cubeX, cubeY, cubeZ);
        y = CalculateY(cubeX, cubeY, cubeZ);
        z = CalculateZ(cubeX, cubeY, cubeZ) + distanceFromCam;

        ooz = 1 / z;

        xp = (int)(width / 2 + horizontalOffset + K1 * ooz * x * 2);
        yp = (int)(height / 2 + K1 * ooz * y);

        idx = xp + yp * width;
        if (idx >= 0 && idx < width * height)
        {
            if (ooz > zBuffer[idx])
            {
                zBuffer[idx] = ooz;
                buffer[idx] = ch;
            }
        }
    }

    static float CalculateX(float i, float j, float k)
    {
        return j * MathF.Sin(A) * MathF.Sin(B) * MathF.Cos(C) - k * MathF.Cos(A) * MathF.Sin(B) * MathF.Cos(C) +
               j * MathF.Cos(A) * MathF.Sin(C) + k * MathF.Sin(A) * MathF.Sin(C) + i * MathF.Cos(B) * MathF.Cos(C);
    }

    static float CalculateY(float i, float j, float k)
    {
        return j * MathF.Cos(A) * MathF.Cos(C) + k * MathF.Sin(A) * MathF.Cos(C) -
               j * MathF.Sin(A) * MathF.Sin(B) * MathF.Sin(C) + k * MathF.Cos(A) * MathF.Sin(B) * MathF.Sin(C) -
               i * MathF.Cos(B) * MathF.Sin(C);
    }

    static float CalculateZ(float i, float j, float k)
    {
        return k * MathF.Cos(A) * MathF.Cos(B) - j * MathF.Sin(A) * MathF.Cos(B) + i * MathF.Sin(B);
    }

    static void Main()
    {
        Console.Clear();
        while (true)
        {
            Array.Fill(buffer, (char)backgroundASCIICode);
            Array.Clear(zBuffer, 0, width * height);
            cubeWidth = 20;
            horizontalOffset = -2 * cubeWidth;

            // first cube
            for (float cubeX = -cubeWidth; cubeX < cubeWidth; cubeX += incrementSpeed)
            {
                for (float cubeY = -cubeWidth; cubeY < cubeWidth; cubeY += incrementSpeed)
                {
                    CalculateForSurface(cubeX, cubeY, -cubeWidth, '@');
                    CalculateForSurface(cubeWidth, cubeY, cubeX, '$');
                    CalculateForSurface(-cubeWidth, cubeY, -cubeX, '~');
                    CalculateForSurface(-cubeX, cubeY, cubeWidth, '#');
                    CalculateForSurface(cubeX, -cubeWidth, -cubeY, ';');
                    CalculateForSurface(cubeX, cubeWidth, cubeY, '+');
                }
            }

            cubeWidth = 10;
            horizontalOffset = 1 * cubeWidth;

            // second cube
            for (float cubeX = -cubeWidth; cubeX < cubeWidth; cubeX += incrementSpeed)
            {
                for (float cubeY = -cubeWidth; cubeY < cubeWidth; cubeY += incrementSpeed)
                {
                    CalculateForSurface(cubeX, cubeY, -cubeWidth, '@');
                    CalculateForSurface(cubeWidth, cubeY, cubeX, '$');
                    CalculateForSurface(-cubeWidth, cubeY, -cubeX, '~');
                    CalculateForSurface(-cubeX, cubeY, cubeWidth, '#');
                    CalculateForSurface(cubeX, -cubeWidth, -cubeY, ';');
                    CalculateForSurface(cubeX, cubeWidth, cubeY, '+');
                }
            }

            cubeWidth = 5;
            horizontalOffset = 8 * cubeWidth;

            // third cube
            for (float cubeX = -cubeWidth; cubeX < cubeWidth; cubeX += incrementSpeed)
            {
                for (float cubeY = -cubeWidth; cubeY < cubeWidth; cubeY += incrementSpeed)
                {
                    CalculateForSurface(cubeX, cubeY, -cubeWidth, '@');
                    CalculateForSurface(cubeWidth, cubeY, cubeX, '$');
                    CalculateForSurface(-cubeWidth, cubeY, -cubeX, '~');
                    CalculateForSurface(-cubeX, cubeY, cubeWidth, '#');
                    CalculateForSurface(cubeX, -cubeWidth, -cubeY, ';');
                    CalculateForSurface(cubeX, cubeWidth, cubeY, '+');
                }
            }

            Console.SetCursorPosition(0, 0);
            for (int k = 0; k < width * height; k++)
            {
                Console.Write(k % width == 0 ? Environment.NewLine : buffer[k].ToString());
            }

            A += 0.05f;
            B += 0.05f;
            C += 0.01f;
            System.Threading.Thread.Sleep(50);
        }
    }
}

