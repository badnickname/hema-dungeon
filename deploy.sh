#!/bin/bash
https=$1
username=$2
password=$3
stack=$4
endpoint=$5

dotnet publish -c Release -r linux-arm64 -o ./publish
docker build --platform linux/arm64 . -t ghcr.io/badnickname/hema-dungeon/hema-dungeon:1.0
docker push ghcr.io/badnickname/hema-dungeon/hema-dungeon:1.0
rm -rf publish

JWT=$(curl -s -X POST ${https}/api/auth -H "Content-Type: application/json" -d "{\"username\": \"${username}\", \"password\": \"${password}\"}" | jq '.jwt' -r)

CONTENT=$(curl -s -X GET ${https}/api/stacks/${stack}/file -H "Content-Type: application/json" -H "Authorization: Bearer ${JWT}"  | jq '.StackFileContent' -r -a | tr -d '"')
echo "Content: "
echo "$CONTENT"

RESULT=$(curl -s -X PUT ${https}/api/stacks/${stack}?endpointId=${endpoint} -H "Content-Type: application/json" -H "Authorization: Bearer ${JWT}" -d "{\"id\":${stack},\"StackFileContent\":\"${CONTENT}\",\"Env\":[],\"Prune\":false,\"PullImage\":true}" | jq)
echo "Result: "
echo "$RESULT"
