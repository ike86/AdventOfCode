using System;

namespace AoC18.Day03
{
    internal class Part2Solver
    {
        private Claim claim;

        public Part2Solver(Claim claim)
        {
            this.claim = claim;
        }

        internal int GetIdOfOnlyNonOverlappingClaim()
        {
            return claim.Id;
        }
    }
}