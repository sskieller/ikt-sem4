using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using MicrowaveOvenClasses;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;

namespace Microwave.Test.Integration
{
	[TestFixture]
	class IT4_UI
	{
		private UserInterface _ui;
		private IDisplay _display;
		private ILight _light;
		private IButton _button;
	}
}
