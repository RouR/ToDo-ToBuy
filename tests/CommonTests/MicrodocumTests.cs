using System;
using System.IO;
using DTO.Public.TODO;
using MicroDocum.Analyzers.Analizers;
using MicroDocum.Graphviz;
using MicroDocum.Themes.DefaultTheme;
using Xunit;
using Xunit.Abstractions;

namespace CommonTests
{
    public class MicrodocumTests
    {
        private readonly ITestOutputHelper _outputHelper;
        private ListTODORequest _linkForLoadAssembly;

        public MicrodocumTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _linkForLoadAssembly = new DTO.Public.TODO.ListTODORequest();
        }
        [Fact]
        public void MicrodocumTests_Should_AnalizeDTO()
        {
            //Given
            var theme = new DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            //When
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes());
            //Then
            Assert.True(c.Nodes.Count > 0, "c.Nodes.Count > 0");
        }

        [Fact]
        public void MicrodocumTests_Should_GenerateGraphwiz()
        {
            //Given
            var theme = new DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes());
            var gen = new GraphvizDotGenerator<DefaultLinkStyle>(new DefaultTheme());
            //When
            var graphwizFileData = gen.Generate(c);
            //Then
            Assert.True(!string.IsNullOrWhiteSpace(graphwizFileData));
            var path = Directory.GetCurrentDirectory();
            path = Path.Combine(path, "DTO_routing.dot");
            using (var file = new StreamWriter(path, false))
            {
                file.Write(graphwizFileData);
                _outputHelper.WriteLine("saved to " + path);
            }
        }
    }
}
