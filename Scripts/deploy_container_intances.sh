#!/usr/bin/bash

set -e
set -o pipefail
# set -x    

SCRIPT_DIR=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )

# Prints an error message and resets the return code to 1
# This is used to handle unexpected errors in the script
# Args:
#     $1: The line number where the error occurred  
on_error() {
    # Enable the non-strict error mode, so we can print the error message
    set +e
    # Print error message
    echo "There was an error, execution halted" >&2
    echo "Error at line $1" >&2
    # Reset the return code to 1
    return 1
}

usage() {
    echo "Usage: $0 -s <service_container>"
    echo "  -r  Name of the Azure container registry to push to"
    echo "  -g  Name of the Azure resource group"
    echo "  -h: Show this help message"
    echo "  Example: ./deploy_container_intances.sh -s StudentService -r aisapis -g rg-ais-apis-student"
    exit 1;
}


# Uncomment the following line if you want to pass arguments to your script
if [[ $# -eq 0 ]]; then 
    echo "No options were passed"; 
    usage; 
fi

service_container=""
azure_container_registry=""
azure_resource_group=""

while getopts ":r:g:h" opt; do
    case ${opt} in
        r)
            azure_container_registry=$OPTARG
            echo "Azure container registry: ${azure_container_registry}"
        ;;
        g)
            azure_resource_group=$OPTARG
            echo "Azure resource group: ${azure_resource_group}"
        ;;

        h)
            usage
            ;;
        \?)
            echo "Invalid option: -$OPTARG" >&2
            usage
            ;;
        *)
            usage
            ;;
    esac
done

azureContainerInstanceGroupExists() {
    local result=""
    result=$(az deployment group show --resource-group "${azure_resource_group}" --name "${SCRIPT_DIR}/CICD/ci-ais-$1-group" &> /dev/null)
    if [[ $result ]]; then
        echo 1
    else
        echo 0
    fi
}

main(){
    az acr login --name aisapis
    
    local service_containers=("StudentService" "RegisterStudent" "SagaService" "EmailService")
    for service_container in "${service_containers[@]}"; do
        echo "Building container for ${service_container,,}"
        bash "${SCRIPT_DIR}"/build_containers.sh -s "${service_container,,}"

        echo "Pushing container for ${service_container,,}"
        docker tag "${service_container,,}":latest "${azure_container_registry}.azurecr.io/apis/${service_container,,}"
        docker push "${azure_container_registry}.azurecr.io/apis/${service_container,,}"    
    done  
    
    if [[ $(azureContainerInstanceGroupExists "sagaservices") -eq 1 ]]; then
        echo "Remove container instance group: ci-ais-sagaservices-group"
        az deployment group delete --resource-group "${azure_resource_group}" --name "${SCRIPT_DIR}/../CICD/ci-ais-sagaservices-group"
    fi

    if [[ $(azureContainerInstanceGroupExists "sagaservices") -eq 0 ]]; then    
        echo "Deploying container instance group: ci-ais-sagaservices-group"
        az deployment group create --resource-group "${azure_resource_group}" --template-file "${SCRIPT_DIR}/../CICD/azuredeploy_sagaservices.json"
    fi

    if [[ $(azureContainerInstanceGroupExists "studentservices") -eq 1 ]]; then
        echo "Remove container instance group: ci-ais-studentservices-group"
        az deployment group delete --resource-group "${azure_resource_group}" --name "${SCRIPT_DIR}/../CICD/ci-ais-studentservices-group"
    fi

    if [[ $(azureContainerInstanceGroupExists "studentservices") -eq 0 ]]; then
        echo "Deploying container instance group: ci-ais-studentservices-group"
        az deployment group create --resource-group "${azure_resource_group}" --template-file "${SCRIPT_DIR}/../CICD/azuredeploy_studentservices.json"
    fi
}

main "$@"    