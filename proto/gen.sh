protoc -I=. --plugin=protoc-gen-grpc-csharp=/Users/lzq/tools/protoc/bin/grpc_csharp_plugin --csharp_out=./ --grpc-csharp_out=./ runtime.proto
protoc -I=. --plugin=protoc-gen-grpc-csharp=/Users/lzq/tools/protoc/bin/grpc_csharp_plugin --csharp_out=./ --grpc-csharp_out=./ appcallback.proto

rm -f ../src/Layotto/Protocol/*.cs
mv ./*.cs ../src/Layotto/Protocol/