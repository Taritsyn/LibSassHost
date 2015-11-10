param($installPath, $toolsPath, $package, $project)

$assemblyDirectoryName = "LibSassHost.Native"
$assemblyFileNames = "LibSassHost.Native-32.dll", "LibSassHost.Native-64.dll"
$projectDirectoryPath = $project.Properties.Item("FullPath").Value

$packageAssemblyDirectoryPath = Join-Path $installPath (Join-Path "content" $assemblyDirectoryName)
$projectAssemblyDirectoryPath = Join-Path $projectDirectoryPath $assemblyDirectoryName

if (!(Test-Path $projectAssemblyDirectoryPath)) {
	New-Item -ItemType Directory -Force -Path $projectAssemblyDirectoryPath
}

foreach ($assemblyFileName in $assemblyFileNames) {
	$packageAssemblyFilePath = Join-Path $packageAssemblyDirectoryPath $assemblyFileName
	$projectAssemblyFilePath = Join-Path $projectAssemblyDirectoryPath $assemblyFileName

	if (!(Test-Path $projectAssemblyFilePath)) {
		Copy-Item $packageAssemblyFilePath $projectAssemblyDirectoryPath
	}
}

if ($project.Type -eq "Web Site") {
	$binDirectoryPath = Join-Path $projectDirectoryPath "bin"

	if (!(Test-Path $binDirectoryPath)) {
		New-Item -ItemType Directory -Force -Path $binDirectoryPath
	}

	Move-Item $projectAssemblyDirectoryPath $binDirectoryPath -Force
}
else {
	$assemblyDirectoryItem = $project.ProjectItems.Item($assemblyDirectoryName)

	foreach ($assemblyFileName in $assemblyFileNames) {
		$assemblyItem = $assemblyDirectoryItem.ProjectItems.Item($assemblyFileName)
		$assemblyItem.Properties.Item("BuildAction").Value = 0
		$assemblyItem.Properties.Item("CopyToOutputDirectory").Value = 2
	}
}