#include <avdweb_Switch.h>
#include <LiquidCrystal_I2C.h>
#include <Button.h>
#include <Keypad.h>
#include <Wire.h>
#include <SoftwareSerial.h>
#include <SerialCommand.h>

//SERIAL COMMUNICATION
SerialCommand serialCom;

//KEYPAD
const byte ROWS = 4;
const byte COLS = 4;
char keys[ROWS][COLS] = {
  {'1', '2', '3', 'A'},
  {'4', '5', '6', 'B'},
  {'7', '8', '9', 'C'},
  {'*', '0', '#', 'D'}
};
byte rowPins[ROWS] = {5, 4, 3, 2};
byte colPins[COLS] = {9, 8, 7, 6};
Keypad keypad = Keypad( makeKeymap(keys), rowPins, colPins, ROWS, COLS );

//SELECTORs
int selectorPins[] = {A0, A1};
int numOfSteps[] = {8, 11};
int dividers[3];
int lastSelected[] = {0, 0};
int selected[] = {0, 0};
char codeLetter[] = {'s', 'r'};
unsigned long lastDebounceTime = 0; //last time the pin was toggled, used to keep track of time
unsigned long debounceDelay = 100;   //the debounce time which user sets prior to run

//LCDs
LiquidCrystal_I2C lcd0(0x3B, 16, 2);
LiquidCrystal_I2C lcd1(0x3D, 16, 2);
LiquidCrystal_I2C lcd2(0x3E, 16, 2);
LiquidCrystal_I2C lcd3(0x3F, 16, 2);

//BUTTONs
//Button yellow = Button(10, BUTTON_PULLUP_INTERNAL, true, 50);
//Button black = Button(11, BUTTON_PULLUP_INTERNAL, true, 50);
Switch switch0 =  Switch(10);
Switch switch1 = Switch(11);
Switch switch2 = Switch(12);
Switch switch3 = Switch(13);
Switch switch4 = Switch(A2);
Switch switch5 = Switch(A3);
Switch switches[] = {switch0, switch1, switch2, switch3, switch4, switch5};
char codeLetterSwitch[] = {'d', 'c', 'b', 'a', 'e', 'l'};
unsigned long lastDebounceTimeSwitch = 0;
int switchStatus[] = {0, 0, 0, 0, 0, 0};
int lastStatus[] = {0, 0, 0, 0, 0, 0};

void setup() {
  //  yellow.pressHandler(onPressYellow);
  //  black.pressHandler(onPressBlack);

  for (int i = 0; i < sizeof(selectorPins) / sizeof(selectorPins[0]); i++) {
    pinMode(selectorPins[i], INPUT_PULLUP);
    dividers[i] = 1024.0 / numOfSteps[i];
  }

  Serial.begin(57600);
  while (!Serial);

  serialCom.addCommand("WCC", WriteCodeChar);
  serialCom.addCommand("CR", ClearRow);
  serialCom.addCommand("TA", UpdateTotalAmount);
  serialCom.addCommand("CA", UpdateCurrentAmount);
  serialCom.addCommand("OFF", BeamOff);
  serialCom.addCommand("DEP", Depleated);
  serialCom.addCommand("SB", StartBlink);
  serialCom.addCommand("RA", ReadAll);
  serialCom.addCommand("R", Ready);
  serialCom.addCommand("NBA", NoBlinkAll);

  lcd0.begin();
  lcd1.begin();
  lcd2.begin();
  lcd3.begin();

  Ready(0);
  Ready(1);
  Ready(2);
  Ready(3);

  ReadAll();
}

void BeamOff() {
  char *arg;

  arg = serialCom.next();
  int lcdIndex = atoi(arg);

  Ready(lcdIndex);
}

void Ready() {
  Ready(0);
  Ready(1);
  Ready(2);
  Ready(3);
}

void ReadAll() {
  for (int i = 0; i < sizeof(selectorPins) / sizeof(selectorPins[0]); i++) {
    float readingFloat = analogRead(selectorPins[i]) / dividers[i];
    int reading = readingFloat;

    Serial.print(codeLetter[i]);
    Serial.println(selected[i]);

    lastSelected[i] = reading;
  }

  for (int i = 0; i < sizeof(switches) / sizeof(switches[0]); i++) {
    switches[i].poll();
    int reading = switches[i].on();

    Serial.print(codeLetterSwitch[i]);
    Serial.println(switchStatus[i]);
  }
}

void NoBlinkAll() {
  lcd0.noBlink();
  lcd1.noBlink();
  lcd2.noBlink();
  lcd3.noBlink();
}

