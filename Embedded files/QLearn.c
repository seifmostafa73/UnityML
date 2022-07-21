//#setting up our parameters we discussed earlier

/*

quick reminder of what we want to do in the algorithm

at the beggining of each episdoe we need to reset the state to inital
now for each step in the episdoe we need to randomly select wether to explore or exploit according to a random value r realtive to the ε value (r<ε -> explore) (r>ε -> exploit)
take an acction with explor-exploit in-mind
updating our Q-table values
then set our state to the new state and add the reward to the total rewards of the episode
finally we would update ε using exploration decay rate


episodes = 10000  #this refers to the number of episodes we want or agent to play
Max_steps = 50 #this refers to the Maxnumber of steps at each episode , if exceeded the episode will end immeditely  
learning_rate  = 0.1 # α : rate of updating Q value  
discount_rate  = 0.99 # 𝛾 : rate of decay of rewards
exploration_rate = 1 #ε : prob. of our agent exploring the environment instead of exploiting it
max_exploration_rate = 1
min_exploration_rate = 0.01
explortion_decay = 0.009
rewards_all_episodes = [] # we will use this to visulatize the rewards for each episode*/
#include <stdio.h>
#include <time.h>
#include <stdlib.h>
#include <math.h>

#define episodes 10000  
#define Max_steps  50 
#define learning_rate   0.1 
#define discount_rate   0.99 
#define max_exploration_rate  1
#define min_exploration_rate  0.01
#define explortion_decay  0.009

#define actionsNumber 8
#define statesNumber 9
#define actionDelay 1500

enum States
{
    NO_STATE = -1,
    State_ZERO = 0,
    State_ONE = 1,
    State_TWO = 2,
    State_THREE = 3,
    State_FOUR = 4,
    State_FIVE = 5,
    State_SIX = 6,
    State_SEVEN = 7,
    State_EIGHT = 8,
};
enum Actions
{
    SERVO_ONE_NINETY_DEGREE_UP=0,
    SERVO_ONE_NINETY_DEGREE_DOWN,
    SERVO_ONE_ONEEIGHTY_DEGREE_UP,
    SERVO_ONE_ONEEIGHTY_DEGREE_DOWN,
    SERVO_TWO_NINETY_DEGREE_UP,
    SERVO_TWO_NINETY_DEGREE_DOWN,
    SERVO_TWO_ONEEIGHTY_DEGREE_UP,
    SERVO_TWO_ONEEIGHTY_DEGREE_DOWN
};

struct StateReward
{
    enum States state;
    float reward;
};

// TODO : Encoder initializiition and pin configruation 
// Store initial value of encoder 

float exploration_rate = 1 ;
float rewards_all_episodes[episodes] = {0};
float Qtable[statesNumber][actionsNumber] = {0};
enum States nextStates[statesNumber][actionsNumber] = 
{
    /*State0*/  {State_ONE,State_FOUR,NO_STATE,NO_STATE,State_SEVEN,State_EIGHT,NO_STATE,NO_STATE},
    /*State1*/  {NO_STATE,State_ZERO,NO_STATE,State_FOUR,State_TWO,State_THREE,NO_STATE,NO_STATE},
    /*State2*/  {NO_STATE,State_SEVEN,NO_STATE,State_SIX,NO_STATE,State_ONE,NO_STATE,State_THREE},
    /*State3*/  {NO_STATE,State_EIGHT,NO_STATE,State_FIVE,State_ONE,NO_STATE,State_TWO,NO_STATE},
    /*State4*/  {State_ZERO,NO_STATE,State_ONE,NO_STATE,State_SIX,State_FIVE,NO_STATE,NO_STATE},
    /*State5*/  {State_EIGHT,NO_STATE,State_THREE,NO_STATE,State_FOUR,NO_STATE,State_SIX,NO_STATE},
    /*State6*/  {State_SEVEN,NO_STATE,State_TWO,NO_STATE,NO_STATE,State_FOUR,NO_STATE,State_FIVE},
    /*State7*/  {State_TWO,State_SIX,NO_STATE,NO_STATE,NO_STATE,State_ZERO,NO_STATE,State_EIGHT},
    /*State8*/  {State_THREE,State_FIVE,NO_STATE,NO_STATE,State_ZERO,NO_STATE,State_SEVEN,NO_STATE}
};

