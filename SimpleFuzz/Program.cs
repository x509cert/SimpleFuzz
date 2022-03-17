var data = "98722, 2022-03-16T15:50-06:00, PharmaA, 4, These are the last batches of PharmaA";
var fuzz = new SimpleFuzz(data);

class SimpleFuzz
{
    public SimpleFuzz(byte[] input, int threshold=5) { 
        _input = input;
        _threshold = threshold;
    }

    public SimpleFuzz(string input, int threshold = 5) {
        _input = System.Text.Encoding.UTF8.GetBytes(input);
        _threshold = threshold;
    }

    static byte[]? _input;
    static int ? _threshold;

    static public bool Fuzz(int? seed=null) {
        var wasFuzzed = false;
        var rnd = new Random(seed is null ? );

        if (_input is null) return false;
        if (rnd.Next(0, 100) > _threshold) return false;



        var input = new Span<byte>(_input);

        return wasFuzzed;
    }
}

