using System;
using System.IO;
using DTO.Public.TODO;
using MicroDocum.Analyzers.Analizers;
using MicroDocum.Graphviz;
using MicroDocum.Themes.DefaultTheme;
using Xunit;

namespace CommonTests
{
    public class MicrodocumTests
    {
        private ListTODORequest link;

        public MicrodocumTests()
        {
            link = new DTO.Public.TODO.ListTODORequest();
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
            using (var file = new StreamWriter(path, false))
            {
                file.Write(graphwizFileData);
                Console.WriteLine("saved to " + path);
            }
        }
    }
}
