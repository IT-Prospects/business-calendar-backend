import time
import jwt
import sys

service_account_id = sys.argv[1]
key_id = sys.argv[2]
private_key = sys.argv[3]
        
now = int(time.time())
payload = {
        'aud': 'https://iam.api.cloud.yandex.net/iam/v1/tokens',
        'iss': service_account_id,
        'iat': now,
        'exp': now + 600}

encoded_token = jwt.encode(
    payload,
    private_key,
    algorithm='PS256',
    headers={'kid': key_id})

print(encoded_token)