enum States initialState = State_ZERO;
enum States currentState;
void restart_Enivironment(void);
void print_Qtable(void);
void print_Rewards(void);
void Initialize_Environment()
{
    print_Qtable();
    print_Rewards();
    restart_Enivironment();
}
void restart_Enivironment(void)
{
    currentState = initialState;
    //Servo_init(1);
    //Servo_init(2);

}
void print_Qtable(void)
{   
    printf("***************************** Q Table ***************************** \n");
    for(int i =0;i<statesNumber;i++)
        { 
            printf("State_%d \t",i);
            for (int j = 0; j < actionsNumber; j++)
            {
                printf(" %f \t", Qtable[i][j]);
            }
            printf("\n");
       }
}
void print_Rewards()
{   
    printf("***************************** Rewards Table ***************************** \n");
    for(int i =0;i<episodes;i++)
        { 
            printf("Episode_%d  %f \n",i,rewards_all_episodes[i]);
       }
        
}
double get_random_double(double min ,double max) { 
        srand(time(NULL)); // randomize seed
        double X= ((double)rand()/(double)(RAND_MAX)) * max ;
        return X+min;
     }
int get_random_int(int min ,int max) { 
        srand(time(0)); // randomize seed
        int X= ((int)rand()%(int)(max-min+1)) ;
        return X+min;
        }
struct StateReward take_Action(enum Actions action)
{
    enum States nextState = nextStates[currentState][action];
      if(nextState == -1 || (action > actionsNumber || action < 0))
        {
            struct StateReward same_State;
            same_State.state = currentState;
            same_State.reward = 0;
            return same_State;
        }
        switch (action)
        {
        case 0:
            //ju(90);
            // wait 1500 ms
            break;
        case 1:
            //Dec_step_s1(90);
            // wait 1500 ms
            break;
        case 2:
            //Inc_step_s1(180);
            // wait 1500 ms
            break;
        case 3:
            //Dec_step_s1(180);
            // wait 1500 ms
            break;
        case 4:
            //Inc_step_s2(90);
            // wait 1500 ms
            break;
        case 5:
            //Dec_step_s2(90);
            // wait 1500 ms
            break;
        case 6:
            //Inc_step_s2(180);
            // wait 1500 ms
            break;
        case 7:
            //Dec_step_s2(180);
            // wait 1500 ms
            break;
        
        default:
            break;
        }
        //delay 50 ms
        //float reward = GetReward();
        currentState = nextState;
        struct StateReward new_State;
            new_State.state = currentState;
            new_State.reward = 1;
        return new_State; 
}
int get_max_action_index(enum States state)
{   
    int maxIndex = 0;
    float maxQvalue = Qtable[state][maxIndex];
    for(int a =1 ;a<actionsNumber;a++)
    {
        if(Qtable[state][a] > maxQvalue)
        {
            maxIndex = a;
        }
    }
    return maxIndex;
}
double get_max_Qvalue(enum States state)
{
    float maxQvalue = Qtable[state][0];
    for(int a =1 ;a<actionsNumber;a++)
    {
        if(Qtable[state][a] > maxQvalue)
        {
            maxQvalue = Qtable[state][a];
        }
    }
    return maxQvalue;
}

void Qlearn()
{
    for(int episode =0;episode<episodes;episode++)
    {
        restart_Enivironment();
        float totalEpisodeReward = 0;

        //for each step
        for(int step = 0 ;step < Max_steps; step++)
        {
            //exploration vs exploitation
            float random = get_random_double(0,1);
            enum Actions action ;
            if(random<=exploration_rate)
            {
                //Explore
                action = (enum Actions) get_random_int(0,actionsNumber-1);
            }else
            {
                //exploit
                action = (enum Actions) get_max_action_index(currentState);
            }
            //take action in environment

            float reward = 0; enum States newState;

            struct StateReward statereward = take_Action(action);
            newState = statereward.state;
            reward = statereward.reward;

            Qtable[currentState][action] = (1-learning_rate) * Qtable[currentState][action] + learning_rate * (reward + discount_rate * (get_max_Qvalue(newState) ) );
            
            totalEpisodeReward += reward;
            currentState = newState;

        }
        rewards_all_episodes[episode] = totalEpisodeReward;
        exploration_rate = min_exploration_rate + (max_exploration_rate - min_exploration_rate) * exp(-explortion_decay * episode);
        print_Rewards();
    }
    print_Qtable();
}

int main(int argc, char const *argv[])
{
    Initialize_Environment();
    Qlearn();
    return 0;
}

