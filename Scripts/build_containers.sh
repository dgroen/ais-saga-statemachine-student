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


trap 'on_error $LINENO' ERR

usage() {
    echo "Usage: $0 -s <service_name>"
    echo "  -s: The name of the service whose database should be updated"
    echo "      Supported values: StudentService, RegisterStudent, SagaService"
    echo "  -h: Show this help message"
    exit 1;
}

# Uncomment the following line if you want to pass arguments to your script
if [[ $# -eq 0 ]]; then 
    echo "No options were passed"; 
    usage; 
fi

service_name=""

while getopts ":s:h" opt; do
    case ${opt} in
        s)
            service_name=$OPTARG
            echo "Service name: ${service_name}"
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

# Builds a Docker container for the specified service.
#
# This function navigates to the service directory and executes the .NET publish command
# to build a Docker container for the service. It targets the Linux operating system and
# x64 architecture, and uses the PublishContainer target.
#
# Args:
#     $1: The name of the service for which to build the container.
#
# Globals:
#     SCRIPT_DIR - The directory where the script is located, used to
#                  determine the path for building the container.
#
# Returns:
#     None
build_container(){
    echo "Building containers"
    cd "${SCRIPT_DIR}/../$1"
    dotnet publish --os linux --arch x64 /t:PublishContainer
    cd -
}

# Main function to build containers for specified services.
#
# This function evaluates the provided service name and calls the
# build_container function for the corresponding service.
# If the service name does not match any of the predefined options,
# it prints an error message and shows the usage instructions.
#
# Globals:
#   service_name - The name of the service for which to build the container.
#
# Arguments:
#   None
#
# Returns:
#   None
main(){
    case $service_name in
        "StudentService")
            build_container "StudentService"
            ;;
        "RegisterStudent")
            build_container "RegisterStudent"
            ;;
        "SagaService")
            build_container "SagaService"
            ;;
        "EmailService")
            build_container "EmailService"
            ;;            
        *)
            echo "Invalid container name: $service_name"
            usage
            ;;
    esac
}



main "$@"