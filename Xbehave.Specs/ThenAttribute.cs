using System;

namespace Xbehave.Specs {
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class ThenAttribute : StepDefinitionBaseAttribute {
		public ThenAttribute(string regex) : base(regex) { }
	}
}
