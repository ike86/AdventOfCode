using System.Linq;

namespace AoC19
{
    class Computer
    {
        public Computer(string programCode)
        {
            Memory =
                programCode
                .Split(',')
                .Select(int.Parse)
                .ToArray();
        }

        public int[] Memory { get; private set; }

        public int Run()
        {
            var program = new Program(Memory);
            program.Run();
            return program.Code[0];
        }
    }
}