LibSass Host for .NET [![NuGet version](http://img.shields.io/nuget/v/LibSassHost.svg)](https://www.nuget.org/packages/LibSassHost/)  [![Download count](https://img.shields.io/nuget/dt/LibSassHost.svg)](https://www.nuget.org/packages/LibSassHost/)
=====================

<img src="https://raw.githubusercontent.com/Taritsyn/LibSassHost/master/images/LibSassHost_Logo.png" width="360" height="100" alt="LibSass Host logo" />

.NET wrapper around the [LibSass](http://sass-lang.com/libsass) library with the ability to support a virtual file system.

## Installation
This library can be installed through NuGet.
[LibSassHost](http://nuget.org/packages/LibSassHost/) package does not contain the native implementations of the LibSass.
Therefore, you need to choose and install the most appropriate package(s) for your platform.
The following packages are available:

 * [LibSassHost.Native.win-x86](http://nuget.org/packages/LibSassHost.Native.win-x86/)<sup>*</sup> contains the native assemblies for Windows (x86).
 * [LibSassHost.Native.win-x64](http://nuget.org/packages/LibSassHost.Native.win-x64/)<sup>*</sup> contains the native assemblies for Windows (x64).
 * [LibSassHost.Native.linux-x64](http://nuget.org/packages/LibSassHost.Native.linux-x64/) contains the native assemblies for Linux (x64). Only compatible with .NET Core.
 * [LibSassHost.Native.osx-x64](http://nuget.org/packages/LibSassHost.Native.osx-x64/) contains the native assemblies for OS X (x64). Only compatible with .NET Core.

<sup>* - Requires `msvcp140.dll` assembly from the [Visual C++ Redistributable for Visual Studio 2015](https://www.microsoft.com/en-us/download/details.aspx?id=53840).<sup>

If you need support for other operating systems, then you should read the [“Building LibSass”](#building-libsass) section.

### Mono support

LibSassHost.Native.linux-x64 and LibSassHost.Native.osx-x64 packages do not support installation under [Mono](http://www.mono-project.com/), but you can to install the native assemblies manually.

#### Linux

First you need to get the `libsass.so` assembly file. You have 3 ways to do this:

 1. [Download a assembly file](https://github.com/Taritsyn/LibSassHost/blob/master/lib/linux-x64/libsass.so) from the LibSass Host's project repository.
 1. Extract a assembly file from the [LibSassHost.Native.linux-x64](http://nuget.org/packages/LibSassHost.Native.linux-x64/) package. The `libsass.so` file is located in the `runtimes/linux-x64/native/` directory of NuGet package.
 1. [Build a assembly file](#building-libsass) from the source code.

Afterwards open a terminal window and change directory to the directory where the `libsass.so` file is located. Next, enter the following command:

```
sudo cp libsass.so /usr/local/lib/
sudo ldconfig
```

#### OS X

First you need to get the `libsass.dylib` assembly file. You have 3 ways to do this:

 1. [Download a assembly file](https://github.com/Taritsyn/LibSassHost/blob/master/lib/osx-x64/libsass.dylib) from the LibSass Host's project repository.
 1. Extract a assembly file from the [LibSassHost.Native.osx-x64](http://nuget.org/packages/LibSassHost.Native.osx-x64/) package. The `libsass.dylib` file is located in the `runtimes/osx-x64/native/` directory of NuGet package.
 1. [Build a assembly file](#building-libsass) from the source code.

Afterwards open a terminal window and change directory to the directory where the `libsass.dylib` file is located. Next, enter the following command:

```
mkdir -p /usr/local/lib/ && cp libsass.dylib "$_"
```

## Usage
The main difference between this library from other .NET wrappers around the LibSass (e.g. [libsassnet](https://github.com/darrenkopp/libsass-net/), [SharpScss](https://github.com/xoofx/SharpScss), [NSass](https://github.com/TBAPI-0KA/NSass), [Sass.Net](http://libsassnet.codeplex.com/)) is ability to support a virtual file system. You can set the file manager by using `FileManager` property of the <code title="LibSassHost.SassCompiler">SassCompiler</code> class:

```csharp
SassCompiler.FileManager = CustomFileManager();
```

Any class, that implements an <code title="LibSassHost.IFileManager">IFileManager</code> interface, can be used as a file manager.
By default, file manager is not specified, and for access to the file system are used built-in tools of the LibSass library.
The main advantage of using built-in tools is the low memory consumption.
But there is a disadvantage: there is no ability to process files in UTF-16 encoding.

To resolve this disadvantage you need to use the <code title="LibSassHost.FileManager">FileManager</code> class:

```csharp
SassCompiler.FileManager = FileManager.Instance;
```

But in this case, will increase memory consumption (approximately 3 times).

A good example of implementing a custom file manager, which provides access to the virtual file system, is the <a href="https://bundletransformer.codeplex.com/SourceControl/latest#BundleTransformer.SassAndScss/Internal/VirtualFileManager.cs" target="_blank"><code title="BundleTransformer.SassAndScss.Internal.VirtualFileManager">VirtualFileManager</code></a> class from the <a href="http://nuget.org/packages/BundleTransformer.SassAndScss" target="_blank">BundleTransformer.SassAndScss</a> package.

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

			try
			{
				var options = new CompilationOptions { SourceMap = true };
				CompilationResult result = SassCompiler.Compile(inputContent, "input.scss", "output.css",
					"output.css.map", options);

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
```

First we call the `Compile` method of <code title="LibSassHost.SassCompiler">SassCompiler</code> class with the following parameters:

 1. `content` - text content written on Sass/SCSS.
 1. `inputPath` - path to input Sass/SCSS file. Needed for generation of source map.
 1. `outputPath` (optional) - path to output CSS file. Needed for generation of source map. If path to output file is not specified, but specified a path to input file, then value of this parameter is obtained by replacing extension in the input file path by `.css` extension.
 1. `sourceMapPath` (optional) - path to source map file. If path to source map file is not specified, but specified a path to output file, then value of this parameter is obtained by replacing extension in the output file path by `.css.map` extension.
 1. `options` (optional) - compilation options (instance of the <code title="LibSassHost.CompilationOptions">CompilationOptions</code> class)

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
			const string basePath = "/Projects/TestSass";
			string inputFilePath = Path.Combine(basePath, "style.scss");
			string outputFilePath = Path.Combine(basePath, "style.css");
			string sourceMapFilePath = Path.Combine(basePath, "style.css.map");

			try
			{
				var options = new CompilationOptions { SourceMap = true };
				CompilationResult result = SassCompiler.CompileFile(inputFilePath, outputFilePath,
					sourceMapFilePath, options);

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
```

In this case, the `inputPath` parameter is used instead of the `content` parameter. Moreover, value of the `inputPath` parameter now should contain the path to real file.

## Building LibSass
LibSassHost uses a modified version of the LibSass library.
In most cases, you do not need to build the LibSass from source code, because the native assemblies is published as `LibSassHost.Native.*` NuGet packages.
The only exception is the case, when you want to build library for a specific Linux distro.

To build a modified version of the LibSass you must first clone the LibSassHost repository:

```
mkdir Github && cd Github
git clone https://github.com/Taritsyn/LibSassHost.git
```

Further actions depend on your operating system.

### Windows
In your system must be installed Visual Studio 2013, 2015 or 2017 with C++ support.

To build the LibSass on Windows:

 1. Open `libsass.sln` in Visual Studio.
 1. Select the **Configuration** and target **Platform**, and build the solution.
 1. Build output will be under `src\libsass\bin\[Debug|Release]\[Win32|x64]`.

Alternatively, you can use the build script.
Open a Visual Studio developer command prompt and run the `build-libsass.cmd` script from your `LibSassHost` project directory:

```
C:\Users\username\Github\LibSassHost> build-libsass
```

Build script can also take a options, information about which can be obtained by using the following command:

```
build-libsass /?
```

### Linux
In your system must be installed GCC (GNU Compiler Collection).
In every Linux distro installation of GCC is made in different ways.
For example, in Ubuntu 16.04 this is done as follows:

```
sudo apt-get update
sudo apt-get install build-essential
```

To build the LibSass on Linux open a terminal window and run the `build-libsass.sh` script from your `LibSassHost` project directory:

```
username@ubuntu-16:~/Github/LibSassHost$ ./build-libsass.sh
```

Build output will be under `src/libsass/bin/[Debug|Release]/Linux`.

Build script can also take a options, information about which can be obtained by using the following command:

```
./build-libsass.sh --help
```

### OS X
In your system must be installed Xcode Command Line Tools.
To install Xcode Command Line Tools, in your terminal simply run:

```
xcode-select --install
```

To build the LibSass on OS X open a terminal window and run the `build-libsass.sh` script from your `LibSassHost` project directory:

```
My-Mac:LibSassHost username$ ./build-libsass.sh
```

Build output will be under `src/libsass/bin/[Debug|Release]/OSX`.

Build script can also take a options, information about which can be obtained by using the following command:

```
./build-libsass.sh --help
```

## Who's Using LibSass Host for .NET
If you use the LibSass Host for .NET in some project, please send me a message so I can include it in this list:

 * [Bundle Transformer](https://github.com/Taritsyn/BundleTransformer) by Andrey Taritsyn
  * [VirtoCommerce Storefront for ASP.NET Core](https://github.com/VirtoCommerce/vc-storefront-core) by Virto Solutions LTD
