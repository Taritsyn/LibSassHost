# LibSass Host for .NET

.NET wrapper around the [libSass](http://sass-lang.com/libsass) library with the ability to support a virtual file system.

## Installation
This library can be installed through NuGet:

 * [LibSass Host](http://nuget.org/packages/LibSassHost/1.0.0-alpha1)
   * [Windows (x86)](http://nuget.org/packages/LibSassHost.Native.win-x86/1.0.0-alpha1)<sup>*</sup>
   * [Windows (x64)](http://nuget.org/packages/LibSassHost.Native.win-x64/1.0.0-alpha1)<sup>*</sup>

<sup>* - Requires `msvcp140.dll` assembly from the [Visual C++ Redistributable for Visual Studio 2015](http://www.microsoft.com/en-us/download/details.aspx?id=48145).<sup>

## Usage
The main difference between this library from other .NET wrappers around the libSass (e.g. [libsassnet](https://github.com/darrenkopp/libsass-net/), [SharpScss](https://github.com/xoofx/SharpScss), [NSass](https://github.com/TBAPI-0KA/NSass), [Sass.Net](http://libsassnet.codeplex.com/)) is ability to support a virtual file system. When you create an instance of the <code title="LibSassHost.SassCompiler">SassCompiler</code> class, you can pass an file manager through the constructor:

```csharp
using (var compiler = new SassCompiler(new CustomFileManager()))
{
	…
}
```

Any class, that implements an <code title="LibSassHost.IFileManager">IFileManager</code> interface, can be used as a file manager. By default, as file manager uses an instance of the <code title="LibSassHost.FileManager">FileManager</code> class, that is responsible for access to the Windows file system. A good example of implementing a custom file manager, which provides access to the virtual file system, is the <a href="https://bundletransformer.codeplex.com/SourceControl/latest#BundleTransformer.SassAndScss/Internal/VirtualFileManager.cs" target="_blank"><code title="BundleTransformer.SassAndScss.Internal.VirtualFileManager">VirtualFileManager</code></a> class from the <a href="http://nuget.org/packages/BundleTransformer.SassAndScss" target="_blank">BundleTransformer.SassAndScss</a> package.

It should also be noted, that this library does not write the result of compilation to disk. `Compile` and `CompileFile` methods of the <code title="LibSassHost.SassCompiler">SassCompiler</code> class return the result of compilation in the form of an instance of the <code title="LibSassHost.CompilationResult">CompilationResult</code> class. Consider in detail properties of the <code title="LibSassHost.CompilationResult">CompilationResult</code> class:

<table border="1" style="font-size: 0.7em">
	<thead>
		<tr valign="top">
			<th>Property name</th>
			<th>Data&nbsp;type</th>
			<th>Description</th>
		</tr>
	</thead>
	<tbody>
		<tr valign="top">
			<td><code>CompiledContent</code></td>
			<td><code title="System.String">String</code></td>
			<td>CSS code.</td>
		</tr>
		<tr valign="top">
			<td><code>IncludedFilePaths</code></td>
			<td><code title="System.Collections.Generic.IList&lt;string&gt;">IList&lt;string&gt;</code></td>
			<td>List of included files.</td>
		</tr>
		<tr valign="top">
			<td><code>SourceMap</code></td>
			<td><code title="System.String">String</code></td>
			<td>Source map.</td>
		</tr>
	</tbody>
</table>

Consider a simple example of usage of the `Compile` method:

```csharp
using System;

using LibSassHost;
using LibSassHost.Helpers;

namespace LibSassHost.Example.ConsoleApplication
{
	class Program
	{
		static void Main(string[] args)
		{
			const string inputContent = @"$font-stack:    Helvetica, sans-serif;
$primary-color: #333;

body {
  font: 100% $font-stack;
  color: $primary-color;
}";

			using (var compiler = new SassCompiler())
			{
				try
				{
					var options = new CompilationOptions { SourceMap = true };
					CompilationResult result = compiler.Compile(inputContent, "input.scss", "output.css",
						options);

					Console.WriteLine("Compiled content:{1}{1}{0}{1}", result.CompiledContent,
						Environment.NewLine);
					Console.WriteLine("Source map:{1}{1}{0}{1}", result.SourceMap, Environment.NewLine);
					Console.WriteLine("Included file paths: {0}", 
						string.Join(", ", result.IncludedFilePaths));
				}
				catch (SassСompilationException e)
				{
					Console.WriteLine("During compilation of SCSS code an error occurred. See details:");
					Console.WriteLine();
					Console.WriteLine(SassErrorHelpers.Format(e));
				}
			}
		}
	}
}
```

First we create an instance of the <code title="LibSassHost.SassCompiler">SassCompiler</code> class, and then call its the `Compile` method with the following parameters:

 1. `content` - text content written on Sass/SCSS.
 2. `inputPath` (optional) - path to input Sass/SCSS file. Needed for generation of source map.
 3. `outputPath` (optional) - path to output CSS file. Needed for generation of source map. If path to output file is not specified, but specified a path to input file, then value of this parameter is obtained by replacing extension in the input file path by `.css` extension.
 4. `options` (optional) - compilation options (instance of the <code title="LibSassHost.CompilationOptions">CompilationOptions</code> class)

Then output result of compilation to the console. In addition, we provide handling of the <code title="LibSassHost.SassСompilationException">SassСompilationException</code> exception.

And now let's consider in detail properties of the <code title="LibSassHost.CompilationOptions">CompilationOptions</code> class:

<table border="1" style="font-size: 0.7em">
	<thead>
		<tr valign="top">
			<th>Property name</th>
			<th>Data&nbsp;type</th>
			<th>Default value</th>
			<th>Description</th>
		</tr>
	</thead>
	<tbody>
		<tr valign="top">
			<td><code>IncludePaths</code></td>
			<td><code title="System.Collections.Generic.IList&lt;string&gt;">IList&lt;string&gt;</code></td>
			<td>Empty list</td>
			<td>List of include paths.</td>
		</tr>
		<tr valign="top">
			<td><code>IndentedSyntax</code></td>
			<td><code title="System.Boolean">Boolean</code></td>
			<td><code>false</code></td>
			<td>Flag for whether to enable Sass Indented Syntax for parsing the data string or file.</td>
		</tr>
		<tr valign="top">
			<td><code>IndentType</code></td>
			<td><code title="LibSassHost.IndentType">IndentType</code> enumeration</td>
			<td><code>Space</code></td>
			<td>Indent type. Can take the following values:
				<ul>
					<li><code>Space</code> - space character</li>
					<li><code>Tab</code> - tab character</li>
				</ul>
			</td>
		</tr>
		<tr valign="top">
			<td><code>IndentWidth</code></td>
			<td><code title="System.Int32">Int32</code></td>
			<td><code>2</code></td>
			<td>Number of spaces or tabs to be used for indentation.</td>
		</tr>
		<tr valign="top">
			<td><code>InlineSourceMap</code></td>
			<td><code title="System.Boolean">Boolean</code></td>
			<td><code>false</code></td>
			<td>Flag for whether to embed <code>sourceMappingUrl</code> as data uri.</td>
		</tr>
		<tr valign="top">
			<td><code>LineFeedType</code></td>
			<td><code title="LibSassHost.LineFeedType">LineFeedType</code> enumeration</td>
			<td><code>Lf</code></td>
			<td>Line feed type. Can take the following values:
				<ul>
					<li><code>Cr</code> - Macintosh (CR)</li>
					<li><code>CrLf</code> - Windows (CR LF)</li>
					<li><code>Lf</code> - Unix (LF)</li>
					<li><code>LfCr</code></li>
				</ul>
			</td>
		</tr>
		<tr valign="top">
			<td><code>OmitSourceMapUrl</code></td>
			<td><code title="System.Boolean">Boolean</code></td>
			<td><code>false</code></td>
			<td>Flag for whether to disable <code>sourceMappingUrl</code> in css output.</td>
		</tr>
		<tr valign="top">
			<td><code>OutputStyle</code></td>
			<td><code title="LibSassHost.OutputStyle">OutputStyle</code> enumeration</td>
			<td><code>Nested</code></td>
			<td>Output style for the generated css code. Can take the following values:
				<ul>
					<li><code>Nested</code></li>
					<li><code>Expanded</code></li>
					<li><code>Compact</code></li>
					<li><code>Compressed</code></li>
				</ul>
			</td>
		</tr>
		<tr valign="top">
			<td><code>Precision</code></td>
			<td><code title="System.Int32">Int32</code></td>
			<td><code>5</code></td>
			<td>Precision for fractional numbers.</td>
		</tr>
		<tr valign="top">
			<td><code>SourceComments</code></td>
			<td><code title="System.Boolean">Boolean</code></td>
			<td><code>false</code></td>
			<td>Flag for whether to emit comments in the generated CSS indicating the corresponding source line.</td>
		</tr>
		<tr valign="top">
			<td><code>SourceMap</code></td>
			<td><code title="System.Boolean">Boolean</code></td>
			<td><code>false</code></td>
			<td>Flag for whether to enable source map generation.</td>
		</tr>
		<tr valign="top">
			<td><code>SourceMapFilePath</code></td>
			<td><code title="System.String">String</code></td>
			<td>Empty string</td>
			<td>Path to source map file.</td>
		</tr>
		<tr valign="top">
			<td><code>SourceMapFileUrls</code></td>
			<td><code title="System.Boolean">Boolean</code></td>
			<td><code>false</code></td>
			<td>Flag for whether to create file urls for sources.</td>
		</tr>
		<tr valign="top">
			<td><code>SourceMapIncludeContents</code></td>
			<td><code title="System.Boolean">Boolean</code></td>
			<td><code>false</code></td>
			<td>Flag for whether to include contents in maps.</td>
		</tr>
		<tr valign="top">
			<td><code>SourceMapRootPath</code></td>
			<td><code title="System.String">String</code></td>
			<td>Empty string</td>
			<td>Value will be emitted as <code>sourceRoot</code> in the source map information.</td>
		</tr>
	</tbody>
</table>

Using of the `CompileFile` method quite a bit different from using of the `Compile` method:

```csharp
using System;
using System.IO;

using LibSassHost;
using LibSassHost.Helpers;

namespace LibSassHost.Example.ConsoleApplication
{
	class Program
	{
		static void Main(string[] args)
		{
			const string basePath = @"C:\Projects\TestSass";
			string inputFilePath = Path.Combine(basePath, "style.scss");
			string outputFilePath = Path.Combine(basePath, "style.css");

			using (var compiler = new SassCompiler())
			{
				try
				{
					var options = new CompilationOptions { SourceMap = true };
					CompilationResult result = compiler.CompileFile(inputFilePath, outputFilePath, options);

					Console.WriteLine("Compiled content:{1}{1}{0}{1}", result.CompiledContent,
						Environment.NewLine);
					Console.WriteLine("Source map:{1}{1}{0}{1}", result.SourceMap, Environment.NewLine);
					Console.WriteLine("Included file paths: {0}",
						string.Join(", ", result.IncludedFilePaths));
				}
				catch (SassСompilationException e)
				{
					Console.WriteLine("During compilation of SCSS file an error occurred. See details:");
					Console.WriteLine();
					Console.WriteLine(SassErrorHelpers.Format(e));
				}
			}
		}
	}
}
```

In this case, the `inputPath` parameter became mandatory and is used instead of the `content` parameter. Moreover, value of the `inputPath` parameter now should contain the path to real file.


## Who's Using LibSass Host for .NET
If you use the LibSass Host for .NET in some project, please send me a message so I can include it in this list:

 * [Bundle Transformer](http://bundletransformer.codeplex.com/) by Andrey Taritsyn