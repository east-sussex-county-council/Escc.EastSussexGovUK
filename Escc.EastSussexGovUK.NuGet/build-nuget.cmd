@echo off
set nuspec="%1"
set nuspec=%nuspec:\=\\%
nuget pack "%nuspec%Escc.EastSussexGovUK.nuspec"
nuget pack "%nuspec%ClientDependency\Escc.EastSussexGovUK.ClientDependency.nuspec"
nuget pack "%nuspec%SecurityConfig\Escc.EastSussexGovUK.SecurityConfig.nuspec"