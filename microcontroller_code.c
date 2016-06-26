#include <msp430.h> 
//	PWM:	2850 -> 0 degrees, 2250 -> 45 degrees 1650 -> 90 degrees
#define PWM0 1600
#define PWM45 2220
#define PWM90 2840


    //Instructions
    //connect pin 1.0 to 1.4 (Ain2)	(green --> green)
    //connect pin 1.3 to 1.5 (Ain1) (red --> red)
    //connect pin 1.6 to 3.5 (Bin1) (yellow --> yellow)
    //connect pin 1.7 to 3.4 (Bin2) (blue --> blue)
	//connect pin 3.6 to servo
	//connnect MCU ground to common ground on breadboard
/*
 * main.c
 */

char sendByte1;
char sendByte2;
char sendByte3;
char sendByte4;
unsigned char dequeuedData;
unsigned char receivedByte1;
volatile int currentByteID = 0;
unsigned int zAngle;			//steps (0.9 deg per step)
unsigned char xAngle;
unsigned char xAngleCommand;
volatile unsigned int queueCounter = 0;
volatile unsigned int sizeOfQueue = 50;
volatile char queue[50];
volatile unsigned int queueStart = 0;
volatile unsigned int queueEnd = 0;
volatile unsigned int queueSize = 0;
volatile unsigned char receivedBuffer;
volatile unsigned int messageSendReadyFlag = 0;
volatile int currentDirection = 0;
volatile int currentStep = 0;
volatile int firstStep = 1;
volatile unsigned char yellow = BIT6;
volatile unsigned char blue = BIT7;
volatile unsigned char red = BIT3;
volatile unsigned char green = BIT0;
volatile unsigned char step0;
volatile unsigned char step1;
volatile unsigned char step2;
volatile unsigned char step3;
volatile unsigned char step4;
volatile unsigned char step5;
volatile unsigned char step6;
volatile unsigned char step7;
volatile unsigned int currentServoPWM = PWM0;

void sendMessage();
void addToQueue(unsigned char element);
unsigned char getFromQueue(void);
unsigned int getQueueSize(void);
void processReceivedMessage(void);
unsigned int parseReset(char receivedDataByte1);
void parseXAngle(char receivedDataByte1);
void setServo(unsigned char angle);
void rotateMotor(int direction);
void setDirection(int direction);
void resetMotorPosition();

