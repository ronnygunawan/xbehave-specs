using System;

namespace Xbehave.Specs {
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class GivenAttribute : StepDefinitionBaseAttribute {
		public GivenAttribute(string regex) : base(regex) { }
	}
}
