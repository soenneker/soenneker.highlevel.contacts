using Soenneker.HighLevel.Contacts.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.HighLevel.Contacts.Tests;

[Collection("Collection")]
public sealed class HighLevelContactsUtilTests : FixturedUnitTest
{
    private readonly IHighLevelContactsUtil _util;

    public HighLevelContactsUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IHighLevelContactsUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
