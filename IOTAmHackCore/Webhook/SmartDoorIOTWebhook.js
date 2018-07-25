
// call the packages we need
var express = require("express");
var app = express();
 
var port = process.env.PORT || 3333; // set our port
 
var Gpio = require('onoff').Gpio; //include onoff to interact with the GPIO
var LED = new Gpio(4, 'out'); //use GPIO pin 4, and specify that it is output

var blinkInterval=0;

// Functions for LED
// =============================================================================
function blinkLED() { //function to start blinking
  if (LED.readSync() === 0) { //check the pin state, if the state is 0 (or off)
    LED.writeSync(1); //set pin state to 1 (turn LED on)
  } else {
    LED.writeSync(0); //set pin state to 0 (turn LED off)
  }
}

function endBlink() { //function to stop blinking
  clearInterval(blinkInterval); // Stop blink intervals
  LED.writeSync(0); // Turn LED off
  LED.unexport(); // Unexport GPIO to free resources
}

// ROUTES FOR OUR API
// =============================================================================
 
// create our router
var router = express.Router();
 
// middleware to use for all requests
router.use(function(req, res, next) {
    console.log("Request start.");
    next();
});
 
// test route to make sure everything is working (accessed at GET http://localhost:3333/api)
router.get("/", function(req, res) {
    res.json({ message: "SmartHome IOT" });   
});
 
// on routes that end in /approved
// ----------------------------------------------------
router.route("/approved")
 
    // (accessed at POST http://localhost:3333/api/approved)
    .post(function(req, res) {
       
                           res.status(200).send("approved");
                           // blink green led
			   blinkInterval = setInterval(blinkLED, 250); //run the blinkLED function every 250ms
			   //blinkLED();
			   setTimeout(endBlink, 5000); //stop blinking after 5 seconds
       
    });
   
// on routes that end in /rejected
// ----------------------------------------------------
router.route("/rejected")
 
    // (accessed at POST http://localhost:3333/api/rejected)
    .post(function(req, res) {
       
                           res.status(200).send("rejected");
                           // blink red led
       
    });   
 
// REGISTER OUR ROUTES -------------------------------
app.use("/api", router);
 
// START THE SERVER
// =============================================================================
app.listen(port);
console.log("SmartHome Webhook IOT on port " + port);