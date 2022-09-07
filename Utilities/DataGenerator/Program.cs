byte[] digits = {0x3F, 0x06, 0x5B, 0x4F, 0x66, 0x6D, 0x7D, 0x07, 0x7F, 0x6F};
int outCount = 0;
var newLineEvery = 10;

// 1s place
for (int value = 0; value <= byte.MaxValue; value++)
{
    Output(digits[(value / 1) % 10]);
}

// 10s place
for (int value = 0; value <= byte.MaxValue; value++)
{
    Output(digits[(value / 10) % 10]);
}

// 100s place
for (int value = 0; value <= byte.MaxValue; value++)
{
    Output(digits[(value / 100) % 10]);
}

// 1000s place
for (int value = 0; value <= byte.MaxValue; value++)
{
    Output(0); // Blank
}

void Output(byte value)
{
    Console.Write("0x{0:X2}, ", value);

    if (++outCount % newLineEvery == 0)
        Console.WriteLine();
}
