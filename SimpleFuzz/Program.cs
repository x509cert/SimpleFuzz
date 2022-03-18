var data = "98722, 2022-03-16T15:50-06:00, PharmaA, 4, These are the last batches of PharmaA";
var fuzz = new SimpleFuzz(data, 88);
byte[]? fuzzedResult = fuzz.Fuzz(null) ;

if (fuzzedResult is not null) {
    string res = System.Text.Encoding.UTF8.GetString(fuzzedResult);
    Console.WriteLine(res);
}

public class SimpleFuzz
{
    public SimpleFuzz(byte[] input, int threshold=5) {
        _input = input;
        _threshold = threshold;
    }

    public SimpleFuzz(string input, int threshold = 5) {
        _input = System.Text.Encoding.UTF8.GetBytes(input);
        _threshold = threshold;
    }

    private readonly byte[]? _input;
    private readonly int ? _threshold;

    public byte[] Fuzz(int? seed=null) {
        var rnd = seed is null ? new Random() : new Random((int)seed);

        if (_input is null) return null;
        if (rnd.Next(0, 100) > _threshold) return null;

        var input = new Span<byte>(_input);

        var mutationCount = rnd.Next(1, 5);
        for (int i = 0; i < mutationCount; i++) {
            var whichMutation = rnd.Next(0, 7);

            int lo = rnd.Next(0, input.Length);
            int range = rnd.Next(1, input.Length / 10);
            if (lo + range >= input.Length) range = input.Length - lo;

            switch (whichMutation)
            {
                case 0: // set all upper bits to 1
                    for (int j = lo; j < lo + range; j++)
                        input[j] |= 0x80;
                    break;

                case 1: // set all upper bits to 0
                    for (int j = lo; j < lo + range; j++)
                        input[j] &= 0x7F;
                    break;

                case 2: // set one char to a random value
                    input[lo] = (byte)rnd.Next(0, 256);
                    break;

                case 3: // insert interesting numbers
                    byte[] interesting = new byte[] { 0, 1, 8, 7, 9, 16, 15, 17, 63, 64, 127, 128, 255 };
                    input[lo] = interesting[rnd.Next(0,interesting.Length)];
                    break;

                case 4: // swap bytes
                    for (int j = lo; j < lo + range; j++) {
                        if (j + 1 < input.Length)
                            (input[j + 1], input[j]) = (input[j], input[j + 1]);
                    }
                    break;

                case 5: // remove sections of the data
                    if (rnd.Next(100) > 50)
                        input = input[..lo];
                    else
                        input = input[(lo + range)..];
                    break;

                case 6: // add interesting pathname/filename characters
                    var fname = new string[] { "\\", "/", ":", ".."};

                    int which = rnd.Next(fname.Length);
                    for (int j = 0; j < fname[which].Length; j++)
                        if (lo+j < input.Length)
                            input[lo+j] = (byte)fname[which][j];
           
                    break;

                default:
                    break;
            }
        }

        return input.ToArray();
    }
}
