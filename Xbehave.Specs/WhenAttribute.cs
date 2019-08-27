using System;

namespace Xbehave.Specs {
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class WhenAttribute : StepDefinitionBaseAttribute {
		public WhenAttribute(string regex) : base(regex) { }
	}
}
