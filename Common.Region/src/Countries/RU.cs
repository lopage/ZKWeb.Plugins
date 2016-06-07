﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Region.src.Model;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.Region.src.Countries {
	/// <summary>
	/// 俄罗斯
	/// </summary>
	[ExportMany, SingletonReuse]
	public class RU : Country {
		public override string Name { get { return "RU"; } }
	}
}
