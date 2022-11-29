import json
import boto3


def scan_table(dynamo_client, *, TableName, **kwargs):
    """
    Generates all the items in a DynamoDB table.

    :param dynamo_client: A boto3 client for DynamoDB.
    :param TableName: The name of the table to scan.

    Other keyword arguments will be passed directly to the Scan operation.
    See https://boto3.amazonaws.com/v1/documentation/api/latest/reference/services/dynamodb.html#DynamoDB.Client.scan

    """
    paginator = dynamo_client.get_paginator("scan")

    for page in paginator.paginate(TableName=TableName, **kwargs):
        yield from page["Items"]
        
        
def lambda_handler(event, context):
    dynamo_client = boto3.client("dynamodb", region_name='us-west-2' )
    data = []
    for item in scan_table(dynamo_client, TableName="pupvr"):
        pl = {}
        pl['playerName'] = item.get("playerName", {"S": "-"})["S"]
        pl['componentName'] = item.get("componentName", {"S": "-"})["S"]
        pl['created_on'] = item.get("created_on", {"S": "-"})["S"]
        pl['dropDownText'] = item.get("dropDownText", {"S": "-"})["S"]
        pl['commentText'] = item.get("commentText", {"S": "-"})["S"]
        data.append(pl)
    return {
        'statusCode': 200,
        'body': json.dumps({"data": data}),
        'headers': {
            'Access-Control-Allow-Origin': '*'
      }
    }
