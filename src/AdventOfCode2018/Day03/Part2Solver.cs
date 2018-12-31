using System;

namespace AoC18.Day03
{
    internal class Part2Solver
    {
        private Claim[] claims;

        public Part2Solver(params Claim[] claims)
        {
            this.claims = claims;
        }

        internal int GetIdOfOnlyNonOverlappingClaim()
        {
            /*var claims = input
                .Select(i => Claim.Parse(i))
                .ToArray();*/
            var union = new UnionClaim(claims);

            foreach (var claim in claims)
            {
                var isMatching = true;
                for (int i = claim.XOffset; i < claim.BottomRightX; i++)
                {
                    for (int j = claim.YOffset; j < claim.BottomRightY; j++)
                    {
                        if (isMatching && union[i, j] >= 2)
                        {
                            isMatching = false;
                        }
                    }
                }

                if (isMatching)
                {
                    return claim.Id;
                }
            }

            throw new ArgumentException();
        }
    }
}