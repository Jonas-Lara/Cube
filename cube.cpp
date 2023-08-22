#include <cmath>
#include <iostream>
#include <vector>
#include <thread>
#include <chrono>

const int width = 160;
const int height = 44;

class Cube {
private:
    float A, B, C;
    float horizontalOffset;
    const float K1 = 40;
    const int distanceFromCam = 100;
    float incrementSpeed = 0.6;

public:
    Cube(float initialA, float initialB, float initialC, float initialHorizontalOffset, float initialIncrementSpeed)
        : A(initialA), B(initialB), C(initialC), horizontalOffset(initialHorizontalOffset), incrementSpeed(initialIncrementSpeed) {}

    float calculateX(int i, int j, int k) {
        return j * std::sin(A) * std::sin(B) * std::cos(C) - k * std::cos(A) * std::sin(B) * std::cos(C) +
               j * std::cos(A) * std::sin(C) + k * std::sin(A) * std::sin(C) + i * std::cos(B) * std::cos(C);
    }

    float calculateY(int i, int j, int k) {
        return j * std::cos(A) * std::cos(C) + k * std::sin(A) * std::cos(C) -
               j * std::sin(A) * std::sin(B) * std::sin(C) + k * std::cos(A) * std::sin(B) * std::sin(C) -
               i * std::cos(B) * std::sin(C);
    }

    float calculateZ(int i, int j, int k) {
        return k * std::cos(A) * std::cos(B) - j * std::sin(A) * std::cos(B) + i * std::sin(B);
    }

    void calculateForSurface(std::vector<float>& zBuffer, std::vector<char>& buffer, char ch, int cubeX, int cubeY, int cubeZ) {
        float x = calculateX(cubeX, cubeY, cubeZ);
        float y = calculateY(cubeX, cubeY, cubeZ);
        float z = calculateZ(cubeX, cubeY, cubeZ) + distanceFromCam;

        float ooz = 1 / z;

        int xp = static_cast<int>(width / 2 + horizontalOffset + K1 * ooz * x * 2);
        int yp = static_cast<int>(height / 2 + K1 * ooz * y);

        int idx = xp + yp * width;
        if (idx >= 0 && idx < width * height) {
            if (ooz > zBuffer[idx]) {
                zBuffer[idx] = ooz;
                buffer[idx] = ch;
            }
        }
    }

    void drawCube(std::vector<float>& zBuffer, std::vector<char>& buffer) {
        float cubeWidth = 20;
        // Draw first cube
        for (float cubeX = -cubeWidth; cubeX < cubeWidth; cubeX += incrementSpeed) {
            for (float cubeY = -cubeWidth; cubeY < cubeWidth; cubeY += incrementSpeed) {
                calculateForSurface(zBuffer, buffer, '@', cubeX, cubeY, -static_cast<int>(cubeWidth));
                calculateForSurface(zBuffer, buffer, '$', cubeWidth, cubeY, cubeX);
                calculateForSurface(zBuffer, buffer, '~', -cubeWidth, cubeY, -cubeX);
                calculateForSurface(zBuffer, buffer, '#', -cubeX, cubeY, cubeWidth);
                calculateForSurface(zBuffer, buffer, ';', cubeX, -cubeWidth, -cubeY);
                calculateForSurface(zBuffer, buffer, '+', cubeX, cubeWidth, cubeY);
            }
        }

        cubeWidth = 7;
        // Draw second cube
        for (float cubeX = -cubeWidth; cubeX < cubeWidth; cubeX += incrementSpeed) {
            for (float cubeY = -cubeWidth; cubeY < cubeWidth; cubeY += incrementSpeed) {
                calculateForSurface(zBuffer, buffer, '@', cubeX, cubeY, -static_cast<int>(cubeWidth));
                calculateForSurface(zBuffer, buffer, '$', cubeWidth, cubeY, cubeX);
                calculateForSurface(zBuffer, buffer, '~', -cubeWidth, cubeY, -cubeX);
                calculateForSurface(zBuffer, buffer, '#', -cubeX, cubeY, cubeWidth);
                calculateForSurface(zBuffer, buffer, ';', cubeX, -cubeWidth, -cubeY);
                calculateForSurface(zBuffer, buffer, '+', cubeX, cubeWidth, cubeY);
            }
        }

        cubeWidth = 3;
        // Draw third cube
        for (float cubeX = -cubeWidth; cubeX < cubeWidth; cubeX += incrementSpeed) {
            for (float cubeY = -cubeWidth; cubeY < cubeWidth; cubeY += incrementSpeed) {
                calculateForSurface(zBuffer, buffer, '@', cubeX, cubeY, -static_cast<int>(cubeWidth));
                calculateForSurface(zBuffer, buffer, '$', cubeWidth, cubeY, cubeX);
                calculateForSurface(zBuffer, buffer, '~', -cubeWidth, cubeY, -cubeX);
                calculateForSurface(zBuffer, buffer, '#', -cubeX, cubeY, cubeWidth);
                calculateForSurface(zBuffer, buffer, ';', cubeX, -cubeWidth, -cubeY);
                calculateForSurface(zBuffer, buffer, '+', cubeX, cubeWidth, cubeY);
            }
        }
    }

    void rotate(float dA, float dB, float dC) {
        A += dA;
        B += dB;
        C += dC;
    }
};

class Application {
private:
    std::vector<float> zBuffer;
    std::vector<char> buffer;
    Cube cube1, cube2, cube3;

public:
    Application() : zBuffer(width * height, 0), buffer(width * height, '.'),
                cube1(0.0f, 0.0f, 0.0f, -2 * 20, 0.6f),
                cube2(0.0f, 0.0f, 0.0f, 1 * 25, 0.6f),   // Adjust horizontalOffset value
                cube3(0.0f, 0.0f, 0.0f, 8 * 50, 0.6f) {   // Adjust horizontalOffset value
}


    void run() {
        std::cout << "\x1b[2J";
        while (true) {
            std::fill(buffer.begin(), buffer.end(), '.');
            std::fill(zBuffer.begin(), zBuffer.end(), 0.0f);

            cube1.drawCube(zBuffer, buffer);
            cube2.drawCube(zBuffer, buffer);
            cube3.drawCube(zBuffer, buffer);

            std::cout << "\x1b[H";
            for (int k = 0; k < width * height; k++) {
                std::cout << buffer[k];
                if ((k + 1) % width == 0) {
                    std::cout << '\n';
                }
            }

            cube1.rotate(0.05f, 0.05f, 0.01f);
            cube2.rotate(0.05f, 0.05f, 0.01f);
            cube3.rotate(0.05f, 0.05f, 0.01f);

            std::this_thread::sleep_for(std::chrono::microseconds(8000 * 2));
        }
    }
};

int main() {
    Application app;
    app.run();
    return 0;
}

