using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Xunit;

namespace Escc.EastSussexGovUK.Core.Tests
{
    
    public class ClientDependencySetEvaluatorTests
    {
        [Fact]
        public void ClientDependencySetEvaluator_adds_CSS_when_required()
        {
            var set = new Mock<IClientDependencySet>();
            set.Setup(x => x.IsRequired()).Returns(true);
            set.Setup(x => x.RequiresCss()).Returns(new[] { new CssFileDependency() });
            var evaluator = new ClientDependencySetEvaluator();

            evaluator.EvaluateDependencySet(set.Object);

            Assert.Equal(1, evaluator.RequiredCss.Count);
        }

        [Fact]
        public void ClientDependencySetEvaluator_does_not_add_CSS_when_not_required()
        {
            var set = new Mock<IClientDependencySet>();
            set.Setup(x => x.IsRequired()).Returns(false);
            var evaluator = new ClientDependencySetEvaluator();

            evaluator.EvaluateDependencySet(set.Object);

            Assert.Equal(0, evaluator.RequiredCss.Count);
        }

        [Fact]
        public void ClientDependencySetEvaluator_adds_JS_when_required()
        {
            var set = new Mock<IClientDependencySet>();
            set.Setup(x => x.IsRequired()).Returns(true);
            set.Setup(x => x.RequiresJavaScript()).Returns(new[] { new JsFileDependency() });
            var evaluator = new ClientDependencySetEvaluator();

            evaluator.EvaluateDependencySet(set.Object);

            Assert.Equal(1, evaluator.RequiredJavaScript.Count);
        }

        [Fact]
        public void ClientDependencySetEvaluator_does_not_add_JS_when_not_required()
        {
            var set = new Mock<IClientDependencySet>();
            set.Setup(x => x.IsRequired()).Returns(false);
            var evaluator = new ClientDependencySetEvaluator();

            evaluator.EvaluateDependencySet(set.Object);

            Assert.Equal(0, evaluator.RequiredJavaScript.Count);
        }

        [Fact]
        public void ClientDependencySetEvaluator_adds_CSP_when_required()
        {
            var set = new Mock<IClientDependencySet>();
            set.Setup(x => x.IsRequired()).Returns(true);
            set.Setup(x => x.RequiresContentSecurityPolicy()).Returns(new[] { new ContentSecurityPolicyDependency() });
            var evaluator = new ClientDependencySetEvaluator();

            evaluator.EvaluateDependencySet(set.Object);

            Assert.Equal(1, evaluator.RequiredContentSecurityPolicy.Count);
        }

        [Fact]
        public void ClientDependencySetEvaluator_does_not_add_CSP_when_not_required()
        {
            var set = new Mock<IClientDependencySet>();
            set.Setup(x => x.IsRequired()).Returns(false);
            var evaluator = new ClientDependencySetEvaluator();

            evaluator.EvaluateDependencySet(set.Object);

            Assert.Equal(0, evaluator.RequiredContentSecurityPolicy.Count);
        }
    }
}
