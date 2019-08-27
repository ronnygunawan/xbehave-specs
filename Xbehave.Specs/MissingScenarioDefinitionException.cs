using System;
using System.Reflection;

namespace Xbehave.Specs {
	public class MissingScenarioDefinitionException : Exception {
		public MissingScenarioDefinitionException() : base($"Scenario is not decorated with [SpecBehave.Scenario] attribute.") { }
		public MissingScenarioDefinitionException(MethodBase method) : base($"Scenario '{method.DeclaringType?.Name}.{method.Name}' doesn't have scenario definition.") { }
	}
}
