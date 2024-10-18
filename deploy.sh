#!/bin/bash
https=$1
username=$2
password=$3
stack=$4
endpoint=$5

JWT=$(curl -s -X POST ${https}/api/auth -H "Content-Type: application/json" -d "{\"username\": \"${username}\", \"password\": \"${password}\"}" | jq '.jwt' -r)

CONTENT=$(curl -s -X GET ${https}/api/stacks/${stack}/file -H "Content-Type: application/json" -H "Authorization: Bearer ${JWT}"  | jq '.StackFileContent' -r -a | tr -d '"')
echo "Content: "
echo "$CONTENT"

RESULT=$(curl -s -X PUT ${https}/api/stacks/${stack}?endpointId=${endpoint} -H "Content-Type: application/json" -H "Authorization: Bearer ${JWT}" -d "{\"id\":${stack},\"StackFileContent\":\"${CONTENT}\",\"Env\":[],\"Prune\":false,\"PullImage\":true}" | jq)
echo "Result: "
echo "$RESULT"