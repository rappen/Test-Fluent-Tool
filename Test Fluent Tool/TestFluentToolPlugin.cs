using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace Test_Fluent_Tool
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Test Fluent Tool"),
        ExportMetadata("Description", "This is a description for my first plugin"),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAMAAABEpIrGAAAAFXRFWHRDcmVhdGlvbiBUaW1lAAflBw4TDR5cYjbFAAAAB3RJTUUH5gwDDgoS2ESkGAAAAAlwSFlzAAAKjAAACowBvcbP2AAAAblQTFRFvb29lKW9c4y9Wnu1SnO1Unu1a4y9nKW9CEqtAEKtAEq9pa29OWu1OWuEnLVClLVjAGP/AFrvAFLWOWO1MWuMxtYp//8Arc5SAFrnCEKthKVSUnO1Y5R7e5S9a6WUAErOQmu1AEK1AGP3KWveY4S1a4StEGvvnK29AFLenMZSpc5KlJSUQnPOCGP3OVLGY0KUvdY5AHPGOZSctcZaQmPWOVK9SkqtWkqce3uUlGN7lDljxikppWtrtTE5EEqtCHPGEHveOYzvSpT3lL3/lJyltcbWtZS1nHNztTlKnL1Szucpe7VjrcatnISEpcZSEFKcGHO1jIyMlJSMnHt7vTExGFKcMWOM3u8YIXO1hIyMxsbG////771zxiEpQnN71tbWSntzQnOEzs4xhISMzs5S///n/+9rzkIhSnt79/cI7+8IjIyEMXut//9C//9j9+cACEqlpaVShISEMXul//+M//9rra1KjIx7jIxzIWveAGveCGvvEGv/5+//c63/IVreAGPvGFKtlJRrY6X/3t4hAHPOUpT/1tYpc4yUra1SrbW9hJy9a4S1WoyEa5RjIVqUrcY5hJS9hK1r+WS21QAAAAF0Uk5TAEDm2GYAAAIUSURBVHjabZPpW9NAEMbTZiuZVnQNugnUFpBqK3GVxdtq8D6qovHWiEdRa4uKRi0ICq23gGL9i80maZLSvp/22fk9O7Mz7whCSInGiihmGgmhoxJiHSBuC6AutjNLCFb7/mFHm+Q4oKXW+LK++pcQBTelxvVGOL4Cf8okDGAsQyYc/03IGgDnAmIZcul2wCZSXn16Pw4AGsoiuZWiQcyB6q/iOMa1+dAb8QHn/6A6wA/L+onxXBhQIWsDov0ATlcr5Ks1SfE3Dnz3n0jaQF3mwOznSnnGojVmA18W/Dp1JwOlNF21ZmYt68Mcm6cfPy1Quki5atAlpF47Hyi/sWxNvWUF8s58T6ZLzi25GRUyt9wjKVeKU4bxovDSNF9Nl/LuZbcoiI+Jr+KkYTx5aprPShMe8BwJsQcBcPHOXcN4aJr3J5rAIxSkIOT2GOPEvRBgp0hd8YFRxthVw9Cuaddv5P0is3BBUZQCT8C4Lmlcl/MK1zhEBEF3GkXIMGNjx0+cPHVaO3P2nHbebZRkdzLptJqMsNGRvfv2Hzh4SDt8JH/0mNtqZANZyHFg57DnhyFN20V373GHFeHjHBjchsOGGcps3+HNqsf1u9SPOzkKy72e+1MgdwJUiDZNKcLWdkCFWGBrEfrWAjIkw4sRlTZvaV2c3qjQosR66N6wMVi9nvbt7EJSc3klFOm84JGoiFBsXUv0P3p5d/+kHBttAAAAAElFTkSuQmCC"),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
        ExportMetadata("BackgroundColor", "#FFFFC0"),
        ExportMetadata("PrimaryFontColor", "#0000C0"),
        ExportMetadata("SecondaryFontColor", "#0000FF")]
    public class TestFluentToolPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new TestFluentTool();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TestFluentToolPlugin()
        {
            // If you have external assemblies that you need to load, uncomment the following to
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}