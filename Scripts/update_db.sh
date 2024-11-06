#!/usr/bin/env bash

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

# ef_database_update <service_name>
#
# Args:
#     $1: The name of the service whose database should be updated
#
# Description:
#     This function is used to update the database for the given service.
#     It changes to the directory of the service, runs the ef database update
#     command and then switches back.
ef_database_update(){
    echo "Updating database for the $1."
    cd "${SCRIPT_DIR}/../$1"
    dotnet ef database update
    cd -
}

# Main function to update the database for a specified service.
#
# This function evaluates the provided service name and calls the
# ef_database_update function for the corresponding service.
# If the service name does not match any of the predefined options,
# it prints an error message and shows the usage instructions.
#
# Globals:
#   service_name - The name of the service to update.
#
# Arguments:
#   None
#
# Returns:
#   None
#
main(){
    case $service_name in
        "StudentService")
            ef_database_update "StudentService"
            ;;
        "RegisterStudent")
            ef_database_update "RegisterStudent"
            ;;
        "SagaService")
            ef_database_update "SagaService"
            ;;
        *)
            echo "Invalid service name: $service_name"
            usage
            ;;
    esac
}

main "$@"