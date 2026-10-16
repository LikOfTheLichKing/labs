using NetArchTest.Rules;
using Xunit;

namespace Marketplace.Tests.Architecture;

public class DependencyTests
{
    private const string DomainNamespace = "Marketplace.Domain";
    private const string ApplicationNamespace = "Marketplace.Application";
    private const string InfrastructureNamespace = "Marketplace.Infrastructure";
    private const string PresentationNamespace = "Marketplace.Presentation";

    [Fact]
    public void Domain_ShouldNotHaveDependencyOn_OtherLayers()
    {
        var assembly = typeof(Marketplace.Domain.Entities.Ad).Assembly;

        var otherNamespaces = new[]
        {
            ApplicationNamespace,
            InfrastructureNamespace,
            PresentationNamespace
        };

        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherNamespaces)
            .GetResult();

        Assert.True(result.IsSuccessful, "Domain шар не повинен залежати від інших шарів!");
    }

    [Fact]
    public void Application_ShouldNotHaveDependencyOn_InfrastructureOrPresentation()
    {
        var assembly = typeof(Marketplace.Application.Services.AdService).Assembly;

        var otherNamespaces = new[]
        {
            InfrastructureNamespace,
            PresentationNamespace
        };

        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherNamespaces)
            .GetResult();

        Assert.True(result.IsSuccessful, "Application шар не повинен залежати від Infrastructure та Presentation!");
    }
}