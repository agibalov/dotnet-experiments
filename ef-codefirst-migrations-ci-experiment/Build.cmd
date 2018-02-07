msbuild Build.xml /target:CreateEmptyBuildDirectory
rem msbuild Build.xml /target:UpdateAppConfiguration
msbuild Build.xml /target:BuildApp
msbuild Build.xml /target:UpdateDatabase
