using Soenneker.HighLevel.Contacts.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.HighLevel.Contacts.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class HighLevelContactsUtilTests : HostedUnitTest
{
    private readonly IHighLevelContactsUtil _util;

    public HighLevelContactsUtilTests(Host host) : base(host)
    {
        _util = Resolve<IHighLevelContactsUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
