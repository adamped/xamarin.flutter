echo "test"
$webclient = New-Object System.Net.WebClient
$url = "https://github.com/flutter/flutter/archive/master.zip"
$zip = "flutter.zip"
echo "download $url to $zip"
$webclient.DownloadFile($url,$zip)
echo "finished download"

echo "unzipping file"  
$subfolder = 'flutter-master\packages\flutter'  # path in the zip
$shellApp = New-Object -ComObject Shell.Application 
$path = $shellApp.namespace("$PSScriptRoot\$zip\$subfolder") # complete subfolder path
$destination = $shellApp.namespace("$PSScriptRoot") #destination
$destination.CopyHere($path)

Move-Item -Path "flutter\*" -Destination ""

echo "cleaning up"
rm -r -fo "flutter"
rm $zip
echo "finished downloading flutter" 
echo "attempt to run flutter package get"
if (Get-Command flutter -errorAction SilentlyContinue)
{  
    flutter packages get
}
else{
    echo "cmdlet flutter is not available" 
} 