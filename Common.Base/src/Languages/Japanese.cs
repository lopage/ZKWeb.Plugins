﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.Base.src.Languages {
	/// <summary>
	/// 日语
	/// </summary>
	[ExportMany]
	public class Japanese : ILanguage {
		public string Name { get { return "ja-JP"; } }
	}
}
