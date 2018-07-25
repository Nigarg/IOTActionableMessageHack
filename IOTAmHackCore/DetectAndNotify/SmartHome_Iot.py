from gpiozero import MotionSensor
##from picamera import PiCamera

import base64
import time
import datetime
import requests
from requests.exceptions import ConnectionError

import sys
import getopt
from smtplib import SMTP as SMTP
from email.mime.text import MIMEText
    
# Create object for PIR Sensor
# PIR Sensor is on GPIO-4 (Pin 7)
pir = MotionSensor(17)
SMTP_SERVER = "smtp-mail.outlook.com"
SMTP_PORT = 25
# Create Object for Camera
##camera = PiCamera()

# Function to create new Filename from date and time
def getFileName():
	return datetime.datetime.now().strftime("SmartDoor_%Y-%m-%d_%H.%M.%S.jpg")

# Function to send http post request to azure service that triggers AM
def triggerAM():
##    try:
##        url = "http://iotamhackcore.azurewebsites.net/api/visitor"
##        #files = {'image': open('Documents/test.jpg','rb')}
##        with open("Documents/test.jpg", "rb") as imageFile:
##          encoded = base64.b64encode(imageFile.read())
##        headers = {
##            'Content-Type': "application/json"
##            }
##        payload = "{\"key\": \"nigarg\", \"name\": \"Nishant\", \"image\": \"" + str(encoded) + "\"}"
##        print(payload)
##        response = requests.post(url, data=payload, headers=headers)
##        print(response)
##    except ConnectionError as e:
##        print(e)
    """The entry point for the script"""
    sender = "admin@inlineconfig.onmicrosoft.com"
    password = "J$p1ter@1234"
    recipient = ""
    payload_file = ""

##    try:
##        opts, _args = getopt.getopt(argv, 'u:p:r:', ['user=', 'password=', 'recipient'])
##    except getopt.GetoptError:
##        print('send.py -u <username> -p <password> [-r <recipient>]  [-f <paylod file name>]')
##        sys.exit(2)

##    for opt, arg in opts:
##        if opt == '-u':
##            sender = arg
##        elif opt == '-p':
##            password = arg
##        elif opt == '-r':
##            recipient = arg
##        elif opt == '-f':
##            payload_file = arg

    if (not sender) or (not password):
        print('send.py -u <username> -p <password> [-r <recipient>]  [-f <paylod file name>]')
        sys.exit(2)

    print('Sending mail from', sender)
    send_message(sender, password, recipient, payload_file)

def send_message(sender, password, recipient, payload_file):
    """Sends a message from sender to self
    Keyword arguments:
    sender -- The email address of the user that will send the message
    password -- The password for the user
    recipient -- (Optional)The recipient email address. Default to sender
    """

    if (not recipient):
        recipient = sender

    if (not payload_file):
        payload_file = "mail.html"
        
    html_content = ""
    with open(payload_file, 'r') as myfile:
        html_content = myfile.read()
    
    with open("/home/pi/Documents/test.jpg", "rb") as imageFile:
        encoded = base64.b64encode(imageFile.read()).decode("utf-8")

    image = "data:image/jpeg;base64," + str(encoded)
    html_content = html_content.replace("<<IMAGEPLACEHOLDER>>", image)
    msg = MIMEText(html_content, 'html')
    msg['Subject'] = 'There is a new visitor at the door'
    msg['From'] = sender

    conn = SMTP(SMTP_SERVER, SMTP_PORT)
    try:
        conn.starttls()
        conn.set_debuglevel(False)
        conn.login(sender, password)
        conn.sendmail(sender, recipient, msg.as_string())
    finally:
        conn.quit()

    print('Sent the mail')

def main():
    
    
    while True:
    # Get a Filename
        filename = getFileName()
    # Wait for a motion to be detected
        pir.wait_for_motion
    # Print text to Shell
        print("You have a visitor")
    # Preview camera on screen until picture is taken
    ## camera.start_preview()
    # Take a picture of intruder
    ## camera.capture(filename)
    ## camera.stop_preview()
    
    ## Send a post request to azure api to trigger AM
        triggerAM()
         
    # Wait 10 seconds before repeating
        time.sleep(60)

if __name__ == '__main__':
    main()

    
   