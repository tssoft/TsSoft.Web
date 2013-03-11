del TsSoft.Web.*.nupkg
del *.nuspec
del .\TsSoft.Web\bin\Release\*.nuspec

function GetNodeValue([xml]$xml, [string]$xpath)
{
	return $xml.SelectSingleNode($xpath).'#text'
}

function SetNodeValue([xml]$xml, [string]$xpath, [string]$value)
{
	$node = $xml.SelectSingleNode($xpath)
	if ($node) {
		$node.'#text' = $value
	}
}

Remove-Item .\TsSoft.Web\bin -Recurse 
Remove-Item .\TsSoft.Web\obj -Recurse 

$build = "c:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe ""TsSoft.Web\TsSoft.Web.csproj"" /p:Configuration=Release" 
Invoke-Expression $build

$Artifact = (resolve-path ".\TsSoft.Web\bin\Release\TsSoft.Web.dll").path

nuget spec -F -A $Artifact

Copy-Item .\TsSoft.Web.nuspec.xml .\TsSoft.Web\bin\Release\TsSoft.Web.nuspec

$GeneratedSpecification = (resolve-path ".\TsSoft.Web.nuspec").path
$TargetSpecification = (resolve-path ".\TsSoft.Web\bin\Release\TsSoft.Web.nuspec").path

[xml]$srcxml = Get-Content $GeneratedSpecification
[xml]$destxml = Get-Content $TargetSpecification
$value = GetNodeValue $srcxml "//version"
SetNodeValue $destxml "//version" $value;
$value = GetNodeValue $srcxml "//description"
SetNodeValue $destxml "//description" $value;
$value = GetNodeValue $srcxml "//copyright"
SetNodeValue $destxml "//copyright" $value;
$destxml.Save($TargetSpecification)


nuget pack $TargetSpecification

del *.nuspec
del .\TsSoft.Web\bin\Release\*.nuspec

# exit
