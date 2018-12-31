using System;

namespace AoC18.Day03
{
    internal class Part2Solver
    {
        private readonly Claim[] claims;
        private readonly UnionClaim union;

        public Part2Solver(params Claim[] claims)
        {
            this.claims = claims;
            union = new UnionClaim(claims);
        }

        internal int GetIdOfOnlyNonOverlappingClaim()
        {
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