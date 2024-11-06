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
    echo "      Supported values: StudentService, RegisterStudent, SagaService, EmailService, ALL"
    echo "      The 'ALL' option will start all containers"
    echo "      If no container is specified, the script will start all containers"
    echo "  -b  BrokerType: The type of message broker to use"
    echo "      Supported values: ASB, RabbitMQ"
    echo "  -h: Show this help message"
    exit 1;
}


# Uncomment the following line if you want to pass arguments to your script
if [[ $# -eq 0 ]]; then 
    echo "No options were passed"; 
    usage; 
fi

service_container=""
broker_type=""

while getopts ":s:b:h" opt; do
    case ${opt} in
        s)
            service_container=$OPTARG
            echo "Service name: ${service_container}"
        ;;
        b)
            broker_type=$OPTARG
            echo "Broker type: ${broker_type}"
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

# Starts a container for the given service, builds the container using the build_containers.sh
# script, starts the container using docker-compose and finally updates the database using
# the update_db.sh script.
#
# Args:
#     $1: The name of the service to start
start_ef_container(){
    bash "${SCRIPT_DIR}"/build_containers.sh -s "$1"
    echo "Starting container for ${1,,}."
    cd "${SCRIPT_DIR}/../CICD"	
    docker-compose -f docker-compose-"${broker_type,,}".yml up -d "${1,,}"
    cd -
    bash "${SCRIPT_DIR}"/update_db.sh -s "$1"
}


# Starts a container for the specified service.
#
# This function builds and starts a Docker container for the given service.
# It uses the build_containers.sh script to build the container, and then
# uses docker-compose to start the container in detached mode.
#
# Args:
#     $1: The name of the service for which to start the container.
#
# Globals:
#     SCRIPT_DIR - The directory where the script is located, used to
#                  determine the path for building and starting containers.
#
# Returns:
#     None
#
start_container(){
    bash "${SCRIPT_DIR}"/build_containers.sh -s "$1"
    echo "Starting container for ${1,,}."
    cd "${SCRIPT_DIR}/../CICD"	
    docker-compose -f docker-compose-"${broker_type,,}".yml up -d "${1,,}"
    cd -
}

# Main function to start the specified service container.
#
# This function is used to start a service container. It evaluates the
# provided service container name and calls the start_ef_container or
# start_container function for the corresponding service.
#
# Args:
#   $1: The name of the service container to start
#
# Returns:
#   None

main(){
    case "${service_container}" in
        "StudentService"|"RegisterStudent"|"SagaService")            
            start_ef_container "${service_container}"            
            ;;
        "EmailService")
            start_container "EmailService"
            ;;
        "ALL")
            start_ef_container "SagaService"
            start_ef_container StudentService
            start_ef_container "RegisterStudent"
            start_container "EmailService"
            ;;
        \?)
            echo "Invalid service container name: $service_container"
            usage
            ;;
        *)
            usage
            ;;
    esac
}

main "$@"    