void StartBlink() {
  char *arg;

  arg = serialCom.next();
  int lcdIndex = atoi(arg);

  switch (lcdIndex) {
    case 0:
      lcd0.blink();
      lcd1.noBlink();
      lcd2.noBlink();
      lcd3.noBlink();
      break;
    case 1:
      lcd0.noBlink();
      lcd1.blink();
      lcd2.noBlink();
      lcd3.noBlink();
      break;
    case 2:
      lcd0.noBlink();
      lcd1.noBlink();
      lcd2.blink();
      lcd3.noBlink();
      break;
    case 3:
      lcd0.noBlink();
      lcd1.noBlink();
      lcd2.noBlink();
      lcd3.blink();
      break;
  }
}

void Ready(int lcdIndex) {
  switch (lcdIndex) {
    case 0:
      lcd0.clear();
      lcd0.setCursor(0, 1);
      lcd0.print("ALPHA Ready");
      lcd0.setCursor(0, 0);
      break;
    case 1:
      lcd1.clear();
      lcd1.setCursor(0, 1);
      lcd1.print("BRAVO Ready");
      lcd1.setCursor(0, 0);
      break;
    case 2:
      lcd2.clear();
      lcd2.setCursor(0, 1);
      lcd2.print("CHARLIE Ready");
      lcd2.setCursor(0, 0);
      break;
    case 3:
      lcd3.clear();
      lcd3.setCursor(0, 1);
      lcd3.print("DELTA Ready");
      lcd3.setCursor(0, 0);
      break;
  }
}

void Depleated() {
  char *arg;

  arg = serialCom.next();
  int lcdIndex = atoi(arg);

  switch (lcdIndex) {
    case 0:
      lcd0.clear();
      lcd0.setCursor(0, 0);
      lcd0.print("SYNCED SOURCE IS");
      lcd0.setCursor(0, 1);
      lcd0.print("DEPLEATED!");
      break;
    case 1:
      lcd1.clear();
      lcd1.setCursor(0, 0);
      lcd1.print("SYNCED SOURCE IS");
      lcd1.setCursor(0, 1);
      lcd1.print("DEPLEATED!");
      break;
    case 2:
      lcd2.clear();
      lcd2.setCursor(0, 0);
      lcd2.print("SYNCED SOURCE IS");
      lcd2.setCursor(0, 1);
      lcd2.print("DEPLEATED!");
      break;
    case 3:
      lcd3.clear();
      lcd3.setCursor(0, 0);
      lcd3.print("SYNCED SOURCE IS");
      lcd3.setCursor(0, 1);
      lcd3.print("DEPLEATED!");
      break;
  }
}

//void onPressYellow(Button & b) {
//  Serial.print("b");
//  Serial.println("0");
//}
//void onPressBlack(Button & b) {
//  Serial.print("b");
//  Serial.println("1");
//}

void UpdateTotalAmount() {
  char *arg;

  arg = serialCom.next();
  int lcdIndex = atoi(arg);

  arg = serialCom.next();
  int amountLength = strlen(arg);
  int totalAmount = atoi(arg);

  ClearBottomRow(lcdIndex);
  WriteTotalAmount(lcdIndex, amountLength, totalAmount);
}

void UpdateCurrentAmount() {
  char *arg;

  arg = serialCom.next();
  int lcdIndex = atoi(arg);

  arg = serialCom.next();
  int currentAmount = atoi(arg);

  WriteCurrentAmount(lcdIndex, currentAmount);
}

void WriteCurrentAmount(int lcdIndex, int currentAmount) {
  switch (lcdIndex) {
    case 0:
      lcd0.setCursor(0, 1);
      lcd0.print("     ");
      lcd0.print(currentAmount);
      break;
    case 1:
      lcd1.setCursor(0, 1);
      lcd1.print("     ");
      lcd1.print(currentAmount);
      break;
    case 2:
      lcd2.setCursor(0, 1);
      lcd2.print("     ");
      lcd2.print(currentAmount);
      break;
    case 3:
      lcd3.setCursor(0, 0);
      lcd3.print("     ");
      lcd3.print(currentAmount);
      break;
  }
}

