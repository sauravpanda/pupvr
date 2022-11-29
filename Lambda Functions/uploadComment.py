import json
import boto3
import uuid
import datetime


def lambda_handler(event, context):
    # TODO implement
    print("Input Event is: ", json.dumps(event))
    if "body" in event:
        event = json.loads(event["body"])
        
    dynamodb = boto3.resource('dynamodb')
    table = dynamodb.Table('pupvr')
    uq = str(uuid.uuid4())
    dt = datetime.datetime.now().strftime("%y-%m-%d %H:%M:%S")
    pl = {
        "uid": uq,
        "created_on": dt
    }
    
    pl.update(event)
    print(pl)
    
    table.put_item(Item=pl)
        
    return {
        'statusCode': 200,
        'body': json.dumps('Hello from Lambda!')
    }
