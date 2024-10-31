#! /usr/bin/bash

set -e
set -o pipefail

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
    echo "Usage: $0 -r <resource_group_name> -n <namespace_name> -t -q -h"
    echo "  -r: The name of the resource group"
    echo "  -n: The name of the namespace"
    echo "  -t: Remove all topics"
    echo "  -q: Remove all queues"
    echo "  -h: Show this help message"
    echo ""
    echo "  Example: ./azure_service_bus.sh -r rg-ais-sbus -n vsaisdev -t -q"
    echo "          Remove all topics and queues"
    exit 1;
}

removeAllTopics(){
    az servicebus topic list --resource-group $1 --namespace-name $2 --query [].name -o tsv | xargs -I {} az servicebus topic delete --name {} --resource-group $1 --namespace-name $2
}
removeAllQueues(){
    az servicebus queue list --resource-group $1 --namespace-name $2 --query [].name -o tsv | xargs -I {} az servicebus queue delete --name {} --resource-group $1 --namespace-name $2
}

main(){

    local resource_group
    local namespace_name

    while getopts ":r:n:thq" opt; do
        case ${opt} in
            r)
                resource_group=$OPTARG
                echo "Resource group: ${resource_group}"
            ;;
            n)
                namespace_name=$OPTARG
                echo "Namespace name: ${namespace_name}"
            ;;
            t)
                removeAllTopics "$resource_group" "$namespace_name"
                echo "Removed all topics"
            ;;
            q)
                removeAllQueues "$resource_group" "$namespace_name"
                echo "Removed all queues"
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

    if [[ -z "$resource_group" || -z "$namespace_name" ]]; then
        usage
    fi

}

main "$@"