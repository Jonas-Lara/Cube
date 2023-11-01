#include <cmath>
#include <iostream>
#include <vector>
#include <emscripten.h>

float A, B, C;

float cubeWidth = 20;
int width = 160, height = 44;
std::vector<float> zBuffer(width * height, 0.0f);
std::vector<char> buffer(width * height, '.');
int backgroundASCIICode = '.';
int distanceFromCam = 100;
float horizontalOffset;
float K1 = 40;

float incrementSpeed = 0.6;

float x, y, z;
float ooz;
int xp, yp;
int idx;

float calculateX(int i, int j, int k) {
    return j * sinf(A) * sinf(B) * cosf(C) - k * cosf(A) * sinf(B) * cosf(C) +
        j * cosf(A) * sinf(C) + k * sinf(A) * sinf(C) + i * cosf(B) * cosf(C);
}

float calculateY(int i, int j, int k) {
    return j * cosf(A) * cosf(C) + k * sinf(A) * cosf(C) -
        j * sinf(A) * sinf(B) * sinf(C) + k * cosf(A) * sinf(B) * sinf(C) -
        i * cosf(B) * sinf(C);
}

float calculateZ(int i, int j, int k) {
    return k * cosf(A) * cosf(B) - j * sinf(A) * cosf(B) + i * sinf(B);
}

void calculateForSurface(float cubeX, float cubeY, float cubeZ, int ch) {
    x = calculateX(cubeX, cubeY, cubeZ);
    y = calculateY(cubeX, cubeY, cubeZ);
    z = calculateZ(cubeX, cubeY, cubeZ) + distanceFromCam;

    ooz = 1.0f / z;

    xp = static_cast<int>(width / 2 + horizontalOffset + K1 * ooz * x * 2);
    yp = static_cast<int>(height / 2 + K1 * ooz * y);

    idx = xp + yp * width;
    if (idx >= 0 && idx < width * height) {
        if (ooz > zBuffer[idx]) {
            zBuffer[idx] = ooz;
            buffer[idx] = ch;
        }
    }
}

extern "C" {
    EMSCRIPTEN_KEEPALIVE
    void update() {
        std::fill(buffer.begin(), buffer.end(), backgroundASCIICode);
        std::fill(zBuffer.begin(), zBuffer.end(), 0.0f);
        cubeWidth = 20;
        horizontalOffset = -2 * cubeWidth;
        // first cube
        for (float cubeX = -cubeWidth; cubeX < cubeWidth; cubeX += incrementSpeed) {
            for (float cubeY = -cubeWidth; cubeY < cubeWidth; cubeY += incrementSpeed) {
                calculateForSurface(cubeX, cubeY, -cubeWidth, '@');
                calculateForSurface(cubeWidth, cubeY, cubeX, '$');
                calculateForSurface(-cubeWidth, cubeY, -cubeX, '~');
                calculateForSurface(-cubeX, cubeY, cubeWidth, '#');
                calculateForSurface(cubeX, -cubeWidth, -cubeY, ';');
                calculateForSurface(cubeX, cubeWidth, cubeY, '+');
            }
        }
        cubeWidth = 10;
        horizontalOffset = 1 * cubeWidth;
        // second cube
        for (float cubeX = -cubeWidth; cubeX < cubeWidth; cubeX += incrementSpeed) {
            for (float cubeY = -cubeWidth; cubeY < cubeWidth; cubeY += incrementSpeed) {
                calculateForSurface(cubeX, cubeY, -cubeWidth, '@');
                calculateForSurface(cubeWidth, cubeY, cubeX, '$');
                calculateForSurface(-cubeWidth, cubeY, -cubeX, '~');
                calculateForSurface(-cubeX, cubeY, cubeWidth, '#');
                calculateForSurface(cubeX, -cubeWidth, -cubeY, ';');
                calculateForSurface(cubeX, cubeWidth, cubeY, '+');
            }
        }
        cubeWidth = 5;
        horizontalOffset = 8 * cubeWidth;
        // third cube
        for (float cubeX = -cubeWidth; cubeX < cubeWidth; cubeX += incrementSpeed) {
            for (float cubeY = -cubeWidth; cubeY < cubeWidth; cubeY += incrementSpeed) {
                calculateForSurface(cubeX, cubeY, -cubeWidth, '@');
                calculateForSurface(cubeWidth, cubeY, cubeX, '$');
                calculateForSurface(-cubeWidth, cubeY, -cubeX, '~');
                calculateForSurface(-cubeX, cubeY, cubeWidth, '#');
                calculateForSurface(cubeX, -cubeWidth, -cubeY, ';');
                calculateForSurface(cubeX, cubeWidth, cubeY, '+');
            }
        }
    }
}

int main() {
    std::cout << "\x1b[2J";
    emscripten_set_main_loop(update, 0, 1);
    return 0;
}