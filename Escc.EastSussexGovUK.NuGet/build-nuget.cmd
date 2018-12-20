@echo off
set nuspec="%1"
set nuspec=%nuspec:\=\\%
nuget pack "%nuspec%ClientDependency\Escc.EastSussexGovUK.ClientDependency.nuspec"
nuget pack "%nuspec%SecurityConfig\Escc.EastSussexGovUK.SecurityConfig.nuspec"
nuget pack "%nuspec%Metadata\Escc.EastSussexGovUK.Metadata.nuspec"