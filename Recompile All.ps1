# Calculate the project root from the invocation.
$projectRoot = $(split-path -parent $SCRIPT:MyInvocation.MyCommand.Path)

# Turn off verification for 64 bit applications.
&"$projectRoot\Client Compiler\bin\Development\Client Compiler.exe" -ns "GammaFour.SubscriptionManager" -i "C:\Source\subscription-manager\GammaFour.SubscriptionManager.ClientDataModel\DataModel.xsd" -out "C:\Source\subscription-manager\GammaFour.SubscriptionManager.ClientDataModel\DataModel.Designer.cs"
&"$projectRoot\Server Compiler\bin\Development\Server Compiler.exe" -ns "GammaFour.SubscriptionManager" -i "C:\Source\subscription-manager\GammaFour.SubscriptionManager.ServerDataModel\DataModel.xsd" -out "C:\Source\subscription-manager\GammaFour.SubscriptionManager.ServerDataModel\DataModel.Designer.cs"
&"$projectRoot\Data Service Compiler\bin\Development\Data Service Compiler.exe" -ns "GammaFour.SubscriptionManager" -i "C:\Source\subscription-manager\GammaFour.SubscriptionManager.DataService\DataModel.xsd" -out "C:\Source\subscription-manager\GammaFour.SubscriptionManager.DataService\DataModel.Designer.cs"
&"$projectRoot\Import Service Compiler\bin\Development\Import Service Compiler.exe" -ns "GammaFour.SubscriptionManager" -i "C:\Source\subscription-manager\GammaFour.SubscriptionManager.ImportService\DataModel.xsd" -out "C:\Source\subscription-manager\GammaFour.SubscriptionManager.ImportService\DataModel.Designer.cs"
&"$projectRoot\Persistent Store Compiler\bin\Development\Persistent Store Compiler.exe" -ns "GammaFour.SubscriptionManager" -i "C:\Source\subscription-manager\GammaFour.SubscriptionManager.ImportService\DataModel.xsd" -out "C:\Source\subscription-manager\GammaFour.SubscriptionManager.PersistentStore\DataModel.Designer.cs"
&"$projectRoot\Database Compiler\bin\Development\Database Compiler.exe" -i "C:\Source\subscription-manager\Database\DataModel.xsd" -out "C:\Source\subscription-manager\Database\DataModel.Designer.sql"