rmdir /s /q C:\Users\menno\CodingConnected\Repos\TLCGen\published
mkdir C:\Users\menno\CodingConnected\Repos\TLCGen\published
rmdir /s /q C:\Users\menno\CodingConnected\Repos\TLCGen\TLCGen\bin\Release
MSBuild C:\Users\menno\CodingConnected\Repos\TLCGen\TLCGen\TLCGen.csproj /property:Configuration=Release
call C:\Users\menno\CodingConnected\Various\CodeCert\sign_tlcGenExe.bat
cd C:\Users\menno\CodingConnected\Repos\TLCGen\TLCGen.Setup
"C:\Program Files (x86)\WiX Toolset v3.11\bin\candle.exe" -d"DevEnvDir=C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\\" -dSolutionDir=C:\Users\menno\CodingConnected\Repos\TLCGen\ -dSolutionExt=.sln -dSolutionFileName=TLCGen.sln -dSolutionName=TLCGen -dSolutionPath=C:\Users\menno\CodingConnected\Repos\TLCGen\TLCGen.sln -dConfiguration=Release -dOutDir=bin\Release\ -dPlatform=x86 -dProjectDir=C:\Users\menno\CodingConnected\Repos\TLCGen\TLCGen.Setup\ -dProjectExt=.wixproj -dProjectFileName=TLCGen.Setup.wixproj -dProjectName=TLCGen.Setup -dProjectPath=C:\Users\menno\CodingConnected\Repos\TLCGen\TLCGen.Setup\TLCGen.Setup.wixproj -dTargetDir=C:\Users\menno\CodingConnected\Repos\TLCGen\TLCGen.Setup\bin\Release\ -dTargetExt=.msi -dTargetFileName=TLCGen.Setup.msi -dTargetName=TLCGen.Setup -dTargetPath=C:\Users\menno\CodingConnected\Repos\TLCGen\TLCGen.Setup\bin\Release\TLCGen.Setup.msi -out obj\Release\ -arch x86 -ext "C:\Program Files (x86)\WiX Toolset v3.11\bin\\WixUIExtension.dll" Product.wxs
"C:\Program Files (x86)\WiX Toolset v3.11\bin\Light.exe" -out C:\Users\menno\CodingConnected\Repos\TLCGen\TLCGen.Setup\bin\Release\TLCGen.Setup.msi -pdbout C:\Users\menno\CodingConnected\Repos\TLCGen\TLCGen.Setup\bin\Release\TLCGen.Setup.wixpdb -cultures:null -ext "C:\Program Files (x86)\WiX Toolset v3.11\bin\\WixUIExtension.dll" -sval -contentsfile obj\Release\TLCGen.Setup.wixproj.BindContentsFileListnull.txt -outputsfile obj\Release\TLCGen.Setup.wixproj.BindOutputsFileListnull.txt -builtoutputsfile obj\Release\TLCGen.Setup.wixproj.BindBuiltOutputsFileListnull.txt -wixprojectfile C:\Users\menno\CodingConnected\Repos\TLCGen\TLCGen.Setup\TLCGen.Setup.wixproj obj\Release\Product.wixobj
call C:\Users\menno\CodingConnected\Various\CodeCert\sign_tlcGen.bat
copy C:\Users\menno\CodingConnected\Repos\TLCGen\TLCGen.Setup\bin\Release\TLCGen.Setup.msi C:\Users\menno\CodingConnected\Repos\TLCGen\published\TLCGen.Setup.msi
call C:\Users\menno\CodingConnected\Repos\TLCGen\pack_release.bat
cd C:\Users\menno\CodingConnected\Repos\TLCGen\
