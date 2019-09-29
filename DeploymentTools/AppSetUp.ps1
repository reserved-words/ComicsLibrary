param($DomainName, $AppName, $ServiceUser, $ServicePassword)

Import-Module $PSScriptRoot\SetupTools.psm1

Run-Setup -DomainName $DomainName
Setup-WebApp -AppName $AppName
Setup-Service -UserName $ServiceUser -Password $ServicePassword -AppName $AppName