using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Xbehave.Specs {
	public static class Spec {
		private static readonly ConcurrentDictionary<Type, ImmutableList<(string conjunction, string regex, MethodInfo method)>> _stepDefinitionDictionary = new ConcurrentDictionary<Type, ImmutableList<(string conjunction, string regex, MethodInfo method)>>();
		private const string GIVEN = "Given";
		private const string AND = "And";
		private const string WHEN = "When";
		private const string THEN = "Then";

		public static void IsInXmlComments(object testContext) {
			StackTrace stackTrace = new StackTrace();
			MethodBase? scenarioMethod = stackTrace.GetFrames()
				.FirstOrDefault(frame => frame?.GetMethod()?.GetCustomAttributes(typeof(ScenarioAttribute), false)?.Any() == true)
				?.GetMethod();
			if (scenarioMethod == null) {
				throw new MissingScenarioDefinitionException();
			}
			string? scenarioDefinition = scenarioMethod.GetSummary();
			if (scenarioDefinition == null) {
				throw new MissingScenarioDefinitionException(scenarioMethod);
			}

			RunSteps(
				testContext: testContext,
				scenarioDefinition: string.Join(Environment.NewLine, scenarioDefinition.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Trim())),
				stepDefinitions: GetOrCollectStepDefinitions(scenarioMethod)
			);
		}

		public static void Is(object testContext, string scenarioDefinition) {
			StackTrace stackTrace = new StackTrace();
			MethodBase? scenarioMethod = stackTrace.GetFrames()
				.FirstOrDefault(frame => frame?.GetMethod()?.GetCustomAttributes(typeof(ScenarioAttribute), false)?.Any() == true)
				?.GetMethod();
			if (scenarioMethod == null) {
				throw new MissingScenarioDefinitionException();
			}

			RunSteps(
				testContext: testContext,
				scenarioDefinition: string.Join(Environment.NewLine, scenarioDefinition.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Trim())),
				stepDefinitions: GetOrCollectStepDefinitions(scenarioMethod)
			);
		}

		private static ImmutableList<(string conjunction, string regex, MethodInfo method)> GetOrCollectStepDefinitions(MethodBase scenarioMethod) {
			return _stepDefinitionDictionary.GetOrAdd(scenarioMethod.DeclaringType!, type => {
				ImmutableList<(string conjunction, string regex, MethodInfo method)>.Builder listBuilder = ImmutableList<(string conjunction, string regex, MethodInfo method)>.Empty.ToBuilder();
				foreach (MethodInfo method in scenarioMethod.DeclaringType!.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)) {
					foreach (StepDefinitionBaseAttribute stepDefinition in method.GetCustomAttributes<StepDefinitionBaseAttribute>()) {
						switch (stepDefinition) {
							case GivenAttribute given:
								listBuilder.Add((GIVEN, given.Regex, method));
								break;
							case WhenAttribute @when:
								listBuilder.Add((WHEN, when.Regex, method));
								break;
							case ThenAttribute then:
								listBuilder.Add((THEN, then.Regex, method));
								break;
						}
					}
				}
				return listBuilder.ToImmutable();
			});
		}

		private static void RunSteps(object testContext, string scenarioDefinition, ImmutableList<(string conjunction, string regex, MethodInfo method)> stepDefinitions) {
			string[] steps = scenarioDefinition.Split(Environment.NewLine);
			foreach (string step in steps) {
				Match? regexMatches = null;
				MethodInfo? matchingMethod = null;
				foreach ((string conjunction, string regex, MethodInfo method) in stepDefinitions) {
					string conjugatedStep;
					switch (conjunction) {
						case GIVEN:
							if (step.StartsWith(GIVEN, StringComparison.InvariantCultureIgnoreCase)) {
								conjugatedStep = step.Substring(GIVEN.Length + 1);
							} else if (step.StartsWith(AND, StringComparison.InvariantCultureIgnoreCase)) {
								conjugatedStep = step.Substring(AND.Length + 1);
							} else {
								continue;
							}
							break;
						case WHEN:
							if (step.StartsWith(WHEN, StringComparison.InvariantCultureIgnoreCase)) {
								conjugatedStep = step.Substring(WHEN.Length + 1);
							} else {
								continue;
							}
							break;
						case THEN:
							if (step.StartsWith(THEN, StringComparison.InvariantCultureIgnoreCase)) {
								conjugatedStep = step.Substring(THEN.Length + 1);
							} else {
								continue;
							}
							break;
						default:
							continue;
					}
					Match match = Regex.Match(conjugatedStep, regex, RegexOptions.IgnoreCase);
					if (match.Success) {
						regexMatches = match;
						matchingMethod = method;
						break;
					}
				}
				if (regexMatches == null || matchingMethod == null) {
					throw new MissingStepDefinitionException(step);
				}
				ParameterInfo[] parameters = matchingMethod.GetParameters();
				if (regexMatches.Groups.Count - 1 != parameters.Length) {
					throw new MissingStepDefinitionException(step);
				}
				object?[] parameterValues = new object[parameters.Length];
				for (int i = 0; i < parameters.Length; i++) {
					if (parameters[i].ParameterType == typeof(int)) {
						parameterValues[i] = int.Parse(regexMatches.Groups[i + 1].Value);
					} else if (parameters[i].ParameterType == typeof(double)) {
						parameterValues[i] = double.Parse(regexMatches.Groups[i + 1].Value);
					} else if (parameters[i].ParameterType == typeof(decimal)) {
						parameterValues[i] = decimal.Parse(regexMatches.Groups[i + 1].Value);
					} else if (parameters[i].ParameterType == typeof(string)) {
						parameterValues[i] = regexMatches.Groups[i + 1].Value[1..^1];
					} else {
						throw new NotImplementedException($"Step parameter of type {parameters[i].ParameterType.Name} is not supported yet. Supported types are int, double, decimal, and string.");
					}
				}
				if (matchingMethod.ReturnType == typeof(Task)) {
					step.x(() => (Task)matchingMethod.Invoke(testContext, parameterValues)!);
				} else {
					step.x(() => { matchingMethod.Invoke(testContext, parameterValues); });
				}
			}
		}
	}
}
