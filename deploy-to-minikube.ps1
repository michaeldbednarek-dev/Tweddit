#!/bin/bash

minikube start
& minikube -p minikube docker-env | Invoke-Expression

minikube status

cd ./kube

kubectl delete -f .
kubectl apply -f .

$frontPodName = $(kubectl get pods -l app=frontend -o jsonpath='{.items[0].metadata.name}')

Write-Output "Write the following in this order"
Write-Output "kubectl delete service frontend-service"
Write-Output "kubectl expose pod $frontPodName --type=NodePort --name=frontend-service"
Write-Output "minikube service frontend-service"

