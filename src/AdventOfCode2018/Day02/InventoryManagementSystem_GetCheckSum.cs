using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace AoC18.Day02
{
    public class InventoryManagementSystem_GetCheckSum
    {
        [Fact]
        public void asd()
        {
            var ims = new InventoryManagementSystem();

            int checkSum = ims.GetCheckSum("abcdef");

            checkSum.Should().Be(0);
        }
    }
}
