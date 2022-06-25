using System.Threading.Tasks;
using UnityEngine;

namespace ML {
    public abstract class Environment : MonoBehaviour  {
                public class StateReward
                {
                    States state;
                    float reward;
                    public StateReward(States s,float r)
                    {
                        this.state = s;
                        this.reward = r;
                    }
                    public States GetState(){ return state;}
                    public float GetReward(){return reward;}
                }
        public class Actions {
            public int value;
            public Actions(int v)
            {this.value = v;}
        }
        public class States {
            public int value;
            public States(int v)
            {this.value = v;}
        }
        public int statesNumber ;
        public int actionsNumber ;
        public States currentState ;
        public States initialState;
        public float reward;

        /// <summary>
        /// intializes Environment an returns starting state
        /// </summary>
        /// <returns>intial state</returns>
        public abstract States IntializeEnvironment();

        public abstract States ResetEnvironment();

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"> action to take from currentState</param>
        /// <returns> newState</returns>
        public abstract Task<StateReward> TakeAction(Actions action);

        public abstract Task<float> GetReward();

        public abstract bool IsTerminalState();

        public abstract States GetCurrentState();

    }
}