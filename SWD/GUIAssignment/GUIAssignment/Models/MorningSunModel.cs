using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIAssignment.Models
{
	public class MorningSunModel
	{
		public int LightIntensity { get; private set; }
		public bool Powered { get; private set; }

		public void ToggleLight() => Powered = !Powered;
		public void ChangeLightIntensity(int lightIntensity) => LightIntensity = lightIntensity; 


		public MorningSunModel() => Powered = false;

	}
}
