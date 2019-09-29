param($MigraterPath, $ConnectionString, $DatabaseName, $WebAppUser, $ServiceUserName, $ServiceUserPassword)

Start-Process -FilePath $MigraterPath -ArgumentList ("`"" + $ConnectionString + "`" `"" + $DatabaseName + "`" `"" + $WebAppUser + "`" `"" + $ServiceUserName + "`" `"" + $ServiceUserPassword + "`"")