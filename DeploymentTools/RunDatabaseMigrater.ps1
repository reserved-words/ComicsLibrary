param($MigraterPath, $ConnectionString, $DatabaseName, $WebAppUser)

Start-Process -FilePath $MigraterPath -ArgumentList ("`"" + $ConnectionString + "`" `"" + $DatabaseName + "`" `"" + $WebAppUser + "`"")