# Cube

![Cube](/img/cube.gif)

In this project I port [servetgulnaroglu's](https://github.com/servetgulnaroglu/cube.c) Cube code originally written in C to F#


## Some differences between C and F# code

### Type inferences

```C
float A, B, C;

float cubeWidth = 20;
int width = 160, height = 44;
```

```Fsharp
let mutable A, B, C = 0.0, 0.0, 0.0

let mutable cubeWidth = 20.0
let width, height = 160, 44
```

### Buffer declaration

```C
float zBuffer[160 * 44];
char buffer[160 * 44];
```

```Fsharp
let zBuffer = Array.create (width * height) 0.0
let buffer = Array.create (width * height) ' '
```

### Type inference in functions

```C
void calculateForSurface(float cubeX, float cubeY, float cubeZ, int ch) 
{
  x = calculateX(cubeX, cubeY, cubeZ);
  y = calculateY(cubeX, cubeY, cubeZ);
  z = calculateZ(cubeX, cubeY, cubeZ) + distanceFromCam;

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
```

```Fsharp
let calculateForSurface cubeX cubeY cubeZ ch =
    x <- calculateX cubeX cubeY cubeZ
    y <- calculateY cubeX cubeY cubeZ
    z <- calculateZ cubeX cubeY cubeZ + distanceFromCam

    ooz <- 1.0 / z

    xp <- int (float width / 2.0 + horizontalOffset + K1 * ooz * x * 2.0)
    yp <- int (float height / 2.0 + K1 * ooz * y)

    idx <- xp + yp * width
    if idx >= 0 && idx < width * height then
        if ooz > zBuffer.[idx] then
            zBuffer.[idx] <- ooz
            buffer.[idx] <- ch
```

### Buffer filling

```C
memset(buffer, backgroundASCIICode, width * height);
memset(zBuffer, 0, width * height * 4);
```

```Fsharp
buffer.[0 .. width * height - 1] <- Array.init (width * height) (fun _ -> backgroundASCIICode)
Array.fill zBuffer 0 (width * height) 0.0
```

### Buffer Empty

```C
printf("\x1b[H");
for (int k = 0; k < width * height; k++) 
{
    putchar(k % width ? buffer[k] : 10);
}
```

```Fsharp
Console.SetCursorPosition(0, 0)
for k in 0 .. width * height - 1 do
    match k % width with
    | 0 -> Console.WriteLine()
    | _ -> Console.Write(buffer.[k])
```

