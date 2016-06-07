﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.CustomTranslate.src.Scaffolding;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.CustomTranslators {
	/// <summary>
	/// 日语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Japanese : CustomTranslator {
		public override string Name { get { return "ja-JP"; } }
	}
}
