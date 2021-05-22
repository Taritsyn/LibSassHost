param($installPath, $toolsPath, $package, $project)

if ($project.Type -eq 'Web Site') {
	$projectDir = $project.Properties.Item('FullPath').Value

	$assemblyFile = Join-Path $projectDir 'bin/x64/libsass.dll'
	if (Test-Path $assemblyFile) {
		Remove-Item $assemblyFile -Force
	}
}