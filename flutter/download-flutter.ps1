Write-Output "test"
$url = "https://github.com/flutter/flutter/archive/master.zip"
$zip = "flutter.zip"
$ScriptDir = Split-Path $script:MyInvocation.MyCommand.Path

Write-Output "download $url to $zip"
Invoke-WebRequest -Uri $url -OutFile $zip
Write-Output "finished download"

Write-Output "unzipping file"  
$subfolder = 'flutter-master\packages\flutter'  # path in the zip
$shellApp = New-Object -ComObject Shell.Application 
$path = $shellApp.namespace("$PSScriptRoot\$zip\$subfolder") # complete subfolder path
$destinationDirectory = (Get-Item $ScriptDir).Parent
$destination = $shellApp.namespace($destinationDirectory.FullName) #destination
$destination.CopyHere($path)

Write-Output "cleaning up"
Remove-Item $zip
Write-Output "finished downloading flutter" 

Write-Output "attempt to run flutter package get"
if (Get-Command flutter -errorAction SilentlyContinue)
{  
    flutter packages get
}
else{
    Write-Output "cmdlet flutter is not available"
}