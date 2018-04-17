using System;
using FWPS;
using FWPS.Controllers;
using FWPS.Models;
using Microsoft.AspNetCore.SignalR;
using NSubstitute;
using NUnit.Framework;


namespace WebApi.Unit.Test
{
	[TestFixture]
    public class UT_LightController
	{
	    private LightController lc;
	    private LightItem li;
	    private IHubContext<DevicesHub> hub;

        [SetUp]
        public void SetUp()
        {
            li = new LightItem();
            hub = Substitute.For<IHubContext<DevicesHub>>();

            

            lc = new LightController(, hub);
        }

	    [TearDown]
	    public void TearDown()
	    {

	    }

	    [Test]
	    public void TestSomething()
	    {
            
            
		    bool hej = true;
		    Assert.That(hej);
	    }
    }
}
