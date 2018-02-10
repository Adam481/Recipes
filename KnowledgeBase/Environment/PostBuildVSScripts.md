# Scripts for commping dll after visual studio build action

```ps
SET SartUp="Solution.SomeProject"
echo "Copping files from: "
echo "$(TargetDir)"
echo "to: "
echo "$(SolutionDir)src\$(SartUp)\bin\$(Configuration)\netcoreapp2.0\"
echo "Files: "
copy "$(TargetDir)*.*" "$(SolutionDir)src\$(SartUp)\bin\$(Configuration)\netcoreapp2.0\"
```
