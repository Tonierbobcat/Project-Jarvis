import requests
import json

base_url = 'https://127.0.0.1:7074/'

class User:
    def __init__(self, username, password, guid):
        self.username = username
        self.password = password
        self.guid = guid

    def __str__(self):
        return f'username: {self.username}, password: {self.password}, guid: {self.guid}'


def create_user(username: str, password: str):
    url = base_url + 'create-user'

    data = {
        'id':username,
        'password':password
    }

    response = requests.post(url, json=data, verify=False)

    print(response.text)

    json_obj = json.loads(response.text)

    user_obj = User(json_obj["id"], json_obj["password"], json_obj["guid"])

    print(user_obj)

    print(response.status_code)




def get_user(username, password):

    url = base_url + 'get-user'

    data = {
        'id':username,
        'password':password
    }
    response = requests.post(url, json=data, verify=False)

    print(response.status_code)

    if response.status_code == 200:
        print("user exists")

create_user("pooper", "peeper")

