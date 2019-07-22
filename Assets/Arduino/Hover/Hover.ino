#include <SerialCommand.h>
#include <Button.h>
#include <avdweb_Switch.h>

//SERIAL COMMUNICATION
SerialCommand serialCom;

Switch pushButtonLeft = Switch(2);
Switch keySwitchLeft = Switch(3);
Switch keySwitchRight =  Switch(4);
Switch pushButtonRight = Switch(5);

long potReadInterval = 100;
long previousMillis = 0;
int potPins[] = {A0, A1, A2, A3};
int potVal[] = {0, 0, 0, 0};

void setup() {
  Serial.begin(57600);

  while (!Serial);
  serialCom.addCommand("state", getSwitchesState);
}

void loop() {

  pollSwitches();

  unsigned long currentMillis = millis();
  if (currentMillis - previousMillis > potReadInterval) {
    previousMillis = currentMillis;
    updatePotentiometers();
  }

  serialCom.readSerial();
}

void getSwitchesState(){
    keySwitchLeft.poll();
    Serial.print("K");
    Serial.print("L");
    Serial.println(keySwitchLeft.on());

    pushButtonRight.poll();
    Serial.print("P");
    Serial.print("R");
    Serial.println(pushButtonRight.on());

    keySwitchRight.poll();
    Serial.print("K");
    Serial.print("R");
    Serial.println(keySwitchRight.on());

    pushButtonLeft.poll();
    Serial.print("P");
    Serial.print("L");
    Serial.println(pushButtonLeft.on());
}

void pollSwitches() {
  if (keySwitchLeft.poll()) {
    Serial.print("K");
    Serial.print("L");
    Serial.println(keySwitchLeft.on());
  }
  if (pushButtonRight.poll()) {
    Serial.print("P");
    Serial.print("R");
    Serial.println(pushButtonRight.on());
  }
  if (keySwitchRight.poll()) {
    Serial.print("K");
    Serial.print("R");
    Serial.println(keySwitchRight.on());
  }
  if (pushButtonLeft.poll()) {
    Serial.print("P");
    Serial.print("L");
    Serial.println(pushButtonLeft.on());
  }
}

void updatePotentiometers() {
  for (int i = 0; i < 4; i++) {
    potVal[i] = analogRead(potPins[i]);
  }

  Serial.print(potVal[0]);
  Serial.print(",");
  Serial.print(potVal[1]);
  Serial.print(",");
  Serial.print(potVal[2]);
  Serial.print(",");
  Serial.println(potVal[3]);
}