void WriteTotalAmount(int lcdIndex, int amountLength, int totalAmount) {
  switch (lcdIndex) {
    case 0:
      lcd0.setCursor(7, 1);
      lcd0.print("/");
      lcd0.setCursor(16 - amountLength, 1);
      lcd0.print(totalAmount);
      lcd0.setCursor(0, 1);
      lcd0.noBlink();
      break;
    case 1:
      lcd1.setCursor(7, 1);
      lcd1.print("/");
      lcd1.setCursor(16 - amountLength, 1);
      lcd1.print(totalAmount);
      lcd1.setCursor(0, 1);
      lcd1.noBlink();
      break;
    case 2:
      lcd2.setCursor(7, 1);
      lcd2.print("/");
      lcd2.setCursor(16 - amountLength, 1);
      lcd2.print(totalAmount);
      lcd2.setCursor(0, 1);
      lcd2.noBlink();
      break;
    case 3:
      lcd3.setCursor(7, 1);
      lcd3.print("/");
      lcd3.setCursor(16 - amountLength, 1);
      lcd3.print(totalAmount);
      lcd3.setCursor(0, 1);
      lcd3.noBlink();
      break;
  }
}
void ClearBottomRow(int lcdIndex) {
  switch (lcdIndex) {
    case 0:
      lcd0.setCursor(0, 1);
      lcd0.print("                ");
      lcd0.setCursor(0, 1);
      break;
    case 1:
      lcd1.setCursor(0, 1);
      lcd1.print("                ");
      lcd1.setCursor(0, 1);
      break;
    case 2:
      lcd2.setCursor(0, 1);
      lcd2.print("                ");
      lcd2.setCursor(0, 1);
      break;
    case 3:
      lcd3.setCursor(0, 1);
      lcd3.print("               ");
      lcd3.setCursor(0, 1);
      break;
  }
}

void ClearRow() {
  char *arg;

  arg = serialCom.next();
  int lcdIndex = atoi (arg);

  arg = serialCom.next();
  int rowIndex = atoi (arg);

  switch (lcdIndex) {
    case 0:
      lcd0.setCursor(0, rowIndex);
      lcd0.print("                ");
      lcd0.setCursor(0, rowIndex);
      break;
    case 1:
      lcd1.setCursor(0, rowIndex);
      lcd1.print("                ");
      lcd1.setCursor(0, rowIndex);
      break;
    case 2:
      lcd2.setCursor(0, rowIndex);
      lcd2.print("                ");
      lcd2.setCursor(0, rowIndex);
      break;
    case 3:
      lcd3.setCursor(0, rowIndex);
      lcd3.print("               ");
      lcd3.setCursor(0, rowIndex);
      break;
  }
}

void WriteCodeChar() {
  char *arg;

  arg = serialCom.next();
  int lcdIndex = atoi (arg);

  arg = serialCom.next();

  switch (lcdIndex) {
    case 0:
      lcd0.print(arg);
      break;
    case 1:
      lcd1.print(arg);
      break;
    case 2:
      lcd2.print(arg);
      break;
    case 3:
      lcd3.print(arg);
      break;
  }
}

void loop() {
  CheckKeypad();
  for (int i = 0; i < sizeof(selectorPins) / sizeof(selectorPins[0]); i++) {
    CheckSelector(i);
  }

  for (int i = 0; i < sizeof(switches) / sizeof(switches[0]); i++) {
    CheckSwitch(i);
  }

  serialCom.readSerial();
}

void CheckSwitch(int i) {
  switches[i].poll();
  int reading = switches[i].on();

  if (reading != lastStatus[i]) {
    lastDebounceTimeSwitch = millis();
  }

  if ((millis() - lastDebounceTimeSwitch) > debounceDelay) {
    if (reading != switchStatus[i]) {
      switchStatus[i] = reading;
      Serial.print(codeLetterSwitch[i]);
      Serial.println(switchStatus[i]);
    }
  }

  lastStatus[i] = reading;
}

void CheckSelector(int i) {
  float readingFloat = analogRead(selectorPins[i]) / dividers[i];
  int reading = readingFloat;

  // DEBOUNCING
  if (reading != lastSelected[i]) {
    lastDebounceTime = millis();
  }

  if ((millis() - lastDebounceTime) > debounceDelay) {
    if (reading != selected[i]) {
      selected[i] = reading;
      Serial.print(codeLetter[i]);
      Serial.println(selected[i]);
    }
  }

  lastSelected[i] = reading;
}

void CheckKeypad() {
  char key = keypad.getKey();

  if (key) {
    Serial.print("k");
    Serial.println(key);
  }
}

void Write(int lcd, String key) {
  switch (lcd) {
    case 0:
      lcd0.print(key);
      break;
    case 1:
      lcd1.print(key);
      break;
    case 2:
      lcd2.print(key);
      break;
    case 3:
      lcd3.print(key);
      break;
  }
}

