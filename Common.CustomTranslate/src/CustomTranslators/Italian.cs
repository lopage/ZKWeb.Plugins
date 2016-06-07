﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.CustomTranslate.src.Scaffolding;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.CustomTranslators {
	/// <summary>
	/// 意大利语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Italian : CustomTranslator {
		public override string Name { get { return "it-IT"; } }
	}
}
