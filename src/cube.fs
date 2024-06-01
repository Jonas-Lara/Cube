open System

let mutable A, B, C = 0.0, 0.0, 0.0

let mutable cubeWidth = 20.0
let width, height = 160, 44
let zBuffer = Array.create (width * height) 0.0
let buffer = Array.create (width * height) ' '
let backgroundASCIICode = '.'
let distanceFromCam = 100.0
let mutable horizontalOffset = 0.0
let K1 = 40.0
let incrementSpeed = 0.6

let mutable x, y, z = 0.0, 0.0, 0.0
let mutable ooz = 0.0
let mutable xp, yp, idx = 0, 0, 0

let calculateX i j k =
    j * (sin A) * (sin B) * (cos C) - k * (cos A) * (sin B) * (cos C) +
    j * (cos A) * (sin C) + k * (sin A) * (sin C) + i * (cos B) * (cos C)

let calculateY i j k =
    j * (cos A) * (cos C) + k * (sin A) * (cos C) -
    j * (sin A) * (sin B) * (sin C) + k * (cos A) * (sin B) * (sin C) -
    i * (cos B) * (sin C)

let calculateZ i j k =
    k * (cos A) * (cos B) - j * (sin A) * (cos B) + i * (sin B)

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

[<EntryPoint>]
let main argv =
    Console.Clear()

    while true do
        buffer.[0 .. width * height - 1] <- Array.init (width * height) (fun _ -> backgroundASCIICode)
        Array.fill zBuffer 0 (width * height) 0.0
        cubeWidth <- 20.0
        horizontalOffset <- -2.0 * cubeWidth

        for cubeX in -cubeWidth .. incrementSpeed .. cubeWidth do
            for cubeY in -cubeWidth .. incrementSpeed .. cubeWidth do
                calculateForSurface cubeX cubeY -cubeWidth '@'
                calculateForSurface cubeWidth cubeY cubeX '$'
                calculateForSurface -cubeWidth cubeY -cubeX '~'
                calculateForSurface -cubeX cubeY cubeWidth '#'
                calculateForSurface cubeX -cubeWidth -cubeY ';'
                calculateForSurface cubeX cubeWidth cubeY '+'

        cubeWidth <- 10.0
        horizontalOffset <- 1.0 * cubeWidth

        for cubeX in -cubeWidth .. incrementSpeed .. cubeWidth do
            for cubeY in -cubeWidth .. incrementSpeed .. cubeWidth do
                calculateForSurface cubeX cubeY -cubeWidth '@'
                calculateForSurface cubeWidth cubeY cubeX '$'
                calculateForSurface -cubeWidth cubeY -cubeX '~'
                calculateForSurface -cubeX cubeY cubeWidth '#'
                calculateForSurface cubeX -cubeWidth -cubeY ';'
                calculateForSurface cubeX cubeWidth cubeY '+'

        cubeWidth <- 5.0
        horizontalOffset <- 8.0 * cubeWidth

        for cubeX in -cubeWidth .. incrementSpeed .. cubeWidth do
            for cubeY in -cubeWidth .. incrementSpeed .. cubeWidth do
                calculateForSurface cubeX cubeY -cubeWidth '@'
                calculateForSurface cubeWidth cubeY cubeX '$'
                calculateForSurface -cubeWidth cubeY -cubeX '~'
                calculateForSurface -cubeX cubeY cubeWidth '#'
                calculateForSurface cubeX -cubeWidth -cubeY ';'
                calculateForSurface cubeX cubeWidth cubeY '+'

        Console.SetCursorPosition(0, 0)
        for k in 0 .. width * height - 1 do
            match k % width with
            | 0 -> Console.WriteLine()
            | _ -> Console.Write(buffer.[k])


        A <- A + 0.05
        B <- B + 0.05
        C <- C + 0.01
        System.Threading.Thread.Sleep(10)

    0 
