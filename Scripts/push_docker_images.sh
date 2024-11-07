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
    echo "  -s: The name of the service container to start"
    echo "      Supported values: StudentService, RegisterStudent, SagaService, EmailService"
    echo "      The 'ALL' option will start all containers"
    echo "      If no container is specified, the script will start all containers"
    echo "  -r  Name of the Azure container registry to push to"
    echo ""
    echo "  -h: Show this help message"
    echo "  Example: ./push_docker_images.sh -s StudentService -r aisapis"
    exit 1;
}


# Uncomment the following line if you want to pass arguments to your script
if [[ $# -eq 0 ]]; then 
    echo "No options were passed"; 
    usage; 
fi

service_container=""
azure_container_registry=""

while getopts ":s:r:h" opt; do
    case ${opt} in
        s)
            service_container=$OPTARG
            echo "Service name: ${service_container}"
        ;;
        r)
            azure_container_registry=$OPTARG
            echo "Azure container registry: ${azure_container_registry}"
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

main(){
    az acr login --name aisapis

    docker tag "${service_container,,}":latest "${azure_container_registry}.azurecr.io/apis/${service_container,,}"
    docker push "${azure_container_registry}.azurecr.io/apis/${service_container,,}"        
}

main "$@"    