using System.Threading.Tasks;
using UnityEngine;
using ML;

namespace ML {
    public class CrawlerEnvironment : Environment {


        [SerializeField] ServoHeadController servoOne = null;
        [SerializeField] ServoHeadController servoTwo = null;
        [SerializeField] GameObject crawlerRobot = null;
        Transform initialTransofrm = null;

        public ML.Environment.Actions
        SERVO_ONE_NINETY_DEGREE_UP = new ML.Environment.Actions(0),
        SERVO_ONE_NINETY_DEGREE_DOWN = new ML.Environment.Actions(1),
        SERVO_ONE_ONEEIGHTY_DEGREE_UP = new ML.Environment.Actions(2),
        SERVO_ONE_ONEEIGHTY_DEGREE_DOWN = new ML.Environment.Actions(3),
        SERVO_TWO_NINETY_DEGREE_UP = new ML.Environment.Actions(4),
        SERVO_TWO_NINETY_DEGREE_DOWN = new ML.Environment.Actions(5),
        SERVO_TWO_ONEEIGHTY_DEGREE_UP = new ML.Environment.Actions(6),
        SERVO_TWO_ONEEIGHTY_DEGREE_DOWN = new ML.Environment.Actions(7);

        public ML.Environment.States
        NO_STATE = new ML.Environment.States(-1),      // retrun to current state
        State_ZERO = new ML.Environment.States(0),      // initial State where both motors are 90 degrees |--
        State_ONE = new ML.Environment.States(1),      //  |.__.
        State_TWO = new ML.Environment.States(2),      //  ___.___.
        State_THREE = new ML.Environment.States(3),    //  /.__
        State_FOUR = new ML.Environment.States(4),     //  --|
        State_FIVE = new ML.Environment.States(5),     //  .--/
        State_SIX = new ML.Environment.States(6),      //  .___.___
        State_SEVEN = new ML.Environment.States(7),    //  straight up |
        State_EIGHT = new ML.Environment.States(8);    //  |\


        ML.Environment.States[,] nextStates = null;

        public override States IntializeEnvironment() {
            initialTransofrm = crawlerRobot.transform;
            initialState = State_ZERO;
            statesNumber = 9;
            actionsNumber = 8;
            nextStates = new ML.Environment.States[9,8]
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
            return ResetEnvironment();
        }

        public override ML.Environment.States ResetEnvironment() {
            currentState = initialState;
            servoOne.ResetServo();
            servoTwo.ResetServo();
            crawlerRobot.transform.position = initialTransofrm.position;
            crawlerRobot.transform.rotation = initialTransofrm.rotation;
            return currentState;
        }

        public override async Task<ML.Environment.StateReward> TakeAction(ML.Environment.Actions action) {
            var nextState = nextStates[currentState.value,action.value];
            if(nextState.value == -1 || (action.value > actionsNumber || action.value < 0))
            {
                return new ML.Environment.StateReward(currentState,0);
            }
            switch (action.value)
            {
                case 0:
                    servoOne.MoveWithAngle(90,true);
                    await Task.Delay(150);
                    break;

                case 1:
                    servoOne.MoveWithAngle(90,false);
                    await Task.Delay(150);

                    break;

                case 2:
                    servoOne.MoveWithAngle(180,true);
                    await Task.Delay(150);
                    break;

                case 3:
                    servoOne.MoveWithAngle(180,false);
                    await Task.Delay(150);
                    break;

                case 4:
                    servoTwo.MoveWithAngle(90,false);
                    await Task.Delay(150);
                    break;

                case 5:
                    servoTwo.MoveWithAngle(90,true);
                    await Task.Delay(150);

                    break;

                case 6:
                    servoTwo.MoveWithAngle(180,false);
                    await Task.Delay(150);
                    break;

                case 7:
                    servoTwo.MoveWithAngle(180,true);
                    await Task.Delay(150);
                    break;

            }
            await Task.Delay(50);
            float reward = await GetReward();
            currentState = (nextState.value==-1)? currentState:nextState;
            return new ML.Environment.StateReward(currentState,reward);
        }

        public override ML.Environment.States GetCurrentState() {
            return currentState;
        }


        public override bool IsTerminalState() {
            return false;
        }

        public override async Task<float> GetReward() {
            var reward = await RewardFunction();
            return reward;
        }

        private async Task<float> RewardFunction() {
            var startPostion = crawlerRobot.transform.position.z;
            await Task.Delay(1500);
            var lastPostion = crawlerRobot.transform.position.z;
            return -(lastPostion - startPostion);
        }

    }
}