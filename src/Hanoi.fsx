open System

let print (board: int[]) (fil: int) (col: int) (lastNumber: int) =
    for c = col - 1 downto 0 do
        for f = 0 to fil - 1 do
            let esp = (lastNumber - board[col * f + c]) / 2
            // Left spaces
            for _ = 0 to esp - 1 do
                printf " "
            // Print asterisks
            for _ = 0 to board[col * f + c] - 1 do
                printf "*"
            // Right spaces
            for _ = 0 to esp - 1 do
                printf " "
            printf "\t"
        printfn ""

let moveDisk (board: int[]) (fil: int) (col: int) (lastNumber: int) (originRow: int) (destRow: int) =
    let mutable cO = col - 1
    let mutable cD = col - 1

    // Find the tallest (smallest) disk in the source row
    while cO >= 0 && board[col * originRow + cO] = 0 do
        cO <- cO - 1
    if cO < 0 then
        cO <- 0

    // Finding the First Free Position in the Target Row
    while cD >= 0 && board[col * destRow + cD] = 0 do
        cD <- cD - 1

    // Move the disk
    board[col * destRow + cD + 1] <- board[col * originRow + cO]
    board[col * originRow + cO] <- 0

    // Print the new board
    print board fil col lastNumber

let rec hanoi (board: int[]) (fil: int) (col: int) (disc: int) (lastNumber: int) (O: int) (A: int) (D: int) =
    match disc with
    | 1 -> 
        Console.Clear()
        moveDisk board fil col lastNumber O D
        let sleepTime =
            if col <= 5 then 800
            elif col <= 10 then 300
            elif col <= 15 then 60
            else 20
        System.Threading.Thread.Sleep(sleepTime)
    | _ -> 
        hanoi board fil col (disc - 1) lastNumber O D A
        Console.Clear()
        moveDisk board fil col lastNumber O D
        let sleepTime =
            if col <= 5 then 800
            elif col <= 10 then 300
            elif col <= 15 then 60
            else 20
        System.Threading.Thread.Sleep(sleepTime)
        hanoi board fil col (disc - 1) lastNumber A O D

let fil = 3
printf "Number of disks: "
let col = int (Console.ReadLine())

let newBoard = Array.zeroCreate (fil * col)

// Initialize the board
for c = col - 1 downto 0 do
    newBoard[c] <- col - c

let lastNumber = 2 * col - 1

// Print the initial board 
Console.Clear()
print newBoard fil col lastNumber
System.Threading.Thread.Sleep(1000)

// Initial call to the recursive function
hanoi newBoard fil col col lastNumber 0 1 2

0 