int main(void) {

	WDTCTL = WDTPW | WDTHOLD;	// Stop watchdog timer

    //Clock Settings
    CSCTL0 = CSKEY;
    CSCTL1 |= DCORSEL + DCOFSEL_3;		//Set DCO Frequency to Max Settings (24MHz)
    CSCTL2 = SELA_3 + SELS_3 + SELM_3;	//Set DCO = ACLK = MCLK = SMCLK
    CSCTL3 = DIVA_0 + DIVM_0 + DIVS_0; //Set all dividers to 0

    //UART Settings
    UCA1CTLW0 = UCSWRST;
    UCA1CTLW0 |= UCSSEL1 + UCSSEL0; //select SMCLK
    UCA1BRW = 2500;					//baudrate 9600
    UCA1CTLW0 &= ~(UCSWRST);
	P2SEL1 |= BIT5 + BIT6;				//UART pins
	P2SEL0 &= ~(BIT5 + BIT6);
    UCA1IE = UCRXIE;				//enable receive interrupts

    //Timers and board pins
	P1DIR = BIT0 + BIT6;		//output TA0.1 on pin 1.0 and output (TB1.1) on pin 1.6
	//input into motor driver at 1.5 (AIN1)
	P1DIR &= ~(BIT5);
	P1SEL1 &= ~(BIT5);
	P1SEL0 &= ~(BIT5);

	//input into motor driver at 1.4 (AIN2)
	P1DIR &= ~(BIT4);
	P1SEL1 &= ~(BIT4);
	P1SEL0 &= ~(BIT4);

	//input into motor driver at 3.4 (BIN2)
    P3DIR &= ~BIT4;
    P3SEL1 &= ~BIT4;
    P3SEL0 &= ~BIT4;

	//input into motor driver at 3.5 (BIN1)
    P3DIR &= ~BIT5;
    P3SEL1 &= ~BIT5;
    P3SEL0 &= ~BIT5;

    // pin 1.6 NORMAL output
    P1DIR |= BIT6;
    P1SEL1 &= ~(BIT6);
    P1SEL0 &= ~(BIT6);
    P1OUT &= ~(BIT6);

    // pin 1.0 NORMAL output
    P1DIR |= BIT0;
    P1SEL1 &= ~(BIT0);
    P1SEL0 &= ~(BIT0);
    P1OUT &= ~(BIT0);

    // pin 1.3 NORMAL output
    P1DIR |= BIT3;
    P1SEL1 &= ~(BIT3);
    P1SEL0 &= ~(BIT3);
    P1OUT &= ~(BIT3);

    // pin 1.7 NORMAL output
    P1DIR |= BIT7;
    P1SEL1 &= ~(BIT7);
    P1SEL0 &= ~(BIT7);
    P1OUT &= ~(BIT7);

    //pin 2.0 for testing LED
	P2DIR = BIT0;				//LED for testing
	P2OUT &= ~(BIT0);			//LED off

	/*
    //Setting up TB1.0;
	TB1CTL = TBSSEL1 + MC0 + ID0;	//Use SMCLK, UP mode, interrupts, divide clock by 2
	TB1CTL &= ~(TBIFG);
	TB1EX0 = TBIDEX__8;				//divide clock by 8. (currently 1.5MHz)
    TB1CCR0 = 30000;				//50Hz
    TB1CCTL0 |= OUTMOD_7;			//Reset/set mode
    */

	//Step Variables
    step0 = red & ~(yellow+green+blue);
    step1 = (yellow + red) & ~(green+blue);
    step2 = yellow & ~(red+green+blue);
    step3 = (yellow + green) & ~(red+blue);
    step4 = green & ~(yellow+red+blue);
    step5 = (green + blue) & ~(yellow+red);
    step6 = blue & ~(yellow+green+red);
    step7 = (blue+red) & ~(yellow+green);


    //Setting up TB2.1 and pin 3.6;
	TB2CTL = TASSEL1 + MC0 + ID0;//Use SMCLK, UP mode, interrupts, divide clock by 2
	TB2CTL &= ~(TAIFG);
	TB2EX0 = TAIDEX_7;//divide clock by 8. (currently 1.5MHz)
    TB2CCR0 = 29750; 	//30000				//tuned to 50Hz
    TB2CCR1 = currentServoPWM;		//1859 -> 0 degrees, 2417 -> 45 degrees 2975 -> 90 degrees					//neutral servo at 1.25ms
    TB2CCTL0 |= OUTMOD_7;	//Reset/set mode
    TB2CCTL1 |= OUTMOD_7;	//Reset/set mode
    P3DIR |= BIT6;
    P3SEL1 &= ~(BIT6);
    P3SEL0 |= BIT6;

    //Initial Test Conditions
	zAngle = 65281;
	xAngle = 0;

    _EINT();
	while(1)
	{
//		sendMessage();
		if (queueCounter != 0)
		{
			processReceivedMessage();
			if(messageSendReadyFlag)
			{
				sendMessage();
				messageSendReadyFlag = 0;
			}
		}
	}
	return 0;
}

void sendMessage()
{
	//Breaking up the 16-bit number to bytes
	sendByte1 = 255;
	sendByte2 = zAngle >> 8;	//MSB z-angle
	sendByte3 = zAngle & 0xFF;	//LSB z-angle
	sendByte4 = 0;				//conversion byte

	//Conversion bits
	if (sendByte2 == 255)
	{
		sendByte4 |= BIT1;
		sendByte2 = 0;
	}
	if (sendByte3 == 255)
	{
		sendByte4 |= BIT0;
		sendByte3 = 0;
	}
	if (xAngle == 45)
	{
		sendByte4 |= BIT2;
	}
	else if (xAngle == 90)
	{
		sendByte4 |= BIT3;
	}

//	__delay_cycles(1800000);	//delay for 75ms to let motor move
	__delay_cycles(90000);	//delay for 75ms to let motor move
	while(!(UCTXIFG&UCA1IFG));
	UCA1TXBUF = sendByte1;
	while(!(UCTXIFG&UCA1IFG));
	UCA1TXBUF = sendByte2;
	while(!(UCTXIFG&UCA1IFG));
	UCA1TXBUF = sendByte3;
	while(!(UCTXIFG&UCA1IFG));
	UCA1TXBUF = sendByte4;
}

