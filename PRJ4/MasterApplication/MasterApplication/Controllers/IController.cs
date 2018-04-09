using System;
using System.Collections.Generic;
using System.Text;
using MasterApplication.Models;

namespace MasterApplication.Controllers
{
    public interface IController
    {
	    ItemBase GetLatestItem();
    }
}
