/*
 * IncFile1.h
 *
 * Created: 12/06/2022 08:54:22
 *  Author: Dina
 */ 


#ifndef INCFILE1_H_
#define INCFILE1_H_
int step;
void Servo_init(int pin_n)
{
	if (pin_n == 1) DDRD |= (1<<PD5);
	else if (pin_n == 2) DDRD |= (1<<PD7);
		/* Make OC1A pin as output  PD5 for 1- pd7 2*/
	if (pin_n == 1) timer1_setup();
	else timer2_setup();
	
}
void timer1_setup()
{
	TCNT1 = 0;		/* Set timer1 count zero */
	ICR1 = 2499;		/* Set TOP count for timer1 in ICR1 register */
	

	/* Set Fast PWM, TOP in ICR1, Clear OC1A on compare match, clk/64 */
	TCCR1A = (1<<WGM11)|(1<<COM1A1);
	TCCR1B = (1<<WGM12)|(1<<WGM13)|(1<<CS10)|(1<<CS11);
	OCR1A = 250;
	
}
void timer2_setup()
{
	TCNT2 = 0;		/* Set timer2 count zero */
	/* Set Fast PWM, TOP in ICR1, Clear OC1A on compare match, clk/64 */
	TCCR2 = (1<<WGM21)|(1<<CS22)|(1<<COM20);
	OCR2 = 250;
	
}


 void Inc_step_s1 (int angle)
 {
	 int inc_factor = 500*angle/180;
	 step = step+inc_factor;
	  OCR1A = step;
	 
 }
  void Inc_step_s2 (int angle)
  {
	  int inc_factor = 500*angle/180;
	  step = step+inc_factor;
	  OCR2 = step;
	  
  }
 void Dec_step_1 (int angle)
 {
	  int dec_factor = 500*angle/180;
	 
	  step = step-dec_factor;
	  OCR1A = step;
 }
  void Dec_step_2 (int angle)
  {
	  int dec_factor = 500*angle/180;
	  
	  step = step-dec_factor;
	  OCR2 = step;
  }


#endif /* INCFILE1_H_ */