void processReceivedMessage()
{
	while(queueCounter != 0)
	{
		dequeuedData = getFromQueue();
		queueCounter--;
		if (dequeuedData == 255)
		{
			currentByteID = 1;
		}
		else if (currentByteID == 1)
		{
			receivedByte1 = dequeuedData;
			parseXAngle(receivedByte1);

			if(parseReset(receivedByte1))
			{
				//Reset
				setDirection(1);
				resetMotorPosition();
			}
			else
			{
				//Stepper moves one step
				setDirection(0);
				rotateMotor(currentDirection);
				if (firstStep == 1)
				{
					firstStep = 0;
					zAngle = 0;
				}
				else
				{
					zAngle++;
				}
			}

			//Debug Code to Check Received Message
			/*
			while(!(UCTXIFG&UCA1IFG));
			UCA1TXBUF = parseReset(receivedByte1);
			while(!(UCTXIFG&UCA1IFG));
			UCA1TXBUF = xAngleCommand;
			*/

			currentByteID = 0;
			messageSendReadyFlag = 1;
		}
	}
}

unsigned int parseReset(char receivedDataByte1) {         // this obtains the direction from the command

    unsigned int reset = (int) receivedDataByte1 & BIT7; // extract the msb

    return reset;                               // 1 = reset, 0 = no reset
}

void parseXAngle(char receivedDataByte1)
{
	unsigned int angleBits = (unsigned int) receivedDataByte1 & 0x0F;
	 if (angleBits == 0)
	 {
		 xAngleCommand = 0;
	 }
	 else if (angleBits == 1)
	 {
		 xAngleCommand = 45;
	 }
	 else if (angleBits == 2)
	 {
		 xAngleCommand = 90;
	 }

	 if (xAngleCommand != xAngle)
	 {
		 xAngle = xAngleCommand;
		 setServo(xAngle);
		 __delay_cycles(12000000);
	 }
}

void setServo(unsigned char angle)
{
//	PWM:	2850 -> 0 degrees, 2250 -> 45 degrees 1650 -> 90 degrees
	if(angle == currentServoPWM)
	{}
	else if(angle == 0)
	{
		currentServoPWM = PWM0;
		TB2CCR1 = currentServoPWM;
	}
	else if(angle == 45)
	{
		currentServoPWM = PWM45;
		TB2CCR1 = currentServoPWM;
	}
	else if(angle == 90)
	{
		currentServoPWM = PWM90;//1600 was close
		TB2CCR1 = currentServoPWM;
	}
}
void addToQueue(unsigned char element){             // add element to queue
    if(queueSize < sizeOfQueue){                                 // queue is not over-run
        queue[queueEnd] = element;
        queueSize++;

        if(queueEnd < sizeOfQueue - 1){                  // if not at end of queue, increment endQueue to next position
            queueEnd++;
        }
        else{
            queueEnd = 0;                                // restart endQueue
        }
    }
}

unsigned char getFromQueue(void){
    unsigned char element;

    if(queueSize > 0){                                     // if there's something in the queue
        element = queue[queueStart];                    // get elemet from the start of the queue
        queue[queueStart] = '0';                        // empty element at the start of the queue

        queueSize--;                                         // decrease the size of the queue

        if(queueStart < sizeOfQueue - 1){                 // if not at the end of the queue
            queueStart++;                                // increment queueStart up 1 pos
        }
        else{
            queueStart = 0;                                // restart queueStart
        }

        return element;
    }
    else{
        return '0';
    }
}

unsigned int getQueueSize(void){
    return queueSize;
}

void rotateMotor(int direction)
{

	if (direction == 0)
	{
		currentStep++;
	}
	else
	{
		currentStep--;
	}

	if(currentStep == 8)
	{
		currentStep = 0;
	}
	else if (currentStep == -1)
	{
		currentStep = 7;
	}

	if(currentStep == 0)
	{
		P1OUT = step0;
	}
	else if(currentStep == 1)
	{
		P1OUT = step1;
	}
	else if(currentStep == 2)
	{
		P1OUT = step2;
	}
	else if(currentStep == 3)
	{
		P1OUT = step3;
	}
	else if(currentStep == 4)
	{
		P1OUT = step4;
	}
	else if(currentStep == 5)
	{
		P1OUT = step5;
	}
	else if(currentStep == 6)
	{
		P1OUT = step6;

	}
	else if(currentStep == 7)
	{
		P1OUT = step7;
	}

}

void resetMotorPosition()
{
	//Target: 360 degrees in 10 seconds
	//200 steps/10 sec = 20 steps/sec
	//1/20 = 0.05 seconds/step
	//

	while(zAngle > 0)
	{
		rotateMotor(currentDirection);
		zAngle--;
		__delay_cycles(1000000);
	}
	xAngle = 0;
	setServo(xAngle);// servo reset
	firstStep = 1;
}

void setDirection(int direction)
{
	currentDirection = direction;
}

#pragma vector=USCI_A1_VECTOR
__interrupt void UART_RX(void)
{
	receivedBuffer = UCA1RXBUF;
	addToQueue(receivedBuffer);
	queueCounter++;
	return;
}
