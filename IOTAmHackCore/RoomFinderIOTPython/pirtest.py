import RPi.GPIO as GPIO
import time
import requests
import json

GPIO.setwarnings(False)
GPIO.setmode(GPIO.BOARD)
GPIO.setup(11, GPIO.IN)         #Read output from PIR motion sensor
#GPIO.setup(3, GPIO.OUT)         #LED output pin

fw = open("pirout.txt", "w")
url = 'http://iotamhackcore.azurewebsites.net/api/room'
while True:
    i = GPIO.input(11)
    isActivityPresent = True if i == 1 else False
    payload = {}
    payload["key"] = "8464"
    payload["name"] = "Focus Room"
    payload["activity"] = isActivityPresent
    payload["timeStamp"] = int(time.time())
    print(payload)
    headers = {
        'content-Type' : 'application/json'
        }
    response = requests.post(url, data = json.dumps(payload), headers=headers)#payload)
    print(response.status_code)
    if i == 0:                 #When output from motion sensor is LOW
        print("No intruders")
        time.sleep(4)
    elif i == 1:               #When output from motion sensor is HIGH
        print("Intruder detected")
        time.sleep(4)

fw.close()