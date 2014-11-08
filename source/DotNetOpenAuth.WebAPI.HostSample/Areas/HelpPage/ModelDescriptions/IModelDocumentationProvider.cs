using System;
using System.Reflection;

namespace DotNetOpenAuth.WebAPI.HostSample.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}