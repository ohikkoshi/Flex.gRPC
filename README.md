```
dotnet new console
```
```
dotnet add package Grpc.Core
dotnet add package Grpc.Tools
dotnet add package Google.Protobuf
```
```
mkdir Protos
```
```
  <ItemGroup>
    <Protobuf Include="Protos\*.proto" GrpcServices="Both" />
  </ItemGroup>
```
```
dotnet run
```
